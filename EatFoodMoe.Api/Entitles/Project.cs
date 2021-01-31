using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatFoodMoe.Api.Entitles
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string GameName { get; set; }
        public string MemberNames { get; set; }
        public int FileCount { get; set; }

        public List<EatFoodFile> Files { get; set; }

        public Guid GroupId { get; set; }

        public SinicizationGroup Group { get; set; }
    }
}
