//    Copyright 2017-2019 University of Toronto
// 
//    This file is part of XTMF2.
// 
//    XTMF2 is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
// 
//    XTMF2 is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
// 
//    You should have received a copy of the GNU General Public License
//    along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

using AutoMapper;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Data
{
    /// <summary>
    /// </summary>
    public class DataAutoMapper
    {
        public DataAutoMapper()
        {
            ConfigureAutoMapper();
        }

        public MapperConfiguration Configuration { get; set; }

        private void ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, XtmfUser>()
                    .ForMember(d => d.User.UserName, o => o.MapFrom(s => s.UserName))
                    .ForMember(d => d.User.IsAdmin, o => o.MapFrom(s => s.IsAdmin));
            });
            Configuration = config;
        }
    }
}