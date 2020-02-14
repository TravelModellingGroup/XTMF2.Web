
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace XTMF2.Web.Server.Authorization
{

    /// <summary>
    /// 
    /// </summary>
    public class ModelSystemAuthorizationHandler : AuthorizationHandler<ModelSystemAccessRequirement>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ModelSystemAccessRequirement requirement)
        {
            var user = context.User;
            return Task.CompletedTask;
        }
    }
}