using DCI.Core.Utils;
using DCI.Entities.ViewModels;
using DCI.Entities.ViewModels.LoginVMs;
using DCI.Entities.ViewModels.UserVMs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DCI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ResultModel<LoginResponseVM>> LoginAsync(LoginVM model, DateTime currentDate);
        Task<ResultModel<bool>> ResetPassword(ResetPasswordVM model, string userId, DateTime currentDate);
        Task<ResultModel<LoginResponseVM>> RefreshTokenAsync(string token, DateTime date);
        Task<ResultModel<bool>> InviteUser(RegisterUserVM model);
        Task<ResultModel<PaginatedList<UserVM>>> GetAllUsers(BaseSearchViewModel model);
        Task<ResultModel<UserVM>> GetUserAsync(string email);
        Task<ResultModel<string>> AssignUserToRole(string email, string role, string CurrentUserID);
        Task<ResultModel<List<RoleVm>>> GetAllRoles();




    }
}
