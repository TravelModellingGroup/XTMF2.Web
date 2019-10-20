using System;

namespace XTMF2.Web.Data.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class XtmfUser
    {
        /// <summary>
        /// Default no argument constructor.
        /// </summary>
        public XtmfUser()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="isAdmin"></param>
        public XtmfUser(string userName, bool isAdmin)
        {
            this.UserName = userName;
            this.IsAdmin = isAdmin;
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
