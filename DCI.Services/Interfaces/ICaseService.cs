using DCI.Core.Enums;
using DCI.Core.Utils;
using DCI.Entities.ViewModels;
using DCI.Entities.ViewModels.CaseVMs;
using System;
using System.Threading.Tasks;

namespace DCI.Services.Interfaces
{
    public interface ICaseService
    {
        Task<ResultModel<bool>> SubmitAsync(SubmitCaseVM model, string userId);
        Task<ResultModel<PaginatedList<CaseVMAll>>> GetAllPaginated(BaseSearchViewModel model);
        Task<ResultModel<PaginatedList<CaseVMAll>>> GetAllPaginatedForCSO(BaseSearchViewModel model, string userId);
        Task<ResultModel<PaginatedList<CaseVMAll>>> GetAllPaginatedForSupervisor(BaseSearchViewModel model, string userId);
        Task<ResultModel<PaginatedList<CaseVMAll>>> GetAllPaginatedForAdmin(BaseSearchViewModel model, string userId);
        Task<ResultModel<CaseVM>> GetAsync(string id);
        Task<ResultModel<PaginatedList<CaseVMAll>>> GetAllPaginatedValidatedbySupervisor(BaseSearchViewModel model, string userId);
        Task<ResultModel<bool>> ActionAsync(string id, string currentUserId, ApprovalModel model, DateTime currentDate, ApprovalStatus status);
    }
}
