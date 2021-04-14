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
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public async Task<ActionResult<UserMessage>> GetUserMessage()
        {
          

            var userMessage = await _fileDbContext.UserMessages.ToListAsync();
            if (userMessage is null)
            {
                return NotFound();
            }

            return  Ok(userMessage.Select(u =>new
            {   id=u.Id,
                author = u.UserName,
                content= u.Content,
                datatime=u.LastEditTime,
                avatar= "https://camo.githubusercontent.com/c26f325fa8d482c5dfbc30be326e4ce27ac2ea70e1db3facf269bcb40de16a11/68747470733a2f2f73312e617831782e636f6d2f323032302f30392f30382f774d723373782e74682e6a7067"
            }));
        }

        [HttpPost("userMessage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserMessage>> AddUserMessage([Required] [FromForm]string content)
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