//    Copyright 2017-2019 University of Toronto
// 
//    This file is part of XTMF2.
// 
//    XTMF2 is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
// 
//    XTMF2 is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
// 
//    You should have received a copy of the GNU General Public License
//    along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

using System.Text;
using BlazorQuery.Extensions;
using BlazorStrap;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using XTMF2.Web.Data;
using XTMF2.Web.Services;

namespace XTMF2.Web {
	public class Startup {
		/// <summary>
		/// </summary>
		/// <param name="configuration"></param>
		public Startup (IConfiguration configuration) {
			Configuration = configuration;
		}
		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices (IServiceCollection services) {
			services.AddRazorPages ();
			services.AddServerSideBlazor ();
			services.AddBlazorQuery ();
			services.AddBootstrapCSS ();
			services.AddSingleton (XTMFRuntime.CreateRuntime ());
			services.AddScoped (provider => {
				var runtime = provider.GetService<XTMFRuntime> ();
				return runtime.UserController.GetUserByName ("local");
			});

			//configure the automapping sercices
			ConfigureAutoMapping (services);

			//configure the authentication and authorization services
			services.AddScoped<AuthenticationStateProvider, XtmfAuthStateProvider> ();
			services.AddIdentity<User, string> ().AddUserStore<XtmfUserStore<User>> ()
				.AddRoleStore<XtmfRoleStore<string>> ().AddSignInManager<XtmfSignInManager<User>> ();

			// add authentication services
			services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer (options => {
					options.TokenValidationParameters = new TokenValidationParameters {
						ValidateIssuer = true,
							ValidateAudience = true,
							ValidateLifetime = true,
							ValidateIssuerSigningKey = true,
							ValidIssuer = Configuration["JwtIssuer"],
							ValidAudience = Configuration["JwtAudience"],
							IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (Configuration["JwtSecurityKey"]))
					};
				});
		}

		/// <summary>
		///     Creates the automapping configuration between various entities.
		/// </summary>
		/// <param name="services"></param>
		private void ConfigureAutoMapping (IServiceCollection services) {
			var dataAutoMapper = new DataAutoMapper ();
			services.AddSingleton (dataAutoMapper);
			services.AddSingleton (dataAutoMapper.Configuration.CreateMapper ());
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment ()) {
				app.UseDeveloperExceptionPage ();
			} else {
				app.UseExceptionHandler ("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts ();
			}
			app.UseHttpsRedirection ();
			app.UseStaticFiles ();
			app.UseRouting ();
			app.UseEndpoints (endpoints => {
				endpoints.MapBlazorHub ();
				endpoints.MapFallbackToPage ("/_Host");
			});

			//enable authentication and authorization
			app.UseAuthentication ();
			app.UseAuthorization ();
		}
	}
}