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

using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using XTMF2.ModelSystemConstruct;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Session;

namespace XTMF2.Web.Server.Mapping.Actions
{
    /// <summary>
    ///     Maps Links to the appropriate link type
    /// </summary>
    public class GenerateModelSystemObjectIdAction<TSrc, TDest> : IMappingAction<TSrc, TDest>
            where TDest : ViewObject
    {
        public void Process(TSrc source, TDest destination, ResolutionContext context)
        {
            destination.Id = System.Guid.NewGuid();
        }
    }
}