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

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;
using XTMF2.Web.Client.Services;
using XTMF2.Web.Client.Services.Api;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Http.Connections.Client;

namespace XTMF2.Web.Client
{
    /// <summary>
    ///     Main Program entry XTMF2.Web.Client
    /// </summary>
    public class Program
    {
        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        public async static Task Main(string[] args)
        {
            ConfigureLogger();
            var builder = CreateHostBuilder(args);
            AddServices(builder.Services);
            builder.RootComponents.Add<App>("app");
            await builder.Build().RunAsync();
        }

        /// <summary>
        /// Configure client side services.
        /// </summary>
        /// <param name="services"></param>
        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<AuthenticationClient>();
            services.AddScoped<AuthenticationStateProvider, XtmfAuthenticationStateProvider>();
            services.AddScoped<ProjectClient>(provider =>
            {
                return new ProjectClient(provider.GetService<System.Net.Http.HttpClient>(),
                    (XtmfAuthenticationStateProvider)provider.GetService<AuthenticationStateProvider>());
            });
            services.AddAuthorizationCore();
            services.AddScoped<ModelSystemClient>();
            services.AddScoped<AuthenticationService>();
            services.AddLogging(builder => { builder.SetMinimumLevel(LogLevel.Trace); });
            services.AddBlazoredSessionStorage();
        }

        /// <summary>
        ///     Sets up logger for the browser
        /// </summary>
        private static void ConfigureLogger()
        {
            SelfLog.Enable(m => Console.Error.WriteLine(m));
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.BrowserConsole()
                .CreateLogger();
        }

        /// <summary>
        ///     Creates the blazor host builder.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static WebAssemblyHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            return builder;
        }
    }
}