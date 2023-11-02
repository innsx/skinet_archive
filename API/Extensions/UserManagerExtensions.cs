using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByUserAddressFromClaimsPrincipalAsync(this UserManager<AppUser> input, ClaimsPrincipal user)   
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var appUserWithAddressFromClaimsPrincipal =  await input.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);

            return appUserWithAddressFromClaimsPrincipal;
        }

        public static async Task<AppUser> FindByEmailFromClaimsPrincipalAsync(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var appUserEmailFromClaims =  await input.Users.SingleOrDefaultAsync(x => x.Email == email);

            return appUserEmailFromClaims;
        }
    }
}
