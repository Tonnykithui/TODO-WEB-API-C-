using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebTODO.Models;

namespace WebTODO.ILoginRegisterServices
{
    public class LoginRegisterServices : ILoginRegister
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginRegisterServices
            (
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ValidationMessage> LoginAsync(Login login)
        {
            if (login == null)
                return new ValidationMessage
                {
                    IsError = true,
                    Message = "Enter credentials to Login"
                };

            var result = await _userManager.FindByEmailAsync(login.Email);

            if (result == null)
            {
                return new ValidationMessage
                {
                    IsError = true,
                    Message = "Invalid login Credential"
                };
            }

            var credConfirmed = await _userManager.CheckPasswordAsync(result, login.Password);

            if (!credConfirmed)
            {
                return new ValidationMessage
                {
                    IsError = true,
                    Message = "Incorrect login credentials,try again."
                };
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Thisisthesecuritykeybeingused"));

            var claims = await GetUserClaims(result);

            var token = new JwtSecurityToken
                (
                issuer: "http://localhost:55442",
                audience:"Users",
                expires:DateTime.Now.AddDays(20),
                claims:claims,
                signingCredentials:new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new ValidationMessage
            {
                IsError = false,
                Message = tokenString,
                Expiry = DateTime.Now.AddDays(20)
            };
        }

        private async Task<List<Claim>> GetUserClaims(IdentityUser result)
        {
            var _options = new IdentityOptions();

            var claims = new List<Claim>
            {
                new Claim("Email", result.Email),
                new Claim(ClaimTypes.NameIdentifier, result.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(result);

            if (userClaims != null)
                claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(result);

            foreach(var userRole in userRoles)
            {
                var roleName = await _roleManager.FindByNameAsync(userRole);
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var roleClaims = await _roleManager.GetClaimsAsync(roleName);

                foreach(var roleClaim in roleClaims)
                {
                    claims.Add(roleClaim);
                }
            }

            return claims;
        }

        public async Task<ValidationMessage> RegisterAsync(Register register)
        {
            if (register == null)
                return new ValidationMessage
                {
                    IsError = true,
                    Message = "Provide Register Details"
                };

            var userExists = await _userManager.FindByEmailAsync(register.Email);


            if (userExists != null)
                return new ValidationMessage
                {
                    IsError = true,
                    Message = "User email already exists"
                };

            if (register.Password != register.ConfirmPassword)
                return new ValidationMessage
                {
                    IsError = true,
                    Message = "Passwords provided do not match"
                };

            var identityUser = new IdentityUser
            {
                Email = register.Email,
                UserName = register.Email
            };

            var result = await _userManager.CreateAsync(identityUser, register.Password);

            if (!result.Succeeded)
                return new ValidationMessage
                {
                    IsError = true,
                    Message = "User not created",
                    Error = result.Errors.Select(e => e.Description)
                };

            return new ValidationMessage
            {
                IsError = false,
                Message = "User created successfully"
            };
        }
    }
}
