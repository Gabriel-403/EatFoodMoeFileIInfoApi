using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatFoodMoe.Api.Entitles
{
    public class EatFoodIFilesInfo
    {
        public string Title { get; set; }
        public IFormFile File { get; set; }
}
}
