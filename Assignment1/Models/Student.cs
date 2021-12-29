﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Assignment1.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
        public string FullName {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
        public ICollection<CommunityMembership> CommunityMemberships { get; set; }
    }
}