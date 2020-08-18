using EatFoodMoe.Api.Entitles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
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
        private readonly IContentTypeProvider _contentTypeProvider;

        public FilesController(IHostEnvironment environment, IContentTypeProvider contentTypeProvider)
        {
            _environment = environment;
            _contentTypeProvider = contentTypeProvider;
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
            if (!Exists(filePath))
            {
                return NotFound();
            }

            if (_contentTypeProvider.TryGetContentType(Path.GetExtension(filePath), out var contextType))
            {
                return File(new FileStream(filePath, FileMode.Open), contextType);
            }

            return NotFound();
        }
    }
}
