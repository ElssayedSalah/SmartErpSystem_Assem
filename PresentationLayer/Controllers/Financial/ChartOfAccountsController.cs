using AutoMapper;
using BusinessLayer.Models.Financial;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Controllers.Financial
{
    public class ChartOfAccountsController : BaseAdminController
    {
        private readonly IBaseService<ChartOfAccountSettings> _AccountsLevelsService;
        private readonly IBaseService<AccountSetting> _AccountsLevelsDetailsService;
        private readonly IBaseService<Account> _AccountsService;
        private readonly IBaseService<AccountOpenBalance> _AccountOpenBalanceService;
        private readonly IHelperRepository _HelperRepository;
        private readonly IMapper _Mapper;
        private readonly string _currentLanguage;
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly LocalizationService _LocalizationService;
        private readonly IidentityService _identityService;
        private readonly IWebHelper _webHelper;

        public ChartOfAccountsController(IBaseService<ChartOfAccountSettings> AccountsLevelsService, IBaseService<AccountSetting> AccountsLevelsDetailsService, IBaseService<Account> AccountsService, IHelperRepository HelperRepository, IBaseService<Currency> CurrencyService, IMapper Mapper, LocalizationService localizationService, IidentityService IdentityService, IWebHelper webHelper, IBaseService<AccountOpenBalance> AccountOpenBalanceService)
        {
            _AccountsLevelsService = AccountsLevelsService;
            _AccountsLevelsDetailsService = AccountsLevelsDetailsService;
            _AccountsService = AccountsService;
            _HelperRepository = HelperRepository;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _CurrencyService = CurrencyService;
            _Mapper = Mapper;
            _LocalizationService = localizationService;
            _identityService = IdentityService;
            _webHelper = webHelper;
            _AccountOpenBalanceService = AccountOpenBalanceService;

        }

        public void IntializeDropdowens()
        {
            var Currencies = _CurrencyService.GetAll();
            ViewBag.Currencies = Currencies.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            ViewBag.AccountNatures = PresentationExtensions.ConvertEnumToSelectListItems(typeof(AccountNatures), _LocalizationService);
            ViewBag.PostingAccounts = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(PostingAccounts), _LocalizationService);
            ViewBag.PostingAccountTypes = PresentationExtensions.ConvertEnumToSelectListItems(typeof(PostingAccountTypes), _LocalizationService);


        }
        public IActionResult ChartOfAccounts()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageChartOfAccounts.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            var AccountsLevels = _AccountsLevelsService.GetAll().FirstOrDefault();
            if (AccountsLevels!=null)
            {
                var AccountsLevelsDetails = _AccountsLevelsDetailsService.GetAll();
                if (AccountsLevelsDetails.Count==0)
                {
                    return RedirectToAction("Create", "ChartOfAccountSettings");
                }
            }     
            return View();
        }
        public IActionResult ListAccountsTree()
        {
            var AccountsLevelsDetails = _AccountsLevelsDetailsService.GetAll();
            var AccountSModel = _AccountsService.GetAll().Select(x =>
            {
                var level = x.Level<AccountsLevelsDetails.Count ? x.Level : AccountsLevelsDetails.Count - 1;
                return new
                {
                    id= x.Id,
                    parentId= x.ParentId==0? null: x.ParentId,
                    NameAr =_currentLanguage == "ar" ? x.NameAr : x.NameEn,
                    x.AccountCode,
                    x.LastLevelInTree,
                    Color = AccountsLevelsDetails[level].Color
                };               
            }).ToList();

            return Json(AccountSModel);
        }

        public IActionResult CreateAccount(int? ParentId)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageChartOfAccounts.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedJson(_LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied"));

            var account = new Account() { ParentId = ParentId.HasValue? ParentId.Value:0 };
            _HelperRepository.PrepareNewAccount(account);
            var model = _Mapper.Map<AccountModel>(account);
            var AccountsLevels = _AccountsLevelsDetailsService.GetAll().OrderBy(x => x.Id).FirstOrDefault();

            if (AccountsLevels != null && account.AccountCode.Length == AccountsLevels.Length)
            {
                model.AllowPost = true;
            }
            else
            {
                model.AllowPost = false;
            }

            if (model.LastLevelInTree)
            {
                model.AllowUpdateOpenBalance = true;
            }
            else
            {
                model.AllowUpdateOpenBalance = false;

            }

            model.OpenDate = DateTime.Now;

            IntializeDropdowens();

            return View(model);
        }
        [HttpPost]
        public IActionResult CreateAccount(AccountModel model)
        {     
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageChartOfAccounts.SystemName, CurrentUser, PermissionActions.Create))
                {
                    var result1 = new AccountResultModel()
                    {                      
                        Message = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied")
                    };
                    return Json(result1);
                }
                    

                var result = new AccountResultModel();
                if (ModelState.IsValid)
                {
                    var account = _Mapper.Map<Account>(model);

                    var AcountsSettings = _AccountsLevelsService.GetAll().FirstOrDefault();
                    if (AcountsSettings !=null)
                    {
                        if (account.AccountCode.Length>= AcountsSettings.AccountSettingsTotalLength)
                        {
                            account.LastLevelInTree = true;
                        }
                    }
                    var AccountParent = _AccountsService.GetWithCondetion(x=>x.Id==account.ParentId).FirstOrDefault();
                    if (AccountParent!=null)
                    {
                        account.PostType = AccountParent.PostType;
                        account.PostTo = AccountParent.PostTo;
                    }
                    //save account to database
                    _AccountsService.Add(account);

                    //add account open balance if it is last account
                    if (account.LastLevelInTree)
                    {
                        AccountOpenBalance AccountOpenBalance = new()
                        {
                            AccountId = account.Id,
                            FinancialPeriodId=CurrentUser.FinancialPeriodId.Value,
                            CompanyId=CurrentUser.CompanyId.Value,
                            OpenBalanceDebit=account.OpenBalanceDebit,
                            OpenBalanceCredit=account.OpenBalanceCredit
                        };
                        _AccountOpenBalanceService.Add(AccountOpenBalance);                        
                    }
                 



                    result = new AccountResultModel()
                    {
                        IsSaved = true,
                        Value = account.Id.ToString(),
                        Message = $"{_LocalizationService.GetLocalizedHtmlString("Account.Created")} {account.AccountCode}"
                    };
                    return Json(result);
                }
                else
                {
                    result = new AccountResultModel()
                    {
                        IsSaved = false,
                        Message = ModelState.ErrorCount.ToString()
                    };
                    return Json(result);
                }
         
            }
            catch (Exception ex)
            {
                var result = new AccountResultModel()
                {
                    IsSaved = false,
                    Message = ex.InnerException.ToString()
                };
                return Json(result);
            }    
           
        }

        public IActionResult EditAccount(int id)
        {          

            var Account = _AccountsService.GetById(id);

            if (Account == null)
                return RedirectToAction("ChartOfAccounts");

            var model = _Mapper.Map<AccountModel>(Account);
            var parentAccount = _AccountsService.GetById(model.ParentId);
            if (parentAccount != null)
            {
                model.ParentId = parentAccount.Id;
            }

            //get account open balance if the account is last account
            if (model.LastLevelInTree)
            {
                var AccountOpenBalance = _AccountOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId.Value && x.CompanyId == CurrentUser.CompanyId.Value && x.AccountId == id).FirstOrDefault();

                if (AccountOpenBalance!=null)
                {
                    model.OpenBalanceDebit = AccountOpenBalance.OpenBalanceDebit;
                    model.OpenBalanceCredit = AccountOpenBalance.OpenBalanceCredit;
                }

                model.AllowUpdateOpenBalance = true;
            }
            else
            {
                model.AllowUpdateOpenBalance = false;
                var accounts = _AccountsService.GetAll();

                //get account childs that is last level 
                var AccountChilds = GetAccountChilds(id, accounts).Where(x=>x.LastLevelInTree).Select(x=>x.Id).ToList();

                //get open balance for all account childs in the currunt finance and company
                var AccountChildsOpenBalance =_AccountOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId.Value && x.CompanyId == CurrentUser.CompanyId.Value && AccountChilds.Contains( x.AccountId)).ToList();

                model.OpenBalanceDebit = AccountChildsOpenBalance.Sum(x=>x.OpenBalanceDebit);
                model.OpenBalanceCredit = AccountChildsOpenBalance.Sum(x => x.OpenBalanceCredit);

            }
    

            IntializeDropdowens();

            return View(model);
        }

        [HttpPost]
        public IActionResult EditAccount(AccountModel model)
        {           
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageChartOfAccounts.SystemName, CurrentUser, PermissionActions.Edit))
                {
                    var result1 = new AccountResultModel()
                    {
                        Message = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied")
                    };
                    return Json(result1);
                }

                var result = new AccountResultModel();

                if (ModelState.IsValid)
                {
                    var account = _Mapper.Map<Account>(model);
                    var AcountsSettings = _AccountsLevelsService.GetAll().FirstOrDefault();
                    if (AcountsSettings != null)
                    {
                        if (account.AccountCode.Length >= AcountsSettings.AccountSettingsTotalLength)
                        {
                            account.LastLevelInTree = true;
                        }                  
                    }

                    var AccountParent = _AccountsService.GetWithCondetion(x => x.Id == account.ParentId).FirstOrDefault();
                    if (AccountParent != null)
                    {
                        account.PostType = AccountParent.PostType;
                        account.PostTo = AccountParent.PostTo;
                    }

                    //update account
                    _AccountsService.Update(account);

                    //update account open balance if the account is last account
                    if (account.LastLevelInTree)
                    {
                        var AccountOpenBalance = _AccountOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId.Value && x.CompanyId == CurrentUser.CompanyId.Value && x.AccountId == account.Id).FirstOrDefault();

                        if (AccountOpenBalance != null)
                        {
                            AccountOpenBalance.OpenBalanceDebit = account.OpenBalanceDebit;
                            AccountOpenBalance.OpenBalanceCredit = account.OpenBalanceCredit;
                            _AccountOpenBalanceService.Update(AccountOpenBalance);
                        }

                    }


                    result = new AccountResultModel()
                    {
                        IsSaved = true,
                        Value = account.Id.ToString(),
                        Message = $"{_LocalizationService.GetLocalizedHtmlString("Account.Updated")} {account.AccountCode}"
                    };
                    return Json(result);
                }
                else
                {
                    result = new AccountResultModel()
                    {
                        IsSaved = false,
                        Message = ModelState.ErrorCount.ToString()
                    };
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                var result = new AccountResultModel()
                {
                    IsSaved = false,
                    Message = ex.InnerException.ToString()
                };
                return Json(result);
            }          
           
        }

        public IActionResult DeleteAccount(int id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageChartOfAccounts.SystemName, CurrentUser, PermissionActions.Delete))
                return Json(new { type = "faild",msg= _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

            var accounts = _AccountsService.GetAll();
            var AccountChilds = GetAccountChilds(id, accounts).ToList();
            var AccountChildsIds = AccountChilds.OrderBy(o => o.Id).Select(x => x.Id).ToList();
            //prevent delete account if it was used in entries
            string msg = "";
            msg = _webHelper.IsAllowedDeleteAccount(AccountChildsIds);

            if (msg != "")
            {
                return Json(new { type = "faild", msg = msg });
            }

            foreach (var account in AccountChilds)
            {
                _AccountsService.Delete(account);

                var AccountOpenBalance = _AccountOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId.Value && x.CompanyId == CurrentUser.CompanyId.Value && x.AccountId == account.Id).FirstOrDefault();

                if (AccountOpenBalance != null)
                {
                    _AccountOpenBalanceService.Delete(AccountOpenBalance);
                }

            }

            return Json(new { type = "success" });

        }
        public IActionResult DeleteAccountSub(int id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageChartOfAccounts.SystemName, CurrentUser, PermissionActions.Delete))
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

            var accounts = _AccountsService.GetAll();
            var AccountChilds = GetAccountChilds(id, accounts).Where(a => a.Id != id).ToList();
            var AccountChildsIds = AccountChilds.OrderBy(o => o.Id).Select(x => x.Id).ToList();

            //prevent delete account if it was used in entries
            string msg ="";
             msg = _webHelper.IsAllowedDeleteAccount(AccountChildsIds);     

            if (msg!="")
            {
                return Json(new { type = "faild", msg = msg });
            }
           

            foreach (var account in AccountChilds)
            {
                _AccountsService.Delete(account);

                var AccountOpenBalance = _AccountOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId.Value && x.CompanyId == CurrentUser.CompanyId.Value && x.AccountId == account.Id).FirstOrDefault();

                if (AccountOpenBalance!=null)
                {
                    _AccountOpenBalanceService.Delete(AccountOpenBalance);
                }
            }
            return Json(new { type = "success" });
        }

        public IActionResult GetPostTypes(int Post)
        {
            var lst = new List<dynamic>() { };
            foreach (int item in Enum.GetValues(typeof(PostingAccountTypes)))
            {
                if ((item + "").StartsWith(Post + ""))
                    lst.Add(new { Id = item, Name = _LocalizationService.GetLocalizedHtmlString(((PostingAccountTypes)item).ToString()).Value });
            }
            return Json(lst);
        }

        private IList<Account> GetAccountChilds(int id, IList<Account> items)
        {
            var childs = items
                .Where(x => x.ParentId == id || x.Id == id)
                .Union(items.Where(x => x.ParentId == id)
                .SelectMany(y => GetAccountChilds(y.Id, items)));

            return childs.ToList();
        }

    }
}
