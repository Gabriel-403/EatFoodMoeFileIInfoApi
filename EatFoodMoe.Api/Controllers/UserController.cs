using EatFoodMoe.Api.dto;
using EatFoodMoe.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EatFoodMoe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHostEnvironment _environment;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IHostEnvironment environment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _environment = environment;
        }
        [HttpGet("myuser")]
        public async Task<ActionResult<AppUser>> GetMyuser()
        {
            var result = await _userManager.GetUserAsync(User);
            return Ok(result);
        }

        [HttpGet("users")]
        [Authorize(Roles = "admin")]
        public  async Task<ActionResult<AppUser>> GetUsers()
        {
            
            var users = await _userManager.Users.ToListAsync();
            return Ok(users.Select(u=>
            new
            {
                user = u.UserName,
                roles = _userManager.GetRolesAsync(u).Result,
                islogin= "否"
            }));
        }
      
        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetUser([Required] string userName)
        {
            if (userName is null)
            {
                var result = await _userManager.GetUserAsync(User);
                return Ok(result);
            }

            AppUser user = await _userManager.FindByNameAsync(userName);
            return Ok(user);
        }
        [Authorize]
        [HttpPost("user")]
        public async Task<ActionResult<AppUser>> CreateUser([FromForm] CreateUserDto dto)
        {
            string userName = dto.UserName;

            IdentityResult result = await _userManager.CreateAsync(new AppUser
            {
                UserName = userName
            },dto.Password);

            if (result.Succeeded is false)
            {
                return IdentityValidationError(result.Errors);
            }

            AppUser user = await _userManager.FindByNameAsync(dto.UserName);

            string roleName = dto.RoleName;
            if (roleName is not null)
            {
                return await AddUserToRole(userName, roleName);
            }
            await AddUserToRole(userName, "Audience");
            return Ok(user);
        }
        [Authorize(Roles ="admin")]
        [HttpDelete("user")]
        public async Task<ActionResult<AppUser>> DeleteUser( string userName,string roleName) 
        { 
            AppUser user = await _userManager.FindByNameAsync(userName);
            if (roleName == "admin")
            {

                return Forbid();
            }
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (result.Succeeded is false) 
            {
                return IdentityValidationError(result.Errors);
            }
            return Ok();

        }


        [HttpPost("role")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<AppUser>> AddUserToRole(string userName, string roleName)
        {
            bool roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (roleExist is false)
            {
                var role = new IdentityRole {Name = roleName};
                IdentityResult roleCreateResult = await _roleManager.CreateAsync(role);
                if (roleCreateResult.Succeeded is false)
                {
                    return IdentityValidationError(roleCreateResult.Errors);
                }
            }

            AppUser user = await _userManager.FindByNameAsync(userName);
            IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);

            if (addToRoleResult.Succeeded is false)
            {
                return IdentityValidationError(addToRoleResult.Errors);
            }

            return Ok(user);
        }



        [HttpDelete("role")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<AppUser>> RemoveUserFromRole(string userName, string roleName)
        {
            bool roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (roleExist is false)
            {
                return NotFound();
            }

            AppUser user = await _userManager.FindByNameAsync(userName);
            if (roleName=="admin") {

                return Forbid();
            }
            IdentityResult deletToRoleResult = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (deletToRoleResult.Succeeded is false)
            {
                return IdentityValidationError(deletToRoleResult.Errors);
            }
           

            return Ok(user);
        }

        [HttpPut("role")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<AppUser>> ChangeRole(string userName, string oldRoleName, string roleName)
        {
            if (oldRoleName == "admin")
            {

                return Forbid();
            }
            var actionResult = await RemoveUserFromRole(userName, oldRoleName);
            if (actionResult.Result is IStatusCodeActionResult {StatusCode: not 200})
            {
                return actionResult;
            }
            return await AddUserToRole(userName, roleName);
        }

        private ActionResult IdentityValidationError(IEnumerable<IdentityError> identityErrors)
        {
            foreach (IdentityError error in identityErrors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(ModelState);
        }
    }
}
