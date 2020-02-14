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

using System;
using AutoMapper;
using XTMF2.ModelSystemConstruct;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Mapping.Converters;

namespace XTMF2.Web.Server.Mapping.Profiles
{
    /// <summary>
    ///     AutoMapper profile
    /// </summary>
    public class ModelSystemProfile : Profile
    {
        /// <summary>
        ///     Create mapping profiles for model system editor objects
        /// </summary>
        private void MapEditorProfile()
        {
            CreateMap<ModelSystem, ModelSystemEditingModel>();
            CreateMap<Boundary, BoundaryModel>().BeforeMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
            }).AfterMap((src, dest) =>
            {
                // empty
            });
            CreateMap<Link, LinkModel>().BeforeMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
            }).AfterMap((src, dest) =>
            {
                // empty
            }).ConvertUsing(typeof(Converters.LinkConverter))
            ;
            CreateMap<MultiLink, MultiLinkModel>().BeforeMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
            }).AfterMap((src, dest) =>
            {
                // empty
            });
            CreateMap<SingleLink, SingleLinkModel>()
            .BeforeMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
            })
            .AfterMap((src, dest) =>
            {
                dest.OriginHookId = dest.OriginHook.Id;
                dest.OriginId = dest.Origin.Id;
            });
            CreateMap<Start, StartModel>().BeforeMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
            }).AfterMap((src, dest) =>
            {
                // empty
            });
            CreateMap<Node, NodeModel>().ForMember(m => m.ContainedWithin, opt =>
            {
                opt.MapFrom(x => x.ContainedWithin);
            })
            .BeforeMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
            })
            .AfterMap((src, dest) =>
            {
                dest.ContainWithinId = dest.ContainedWithin.Id;
            });
            CreateMap<NodeHook, NodeHookModel>().BeforeMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
            }).AfterMap((src, dest) =>
            {
                // empty

            });
            CreateMap<CommentBlock, CommentBlockModel>().BeforeMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
            }).AfterMap((src, dest) =>
            {
                // empty
            });
            CreateMap<Rectangle, Data.Types.Rectangle>();
        }

        public ModelSystemProfile()
        {
            CreateMap<ModelSystemHeader, ModelSystemModel>();
            MapEditorProfile();
        }
    }
}