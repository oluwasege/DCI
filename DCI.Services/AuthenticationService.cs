using DCI.Core.Utils;
using DCI.Core.ViewModels;
using DCI.Core.ViewModels.LoginVMs;
using DCI.Entities;
using DCI.Entities.DataAccess;
using DCI.Entities.Entities;
using DCI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace DCI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<DCIUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public AuthenticationService(UserManager<DCIUser> userManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }
        public async Task<ResultModel<LoginResponseVM>> LoginAsync(LoginVM model,DateTime currentDate)
        {
           var resultModel = new ResultModel<LoginResponseVM>();
            try
            {
                var user = GetUser(model.EmailAddress).Result;
                if(user==null)
                {
                    resultModel.AddError(ErrorConstants.IncorrectUserOrPass);
                    return resultModel;
                }

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var response = await _userManager.IsEmailConfirmedAsync(user);

                    if (response == false)
                    {
                        resultModel.AddError("Email not confirmed!");
                        return resultModel;
                    }

                    var userRoles = await _userManager.GetRolesAsync(user);
                    var (token, expiration) = CreateJwtTokenAsync(user, userRoles);

                    await UpdateUserLastLogin(user.Email, currentDate);

                    var data = new LoginResponseVM()
                    {
                        Token = token,
                        Expiration = expiration,
                        Roles = userRoles,
                    };

                    if (user.RefreshTokens.Any(a => a.IsActive))
                    {
                        var activeRefreshToken = user.RefreshTokens
                            .Where(a => a.IsActive == true)
                            .FirstOrDefault();
                        // authenticationModel.RefreshToken = activeRefreshToken.Token;
                        // authenticationModel.RefreshTokenExpiration = activeRefreshToken.Expires;
                    }
                    else
                    {
                        var refreshToken = CreateRefreshToken(currentDate);
                        //authenticationModel.RefreshToken = refreshToken.Token;
                        //authenticationModel.RefreshTokenExpiration = refreshToken.Expires;
                        user.RefreshTokens.Add(refreshToken);
                        _context.Update(user);
                        _context.SaveChanges();
                    }
                    resultModel.Data = data;
                    return resultModel;
                }

                resultModel.AddError(ErrorConstants.IncorrectUserOrPass);
                resultModel.Message = ErrorConstants.IncorrectUserOrPass;
                return resultModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultModel<LoginResponseVM>> RefreshTokenAsync(string token,DateTime date)
        {
            var resultModel = new ResultModel<LoginResponseVM>();
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                resultModel.AddError($"Token did not match any users.");
                return resultModel;
            }
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive)
            {
                resultModel.AddError($"Token Not Active.");
                return resultModel;
            }

            //Revoke current refresh token
            refreshToken.Revoked = DateTime.UtcNow;

            //Generate new Refresh Token and save to Database
            var newRefreshToken = CreateRefreshToken(date);
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            _context.SaveChanges();

            var userRoles = await _userManager.GetRolesAsync(user);
            var (newtoken, expiration) = CreateJwtTokenAsync(user, userRoles);

            await UpdateUserLastLogin(user.Email, date);

            var data = new LoginResponseVM()
            {
                Token = token,
                Expiration = expiration,
                Roles = userRoles,
            };

            resultModel.Data= data;
            return resultModel;
        }
        private RefreshToken CreateRefreshToken(DateTime date)
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = date.AddMinutes(Convert.ToInt32(_configuration.GetSection("JWT:DurationInMinutes").Value)),
                Created = date
            };
        }
        private async Task<ResultModel<bool>> UpdateUserLastLogin(string emailAddress, DateTime CurrentDate)
        {
            var result = new ResultModel<bool>();
            try
            {
                var user =GetUser(emailAddress).Result;

                if (user == null)
                {
                    result.AddError(ErrorConstants.IncorrectUserOrPass);
                    result.Data = false;
                    return result;
                };


                user.LastLoginDate = CurrentDate;
                await _context.SaveChangesAsync();

                result.Data = true;
                return result;
            }
            catch (Exception ex)
            {
               // _log.LogError("{0}--------at {1}()", ex.Message ?? ex.InnerException.Message, nameof(UpdateUserLastLogin));
                result.AddError("An error occured!");
                return result;
            }
        }
        private (string, DateTime) CreateJwtTokenAsync(DCIUser user, IList<string> userRoles)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]);

            IdentityOptions identityOptions = new IdentityOptions();

            var userClaims = new List<Claim>()
            {
                new Claim(identityOptions.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(identityOptions.ClaimsIdentity.UserNameClaimType, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
                new Claim("LastLoginDate", user.LastLoginDate == null ? "Not set" : user.LastLoginDate.ToString()),
                new Claim("FirstName", user.FirstName?.ToString()),
                new Claim("LastName", user.LastName?.ToString()),
                new Claim("IsAdmin", user.IsAdmin ? "True" : "False"),
                new Claim("IsCSO", user.IsCSO ? "True" : "False"),
                new Claim("IsSupervisor", user.IsSupervisor ? "True" : "False"),
            };

            foreach (var userRole in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var signKey = new SymmetricSecurityKey(key);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWT:Issuer").Value,
                audience: _configuration.GetSection("JWT:Audience").Value,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration.GetSection("JWT:DurationInMinutes").Value)),
                claims: userClaims, signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256));

            return (new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), jwtSecurityToken.ValidTo);
        }

        public async Task<ResultModel<bool>> ResetPassword(ResetPasswordVM model,string userId,DateTime currentDate)
        {
            var resultModel = new ResultModel<bool>();

            try
            {
                var user = GetUserById(userId).Result;
                if (user == null)
                {
                    resultModel.AddError(ErrorConstants.IncorrectUserOrPass);
                    return resultModel;
                };

                if (model.Password != model.ConfirmPassword)
                {
                    resultModel.AddError("Passwords do not match");
                    return resultModel;
                }
                   

                user.PasswordHash = new PasswordHasher<DCIUser>().HashPassword(user, model.Password);
                user.LastModifiedDate = currentDate;
                var resetResult = await _userManager.UpdateAsync(user);

                if (!resetResult.Succeeded)
                {
                    foreach (var error in resetResult.Errors)
                    {
                        resultModel.AddError(error.Description);
                    }
                    return resultModel;
                };

                resultModel.Message = "Password Changed Successfully";
                resultModel.Data = true;
                return resultModel;
            }
            catch (Exception ex)
            {
                _log.LogError("{0}--------at {1}()", ex.Message ?? ex.InnerException.Message, nameof(ResetPassword));
                result.AddError("An error occured!");
                return result;
            }
        }
        private async Task<DCIUser> GetUser(string emailAddress)
            => await _userManager.FindByEmailAsync(emailAddress);
        private async Task<DCIUser> GetUserById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }
    }
}
