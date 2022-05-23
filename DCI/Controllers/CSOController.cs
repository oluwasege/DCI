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
    [Authorize(Roles = AppRoles.CSORole)]
    public class CSOController : BaseController
    {
        private readonly ICaseService _caseService;
        public CSOController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<CaseVMAll>>), 200)]
        public async Task<IActionResult> GetAllCasesSubmittedbyACSO([FromQuery] BaseSearchViewModel model)
        {

            try
            {
                var result = await _caseService.GetAllPaginatedForCSO(model, UserId);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK,result.Data.TotalCount);

                return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> SubmitACase([FromBody] SubmitCaseVM model)
        {

            try
            {
                var result = await _caseService.SubmitAsync(model, UserId);

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
