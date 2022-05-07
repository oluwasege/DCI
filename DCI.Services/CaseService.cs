using AzureRays.Shared.ViewModels;
using DCI.Core.Enums;
using DCI.Core.Utils;
using DCI.Core.ViewModels;
using DCI.Entities.DataAccess;
using DCI.Entities.Entities;
using DCI.Entities.ViewModels;
using DCI.Entities.ViewModels.CaseVMs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DCI.Services
{
    public class CaseService
    {
        private readonly ApplicationDbContext _context;
        public CaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResultModel<bool>>SubmitAsync(SubmitCaseVM model, string userId, DateTime currentDate)
        {
            var resultModel=new ResultModel<bool>();
            try
            {
                if (model == null)
                {
                    resultModel.AddError("Empty Submission");
                    resultModel.Data = false;
                    return resultModel;
                }
                var user=GetUserById(userId).Result;
                if(user==null)
                {
                    resultModel.AddError("User does not exists");
                    return resultModel;
                }
                if(user.State.ToLower()!=model.State.ToLower())
                {
                    resultModel.AddError("You do not have jurisdiction");
                    return resultModel;
                }
                var newCase = new Case
                {
                    CSOUserId = userId,
                    StateOfCase = model.StateOfCase,
                    Statement = model.Statement,
                    State = model.State,
                    LGA = model.LGA,
                    IsPerpetratorArrested = model.IsPerpetratorArrested,
                    IsFatal = model.IsFatal,
                    ApprovalStatus = ApprovalStatus.PENDING,
                    IsDeleted = false
                };

                newCase.ViolenceType = model.ViolenceType; 
                
                await _context.Cases.AddAsync(newCase);
                await _context.SaveChangesAsync();
                resultModel.Data = true;
                resultModel.Message = "Case Submited";
                return resultModel;
                
            }
            catch (Exception ex)
            {
                resultModel.AddError(ex.Message);
                return resultModel;
            }
        }
        public async Task<ResultModel<PaginatedList<CaseVM>>>GetAllPaginated(BaseSearchViewModel model)
        {
            var resultModel = new ResultModel<PaginatedList<CaseVM>>();
            try
            {
                var allCases = GetAllCase();
                if(allCases ==null)
                {
                    resultModel.AddError("No case available");
                    return resultModel;
                }
                var casesPaged = allCases.ToPaginatedList((int)model.PageIndex, (int)model.PageSize);
                var casesVms = casesPaged.Select(x => (CaseVM)x).ToList();
                var data = new PaginatedList<CaseVM>(casesVms, (int)model.PageIndex, (int)model.PageSize, casesPaged.TotalCount);
                resultModel.Data = data;
                resultModel.Message = $"Found {casesPaged.Count} cases";
                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.AddError(ex.Message);
                return resultModel;
            }
        }

        public async Task<ResultModel<PaginatedList<CaseVM>>> GetAllPaginatedForCSO(BaseSearchViewModel model,string userId)
        {
            var resultModel = new ResultModel<PaginatedList<CaseVM>>();
            try
            {
                var user = GetUserById(userId).Result;
                if (user == null)
                {
                    resultModel.AddError("User does not exists");
                    return resultModel;
                }
                var allCases = GetAllCase().Where(x=>x.CSOUserId==userId);
                if (allCases == null)
                {
                    resultModel.AddError("No case available");
                    return resultModel;
                }

                if (model != null)
                    allCases = EntityFilter(allCases, model);
                var casesPaged = allCases.ToPaginatedList((int)model.PageIndex, (int)model.PageSize);
                var casesVms = casesPaged.Select(x => (CaseVM)x).ToList();
                var data = new PaginatedList<CaseVM>(casesVms, (int)model.PageIndex, (int)model.PageSize, casesPaged.TotalCount);
                resultModel.Data = data;
                resultModel.Message = $"Found {casesPaged.Count} cases";
                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.AddError(ex.Message);
                return resultModel;
            }
        }

        public async Task<ResultModel<PaginatedList<CaseVM>>> GetAllPaginatedForSupervisor(BaseSearchViewModel model, string userId)
        {
            var resultModel = new ResultModel<PaginatedList<CaseVM>>();
            try
            {
                var user = GetUserById(userId).Result;
                if (user == null)
                {
                    resultModel.AddError("User does not exists");
                    return resultModel;
                }
                var allCases = GetAllCase().Where(x => x.CSOUser.State == user.State);
                if (allCases == null)
                {
                    resultModel.AddError("No case available");
                    return resultModel;
                }

                if (model != null)
                    allCases = EntityFilter(allCases, model);
                var casesPaged = allCases.ToPaginatedList((int)model.PageIndex, (int)model.PageSize);
                var casesVms = casesPaged.Select(x => (CaseVM)x).ToList();
                var data = new PaginatedList<CaseVM>(casesVms, (int)model.PageIndex, (int)model.PageSize, casesPaged.TotalCount);
                resultModel.Data = data;
                resultModel.Message = $"Found {casesPaged.Count} cases";
                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.AddError(ex.Message);
                return resultModel;
            }
        }
        public async Task<ResultModel<PaginatedList<CaseVM>>> GetAllPaginatedForAdmin(BaseSearchViewModel model, string userId)
        {
            var resultModel = new ResultModel<PaginatedList<CaseVM>>();
            try
            {
                var user = GetUserById(userId).Result;
                if (user == null)
                {
                    resultModel.AddError("User does not exists");
                    return resultModel;
                }
                var allCases = GetAllCase();
                if (allCases == null)
                {
                    resultModel.AddError("No case available");
                    return resultModel;
                }

                if (model != null)
                    allCases = EntityFilter(allCases, model);
                var casesPaged = allCases.ToPaginatedList((int)model.PageIndex, (int)model.PageSize);
                var casesVms = casesPaged.Select(x => (CaseVM)x).ToList();
                var data = new PaginatedList<CaseVM>(casesVms, (int)model.PageIndex, (int)model.PageSize, casesPaged.TotalCount);
                resultModel.Data = data;
                resultModel.Message = $"Found {casesPaged.Count} cases";
                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.AddError(ex.Message);
                return resultModel;
            }
        }
        public async Task<ResultModel<bool>> ActionAsync(string Id, string currentUserId, ApprovalModel model, DateTime currentDate, ApprovalStatus status)
        {
            var result=new ResultModel<bool>();
        }
        private async Task<DCIUser> GetUserById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }
        private IQueryable<Case> GetAllCase() => _context.Cases.AsQueryable()
                                                               .Include(x => x.ApprovalAction)
                                                                .Include(x=>x.CSOUser);

        private IQueryable<Case> EntityFilter(IQueryable<Case> query, BaseSearchViewModel model)
        {

            //var keyword = model.Keyword.ToLower().Trim();

            if (!string.IsNullOrEmpty(model.Keyword) && !string.IsNullOrEmpty(model.Filter))
            {
                var keyword = model.Keyword.ToLower().Trim();
                switch (model.Filter)
                {
                    case "Pending":
                        {
                            query = query.Where(x => x.ApprovalStatus==ApprovalStatus.PENDING);
                            break;
                        }
                    case "Rejected":
                        {
                            query = query.Where(x => x.ApprovalStatus == ApprovalStatus.REJECTED_BY_SUPERVISOR &&x.ApprovalStatus==ApprovalStatus.REJECTED_BY_ADMIN);
                            break;
                        }
                    case "Approved":
                        {
                            query = query.Where(x => x.ApprovalStatus == ApprovalStatus.APPROVED_BY_SUPERVISOR && x.ApprovalStatus == ApprovalStatus.APPROVED_BY_ADMIN);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            return query;
        }

    }
}
