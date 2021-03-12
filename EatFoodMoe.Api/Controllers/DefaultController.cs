using IdentityServer4.Extensions;
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
            string BaseUrl = $"{Request.Scheme}://{Request.Host}";
            return new JsonResult( new { 
                message = "Welcoe to Qiafan-moe api.",
                file_infos = $"{BaseUrl}/api/fileInfos",
                openid_configuration = $"{HttpContext.GetIdentityServerBaseUrl()}/.well-known/openid-configuration"
            });
        }
    }
}
