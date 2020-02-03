using AutoMapper;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Server.Profiles {

    /// <summary>
    /// AutoMapper profile
    /// </summary>
    public class ModelSystemProfile : Profile {

        public ModelSystemProfile () {
            CreateMap<ModelSystemHeader, ModelSystemModel> ();
        }
    }
}