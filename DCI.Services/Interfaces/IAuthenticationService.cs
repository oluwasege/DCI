using DCI.Core.ViewModels;
using DCI.Entities.ViewModels.LoginVMs;
using System;
using System.Threading.Tasks;

namespace DCI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ResultModel<LoginResponseVM>> LoginAsync(LoginVM model, DateTime currentDate);
        Task<ResultModel<bool>> ResetPassword(ResetPasswordVM model, string userId, DateTime currentDate);
        Task<ResultModel<LoginResponseVM>> RefreshTokenAsync(string token, DateTime date);
    }
}
