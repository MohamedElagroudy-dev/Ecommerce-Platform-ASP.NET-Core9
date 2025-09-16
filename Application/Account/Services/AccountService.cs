using Core.Interfaces;
using Core.Sharing.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Account.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAuthService authService, ILogger<AccountService> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _logger.LogInformation("User registration for email: {Email}", model.Email);
            return await _authService.RegisterAsync(model);
        }

        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _logger.LogInformation("Token request for email: {Email}", model.Email);
            return await _authService.GetTokenAsync(model);
        }

        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.UserId))
                throw new ArgumentException("UserId is required");

            _logger.LogInformation("Adding role '{Role}' to user: {UserId}", model.Role, model.UserId);
            return await _authService.AddRoleAsync(model);
        }
    }
}