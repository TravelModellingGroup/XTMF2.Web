
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using XTMF2;
using XTMF2.Editing;

namespace XTMF2.Web.Pages
{

    /// <summary>
    ///  Projects (page) base component.
    /// </summary>
    public class ProjectsBase : ComponentBase
    {

        [Inject]
        protected XTMF2.XTMFRuntime XTMFRuntime { get; set; }

        [Inject]
        protected XTMF2.User XTMFUser { get; set; }

        public List<XTMF2.Project> Projects { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ProjectsBase()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInit()
        {
            ProjectSession session = null;
            string error = "";
            Projects = new List<XTMF2.Project>();
            
            if(XTMFRuntime.ProjectController.CreateNewProject(XTMFUser, "Project" + Guid.NewGuid(),
            out session, ref error)) {

                Projects.AddRange(XTMFRuntime.ProjectController.GetProjects(XTMFUser));

            }


        }
    }

}