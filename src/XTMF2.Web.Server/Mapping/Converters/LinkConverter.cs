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

using AutoMapper;
using XTMF2.ModelSystemConstruct;
using XTMF2.Web.Data.Models.Editing;

namespace XTMF2.Web.Server.Mapping.Converters
{
    /// <summary>
    ///     Maps Links to the appropriate link type
    /// </summary>
    public class LinkConverter : ITypeConverter<Link, LinkModel>
    {
        /// <summary>
        ///     Converts the Link to the appropriate type
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public LinkModel Convert(Link source, LinkModel destination, ResolutionContext context)
        {
            if (source is MultiLink)
            {
                return context.Mapper.Map<MultiLinkModel>(source);
            }

            return context.Mapper.Map<SingleLinkModel>(source);
        }
    }
}