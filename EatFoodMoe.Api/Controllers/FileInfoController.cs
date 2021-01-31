using EatFoodMoe.Api.Data;
using EatFoodMoe.Api.Entitles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EatFoodMoe.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class FileInfoController : ControllerBase
    {
        private readonly FileDbContext _dbContext;

        public FileInfoController(FileDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("fileinfo")]
        public async Task<ActionResult<EatFoodFile>> GetFileInfo([Required] string id)
        {
            Guid guid = new Guid(id);
            EatFoodFile fileInfo = await _dbContext.EatFoodFiles.FirstOrDefaultAsync(f => f.Id.Equals(guid));
            if (fileInfo is null)
            {
                return NotFound();
            }
            return fileInfo;
        }

        [HttpGet("fileinfo")]
        public async Task<ActionResult<EatFoodFile>> UpdateFileInfo([Required] string id, [FromQuery] EatFoodFile eatFoodFile)
        {
            Guid guid = new Guid(id);
            EatFoodFile fileInfo = await _dbContext.EatFoodFiles.FirstOrDefaultAsync(f => f.Id.Equals(guid));
            if (fileInfo is null)
            {
                return NotFound();
            }
            fileInfo.Translation = eatFoodFile.Translation;
            fileInfo.Embellishment = eatFoodFile.Embellishment;
            fileInfo.Proofreading = eatFoodFile.Proofreading;
            fileInfo.Description = eatFoodFile.Description;
            fileInfo.LastModityTIme = DateTime.UtcNow;
            _dbContext.Update(fileInfo);
            _ = await _dbContext.SaveChangesAsync();
            return fileInfo;
        }
    }
}
