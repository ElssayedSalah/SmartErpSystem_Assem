using BusinessLayer.Models.Identity;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Controllers.Identity
{
    [Authorize]
    public class PermissionsController : BaseAdminController
    {
        private readonly IidentityService _IdentityService;
        private readonly string _currentLanguage;
        private readonly LocalizationService _LocalizationService;

        public PermissionsController(IidentityService IdentityService, LocalizationService localizationService)
        {
            _IdentityService = IdentityService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _LocalizationService = localizationService;

        }
        public virtual async Task<IActionResult> AccessDenied(string pageUrl)
        {
            //var currentCustomer = await _workContext.GetCurrentCustomerAsync();
            //if (currentCustomer == null || await _customerService.IsGuestAsync(currentCustomer))
            //{
            //    await _logger.InformationAsync($"Access denied to anonymous request on {pageUrl}");
            //    return View();
            //}

            //await _logger.InformationAsync($"Access denied to user #{currentCustomer.Email} '{currentCustomer.Email}' on {pageUrl}");

            return View();
        }

       
        public IActionResult Index()
        {
            var r = _IdentityService.IsSuperUser(CurrentUser).Result;
            if (!_IdentityService.Authorize(StandardPermissionProvider.ManagePermissions.SystemName, CurrentUser, PermissionActions.List) && !_IdentityService.IsSuperUser(CurrentUser).Result)
                return AccessDeniedView();

            var provider = (IPermissionProvider)Activator.CreateInstance(typeof(StandardPermissionProvider));
            var model = new PermissionsMappingModel();
            var RolesModel = new List<RoleModel>();

            var Roles = _IdentityService.GetAllRoles();

            var defualtPermissions = provider.GetPermissions();
            var permissions = _IdentityService.GetAllPermissions();

            //loop over all rules and add permissions and permissions actions to each rule to be viewed in the permissions form
            foreach (var role in Roles)
            {
                var newRole = new RoleModel();
                newRole.Name = _currentLanguage=="ar"? role.NameAr: role.Name;
                newRole.Id = role.Id;

                //add permissions to rules
                foreach (var permission in permissions)
                {
                    var defualtPermission = defualtPermissions.Where(x => x.SystemName == permission.SystemName ).FirstOrDefault() ;
                    if (defualtPermission!=null)
                    {
                        var newPermission = new PermissionsModel()
                        {
                            Id = permission.Id,
                            Name = _LocalizationService.GetLocalizedHtmlString($"Permission.{permission.SystemName}"),
                            SystemName = permission.SystemName,
                            Category = permission.Category,
                            SubCategory = permission.SubCategory,

                        };
                        var permissionActions = _IdentityService.GetRolePermissionActions(role.Id, permission.Id);
                        if (permissionActions != null && permissionActions.Count > 0)
                        {
                            //add actions  to permissions
                            foreach (var a in permissionActions)
                            {
                                newPermission.Actions.Where(x => x.Id == a.ActionId).FirstOrDefault().Selected = a.Selected;
                                newPermission.Actions.Where(x => x.Id == a.ActionId).FirstOrDefault().Allowed = defualtPermission.AllowedActions.Contains(a.ActionId);
                            }
                        }
                        else
                        {
                            //add actions  to permissions
                            foreach (var a in newPermission.Actions.ToList())
                            {
                                newPermission.Actions.Where(x => x.Id == a.Id).FirstOrDefault().Allowed = defualtPermission.AllowedActions.Contains(a.Id);
                            }
                        }
                        newRole.RolePermissions.Add(newPermission);
                    }
          
                }

                RolesModel.Add(newRole);
            }
            model.AvailableRoles = RolesModel;
            model.AvailableRoles.Select(x => {
                x.RolePermissions = x.RolePermissions.OrderBy(rp => rp.Category).ThenBy(g=>g.SubCategory).ToList();
                return x;
            }).ToList();




            return View(model);
        }
        [HttpPost]
        public IActionResult Index(PermissionsMappingModel model)
        {
            if (!_IdentityService.Authorize(StandardPermissionProvider.ManagePermissions.SystemName, CurrentUser, PermissionActions.Create) && !_IdentityService.IsSuperUser(CurrentUser).Result)
                    return AccessDeniedView();
                foreach (var role in model.AvailableRoles)
                {
                    foreach (var p in role.RolePermissions)
                    {
                        var oldRolePermission = _IdentityService.GetPermissionById(p.Id);

                        foreach (var a in p.Actions)
                        {
                            var permissionAction = new AspNetRoleClaimActions()
                            {
                                RoleId = role.Id,
                                ClaimId = p.Id,
                                ActionId = a.Id,
                                Selected = a.Selected
                            };
                            //update permission actions with selected actions
                            var oldPermissionAction = _IdentityService.GetRolePermissionAction(role.Id, p.Id, a.Id);
                            if (oldPermissionAction != null)
                            {
                                _IdentityService.DeleteRolePermissionAction(role.Id, p.Id, a.Id);
                                _IdentityService.AddRolePermissionAction(permissionAction);
                            }
                            else
                            {
                                _IdentityService.AddRolePermissionAction(permissionAction);

                            }
                        }

                        //update rule permissions with selected permissions
                        var oldPermissionMaping = _IdentityService.GetRolePermission(role.Id, p.Id);
                        if (oldPermissionMaping != null)
                        {
                            _IdentityService.DeleteRolePermission(oldPermissionMaping.RoleId, oldRolePermission.Id);
                            _IdentityService.AddRolePermission(new ApplicationRoleClaim { ClaimId = p.Id, RoleId = role.Id, ClaimType = "Permission", ClaimValue = "Permission" });
                        }
                        else
                        {
                            _IdentityService.AddRolePermission(new ApplicationRoleClaim { ClaimId = p.Id, RoleId = role.Id, ClaimType = "Permission", ClaimValue = "Permission" });
                        }
                    }
                }                    

            return RedirectToAction("Index");

        }

        public virtual IActionResult FixPermissions()
        {
            var permissionProviders = new List<Type> { typeof(StandardPermissionProvider) };
            foreach (var providerType in permissionProviders)
            {
                var provider = (IPermissionProvider)Activator.CreateInstance(providerType);
                _IdentityService.InstallPermissions(provider);
            }

            return Json("Ok");
        }


    }
}
