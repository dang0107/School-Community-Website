using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Assignment1.Models
{
    public class Community : System.IEquatable<Community>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Registration Number")]
        [Required]
        public string ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }
        public ICollection<CommunityMembership> CommunityMemberships { get; set; }

        public bool Equals(Community other)
        {
            if (other.ID.Equals(this.ID)) return true;
            return false;
        }
        public ICollection<Advertisement> Advertisements { get; set; }
    }
}
