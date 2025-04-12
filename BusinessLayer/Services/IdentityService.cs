using BusinessLayer.Models.Identity;
using BusinessLayer.Seeds;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Http;
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
   public class IdentityService : IidentityService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBaseService<FinancialPeriod> _financialPeriod;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IBaseService<AspNetClaims> _claimService;
        private readonly IBaseService<AspNetRoleClaimActions> _roleClaimActionService;
        private readonly IBaseService<ApplicationRoleClaim> _roleClaimService;


        public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, IBaseService<FinancialPeriod> financialPeriod, RoleManager<ApplicationRole> roleManager, IBaseService<AspNetClaims> claimService, IBaseService<AspNetRoleClaimActions> roleClaimActionService, IBaseService<ApplicationRoleClaim> roleClaimService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpContextAccessor = httpContextAccessor;
            _financialPeriod = financialPeriod;
            _roleManager = roleManager;
            _claimService = claimService;
            _roleClaimActionService = roleClaimActionService;
            _roleClaimService = roleClaimService;
        }
       
        public async Task<SignInResult> login(LoginModel LoginModel)
        {
            var result = await signInManager.PasswordSignInAsync(LoginModel.UserName, LoginModel.Password, LoginModel.RememberMe, false);
            return result;
        }

        public async Task LogOut()
        {
            await signInManager.SignOutAsync();            
        }      

        public async Task<IdentityResult> Register(RegisterModel RegisterModel)
        {
            var user = new ApplicationUser { UserName = RegisterModel.UserName, Email = RegisterModel.Email,CompanyId= RegisterModel.CompanyId,EmployeeId=RegisterModel.EmployeeId };
            var result= await userManager.CreateAsync(user, RegisterModel.Password);
           
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);

            }
            return result;

        }
        public bool IsSignedInUser(ClaimsPrincipal principal)
        {
            var IsSignedInUser = signInManager.IsSignedIn(principal);
            return IsSignedInUser;
        }


        public async Task<IdentityResult> AddUser(UserModel UserModel)
        {
            var user = new ApplicationUser { UserName = UserModel.UserName, Email = UserModel.Email, CompanyId = UserModel.CompanyId, EmployeeId = UserModel.EmployeeId ,BranchId= UserModel .BranchId,ActivationState=UserModel.ActivationState};
            var result = await userManager.CreateAsync(user, UserModel.Password);            
            return result;

        }
        public async Task<IdentityResult> UpdateUser(UserModel UserModel)
        {            
            var user = await userManager.FindByNameAsync(UserModel.UserName);
            user.UserName = UserModel.UserName;
            user.Email = UserModel.Email;
            user.CompanyId = UserModel.CompanyId;
            user.EmployeeId = UserModel.EmployeeId;
            user.BranchId = UserModel.BranchId;
            user.ActivationState = UserModel.ActivationState;
            
            var result = await userManager.UpdateAsync(user);
            return result;

        }
        public ApplicationUser GetUserByUserName(string UserName)
        {
            var u = userManager.FindByNameAsync(UserName);

            return u.Result;
        }
        public ApplicationUser GetUserById(string Id)
        {
            var u = userManager.FindByIdAsync(Id);

            return u.Result;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            var users = userManager.Users;
            return users.ToList();
        }

        public async Task<IdentityResult> DeleteUser(string Id)
        {
            var result = new IdentityResult();
            var user = await userManager.FindByIdAsync(Id);
            if (user != null && !user.IsSystemUser)
            {
                result = await userManager.DeleteAsync(user);
            }
            return result;

        }


        public async Task<IdentityResult> AddUserRole(UserModel UserModel)
        {
            var result = new IdentityResult();            
            var role = _roleManager.FindByIdAsync(UserModel.RoleId).Result;              

            var user = await userManager.FindByNameAsync(UserModel.UserName);
            var oldRoles = userManager.GetRolesAsync(user).Result;
            result = userManager.RemoveFromRolesAsync(user, oldRoles).Result;
            if (result.Succeeded)
            {
                result = userManager.AddToRoleAsync(user, role.Name).Result;
                //await signInManager.RefreshSignInAsync(user);
            }           
            return result;

        }
        public async Task<List<string>> GetUserRoles(UserModel UserModel)
        {
            var user =await  userManager.FindByNameAsync(UserModel.UserName);
            var userRoles =await userManager.GetRolesAsync(user);
            return userRoles.ToList();
        }
        public async Task<bool> IsSuperUser(ApplicationUser user)
        {            
            var IsSuperUser = await userManager.IsInRoleAsync(user, SystemRolesNames.Super.ToString());
            return IsSuperUser;
        }

        #region Roles
        public List<ApplicationRole> GetAllRoles()
        {
            var user = httpContextAccessor.HttpContext.User;
            var roles = _roleManager.Roles;
            if (user.Identity.Name!="super")
            {
                roles = roles.Where(x=>x.Name!="Super");
            }

           
            return roles.ToList();
        }

        public async Task<IdentityResult> AddRole(RoleModel roleModel)
        {
            var result = new IdentityResult();
            if (roleModel != null && !string.IsNullOrEmpty(roleModel.Name.Trim()))
            {
                var role = new ApplicationRole()
                {
                    Name= roleModel.Name, NameAr= roleModel.NameAr,ActivationState= roleModel .ActivationState,
                };
                result = await _roleManager.CreateAsync(role);
            }
            return result;
        }
        public async Task<ApplicationRole> GetRoleById(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            return role;

        }
        public async Task<ApplicationRole> GetUserRoleByUserId(string UserId)
        {
            ApplicationRole userRole = null;
            var user = await userManager.FindByIdAsync(UserId);
            var userRoles = await userManager.GetRolesAsync(user);
            if (userRoles!=null&& userRoles.Count()>0)
            {
              var role = await _roleManager.FindByNameAsync(userRoles.FirstOrDefault());
                if (role!=null)
                {
                    userRole = role;
                }
             
            }      
            return userRole;

        }
        public async Task<IdentityResult> UpdateRole(RoleModel roleModel)
        {
            var result = new IdentityResult();
            var role = await _roleManager.FindByIdAsync(roleModel.Id);
            if (role!=null)
            {
                role.Name = roleModel.Name;
                role.NameAr = roleModel.NameAr;
                role.ActivationState = roleModel.ActivationState;
                role.IsSystemRole = roleModel.IsSystemRole;

                result = await _roleManager.UpdateAsync(role);
            }
            return result;

        }
        public async Task<IdentityResult> DeleteRole(string Id)
        {
            var result = new IdentityResult();
            var role = await _roleManager.FindByIdAsync(Id);
            if (role != null && !role.IsSystemRole)
            {               
                result = await _roleManager.DeleteAsync(role);
            }
            return result;

        }

        //Permissions
        public List<AspNetClaims> GetAllPermissions()
        {
            var permissions = _claimService.GetAll();
            return permissions;
        }

        public List<AspNetRoleClaimActions> GetRolePermissionActions(string roleId, int permissionId)
        {
            var actions = _roleClaimActionService.GetAll();
            return actions.Where(x=>x.RoleId==roleId && x.ClaimId==permissionId).ToList();
        }
        public AspNetClaims GetPermissionById(int Id)
        {
            var permission = _claimService.GetById(Id);
            return permission;
        }
        public AspNetClaims GetPermissionBySystemName(string SystemName)
        {
            var permission = _claimService.GetWithCondetion(x => x.SystemName == SystemName).FirstOrDefault() ;
            return permission;
        }

        public AspNetRoleClaimActions GetRolePermissionAction(string roleId, int permissionId, int actionId)
        {
            var actions = _roleClaimActionService.GetAll();
            return actions.Where(x => x.RoleId == roleId && x.ClaimId == permissionId && x.ActionId==actionId).FirstOrDefault();
        }
       public void DeleteRolePermissionAction(string roleId, int permissionId, int actionId)
        {
            var actions = _roleClaimActionService.GetAll();
            var action= actions.Where(x => x.RoleId == roleId && x.ClaimId == permissionId && x.ActionId == actionId).FirstOrDefault();
            if (action!=null)
            {
                _roleClaimActionService.Delete(action);
            }

        }
       public void AddRolePermissionAction(AspNetRoleClaimActions action)
        {
            _roleClaimActionService.Add(action);
        }
       public ApplicationRoleClaim GetRolePermission(string roleId, int permissionId)
        {
            var Permissions = _roleClaimService.GetAll();
            var rolePermission = Permissions.Where(x=>x.RoleId==roleId && x.ClaimId==permissionId).FirstOrDefault();
            return rolePermission;

        }
        public List<ApplicationRoleClaim> GetRolePermissions(string roleId)
        {
            var Permissions = _roleClaimService.GetAll();
            var rolePermissions = Permissions.Where(x => x.RoleId == roleId).ToList();
            return rolePermissions;

        }

        public void DeleteRolePermission(string roleId, int permissionId)
        {
            var Permissions = _roleClaimService.GetAll();
            var rolePermission = Permissions.Where(x=>x.RoleId==roleId && x.ClaimId==permissionId).FirstOrDefault();
            if (rolePermission!=null)
            {
                _roleClaimService.Delete(rolePermission);
            }

        }
       public void AddRolePermission(ApplicationRoleClaim rolePermission)
        {     
            _roleClaimService.Add(rolePermission);
        }

        //Authorization        
        public bool Authorize(string permissionSystemName, ApplicationUser user, PermissionActions? action = null)
        {
            if (String.IsNullOrEmpty(permissionSystemName))
                return false;            
            
            var userRole = GetUserRoleByUserId(user.Id).Result;
            if (userRole==null)
                return false;

                if (Authorize(permissionSystemName, userRole, action))
                {
                    //yes, we have such action
                    return true;
                }           
                
            //no permission found
            return false;
        }


        protected bool Authorize(string permissionSystemName, ApplicationRole userRole, PermissionActions? action = null)
        {
            if (String.IsNullOrEmpty(permissionSystemName))
                return false;

            var rolePermissions =  GetRolePermissions(userRole.Id);
            foreach (var rolePermission in rolePermissions)
            {
                var Permission = GetPermissionById(rolePermission.ClaimId);
                if (Permission.SystemName == permissionSystemName)
                {
                    var permissionAction = GetRolePermissionAction(userRole.Id, Permission.Id, (int)action);
                    if (permissionAction != null && permissionAction.Selected)
                        return true;
                }
            }
            return false;
        }



        public void InstallPermissions(IPermissionProvider permissionProvider)
        {
            //install new permissions
            var permissions = permissionProvider.GetPermissions();           

            foreach (var permission in permissions)
            {
                var permission1 = GetPermissionBySystemName(permission.SystemName);
                if (permission1 != null)
                    continue;

                //new permission (install it)
                permission1 = new AspNetClaims
                {
                    Name = permission.Name,
                    SystemName = permission.SystemName,
                    Category = permission.Category,
                     SubCategory = permission.SubCategory,
                };

                //save new permission
                _claimService.Add(permission1);

            }
            //_roleManager.SeedClaimsForSuperUser(permissionProvider, this).Wait();
        }

        #endregion



    }
}
