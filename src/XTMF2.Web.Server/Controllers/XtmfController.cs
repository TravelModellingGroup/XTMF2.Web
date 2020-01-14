using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace XTMF2.Web.Controllers {
    /// <summary>
    /// API controller for methods and functions related to the XTMF runtime.
    /// </summary>
    [Route ("api/[controller]")]
    [ApiController]
    public class XtmfController : ControllerBase {
        private XTMFRuntime _xtmfRuntime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runtime"></param>
        public XtmfController (XTMFRuntime runtime) {
            this._xtmfRuntime = runtime;

        }
    }
}