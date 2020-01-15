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
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private XTMFRuntime _xtmfRuntime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runtime"></param>
        public ProjectController(XTMFRuntime runtime){
            this._xtmfRuntime = runtime;

        }

        /// <summary>
        /// Creates a new project from the model passed.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(IProject project) {

            return new OkResult();
        }

        [HttpGet("{projectName}")]
        public ActionResult Get(string projectName) {
            return new OkResult();
        }

        [HttpGet()]
        public ActionResult List() {
            return new OkResult();
        }

        /// <summary>
        /// Deletes the specified project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult Delete(IProject project) {
            return new OkResult();
        }
    }
}