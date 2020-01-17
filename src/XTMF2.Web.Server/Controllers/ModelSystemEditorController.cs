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

using Microsoft.AspNetCore.Mvc;

namespace XTMF2.Web.Server.Controllers
{
    /// <summary>
    ///     API controller for the management of model systems (meta). This controller does not contain endpoints for the
    ///     editing
    ///     of model systems.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ModelSystemEditorController : ControllerBase
    {
        private XTMFRuntime _xtmfRuntime;

        /// <summary>
        /// </summary>
        /// <param name="runtime"></param>
        public ModelSystemEditorController(XTMFRuntime runtime)
        {
            _xtmfRuntime = runtime;
        }

        /// <summary>
        /// </summary>
        /// <param name="project"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get()
        {
            return new OkResult();
        }
    }
}