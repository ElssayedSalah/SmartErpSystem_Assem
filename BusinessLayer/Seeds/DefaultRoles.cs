using DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BusinessLayer.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            var role = new ApplicationRole()
            {
                Name = "Super",
                NameAr = "سوبر",
                ActivationState = true,
                IsSystemRole=true
            };
            await roleManager.CreateAsync(role);
            
        }      

    }
}
