using AutoMapper;
using BusinessLayer.Models.Financial;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace PresentationLayer.Controllers.Financial
{
    public class ChartOfAccountSettingsController : BaseAdminController
    {
        private readonly IBaseService<ChartOfAccountSettings> _ChartOfAccountSettingsService;
        private readonly IBaseService<AccountSetting> _AccountSettingService;  
        private readonly IMapper _Mapper;

        public ChartOfAccountSettingsController(IBaseService<ChartOfAccountSettings> ChartOfAccountSettingsService, IBaseService<AccountSetting> AccountSettingService, IMapper mapper)
        {
            _ChartOfAccountSettingsService = ChartOfAccountSettingsService;
            _AccountSettingService = AccountSettingService;
            _Mapper = mapper;
        }

        public IActionResult Create()
        {
            
            var model = new ChartOfAccountSettingsModel();
           var settings= _ChartOfAccountSettingsService.GetAll().FirstOrDefault();
           var settingsDetails= _AccountSettingService.GetAll();
            if (settings!=null)
            {
                ChartOfAccountSettingsModel ChartOfAccountSettingsModel = _Mapper.Map<ChartOfAccountSettingsModel>(settings);
                model = ChartOfAccountSettingsModel;
                if (settingsDetails!=null && settingsDetails.Count>0)
                {
                    foreach (var item in settingsDetails)
                    {
                        AccountSettingModel AccountSettingModel = _Mapper.Map<AccountSettingModel>(item);
                        model.AccountSettingsModel.Add(AccountSettingModel);

                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(ChartOfAccountSettingsModel model)
        {
            ChartOfAccountSettings ChartOfAccountSettings = _Mapper.Map<ChartOfAccountSettings>(model);
            _ChartOfAccountSettingsService.Add(ChartOfAccountSettings);
            var AccountSettings = model.AccountSettingsModel.Skip(1).ToList();
            foreach (var item in AccountSettings)
            {
                AccountSetting AccountSetting = _Mapper.Map<AccountSetting>(item);
                _AccountSettingService.Add(AccountSetting);

            }
            return View(model);
        }
    }
}
