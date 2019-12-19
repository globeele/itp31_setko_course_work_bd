using Microsoft.AspNetCore.Identity;

namespace CourseWorkDb.Models.Authentication
{
    public class User : IdentityUser
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public override string Email { get; set; }
    }
}
