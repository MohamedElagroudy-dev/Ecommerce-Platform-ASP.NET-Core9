using Core.Sharing.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Account.Services
{
    public interface IAccountService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
        Task<string?> AddRoleAsync(AddRoleModel model);
        Task<string?> UnassignRoleAsync(UnassignRoleModel model);
    }
}
