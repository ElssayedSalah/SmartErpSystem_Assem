using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helpers
{
   public static class ServiceResolver
    {
        //private static readonly IServiceProvider _serviceProvider;
        static ServiceResolver()
        {
            //_serviceProvider = serviceProvider;
        }

        public static T GetService<T>(IServiceProvider serviceProvider)
        {
            try
            {
                var service = (T)serviceProvider.GetService(typeof(T));
                return service;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception($"Service  not found.", ex);
            }
        }

    }
}
