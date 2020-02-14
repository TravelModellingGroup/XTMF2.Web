
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using XTMF2.Web.Server.Session;

namespace XTMF2.Web.Server.Authorization
{

    /// <summary>
    /// 
    /// </summary>
    public class ModelSystemAccessAuthorizationHandler : AuthorizationHandler<ModelSystemAccessRequirement>
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public ModelSystemAccessAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ModelSystemAccessRequirement requirement)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            var routedata = httpContext.GetRouteData();
            var userSession = httpContext.RequestServices.GetService<UserSession>();
            var projectSession = httpContext.RequestServices.GetService<ProjectSessions>();

            var project = projectSession.Sessions[userSession.User].Find(p => p.Project.Name == (string)routedata.Values["projectName"]);
            // projectSession.Sessions[userSession.User]
            var user = context.User;
            return Task.CompletedTask;
        }
    }
}