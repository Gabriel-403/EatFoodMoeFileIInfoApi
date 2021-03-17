using EatFoodMoe.Api.Models;
using IdentityModel;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EatFoodMoe.Api.Extends
{
    public class EatFoodClaimService : DefaultClaimsService
    {
        private readonly UserManager<AppUser> _userManager;

        public EatFoodClaimService(UserManager<AppUser> userManager,IProfileService profile, ILogger<EatFoodClaimService> logger) 
            : base (profile, logger)
        {
            _userManager = userManager;
        }

        public override async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, ResourceValidationResult resourceResult, ValidatedRequest request)
        {
            var claims = await base.GetAccessTokenClaimsAsync(subject, resourceResult, request);
            var claimList = claims as ICollection<Claim> ?? claims.ToList();
            
            AppUser user = await _userManager.FindByIdAsync(subject.FindFirst(JwtClaimTypes.Subject).Value);
            claimList.Add(new Claim(JwtClaimTypes.Name, user.UserName));
            var roles = await _userManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                claimList.Add(new Claim(JwtClaimTypes.Role, role));
            }
            return claimList;
        }
    }
}