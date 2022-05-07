using DCI.Core.Enums;
using DCI.Core.Utils;
using DCI.Core.ViewModels;
using DCI.Entities.ViewModels.LoginVMs;
using DCI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DCI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AunthenticationController : BaseController
    {
        private readonly IAuthenticationService _authService;

        public AunthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseVM>), 200)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginVM model)
        {

            try
            {
                var result = await _authService.LoginAsync(model, CurrentDateTime);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<LoginResponseVM>(null, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
               // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordVM model)
        {

            try
            {
                var result = await _authService.ResetPassword(model,UserId, CurrentDateTime);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<bool>(false, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }
    }
}
