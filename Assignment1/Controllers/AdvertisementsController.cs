using Assignment1.Data;
using Assignment1.Models;
using Assignment1.Models.ViewModels;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment1.Controllers
{
    public class AdvertisementsController : Controller
    {
        private readonly SchoolCommunityContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName = "smilies";

        public AdvertisementsController(SchoolCommunityContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }
        public async Task<IActionResult> Index(string id)
        {
            if(id == null)
            {
               return NotFound();
            }
            var ads = await (from ad in _context.Advertisements where ad.CommunityID.Equals(id) select ad).ToListAsync();
            var community = await _context.Communities.FindAsync(id);
            //var temp = new List<Community> { com };
            var viewModel = new ViewModel
            {
                Communities = new[] { community },
                Advertisements = ads
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Upload(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var community = await _context.Communities.FindAsync(id);
            //var temp = new List<Community> { com };
            var viewModel = new ViewModel
            {
                Communities = new[] { community }
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file, string communityID)
        {
            if(file == null)
            {
                return View("Error");
            }

            BlobContainerClient containerClient;
            // Create the container and return a container client object
            try
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
                // Give access to public
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }


            try
            {
                // create the blob to hold the data
                var blockBlob = containerClient.GetBlobClient(file.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                using (var memoryStream = new MemoryStream())
                {
                    // copy the file data into memory
                    await file.CopyToAsync(memoryStream);

                    // navigate back to the beginning of the memory stream
                    memoryStream.Position = 0;

                    // send the file to the cloud
                    await blockBlob.UploadAsync(memoryStream);
                    memoryStream.Close();
                }

                // add the photo to the database if it uploaded successfully
                var image = new Advertisement();
                image.Url = blockBlob.Uri.AbsoluteUri;
                image.FileName = file.FileName;
                image.CommunityID = communityID;

                _context.Advertisements.Add(image);
                _context.SaveChanges();
            }
            catch (RequestFailedException)
            {
                View("Error");
            }
            return RedirectToAction(nameof(Index), new { @id = communityID });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var image = await _context.Advertisements
                .FirstOrDefaultAsync(m => m.AdsId == id);
            if (image == null)
            {
                return NotFound();
            }
            var community = await _context.Communities
                .FirstOrDefaultAsync(m => m.ID == image.CommunityID);

            //var temp = new List<Community> { com };
            var viewModel = new ViewModel
            {
                Communities = new[] { community },
                Advertisements = new [] { image }
            };
            
            return View(viewModel);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Advertisements.FindAsync(id);
            var communityID = image.CommunityID;


            BlobContainerClient containerClient;
            // Get the container and return a container client object
            try
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            try
            {
                // Get the blob that holds the data
                var blockBlob = containerClient.GetBlobClient(image.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                _context.Advertisements.Remove(image);
                await _context.SaveChangesAsync();

            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            return RedirectToAction(nameof(Index), new { @id = communityID });
        }

    }
}

