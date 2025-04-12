using BusinessLayer.Models.System;
using DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PresentationLayer.Helpers
{
    public static class SessionExtensions
    {       

        public static ApplicationUser GetCurrentUser<ApplicationUser>(this ISession session, string key)
        {

            var data = session.GetString(key);
            if (data == null)
            {
                return default(ApplicationUser);
            }
            return JsonConvert.DeserializeObject<ApplicationUser>(data);
        }

        public static void SetCurrentUser(this ISession session, string key, ApplicationUser value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static void SetCompanyData(this ISession session, string key, CompanyMobel CompanyMobel)
        {
            session.SetString(key, JsonConvert.SerializeObject(CompanyMobel));
        }
        public static CompanyMobel GetCompanyData<CompanyMobel>(this ISession session, string key)
        {

            var data = session.GetString(key);
            if (data == null)
            {
                return default(CompanyMobel);
            }
            return JsonConvert.DeserializeObject<CompanyMobel>(data);
        }

    }
}
