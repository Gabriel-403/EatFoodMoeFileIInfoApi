using EatFoodMoe.Api.Data;
using EatFoodMoe.Api.Entitles;
using EatFoodMoe.Api.Models;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly FileDbContext _dbContext;

        public FileInfoController(UserManager<AppUser> userManager, FileDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet("fileinfos")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EatFoodFile>>> GetFileInfos()
        {
            
            var fileInfos = await _dbContext.EatFoodFiles.AsNoTracking()
                .Where(info => string.Equals(info.UserId, _userManager.GetUserId(User))).ToListAsync();
            return fileInfos;
        }


        [HttpGet("admin/fileinfos")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<EatFoodFile>>> SuperGetFileInfo()
        {
            var fileInfos = await _dbContext.EatFoodFiles.ToListAsync();
            return fileInfos;
        }

        [HttpGet("fileinfo")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<EatFoodFile>> GetFileInfo([Required] string id)
        {
            if (Guid.TryParse(id, out var fileInfoGuid) is false)
            {
                return Problem();
            }

            EatFoodFile fileInfo = await _dbContext.EatFoodFiles.FirstOrDefaultAsync(f => f.Id.Equals(fileInfoGuid));
            if (fileInfo is null)
            {
                return NotFound();
            }

            return fileInfo;
        }

        [HttpPut("fileinfo")]
        public async Task<ActionResult<EatFoodFile>> UpdateFileInfo([Required] string id,[FromQuery] EatFoodFile eatFoodFile)
        {
         
            if (Guid.TryParse(id, out var fileInfoGuid) is false)
            {
                return Problem();
            }

            EatFoodFile fileInfo = await _dbContext.EatFoodFiles.FirstOrDefaultAsync(f => f.Id.Equals(fileInfoGuid));
            if (fileInfo is null)
            {
                return NotFound();
            }
            fileInfo.UserId = _userManager.GetUserId(User);
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
