using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorQuery.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorStrap;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using XTMF2.Web.Data;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Services;

namespace XTMF2.Web
{
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazorQuery();
            services.AddBootstrapCSS();
            services.AddSingleton<XTMF2.XTMFRuntime>(XTMF2.XTMFRuntime.CreateRuntime());
            services.AddScoped<XTMF2.User>((provider) =>
            {
                var runtime = provider.GetService<XTMF2.XTMFRuntime>();
                return runtime.UserController.GetUserByName("local");
            });

            //configure the automapping sercices
            this.ConfigureAutoMapping(services);

            //configure the authentication and authorization services
            services.AddScoped<AuthenticationStateProvider, XtmfAuthStateProvider>();
            services.AddIdentity<XtmfUser, string>().
                AddUserStore<XtmfUserStore<XtmfUser>>().
                AddRoleStore<XtmfRoleStore<string>>().
                AddSignInManager<XtmfSignInManager<XtmfUser>>();
        }

        /// <summary>
        /// Creates the automapping configuration between various entities.
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureAutoMapping(IServiceCollection services)
        {
            var dataAutoMapper = new DataAutoMapper();
            services.AddSingleton<DataAutoMapper>(dataAutoMapper);
            services.AddSingleton<IMapper>(dataAutoMapper.Configuration.CreateMapper());
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
