using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatFoodMoe.Api.Controllers
{
    [Route("")]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return new JsonResult(new { message = "Welcoe to Qiafan-moe api." });
        }
    }
}
