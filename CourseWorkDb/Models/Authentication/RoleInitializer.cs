using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CourseWorkDb.Models.Authentication
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "nsetko@bk.ru";
            string adminSurname = "Сетко";
            string adminName = "Анастасия";
            string adminMiddleName = "Игоревна";
            string adminPhoneNumber = "+375445487111";
            string adminPassword = "1111";

            if (await roleManager.FindByNameAsync(Roles.Admin) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }
            if (await roleManager.FindByNameAsync(Roles.User) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.User));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User 
                { 
                    Email = adminEmail, 
                    UserName = adminEmail, 
                    PhoneNumber = adminPhoneNumber,
                    Surname = adminSurname,
                    Name = adminName,
                    MiddleName = adminMiddleName
                };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(admin, new string[]{ Roles.User, Roles.Admin});
                }
            }
        }
    }
}
