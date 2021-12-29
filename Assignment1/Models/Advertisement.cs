using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment1.Models
{
    public class Advertisement
    {
        [Key]
        public int AdsId { get; set; }

        [ForeignKey("Community")]
        public string CommunityID { get; set; }

        [Required]
        [DisplayName("File Name")]
        public string FileName { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
    }
}
