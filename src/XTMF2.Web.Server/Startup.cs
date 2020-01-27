//     Copyright 2017-2020 University of Toronto
// 
//     This file is part of XTMF2.
// 
//     XTMF2 is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     XTMF2 is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using XTMF2.Web.Server.Services;
using XTMF2.Web.Server.Services.Interfaces;

namespace XTMF2.Web.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(XTMFRuntime.CreateRuntime());
            services.AddSingleton(Configuration);
            services.AddScoped(provider =>
            {
                var runtime = provider.GetService<XTMFRuntime>();
                return runtime.UserController.GetUserByName("local");
            });

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] {"application/octet-stream"});
            });
            services.AddMvcCore()
                .AddApiExplorer();

            services.AddLogging(builder => { builder.SetMinimumLevel(LogLevel.Trace); });
            //configure the automapping services
            ConfigureAutoMapping(services);
            services.AddHttpContextAccessor();
            services.AddAuthorization();
            //configure the authentication and authorization services
            services.AddScoped<AuthenticationStateProvider, XtmfAuthStateProvider>();
            services.AddIdentity<User, string>().AddUserStore<XtmfUserStore<User>>()
                .AddRoleStore<XtmfRoleStore<string>>().AddSignInManager<XtmfSignInManager<User>>();

            services.AddScoped(typeof(IAuthenticationService), typeof(AuthenticationService));

            services.AddScoped(providers =>
            {
                /* This section of code is commented out temporarily until some further changes are mae on the client */
                /*
                var context = (IHttpContextAccessor) providers.GetService(typeof(IHttpContextAccessor));
                var userManager = (UserManager<User>) providers.GetService(typeof(UserManager<User>));
                var user = userManager.FindByNameAsync(context.HttpContext.User.Claims.FirstOrDefault()?.Value); */
                return ((XTMFRuntime) providers.GetService(typeof(XTMFRuntime))).UserController.Users.FirstOrDefault();
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;

                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtSecurityKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddOpenApiDocument(document =>
            {
                document.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                document.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            IdentityModelEventSource.ShowPII = true;
        }

        /// <summary>
        ///     Creates the automapping configuration between various entities.
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureAutoMapping(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
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
            app.UseClientSideBlazorFiles<Client.Startup>();
            app.UseRouting();
            app.UseAuthorization();
            //enable authentication and authorization
            app.UseAuthentication();
            app.UseBlazorDebugging();
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapBlazorHub ();
                // endpoints.MapFallbackToPage ("/_Host");
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
            });
        }
    }
}