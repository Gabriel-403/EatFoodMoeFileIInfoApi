using EatFoodMoe.Api.Entitles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatFoodMoe.Api.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FilesController:ControllerBase
    {
        [HttpPost]
        public IActionResult UpLoad([FromForm] EatFoodIFilesInfo eatFoodIFilesInfo) 
        {

            return Ok();
        }


    }
}
