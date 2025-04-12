using DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PresentationLayer.Filters
{
    public class AuthorizeAdminAttribute: TypeFilterAttribute
    {
        public AuthorizeAdminAttribute() : base(typeof(AuthorizeAdminFilter))
        {
        }  
    }

    public class AuthorizeAdminFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public AuthorizeAdminFilter(IHttpContextAccessor HttpContextAccessor)
        {
            _HttpContextAccessor = HttpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            //return to log in form if current user is null or user current user sessioin is end
            string CurrentUser = null;
            CurrentUser = _HttpContextAccessor.HttpContext.Session.GetString("CurrentUser");
            if (CurrentUser == null)
                filterContext.Result = new ChallengeResult();


        }

    }
}
