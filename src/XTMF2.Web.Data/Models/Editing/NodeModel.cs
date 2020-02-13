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
using System.Text.Json.Serialization;
using XTMF2.Web.Data.Interfaces.Editing;

namespace XTMF2.Web.Data.Models.Editing
{
    public class NodeModel : ViewObject, INode
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // [JsonIgnore]
        //public IBoundary ContainedWithin { get; set; }

        public Type Type { get; set; }
    }
}