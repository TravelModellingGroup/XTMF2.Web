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
using System.Collections.Generic;
using System.Text.Json.Serialization;
using XTMF2.Web.Data.Interfaces.Editing;
using XTMF2.Web.Data.Types;

namespace XTMF2.Web.Data.Models.Editing
{
    public class BoundaryModel : ViewObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BoundaryModel> Boundaries { get; set; }
        public List<NodeModel> Modules { get; set; }
        public List<CommentBlockModel> CommentBlocks { get; set; }
        public List<LinkModel> Links { get; set; }
        public List<StartModel> Starts { get; set; }
    }
}