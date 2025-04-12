using DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace PresentationLayer.Filters
{
    /// <summary>
    /// لمعرفة حالة السنة المالية الحالية إذا كانت مغلقة أو مفتوحة ومنع الحركات عليها إذا كانت مغلقة نهائيا
    /// </summary>
    public class ValidateFinancePeriodStateFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public ValidateFinancePeriodStateFilter(IHttpContextAccessor HttpContextAccessor)
        {
            _HttpContextAccessor = HttpContextAccessor;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var currentUser = _HttpContextAccessor.HttpContext.Session.GetString("CurrentUser");          

            if (currentUser != null)
            {
                var user = JsonSerializer.Deserialize<ApplicationUser>(currentUser);
                if (user.FinancialPeriodId.HasValue)
                {
                    var controllerName = _HttpContextAccessor.HttpContext.Request.RouteValues["controller"] as string;
                    var returnUrl = $"/{controllerName}/Index";
                    
                    _HttpContextAccessor.HttpContext.Session.SetString("ClosedFinancePeriodMsg", "TheCurruntFinancePeriodClosed");
                    context.Result = new RedirectResult(returnUrl);
                }
            }
            else
            {
                var controllerName = _HttpContextAccessor.HttpContext.Request.RouteValues["controller"] as string;
                var returnUrl = $"/{controllerName}/Index";
                context.Result = new RedirectResult(returnUrl);
            }
        }
    }
}
