using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XTMF2.Web.Pages.Projects
{
    public class NewProjectModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must enter a valid project name.")]
        public string ProjectName { get; set; }
    }
}
