using System;
using System.Net.Http;
using System.Text;
using System.Runtime.Serialization;
using IdentityApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;


using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Newtonsoft.Json;
using Serilog;

namespace IdentityApi.Data.Repos
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await this.TransferClaimsToUser(user);
            return user;
        }

        public async Task<List<ApplicationUser>> GetAllUser()
        {
            var users = await this.GetAll();
            foreach (var user in users)
            {
                await this.TransferClaimsToUser(user);
            }
            return users;
        }

        public async Task<List<ApplicationUser>> GetUsersByRole(string role)
        {
            var users = (await this.GetAll()).Where(m => m.Role.Equals(role)).ToList();
            foreach (var user in users)
            {
                await this.TransferClaimsToUser(user);
            }
            return users;
        }

        public async Task UpdateUser(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(await _userManager.FindByIdAsync(user.Id));
            var replace = this.TransferUserToClaims(user);

            //update claims
            foreach (var temp in replace)
            {
                var toBeReplaced = GetClaim(claims, temp.Type);
                if (!toBeReplaced.Value.Equals(temp.Value))
                    await _userManager.ReplaceClaimAsync(user, toBeReplaced, temp);
            }

        }
        public async Task<bool> CreateUser(ApplicationUser temp)
        {
            var user = await _userManager.FindByNameAsync(temp.UserName);
            var success = true;
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = temp.UserName
                };
                var result = _userManager.CreateAsync(user, temp.Password).Result;
                if (!result.Succeeded)
                {
                    success = false;
                    throw new Exception(result.Errors.First().Description);
                }

                result = _userManager.AddToRoleAsync(user, temp.Role).Result;
                if (!result.Succeeded)
                {
                    success = false;
                    throw new Exception(result.Errors.First().Description);
                }

                result = _userManager.AddClaimsAsync(user, this.TransferUserToClaims(temp)).Result;
                if (!result.Succeeded)
                {
                    success = false;
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug($"{temp.UserName} created");
                return success;
            }
            else
            {
                Log.Debug($"{temp.UserName} already exists");
                return false;
            }
        }
        public async Task<bool> DeleteUser(ApplicationUser user)
        {
            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);
                var username = user.UserName;
                var success = true;

                var res = await _userManager.RemoveFromRolesAsync(user, roles);
                if (!res.Succeeded)
                {
                    success = false;
                    throw new Exception(res.Errors.First().Description);
                }

                res = await _userManager.RemoveClaimsAsync(user, claims);
                if (!res.Succeeded)
                {
                    success = false;
                    throw new Exception(res.Errors.First().Description);
                }

                res = await _userManager.DeleteAsync(user);
                if (!res.Succeeded)
                {
                    success = false;
                    throw new Exception(res.Errors.First().Description);
                }
                if (success)
                {
                    Log.Debug($"{username} deleted successfully");
                }
                Log.Debug($"Delete {username} failed. Some errors happened");

                return success;
            }

            Log.Debug($"Application user doesn't exist. Delete failed");
            return false;

        }

        public async Task<bool> UserExists(string id)
        {
            return (await _userManager.FindByIdAsync(id)) == null ? false : true;
        }

        //mapping support function
        private async Task TransferClaimsToUser(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            user.Name = GetClaimValue(claims, "name");
            user.GivenName = GetClaimValue(claims, "given_name");
            user.FamilyName = GetClaimValue(claims, "family_name");
            user.PhoneNumber = GetClaimValue(claims, "phone_number");
            user.Email = GetClaimValue(claims, "email");
            user.Website = GetClaimValue(claims, "website");
            user.PictureUrl = GetClaimValue(claims, "picture");
            user.Address = GetClaimValue(claims, "address");
            user.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
        }

        private List<Claim> TransferUserToClaims(ApplicationUser user)
        {
            return new List<Claim>{
                new Claim("name", user.Name),
                new Claim("given_name", user.GivenName),
                new Claim("family_name", user.FamilyName),
                new Claim("phone_number", user.PhoneNumberStr),
                new Claim("email", user.EmailStr),
                new Claim("website", user.Website),
                new Claim("picture", user.PictureUrl),
                new Claim("address", user.Address)
            };
        }

        private static string GetClaimValue(IList<Claim> userClaims, string claimType)
        {
            return userClaims.FirstOrDefault(c => c.Type == claimType).Value;
        }

        private static Claim GetClaim(IList<Claim> userClaims, string claimType)
        {
            return userClaims.FirstOrDefault(c => c.Type == claimType);
        }





    }
}