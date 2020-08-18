using EatFoodMoe.Api.Entitles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace EatFoodMoe.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class FilesController:ControllerBase
    {
        [HttpPost("file")]
        public IActionResult UpLoad([FromForm] EatFoodIFilesInfo eatFoodIFilesInfo) 
        {
             Stream a=eatFoodIFilesInfo.File.OpenReadStream();
            return Ok();
            
        }
        

    }
}
