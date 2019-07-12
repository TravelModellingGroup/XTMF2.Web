
import { XTMF2Window } from '../XTMF2.Web.Components'

declare let window: XTMF2Window;
declare var $: any;

window.XTMF2.showModal = function (modalId) { 
    $(`#${modalId}`).modal('show') 
} 

window.XTMF2.hideModal = function (modalId) { 
    //hgwello
    $(`#${modalId}`).modal('hide')  
}  
