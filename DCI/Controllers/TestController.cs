using DCI.Core.Enums;
using DCI.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DCI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : BaseController
    {
        [HttpGet]
        [Authorize(Roles = AppRoles.AdminRole)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> GetFund()
        {
            try
            {
                    
                    return Ok(CurrentUser.Email);
                
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
