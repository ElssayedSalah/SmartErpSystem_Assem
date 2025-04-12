using BusinessLayer.Services;
using DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Seeds
{
    public static class DefaultUsers
    {        

        public static async Task SeedSuperUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPermissionProvider permissionProvider, IidentityService IdentityService)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "super",
                Email = "super@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                ActivationState = true,
                IsSystemUser=true
            
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByNameAsync(defaultUser.UserName);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "super");                   
                    await userManager.AddToRoleAsync(defaultUser, "Super");

                    await roleManager.SeedClaimsForSuperUser(permissionProvider, IdentityService);

                }
                else
                {
                    await roleManager.SeedClaimsForSuperUser(permissionProvider, IdentityService);

                }
                //await roleManager.SeedClaimsForSuperUser(permissionProvider, IdentityService);
            }
        }

        public async static Task SeedClaimsForSuperUser(this RoleManager<ApplicationRole> roleManager, IPermissionProvider permissionProvider, IidentityService IdentityService)
        {
            //var permissions = permissionProvider.GetPermissions();
            var permissions = IdentityService.GetAllPermissions();
            var availableActions = permissionProvider.AvailableActions;

            var superRole = await roleManager.FindByNameAsync("Super");
            
            foreach (var p in permissions)
            {
                var oldRolePermission = IdentityService.GetPermissionById(p.Id);

                foreach (var a in availableActions)
                {
                    var permissionAction = new AspNetRoleClaimActions()
                    {
                        RoleId = superRole.Id,
                        ClaimId = p.Id,
                        ActionId = a.Id,
                        Selected = true
                    };
                    //update permission actions with selected actions
                    var oldPermissionAction = IdentityService.GetRolePermissionAction(superRole.Id, p.Id, a.Id);
                    if (oldPermissionAction != null)
                    {
                        //IdentityService.DeleteRolePermissionAction(superRole.Id, p.Id, a.Id);
                        //IdentityService.AddRolePermissionAction(permissionAction);
                    }
                    else
                    {
                        IdentityService.AddRolePermissionAction(permissionAction);

                    }
                }

                //update rule permissions with selected permissions
                var oldPermissionMaping = IdentityService.GetRolePermission(superRole.Id, p.Id);
                if (oldPermissionMaping != null)
                {
                    //IdentityService.DeleteRolePermission(oldPermissionMaping.RoleId, oldRolePermission.Id);
                    //IdentityService.AddRolePermission(new ApplicationRoleClaim { ClaimId = p.Id, RoleId = superRole.Id, ClaimType = "Permission", ClaimValue = "Permission" });
                }
                else
                {
                    IdentityService.AddRolePermission(new ApplicationRoleClaim { ClaimId = p.Id, RoleId = superRole.Id, ClaimType = "Permission", ClaimValue = "Permission" });

                }

            }


        }

       
    }
}
