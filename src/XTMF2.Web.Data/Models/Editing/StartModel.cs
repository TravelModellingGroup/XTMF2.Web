using System;
using XTMF2.Web.Data.Interfaces.Editing;

namespace XTMF2.Web.Data.Models.Editing
{
    public class StartModel : ViewObject, IStart
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public IBoundary ContainedWithin { get; set; }

        public Type Type { get; set; }

    }
}