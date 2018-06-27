using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orion.CRM.WebApp.App_Data;

namespace Orion.CRM.WebApp
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
            // Use Server memory cache.
            services.AddMemoryCache();

            // Adds services required for using options.
            services.AddOptions();

            // Register the IConfiguration instance which MyOptions binds against.
            services.Configure<AppConfig>(Configuration);

            // Add framework services.
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(AuthorizationFilter));
                options.Filters.Add(typeof(MenuFilter));
            });


            // Adds a default in-memory implementation of IDistributedCache.
            //services.AddDistributedMemoryCache();

            // Add Session services.
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });

            services.Configure<IISOptions>(options => {
            });

            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = "localhost:6379";
            //});

            // Use Memcached.
            //services.AddEnyimMemcached(options => Configuration.GetSection("enyimMemcached").Bind(options));
            //services.AddEnyimMemcached(x => x.Servers.Add(new Enyim.Caching.Configuration.Server() { Address = "127.0.0.1", Port = 11211 }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSession();

            if (env.IsDevelopment()) {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Resource}/{action=List}/{id?}");
            });

        }
    }
}
