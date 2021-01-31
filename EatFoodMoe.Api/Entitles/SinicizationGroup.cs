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

        public Guid Id { get; set; }

        public string Nmae { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
