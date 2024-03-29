﻿using DCI.Core;
using DCI.Core.Enums;
using DCI.Core.Utils;
using DCI.Entities.DataAccess;
using DCI.Entities.Entities;
using DCI.Entities.ViewModels;
using DCI.Entities.ViewModels.CaseVMs;
using DCI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCI.Services
{
    public class CaseService:ICaseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        public CaseService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<ResultModel<bool>>SubmitAsync(SubmitCaseVM model, string userId)
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
                var user=await GetUserById(userId);
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
                    IsDeleted = false,
                    ApprovalAction=new Approval(),
                    ViolenceType = new ViolenceType()
                    {
                        
                        ChildAbuse = model.ViolenceType.ChildAbuse,
                        ViolationOfProperty = model.ViolenceType.ViolationOfProperty,
                        SocialAssault = model.ViolenceType.SocialAssault,
                        Rape = model.ViolenceType.Rape,
                        Psychological = model.ViolenceType.Psychological,
                        PhysicalAssault = model.ViolenceType.PhysicalAssault,
                        ForcedMarriage = model.ViolenceType.ForcedMarriage,
                        CyberBullying = model.ViolenceType.CyberBullying,
                        Defilement = model.ViolenceType.Defilement,
                        DenialOfResources = model.ViolenceType.DenialOfResources,
                        EarlyMarriage = model.ViolenceType.EarlyMarriage,
                        FemaleGenitalMutilation = model.ViolenceType.FemaleGenitalMutilation
                    }
                };

                
                await _context.Cases.AddAsync(newCase);
                await _context.SaveChangesAsync();
                var recipients = new List<string>
                    {
                        user.Email
                    };
                string body = $"<h3>Your Case has been submitted and is pending approval.</h3>";
                var status = await SendEmail(recipients, "Case Submission", body);
                if (!status)
                {
                    resultModel.Message="Unable to send mail but case has been created";
                }
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
        public async Task<ResultModel<PaginatedList<CaseVMAll>>>GetAllPaginated(BaseSearchViewModel model)
        {
            var resultModel = new ResultModel<PaginatedList<CaseVMAll>>();
            try
            {
                var allCases = GetAllCase().Where(x=>x.ApprovalStatus==ApprovalStatus.APPROVED_BY_ADMIN);
                if(allCases ==null)
                {
                    resultModel.AddError("No case available");
                    return resultModel;
                }
                var casesPaged = allCases.ToPaginatedList((int)model.PageIndex, (int)model.PageSize);
                var casesVms = casesPaged.Select(x => (CaseVMAll)x).ToList();
                var data = new PaginatedList<CaseVMAll>(casesVms, (int)model.PageIndex, (int)model.PageSize, casesPaged.TotalCount);
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

        public async Task<ResultModel<PaginatedList<CaseVMAll>>> GetAllPaginatedForCSO(BaseSearchViewModel model,string userId)
        {
            var resultModel = new ResultModel<PaginatedList<CaseVMAll>>();
            try
            {
                var user = await GetUserById(userId);
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
                var casesVms = casesPaged.Select(x => (CaseVMAll)x).ToList();
                var data = new PaginatedList<CaseVMAll>(casesVms, (int)model.PageIndex, (int)model.PageSize, casesPaged.TotalCount);
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

        public async Task<ResultModel<PaginatedList<CaseVMAll>>> GetAllPaginatedForSupervisor(BaseSearchViewModel model, string userId)
        {
            var resultModel = new ResultModel<PaginatedList<CaseVMAll>>();
            try
            {
                var user = await GetUserById(userId);
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
                var casesVms = casesPaged.Select(x => (CaseVMAll)x).ToList();
                var data = new PaginatedList<CaseVMAll>(casesVms, (int)model.PageIndex, (int)model.PageSize, casesPaged.TotalCount);
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
        public async Task<ResultModel<PaginatedList<CaseVMAll>>> GetAllPaginatedForAdmin(BaseSearchViewModel model, string userId)
        {
            var resultModel = new ResultModel<PaginatedList<CaseVMAll>>();
            try
            {
                var user = await GetUserById(userId);
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
                var casesVms = casesPaged.Select(x => (CaseVMAll)x).ToList();
                var data = new PaginatedList<CaseVMAll>(casesVms, (int)model.PageIndex, (int)model.PageSize, casesPaged.TotalCount);
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
        public async Task<ResultModel<CaseVM>> GetAsync(string id)
        {
            var resultModel=new ResultModel<CaseVM>();
            try
            {
                var existingCase= await GetCaseById(id);
                if (existingCase == null)
                {
                    resultModel.AddError("Case does not exist");
                    return resultModel;
                }
                resultModel.Data = existingCase;
                resultModel.Message = "Case retrieved";
                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.AddError(ex.Message);
                return resultModel;
            }
        }
        public async Task<ResultModel<PaginatedList<CaseVMAll>>> GetAllPaginatedValidatedbySupervisor(BaseSearchViewModel model, string userId)
        {
            var resultModel = new ResultModel<PaginatedList<CaseVMAll>>();
            try
            {
                var user = await GetUserById(userId);
                if (user == null)
                {
                    resultModel.AddError("User does not exists");
                    return resultModel;
                }
                var allCases = GetAllCase().Where(x=>x.ApprovalAction.ApprovedBy==userId||x.ApprovalAction.RejectedBy==userId);
                if (allCases == null)
                {
                    resultModel.AddError("No case available");
                    return resultModel;
                }

                if (model != null)
                    allCases = EntityFilter(allCases, model);
                var casesPaged = allCases.ToPaginatedList((int)model.PageIndex, (int)model.PageSize);
                var casesVms = casesPaged.Select(x => (CaseVMAll)x).ToList();
                var data = new PaginatedList<CaseVMAll>(casesVms, (int)model.PageIndex, (int)model.PageSize, casesPaged.TotalCount);
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
        public async Task<ResultModel<bool>> ActionAsync(string id, string currentUserId, ApprovalModel model, DateTime currentDate, ApprovalStatus status)
        {
            var resultModel=new ResultModel<bool>();
            try
            {
                var existingCase = await GetCaseById(id);
                if (existingCase == null)
                {
                    resultModel.AddError("Case does not exist");
                    return resultModel;
                }
                var user=await GetUserById(currentUserId);
                var validate=await ValidateCaseBeforeAction(existingCase,user,status);
                if(validate.HasError)
                {
                    resultModel.AddError(validate.ValidationErrors[0].ToString());
                    resultModel.Message = validate.ValidationErrors[0].ToString();
                    resultModel.Data = false;
                    return resultModel;
                }

                existingCase.ApprovalStatus = status;
                //if(status == ApprovalStatus.REJECTED_BY_SUPERVISOR||status==ApprovalStatus.REJECTED_BY_ADMIN)
                //{
                //    if (existingCase.ApprovalAction.ApprovedBy != null)
                //        existingCase.ApprovalAction.ApprovedBy = null;
                //}
                //if (status == ApprovalStatus.APPROVED_BY_SUPERVISOR || status == ApprovalStatus.APPROVED_BY_ADMIN)
                //{
                //    if (existingCase.ApprovalAction.RejectedBy != null)
                //        existingCase.ApprovalAction.RejectedBy = null;
                //}
                existingCase.ApprovalAction.ActionComment = model.Comment;
                existingCase.ApprovalAction.ActionDate = currentDate;
                existingCase.LastDateModified = currentDate;
               

                switch (status)
                {
                    case ApprovalStatus.APPROVED_BY_SUPERVISOR:
                        resultModel.Message = "Case has been approved by Supervisor";
                        existingCase.ApprovalAction.ApprovedBy = currentUserId;
                        existingCase.ApprovalAction.RejectedBy = null;
                        break;

                    case ApprovalStatus.APPROVED_BY_ADMIN:
                        resultModel.Message = "Case has been approved by Admin";
                        existingCase.ApprovalAction.ApprovedBy = currentUserId;
                        existingCase.ApprovalAction.RejectedBy = null;
                        break;

                    case ApprovalStatus.REJECTED_BY_SUPERVISOR:
                        resultModel.Message = "Case has been rejcted by Supervisor";
                        existingCase.ApprovalAction.ApprovedBy = null;
                        existingCase.ApprovalAction.RejectedBy = currentUserId;
                        break;
                    case ApprovalStatus.REJECTED_BY_ADMIN:
                        resultModel.Message = "Case has been rejected by Admin";
                        existingCase.ApprovalAction.ApprovedBy = null;
                        existingCase.ApprovalAction.RejectedBy = currentUserId;
                        break;

                }
                _context.Cases.Update(existingCase);
                await _context.SaveChangesAsync();
                var recipients = new List<string>
                    {
                        existingCase.CSOUser.Email
                    };
                var mailStatus = await SendEmail(recipients, $"Case Validation", $"{resultModel.Message}");
                if (!mailStatus)
                {
                    resultModel.Message = "Unable to send mail but case has been created";
                }
                resultModel.Data = true;
                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.AddError(ex.Message);
                return resultModel;
            }
        }
        private async Task<ResultModel<bool>>ValidateCaseBeforeAction(Case model,DCIUser user,ApprovalStatus status)
        {
            var result = new ResultModel<bool>();
            if(model.CSOUserId== user.Id)
            {
                result.AddError("You cannot validate cases submitted by you");
                result.Data = false;
                return result;
            }

            if(model.ApprovalStatus== ApprovalStatus.PENDING)
            {
                if(user.UserType==UserTypes.Admin)
                {
                    result.AddError("The Supervisor has to validate before an admin can validate");
                    result.Data = false;
                    return result;
                }
                if(status==ApprovalStatus.PENDING)
                {
                    result.AddError("This case is already pending");
                    result.Data = false;
                    return result;
                }
            }
            if(model.ApprovalStatus==ApprovalStatus.REJECTED_BY_SUPERVISOR||model.ApprovalStatus==ApprovalStatus.REJECTED_BY_ADMIN)
            {
                result.AddError("This case has been rejected");
                result.Data = false;
                return result;
            }
            
            result.Data=true;
            return result;
        }
        private async Task<DCIUser> GetUserById(string id)=> await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        private IQueryable<Case> GetAllCase() => _context.Cases.AsQueryable()
                                                               .Include(x => x.ApprovalAction).Include(x=>x.ViolenceType)
                                                                .Include(x=>x.CSOUser);
        private async Task<Case> GetCaseById(string id)=> await _context.Cases.Include(x=>x.ApprovalAction).Include(x=>x.ViolenceType).Include(x=>x.CSOUser).FirstOrDefaultAsync(x => x.Id == id);

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
                    case "RejectedBySupervisor":
                        {
                            query = query.Where(x => x.ApprovalStatus == ApprovalStatus.REJECTED_BY_SUPERVISOR);
                            break;
                        }
                    case "ApprovedBySupervisor":
                        {
                            query = query.Where(x => x.ApprovalStatus == ApprovalStatus.APPROVED_BY_SUPERVISOR);
                            break;
                        }
                    case "Open":
                        {
                            query = query.Where(x => x.StateOfCase == StateOfCase.Open);
                            break;
                        }
                    case "Closed":
                        {
                            query = query.Where(x => x.StateOfCase == StateOfCase.Closed);
                            break;
                        }
                    case "StatementName":
                        {
                            query = query.Where(x => x.Statement.ToLower().Contains(keyword.ToLower()));
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
        private async Task<bool> SendEmail(List<string> recipients, string subject, string body)
        {
            return await _emailService.SendMail(recipients, subject, body);
        }

    }
}
