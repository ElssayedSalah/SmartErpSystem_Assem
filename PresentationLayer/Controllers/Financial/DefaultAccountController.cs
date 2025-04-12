using AutoMapper;
using BusinessLayer.Models.Financial;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Controllers.Financial
{
    public class DefaultAccountController : BaseAdminController
    {

        private readonly IBaseService<DefaultAccount> _DefaultAccountService;
        private readonly IBaseService<Account> _AccountService;
        private readonly IMapper _Mapper;
        private readonly string _currentLanguage;
        private readonly LocalizationService _LocalizationService;
        private readonly IidentityService _identityService;

        public DefaultAccountController(  IBaseService<DefaultAccount> DefaultAccountService, IMapper Mapper, LocalizationService localizationService, IBaseService<Account> AccountService, IidentityService IdentityService)
        {
            _DefaultAccountService = DefaultAccountService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _Mapper = Mapper;
            _LocalizationService = localizationService;
            _AccountService = AccountService;
            _identityService = IdentityService;
        }

        public void IntializeDropdowens()
        {
            var allAccounts = _AccountService.GetAll();
            allAccounts.Add(new Account() { Id = 0, NameAr = "إختار", NameEn = "Select" ,LastLevelInTree=true});

            var Accounts = allAccounts.Where(x => x.LastLevelInTree).ToList();           
            ViewBag.Accounts = Accounts.Select(x => new { Id = x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });
            ViewBag.AllAccounts = allAccounts.Select(x => new { Id = x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

        }


        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDefaultAccounts.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            IntializeDropdowens();

            var DefaultAccounts = _DefaultAccountService.GetAll();

            var DefaultAccountsGroups = new List<DefaultAccountsGroups>();
            DefaultAccountsGroups = DefaultAccounts.GroupBy(g=>g.GroupId).Select(x =>
             {
                 var DefaultAccount = new DefaultAccountsGroups();
                 DefaultAccount.GroupName = _currentLanguage == "ar" ? x.FirstOrDefault().GroupNameAr:x.FirstOrDefault().GroupNameEn;
                 DefaultAccount.DefaultAccountModel = _Mapper.Map<List<DefaultAccountModel>>(x);
                 return DefaultAccount;
             }).ToList();

           
            return View(DefaultAccountsGroups);
        }
        [HttpPost]
        public IActionResult Index(List<DefaultAccountsGroups> model)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDefaultAccounts.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                foreach (var group in model)
                {
                 var DefaultAccounts = _Mapper.Map<List<DefaultAccount>>(group.DefaultAccountModel);
                    foreach (var item in DefaultAccounts)
                    {
                        _DefaultAccountService.Update(item);
                    }
                }
            }

            IntializeDropdowens();

            return View(model);
        }
    }
}
