using AutoMapper;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Server.Profiles {

    public class ProjectProfile : Profile {

        public ProjectProfile () {
            CreateMap<Project, ProjectModel> ();
        }
    }
}