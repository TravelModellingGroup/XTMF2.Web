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
    public class ModelSystemEditorController : ControllerBase
    {
        private XTMFRuntime _xtmfRuntime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runtime"></param>
        public ModelSystemEditorController (XTMFRuntime runtime) {
            this._xtmfRuntime = runtime;

        }


    }
}