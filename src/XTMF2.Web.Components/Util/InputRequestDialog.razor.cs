//    Copyright 2017-2019 University of Toronto
// 
//    This file is part of XTMF2.
// 
//    XTMF2 is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
// 
//    XTMF2 is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
// 
//    You should have received a copy of the GNU General Public License
//    along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.ComponentModel.DataAnnotations;
using BlazorStrap;
using Microsoft.AspNetCore.Components;

namespace XTMF2.Web.Components.Util
{
    /// <summary>
    /// A reusable input dialog / modal that collects exactly 1 string of input data.
    /// </summary>
    public partial class InputRequestDialog
    {
        /// <summary>
        /// The title of the modal displayed.
        /// </summary>
        [Parameter]
        public string DialogTitle { get; set; }

        /// <summary>
        /// Input (field) name
        /// </summary>
        [Parameter]
        public string InputName { get; set; }

        /// <summary>
        /// Callback when a valid input submission was made.
        /// </summary>
        [Parameter]
        public EventCallback<string> OnSubmit { get; set; }

        protected InputModel InputModel = new InputModel();

        private BSModal InputModal;

        /// <summary>
        ///     Attempts to create a new project on submission of the new project form.
        /// </summary>
        protected void OnValidSubmit()
        {
            OnSubmit.InvokeAsync(InputModel.Value);
            InputModal.Hide();
        }

        /// <summary>
        /// Shows the input modal.
        /// </summary>
        public void Show()
        {
            InputModal.Show();
        }

        /// <summary>
        /// Hides the input modal.
        /// </summary>
        public void Hide()
        {
            InputModal.Hide();
        }
    }

    /// <summary>
    /// Input model for dialo.
    /// </summary>
    public class InputModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must enter a valid input.")]
        public string Value { get; set; }
    }
}