
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace XTMF2.Web.Components
{

    public class ModalBase : ComponentBase
    {

        [Inject]
        public IJSRuntime JsRuntime { get; set; }



        protected ElementRef modalElement;

        [Parameter]
        public RenderFragment Content { get; set; }


        [Parameter]
        public RenderFragment Title { get; set; }

        [Parameter]
        public string ModalName { get; set; }

        [Parameter]
        public string ConfirmText { get; set; } = "Confirm";

        protected bool IsShow { get; set; } = false;

        protected string ShowClass => IsShow ? "show" : "";

        [Parameter]
        private EventCallback<UIEventArgs> OnConfirm { get; set; }

        [Parameter]
        private EventCallback<UIEventArgs> OnCancel { get; set; }


        public async void Show()
        {
            await JsRuntime.InvokeAsync<object>("XTMF2.showModal", ModalName);
            this.IsShow = true;
        }

        public async void Hide()
        {
            await JsRuntime.InvokeAsync<object>("XTMF2.hideModal", ModalName);
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