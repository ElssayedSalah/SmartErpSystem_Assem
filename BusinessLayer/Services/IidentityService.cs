using BusinessLayer.Models.Identity;
using DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static BusinessLayer.Helpers.SharedEnums;

namespace BusinessLayer.Services
{
    
   public interface IidentityService
    {
        //identity
        Task<SignInResult> login(LoginModel LoginModel);
        Task LogOut();
        bool IsSignedInUser(ClaimsPrincipal principal);
        Task<IdentityResult> Register(RegisterModel RegisterModel);
        //user
        Task<IdentityResult> AddUser(UserModel UserModel);
        Task<IdentityResult> UpdateUser(UserModel UserModel);
        ApplicationUser GetUserByUserName(string UserName);
        ApplicationUser GetUserById(string Id);
        List<ApplicationUser> GetAllUsers();
        Task<IdentityResult> DeleteUser(string Id);
        Task<bool> IsSuperUser(ApplicationUser user);


        //role
        Task<IdentityResult> AddUserRole(UserModel UserModel);
        Task<List<string>> GetUserRoles(UserModel UserModel);  
        List<ApplicationRole> GetAllRoles();
        Task<IdentityResult> AddRole(RoleModel roleModel);
        Task<ApplicationRole> GetRoleById(string Id);
        Task<IdentityResult> UpdateRole(RoleModel roleModel);
        Task<IdentityResult> DeleteRole(string Id);
        Task<ApplicationRole> GetUserRoleByUserId(string UserId);

        //permissions
        List<AspNetClaims> GetAllPermissions();
        List<AspNetRoleClaimActions> GetRolePermissionActions(string roleId,int permissionId);
        AspNetClaims GetPermissionById(int Id);
        AspNetClaims GetPermissionBySystemName(string SystemName);
        AspNetRoleClaimActions GetRolePermissionAction(string roleId, int permissionId,int actionId);
        void DeleteRolePermissionAction(string roleId, int permissionId,int actionId);
        void AddRolePermissionAction(AspNetRoleClaimActions action);
        ApplicationRoleClaim GetRolePermission(string roleId, int permissionId);
        void DeleteRolePermission(string roleId, int permissionId);
        void AddRolePermission(ApplicationRoleClaim rolePermission);
        void InstallPermissions(IPermissionProvider permissionProvider);

        //Authorization
        bool Authorize(string permissionSystemName, ApplicationUser user, PermissionActions? action = null);



    }
}
