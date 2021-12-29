using System.Collections.Generic;

namespace Assignment1.Models.ViewModels
{
    public class EditMembershipsModel
    {
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Community> RegisteredCommunities { get; set; }
        public IEnumerable<Community> UnregisteredCommunities { get; set; }
    }
}
