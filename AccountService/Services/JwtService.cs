﻿using AccountService.DTOs;
using AccountService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccountService.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public JwtService(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<UserTokenDTO> BuildToken(UserInfoDTO userInfo)
        {
            var user = await _userManager.FindByEmailAsync(userInfo.Email);
            var claims = new List<Claim>
            {
                new Claim("email", userInfo.Email),
                new Claim("id", user.Id),
                new Claim("name", user.FullName),
            };
            var claimsDb = await _userManager.GetClaimsAsync(user);
            claims.AddRange(claimsDb);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddHours(8);
            var token = new JwtSecurityToken(issuer: _configuration["jwt:issuer"], audience: null, claims: claims, expires: expiration, signingCredentials: creds);
            return new UserTokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        public bool ValidateToke(string token)
        {
            try
            {
                var tokeValidatorParams = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                           Encoding.UTF8.GetBytes(_configuration["jwt:key"])),
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _configuration["jwt:issuer"],
                };
                token = token.Replace("Bearer ", string.Empty);
                token = token.Replace("bearer ", string.Empty);
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, tokeValidatorParams, out var validatedtoke);
                return validatedtoke is not null;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
