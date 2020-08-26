using EatFoodMoe.Api.Entitles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using static System.IO.File;

namespace EatFoodMoe.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IHostEnvironment _environment;

        public FilesController(IHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("file")]
        public async Task<IActionResult> UpLoad([FromForm] EatFoodIFilesInfo eatFoodIFilesInfo)
        {
            var stream = eatFoodIFilesInfo.File.OpenReadStream();
            using var file = Create(Path.Combine(
                _environment.ContentRootPath, "wwwroot", eatFoodIFilesInfo.File.FileName));
            await stream.CopyToAsync(file);
            return Ok();
        }

        [HttpGet]
        public IActionResult Download([FromQuery][Required] string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return NotFound();
            }

            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot", fileName);
            var fs = new FileInfo(filePath);
            if (!Exists(filePath))
            {
                return NotFound();
            }
            var fileExtensionContentTypeProvider= new FileExtensionContentTypeProvider();
            if (fileExtensionContentTypeProvider.TryGetContentType(Path.GetExtension(filePath), out var contextType))
            {
                return PhysicalFile(filePath, contextType);
                //return new FileStreamResult(fs.OpenRead(),contextType);
            }

            return NotFound();
        }

        
    }
}
