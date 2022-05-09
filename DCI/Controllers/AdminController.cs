using DCI.Core.Enums;
using DCI.Core.Utils;
using DCI.Entities.ViewModels;
using DCI.Entities.ViewModels.CaseVMs;
using DCI.Entities.ViewModels.UserVMs;
using DCI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DCI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = AppRoles.AdminRole)]
    public class AdminController : BaseController
    {
        private readonly IAuthenticationService _authService;
        private readonly ICaseService _caseService;
        public AdminController(IAuthenticationService authService, ICaseService caseService)
        {
            _authService = authService;
            _caseService = caseService;
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> InviteUser([FromBody] RegisterUserVM model)
        {

            try
            {
                var result = await _authService.InviteUser(model);

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

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<UserVM>>), 200)]
        public async Task<IActionResult> GetAllUsers([FromQuery] BaseSearchViewModel model)
        {

            try
            {
                var result = await _authService.GetAllUsers(model);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpGet("{email}")]
        [ProducesResponseType(typeof(ApiResponse<UserVM>), 200)]
        public async Task<IActionResult> GetAUser(string email)
        {

            try
            {
                var result = await _authService.GetUserAsync(email);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpPost("{email}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> AssignAUserToRole(string email,[FromBody]RoleVm model)
        {

            try
            {
                var result = await _authService.AssignUserToRole(email,model.Role,UserId);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<RoleVm>>), 200)]
        public async Task<IActionResult> GetAllRoles()
        {

            try
            {
                var result = await _authService.GetAllRoles();

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<CaseVMAll>>), 200)]
        public async Task<IActionResult> GetAllCases([FromQuery] BaseSearchViewModel model)
        {

            try
            {
                var result = await _caseService.GetAllPaginatedForAdmin(model,UserId);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK, result.Data.TotalCount);

                return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<CaseVMAll>>), 200)]
        public async Task<IActionResult> GetAllCasesValidatedbySupervisors([FromQuery] BaseSearchViewModel model)
        {

            try
            {
                var result = await _caseService.GetAllPaginatedValidatedbySupervisor(model, UserId);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK, result.Data.TotalCount);

                return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> ApproveACase(string id, [FromBody] ApprovalModel model)
        {

            try
            {
                var result = await _caseService.ActionAsync(id, UserId, model, CurrentDateTime, ApprovalStatus.APPROVED_BY_ADMIN);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> RejectACase(string id, [FromBody] ApprovalModel model)
        {

            try
            {
                var result = await _caseService.ActionAsync(id, UserId, model, CurrentDateTime, ApprovalStatus.REJECTED_BY_ADMIN);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }
    }
}
