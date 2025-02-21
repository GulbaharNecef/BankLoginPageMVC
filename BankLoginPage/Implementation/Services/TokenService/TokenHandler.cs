﻿using BankLoginPage.Abstraction.Services.TokenService;
using BankLoginPage.DTOs;
using BankLoginPage.Models.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BankLoginPage.Implementation.Services.TokenService
{
    public class TokenHandler : ITokenHandler
    {
        IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public TokenHandler(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<TokenDTO> CreateAccessTokenAsync(int minute, AppUser user)
        {
            TokenDTO token = new();
            //Security Key'in simmetrigini aliriq
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            //Sifrelenmis kimligi olusturuyoruz
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            //login olan userin rollarini databazadan getirirem, sonra o rollari claim kimi tokene elave edirem 
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            //Olusturulacak token ayarlarini veriyoruz
            token.Expiration = DateTime.UtcNow.AddMinutes(minute);
            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                signingCredentials: signingCredentials,
                expires: token.Expiration,
                notBefore: DateTime.UtcNow, //Token uretildiyi andan ne qeder sonra devreye girsin?now .AddMinutes(1) desem token 1 deqiqe sonra devreye girer, tokenin timesinden cixilir ama
                claims: claims
                // claims: new List<Claim> { new(ClaimTypes.Name, user.UserName) }
                );

            //Token olusturucu sinifindan bir ornek alalim
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            return token;

        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
