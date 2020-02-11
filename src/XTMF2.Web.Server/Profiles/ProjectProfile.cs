using AutoMapper;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Server.Profiles
{
    /// <summary>
    ///     AutoMapper profile
    /// </summary>
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectModel>();
        }
    }
}