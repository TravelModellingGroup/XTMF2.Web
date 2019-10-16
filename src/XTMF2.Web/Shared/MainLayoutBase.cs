
using Microsoft.AspNetCore.Components;

namespace XTMF2.Web.Components {

    public class MainLayoutBase : LayoutComponentBase {

        [Parameter]
        public RenderFragment Alert {get;set;}


        public void ClearAlert() {
            Alert = null;
        }
        
    }
}