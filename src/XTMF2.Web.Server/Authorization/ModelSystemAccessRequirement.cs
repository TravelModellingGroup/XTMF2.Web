using Microsoft.AspNetCore.Authorization;

namespace XTMF2.Web.Server.Authorization
{
    public class ModelSystemAccessRequirement : IAuthorizationRequirement
    {
        public const string REQUIREMENT_NAME = "ModelSystemAccessRequirement";
        public ModelSystemAccessRequirement()
        {
        }
    }
}