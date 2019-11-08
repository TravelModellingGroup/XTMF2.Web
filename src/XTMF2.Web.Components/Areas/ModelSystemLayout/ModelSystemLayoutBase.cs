
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace XTMF2.Web.Components {

    public class ModelSystemLayoutBase : ComponentBase {


        [Inject]
        private ILogger Logger {get;set;}

        public ModelSystemLayoutBase() {

        }
        
    }
}