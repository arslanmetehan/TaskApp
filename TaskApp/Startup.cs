using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskApp.Persistence;
using TaskApp.Services;

namespace TaskApp
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
            services.AddControllersWithViews();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.Name = ".MvcExample.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();

            //services.AddAntiforgery(options => options.HeaderName = "X-XSRF-Token");

            services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            /*.AddRazorPagesOptions(o =>
			{
				o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
			});
			*/

            services.AddSingleton<IServices, ServiceContainer>();

            services.AddSingleton<IUserRepository, Persistence.Dapper.UserRepository>();
            services.AddSingleton<IForumPostRepository, Persistence.Dapper.ForumPostRepository>();
            services.AddSingleton<IMissionRepository, Persistence.Dapper.MissionRepository>();
            services.AddSingleton<IOperationRepository, Persistence.Dapper.OperationRepository>();
            services.AddSingleton<ILogRepository, Persistence.Dapper.LogRepository>();


            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IMissionService, MissionService>();
            services.AddSingleton<IOperationService, OperationService>();
            services.AddSingleton<IViewService, ViewService>();
            services.AddSingleton<IForumPostService, ForumPostService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseSession();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
