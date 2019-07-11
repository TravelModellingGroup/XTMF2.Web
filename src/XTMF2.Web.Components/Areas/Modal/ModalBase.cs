
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace XTMF2.Web.Components
{

    public class ModalBase : ComponentBase
    {
        [Parameter]
        public RenderFragment Content { get; set; }


        [Parameter]
        public RenderFragment Title { get; set; }

        [Parameter]
        public string ConfirmText {get;set;} = "Confirm";

        protected bool IsShow { get; set; } = false;

        protected string ShowClass => IsShow ? "in" : "";

        [Parameter]
        private EventCallback<UIEventArgs> OnConfirm { get; set; }

        [Parameter]
        private EventCallback<UIEventArgs> OnCancel { get; set; }


        public void Show()
        {
            this.IsShow = true;
        }

        public void Confirm(UIEventArgs e)
        {
            this.IsShow = false;
            OnConfirm.InvokeAsync(e);
        }

        public void Cancel(UIEventArgs e)
        {
            this.IsShow = false;
            OnCancel.InvokeAsync(e);
        }
    }
}