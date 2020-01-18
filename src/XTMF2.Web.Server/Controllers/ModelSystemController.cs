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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XTMF2.Web.Data.Interfaces;

namespace XTMF2.Web.Server.Controllers
{
    /// <summary>
    ///     API controller for the editing of model systems.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ModelSystemController : ControllerBase
    {
        private XTMFRuntime _xtmfRuntime;

        /// <summary>
        /// </summary>
        /// <param name="runtime"></param>
        public ModelSystemController(XTMFRuntime runtime)
        {
            _xtmfRuntime = runtime;
        }

        /// <summary>
        ///     Creates a new model system from the passed model.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Create(IModelSystem project)
        {
            return new OkResult();
        }

        /// <summary>
        ///     Deletes the passed model system.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult Delete(IModelSystem project)
        {
            return new OkResult();
        }

        /// <summary>
        /// </summary>
        /// <param name="project"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{projectName}/{modelSystemName}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ModelSystemHeader> Get(string projectName, string modelSystemName)
        {
            return new OkResult();
        }
    }
}