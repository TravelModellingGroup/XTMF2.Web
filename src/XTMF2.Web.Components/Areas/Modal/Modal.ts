
// import { XTMF2Window } from '../XTMF2.Web.Components'

// declare let window: XTMF2Window;
declare var $: any;

let showModal = function(modalId: string): void { 
    $(`#${modalId}`).modal('show') 
} 

let hideModal = function (modalId: string) { 
    $(`#${modalId}`).modal('hide')  
}  

export {showModal, hideModal};