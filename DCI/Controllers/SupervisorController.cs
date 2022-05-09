using DCI.Core.Enums;
using DCI.Core.Utils;
using DCI.Entities.ViewModels;
using DCI.Entities.ViewModels.CaseVMs;
using DCI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DCI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = AppRoles.SupervisorRole)]
    public class SupervisorController : BaseController
    {
        private readonly ICaseService _caseService;
        public SupervisorController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<CaseVMAll>>), 200)]
        public async Task<IActionResult> GetAllCasesForASupervisor([FromQuery] BaseSearchViewModel model)
        {

            try
            {
                var result = await _caseService.GetAllPaginatedForSupervisor(model, UserId);

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
                var result = await _caseService.ActionAsync(id, UserId,model,CurrentDateTime,ApprovalStatus.APPROVED_BY_SUPERVISOR);

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
                var result = await _caseService.ActionAsync(id, UserId, model, CurrentDateTime, ApprovalStatus.REJECTED_BY_SUPERVISOR);

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
