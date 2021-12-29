using System.Collections.Generic;

namespace Assignment1.Models.ViewModels
{
    public class ViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Community> Communities { get; set; }
        public IEnumerable<CommunityMembership> CommunityMemberships { get; set; }
        public IEnumerable<Advertisement> Advertisements { get; set; }
    }


}
