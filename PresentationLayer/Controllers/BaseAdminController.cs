using DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Filters;
using PresentationLayer.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace PresentationLayer.Controllers
{
    [AuthorizeAdmin]
    [Authorize]
    public class BaseAdminController : Controller
    {
        public BaseAdminController()
        {           
        }
        public void CreateSelectList(string DefaultText, List<SelectListItem> source, string ViewBagKey,
           bool AddSelect = true,  BusinessLayer.Models.Model model = null)
        {
            var lst = new List<SelectListItem>();
            if (AddSelect)
                lst.Add(new SelectListItem() { Text = DefaultText, Value = "" });
            lst.AddRange(source);
            bool AllowSetSelected = model != null &&  string.IsNullOrEmpty(model.Id.ToString());
            if (AllowSetSelected)
            {
                var selectedItem = lst.FirstOrDefault(x => x.Value == model.Id.ToString());
                if (selectedItem != null)
                    selectedItem.Selected = true;
            }
            ViewData[ViewBagKey] = lst;

        }
        public ApplicationUser CurrentUser
        {
            get
            {                
                var user = HttpContext.Session.GetCurrentUser<ApplicationUser>("CurrentUser");
                return user;
            }
            set {;}
        }
        //public ApplicationUser CurrentUser
        //{
        //    get
        //    {
        //        var user = HttpContext.User;

        //        var applicationUser = _identityService.GetUserByUserName(user.Identity.Name);
        //        return applicationUser;
        //    }
        //    set {; }
        //}

        protected virtual IActionResult AccessDeniedView()
        { 
            var rawUrl = Request.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;
            if (string.IsNullOrEmpty(rawUrl))
                rawUrl = $"{Request.PathBase}{Request.Path}{Request.QueryString}";

            return RedirectToAction("AccessDenied", "Permissions", new { pageUrl = rawUrl });
        }
        public IActionResult AccessDeniedJson(string msg)
        {
            return Json(msg);
        }
        protected virtual IActionResult ReLogin()
        {          
            
            if (CurrentUser==null)
            {
                return RedirectToAction("login", "Account");
            }
            else
            {
                var  rawUrl = $"{Request.PathBase}{Request.Path}{Request.QueryString}";
               return RedirectToRoute(rawUrl);
            }
        }

    }
}
