using AutoMapper;
using BusinessLayer.EGElectronicInvoice;
using BusinessLayer.Models.Identity;
using BusinessLayer.Models.System;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers.Identity
{
    public class AccountController : Controller
    {
        private readonly IidentityService _IdentityService;
        private readonly LocalizationService _LocalizationService;
        private readonly IBaseService<Company> _CompanyService;
        private readonly IBaseService<FinancialPeriod> _FinancialPeriodService;
        private readonly IBaseService<SystemSetting> _SystemSetingService;
        private readonly IMapper _Mapper;

        public AccountController(IidentityService IdentityService, LocalizationService localizationService, IBaseService<Company> CompanyService, IMapper mapper, IBaseService<FinancialPeriod> FinancialPeriodService, IBaseService<SystemSetting> SystemSetingService)
        {
            _IdentityService = IdentityService; 
            _CompanyService = CompanyService;
            _LocalizationService = localizationService;
            _FinancialPeriodService = FinancialPeriodService;
            _SystemSetingService = SystemSetingService;

            _Mapper = mapper;

        }
        public void IntializeDropdowens()
        {   
            var FinancialPeriods = _FinancialPeriodService.GetAll();
            ViewBag.FinancialPeriods = FinancialPeriods;
        }

        [HttpGet]
        [AllowAnonymous]     
        //[ValidateAntiForgeryToken]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel RegisterModel)
        {
            if (ModelState.IsValid)
            {
                var result =await _IdentityService.Register(RegisterModel);
                if (result.Succeeded)
                {                   
                    return RedirectToAction("Dashpoard2", "Home");

                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]  
        public ActionResult Login()
        {
            IntializeDropdowens();
            var FinancialPeriod = _FinancialPeriodService.GetWithCondetion(x=>x.Year==DateTime.Now.Year)?.FirstOrDefault();
            if (TempData["UnAuthorizeUser"] != null)
            {
                ModelState.AddModelError("UnAuthorizeUser", "UnAuthorizeUser");
            }

            return View(new LoginModel() { FinancialPeriodId = FinancialPeriod !=null?  FinancialPeriod.Id:0 });
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> login(LoginModel LoginModel)
        {
            if (ModelState.IsValid)
            {
                var user = _IdentityService.GetUserByUserName(LoginModel.UserName);
                if (user!=null)
                {
                    if (user.ActivationState)
                    {
                        var result = await _IdentityService.login(LoginModel);
                        if (result.Succeeded)
                        {
                            if (LoginModel.FinancialPeriodId.HasValue)
                            {
                                var FinancialPeriod = _FinancialPeriodService.GetById(LoginModel.FinancialPeriodId.Value);
                                if (FinancialPeriod != null)
                                {
                                    user.FinancialPeriodId = LoginModel?.FinancialPeriodId;
                                    user.FinancialPeriod = FinancialPeriod.Year;
                                    user.FinancialPeriodFromDate = FinancialPeriod.DateFrom.Date;
                                    user.FinancialPeriodToDate = FinancialPeriod.DateTo.Date;
                                }
                                else
                                {
                                    user.FinancialPeriodId = LoginModel?.FinancialPeriodId;
                                    user.FinancialPeriod = DateTime.Now.Year;
                                    user.FinancialPeriodFromDate = DateTime.Now.Date;
                                    user.FinancialPeriodToDate = DateTime.Now.Date;
                                }
                            }
                                          

                            HttpContext.Session.SetCurrentUser("CurrentUser", user);

                            CompanyMobel companyModel = new CompanyMobel() { NameAr = "Smart Erp System" };
                            var company = _CompanyService.GetWithCondetion(x=>x.Id==user.CompanyId).FirstOrDefault();
                            if (company != null)
                            {
                                companyModel = _Mapper.Map<CompanyMobel>(company);
                                user.CompanyId = companyModel.CompanyId.HasValue? companyModel.CompanyId.Value:0;
                                EGInvoiceCredentials.Client_ID = company.Client_ID;
                                EGInvoiceCredentials.Client_Secret = company.Client_Secret;
                                EGInvoiceCredentials.TokenPassword = company.TokenPassword;
                            }
                            HttpContext.Session.SetCompanyData("CompanyData", companyModel);

                            var setings = _SystemSetingService.GetAll().FirstOrDefault();
                            if (setings != null)
                            {
                                EGInvoiceCredentials.E_InvoiceType = setings.E_InvoiceType;
                                EGInvoiceCredentials.IdentityServiceUrl = setings.IdentityService_Url;
                                EGInvoiceCredentials.SystemAPIUrl = setings.System_APIUrl;
                            }

                            return RedirectToAction("Dashpoard2", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", _LocalizationService.GetLocalizedHtmlString("InvalidUserMsg"));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("notactive", _LocalizationService.GetLocalizedHtmlString("NotActiveUser"));

                    }

                }
                else
                {
                  ModelState.AddModelError("notfound", _LocalizationService.GetLocalizedHtmlString("NotExistUser"));

                }

                IntializeDropdowens();

            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> LogOut(string returnUrl = null)
        {
            await _IdentityService.LogOut();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Login", "Account");
        }









    }
}
