using EatFoodMoe.Api.dto;
using EatFoodMoe.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatFoodMoe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AppUser>> GetUser(string userName)
        {
            var result = this.User.IsInRole("admin");
            var user = await _userManager.FindByNameAsync(userName);
            return Ok(user);
        }

        [HttpPost("role")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AppUser>> AddUserToRole(string userName, string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (roleExist is false)
            {
                var identityResult = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
                if (identityResult.Succeeded is false)
                {
                    return NotFound();
                }
            }

            var user = await _userManager.FindByNameAsync(userName);
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded is false)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AppUser>> CreateUser([FromForm] CreateUserDto dto)
        {
            var result = await _userManager.CreateAsync(new AppUser
            {
                UserName = dto.UserName
            },dto.Password);

            if (result.Succeeded is false)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                
                }
                return ValidationProblem(ModelState);
            }
            var user = await _userManager.FindByNameAsync(dto.UserName);
            return Ok(user);
        }
    }
}
