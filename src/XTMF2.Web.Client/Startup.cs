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

using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Client.Api;

namespace XTMF2.Web.Client
{
    /// <summary>
    /// Startup for client Blazor
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configure services to be used.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(AuthenticationClient));
            services.AddSingleton(typeof(ProjectClient));
            services.AddSingleton(typeof(ModelSystemClient));
            services.AddLogging(builder => { builder.SetMinimumLevel(LogLevel.Trace); });
        }

        /// <summary>
        /// Configure application components.
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}