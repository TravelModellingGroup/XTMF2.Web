using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XTMF2.Web.Data.Interfaces;

namespace XTMF2.Web.Controllers
{
    /// <summary>
    /// API controller for the management of model systems (meta). This controller does not contain endpoints for the editing
    /// of model systems.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ModelSystemController : ControllerBase
    {
        /// <summary>
        /// Creates a new model system from the passed model.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(IModelSystem project) {

            return new OkResult();
        }

        /// <summary>
        /// Deletes the passed model system.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult Delete(IModelSystem project) {
            return new OkResult();
        }
    }
}