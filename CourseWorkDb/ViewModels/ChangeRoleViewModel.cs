using Microsoft.AspNetCore.Identity;

namespace CourseWorkDb.ViewModels
{
    public class ChangeRoleViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string AdminRole { get; set; }
        public string UserAdminRole { get; set; }
    }
}
