using BusinessLayer.Helpers;
using BusinessLayer.Resources;
using BusinessLayer.Services;
using DataAccessLayer.Data;
using DataAccessLayer.Entities.Identity;
using DataAccessLayer.Reposetories;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PresentationLayer.Helpers;
using Reports;
using System;
using System.Globalization;
using System.Reflection;

namespace PresentationLayer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = 1024 * 20;
            });
            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            }).AddRazorRuntimeCompilation();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
           .AddDataAnnotationsLocalization(options =>
           {
               options.DataAnnotationLocalizerProvider = (type, factory) =>
               {
                   var assemblyName = new AssemblyName(typeof(SharedResources).GetTypeInfo().Assembly.FullName);
                   return factory.Create("SharedResources", assemblyName.Name);
               };

           });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                  new CultureInfo("ar"),
                  new CultureInfo("en"),
                };

                foreach (var culture in supportedCultures)
                {
                    culture.NumberFormat.NumberDecimalSeparator = ".";
                    culture.NumberFormat.NumberNegativePattern = 2;
                    culture.NumberFormat.NegativeSign = "-";
                }

                options.DefaultRequestCulture = new RequestCulture("ar");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            services.AddDbContext<AppDbContext>(Options =>
            {
                Options.UseSqlServer(Configuration.GetConnectionString("MyConnection"));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>
                (options =>
                {                    
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 2;
                    options.User.RequireUniqueEmail = true;
                }
                ).AddEntityFrameworkStores<AppDbContext>()  
                .AddDefaultTokenProviders();
        

            services.AddScoped(typeof(IBaseReposetory<>), typeof(BaseReposetory<>));
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped(typeof(IInventoryRepository<,>), typeof(InventoryRepository<,>));
            services.AddScoped(typeof(IInventoryService<,>), typeof(InventoryService<,>));   
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddSingleton(typeof(IPermissionProvider), typeof(StandardPermissionProvider));
            services.AddSingleton(typeof(LocalizationService));
            services.AddSingleton(typeof(SideMenuService)); 
            services.AddScoped(typeof(IidentityService), typeof(IdentityService));
            services.AddScoped(typeof(IWebHelper), typeof(WebHelper));
            services.AddScoped(typeof(IReportService), typeof(ReportService));
            services.AddScoped(typeof(IRerportsReposetory), typeof(RerportsReposetory));
            services.AddScoped(typeof(IRerportsBusinessService), typeof(RerportsBusinessService));
            services.AddScoped(typeof(IFinanceService<,>), typeof(FinanceService<,>));
            services.AddScoped(typeof(IFinanceRepository<,>), typeof(FinanceRepository<,>));
            services.AddScoped(typeof(IHelperRepository), typeof(HelperService));
            services.AddSingleton(typeof(AppHub));
            //services.AddScoped(typeof(ServiceResolver));

            services.AddSignalR();
            services.AddHttpContextAccessor();
            services.AddSession(options => {
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(60*10);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            //show activation page 
            app.RunActivationMiddleware();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            var requestlocalizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(requestlocalizationOptions.Value);
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=login}/{id?}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AppHub>("/AppHub");
            });
        }
    }
}
