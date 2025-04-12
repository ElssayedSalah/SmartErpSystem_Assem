using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Middlewares
{
    public class ActivationMiddleware
    {
        private readonly RequestDelegate _next;

        public ActivationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var ToDate = new DateTime(2040,12,30);
            if (DateTime.Now.Date>= ToDate)
            {
                context.Response.Redirect("/Activation.html");
            }
            else
            {
                await _next(context);
            }
       
        }
    }
}
