using BlazorStrap;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Client.Api;
namespace XTMF2.Web.Client {
    public class Startup {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices (IServiceCollection services) {
            services.AddSingleton (typeof (AuthenticationClient));
            services.AddSingleton (typeof (ProjectClient));
            services.AddSingleton (typeof (ModelSystemClient));
            services.AddLogging (builder => {
                builder.SetMinimumLevel (LogLevel.Trace);
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configure (IComponentsApplicationBuilder app) {
            app.AddComponent<App> ("app");
        }
    }
}