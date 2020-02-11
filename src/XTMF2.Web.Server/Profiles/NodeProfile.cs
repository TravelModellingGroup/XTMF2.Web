using AutoMapper;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Data.Models.Editing;

namespace XTMF2.Web.Server.Profiles
{
    /// <summary>
    ///     AutoMapper profile
    /// </summary>
    public class NodeProfile : Profile
    {
        public NodeProfile()
        {
            CreateMap<Node, NodeModel>();
        }
    }
}