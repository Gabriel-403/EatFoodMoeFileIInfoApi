using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EatFoodMoe.Api.Entitles
{
    public class HomeCarrouselInfo
    {
        public string FileName { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
