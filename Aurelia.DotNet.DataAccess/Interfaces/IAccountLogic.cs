using Aurelia.DotNet.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aurelia.DotNet.Logic.Interfaces
{
    public interface IAccountLogic
    {
        Task<IEnumerable<IdentityError>> CreateRoleAsync(Role role, params string[] claims);
        Task<IEnumerable<IdentityError>> CreateUserAsync(User user, string password, params string[] roles);
        Task<IEnumerable<IdentityError>> DeleteRoleAsync(Role role);
        Task<IEnumerable<IdentityError>> DeleteUserAsync(User user);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByIdAsync(string userId);
        Task<User> FindByNameAsync(string userName);
        Task<Role> FindRoleBuId(string roleId);
        Task<Role> FindRoleByNameAsync(string roleName);
        Task<IEnumerable<string>> GetRolesAsync(User user);
    }
}
