using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatFoodMoe.Api.Entitles
{
    public class SinicizationGroup
    {
        public SinicizationGroup()
        {
            Projects = new List<Project>();
        }

        public ICollection<Project> Projects { get; set; }
    }
}
