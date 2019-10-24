using System.ComponentModel.DataAnnotations;

namespace XTMF2.Web.Views.Projects
{
    public class NewProjectModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must enter a valid project name.")]
        public string ProjectName { get; set; }
    }
}
