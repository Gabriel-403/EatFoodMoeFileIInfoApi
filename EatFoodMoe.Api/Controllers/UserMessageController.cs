using System;
using EatFoodMoe.Api.Data;
using EatFoodMoe.Api.Entitles;
using EatFoodMoe.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace EatFoodMoe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserMessageController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly FileDbContext _fileDbContext;

        public UserMessageController(UserManager<AppUser> userManager, FileDbContext fileDbContext)
        {
            _userManager = userManager;
            _fileDbContext = fileDbContext;
        }

        [HttpGet("userMessage")]
        public async Task<ActionResult<UserMessage>> GetUserMessage([Required] string id)
        {
            if (Guid.TryParse(id, out Guid messageId) is false)
            {
                return Problem();
            }

            UserMessage userMessage = await _fileDbContext.UserMessages.FindAsync(messageId);
            if (userMessage is null)
            {
                return NotFound();
            }

            return userMessage;
        }

        [HttpPost("userMessage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserMessage>> AddUserMessage([Required] string content)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                throw new NullReferenceException(nameof(user));
            }

            var userMessage = new UserMessage
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                UserName = user.UserName,
                Content = content,
                LastEditTime = DateTimeOffset.Now
            };

            await _fileDbContext.UserMessages.AddAsync(userMessage);

            _ = await _fileDbContext.SaveChangesAsync();
            return userMessage;
        }

        
        [HttpDelete("userMessage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserMessage>> DeleteMessage([Required] string id)
        {
            if (Guid.TryParse(id, out Guid messageId) is false)
            {
                return Problem();
            }

            UserMessage userMessage = await _fileDbContext.UserMessages.FindAsync(messageId);
            if (userMessage is null)
            {
                return NotFound();
            }

            _fileDbContext.UserMessages.Remove(userMessage);

            _ = await _fileDbContext.SaveChangesAsync();
            return userMessage;
        }
    }
}