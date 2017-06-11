﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orion.CRM.WebApp.App_Data;
using Orion.CRM.WebTools;
using Enyim.Caching;

namespace Orion.CRM.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

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
            services.AddMvc(options =>{
                options.Filters.Add(typeof(AuthorizationFilter)); // by type
                options.Filters.Add(typeof(MenuFilter)); // by type
                //options.Filters.Add(typeof(AnonymousFilter));// by type
                //options.Filters.Add(new SampleGlobalActionFilter()); // an instance
            });

            // Add Session services.
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.Configure<IISOptions>(options => { 
            });

            // Use Memcached.
            services.AddEnyimMemcached(options => Configuration.GetSection("enyimMemcached").Bind(options));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSession();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Resource}/{action=List}/{id?}");
            });

            app.UseEnyimMemcached();
        }
    }
}
