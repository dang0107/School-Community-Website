using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment1.Data;
using Assignment1.Models;
using Assignment1.Models.ViewModels;

namespace Assignment1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolCommunityContext _context;

        public StudentsController(SchoolCommunityContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(int? ID)
        {
            var viewModel = new ViewModel
            {
                Students = await _context.Students.ToListAsync()
            };
            if (ID is null)
            {
                viewModel.Communities = null;
            }
            else
            {
                viewModel.Communities = await (from com in _context.Communities
                                               join membership in _context.CommunityMemberships
                                               on com.ID equals membership.CommunityID
                                               where membership.StudentID.Equals(ID)
                                               select new Community
                                               {
                                                   Title = com.Title
                                               }).ToListAsync();
            }
            return View(viewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/EditMemberships/5
        public async Task<IActionResult> EditMemberships(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            var allCommunities = await _context.Communities.ToListAsync();
            if (student == null)
            {
                return NotFound();
            }
            var RegisteredCommunities = await (from com in _context.Communities
                                               join membership in _context.CommunityMemberships
                                               on com.ID equals membership.CommunityID
                                               where membership.StudentID == id
                                               select new Community
                                               {
                                                   ID = com.ID,
                                                   Title = com.Title
                                               }).ToListAsync();
            var UnregisteredCommunities = new List<Community>();
            foreach (var Community in allCommunities)
            {
                if (!RegisteredCommunities.Contains(Community))
                {
                    UnregisteredCommunities.Add(Community);
                }
            }
            EditMembershipsModel viewModel = new()
            {
                Students = new[] { student },
                RegisteredCommunities = RegisteredCommunities,
                UnregisteredCommunities = UnregisteredCommunities
            };
            return View(viewModel);
        }

        // POST: Students/RemoveMemberships?studentId=1&communityId=A1
        public async Task<IActionResult> RemoveMemberships(int? studentId, string communityId)
        {
            if (studentId == null || communityId == null)
            {
                return NotFound();
            }

            var CommunityMembership = await (from mem in _context.CommunityMemberships
                                             where mem.StudentID == studentId && mem.CommunityID.Equals(communityId)
                                             select mem).FirstOrDefaultAsync();

            _context.Remove(CommunityMembership);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditMemberships), new { @id = studentId });
        }

        // POST: Students/AddMemberships?studentId=1&communityId=A1
        public async Task<IActionResult> AddMemberships(int? studentId, string communityId)
        {
            if (studentId == null || communityId == null)
            {
                return NotFound();
            }

            var CommunityMembership = await (from mem in _context.CommunityMemberships
                                             where mem.StudentID == studentId && mem.CommunityID.Equals(communityId)
                                             select mem).FirstOrDefaultAsync();

            _context.Add(new CommunityMembership
            {
                StudentID = (int)studentId,
                CommunityID = communityId
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditMemberships), new { @id = studentId });
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
