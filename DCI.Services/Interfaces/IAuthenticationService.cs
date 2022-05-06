using DCI.Core.Utils;
using DCI.Core.ViewModels;
using DCI.Core.ViewModels.LoginVMs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DCI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ResultModel<LoginResponseVM>> LoginAsync(LoginVM model, DateTime currentDate);
    }
}
