using AspNet.Security.OpenIdConnect.Primitives;
using Aurelia.DotNet.DataAccess;
using Aurelia.DotNet.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Aurelia.DotNet.Logic.Interfaces;
using Aurelia.DotNet.DataAccess.Common;

namespace Aurelia.DotNet.Logic
{

    public class AccountLogic : IAccountLogic
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AccountLogic(
                   ApplicationDbContext context,
                   UserManager<User> userManager,
                   RoleManager<Role> roleManager,
                   IHttpContextAccessor httpAccessor)
        {
            _context = context;

            _context.UserId = int.Parse(httpAccessor.HttpContext?.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value ?? "-1");
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<User> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IEnumerable<string>> GetRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<IEnumerable<IdentityError>> CreateUserAsync(User user, string password, params string[] roles)
        {
            roles = roles ?? new string[] { };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return result.Errors;

            //ReUp the user from the DB
            user = await _userManager.FindByNameAsync(user.UserName);

            try
            {
                result = await this._userManager.AddToRolesAsync(user, roles.Distinct());
            }
            catch
            {
                await DeleteUserAsync(user);
                throw;
            }

            if (!result.Succeeded)
            {
                await DeleteUserAsync(user);
            }
            return result.Errors;
        }

        public async Task<IEnumerable<IdentityError>> DeleteUserAsync(User user)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.Errors;
        }

        public async Task<Role> FindRoleBuId(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }
        public async Task<Role> FindRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<IEnumerable<IdentityError>> CreateRoleAsync(Role role, params string[] claims)
        {
            claims = claims ?? new string[] { };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return result.Errors;
            }
            // Re-Up Role
            role = await _roleManager.FindByNameAsync(role.Name);

            foreach (string claim in claims.Distinct())
            {
                result = await this._roleManager.AddClaimAsync(role, new Claim(Claims.Permission, Permissions.Admin));

                if (!result.Succeeded)
                {
                    await DeleteRoleAsync(role);
                    return result.Errors;
                }
            }

            return result.Errors;
        }

        public async Task<IEnumerable<IdentityError>> DeleteRoleAsync(Role role)
        {
            var result = await _roleManager.DeleteAsync(role);
            return result.Errors;
        }

    }
}