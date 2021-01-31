using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatFoodMoe.Api.Entitles
{
    public class EatFoodFile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool Translation  { get; set; }
        public bool Embellishment { get; set; }
        public bool Proofreading { get; set; }
        public string Description { get; set; }
        public DateTimeOffset LastModityTIme { get; set; }
        public long FileSize { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
