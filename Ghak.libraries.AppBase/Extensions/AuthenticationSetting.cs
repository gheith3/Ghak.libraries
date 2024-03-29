﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ghak.libraries.AppBase.DTO;
using Ghak.libraries.AppBase.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Ghak.libraries.AppBase.Extensions;

public static class AuthenticationSetting
{
    /**
     * <summary>
     * This method is used to add jwt token to the application
     * </summary>
     * <param name="services"></param>
     * <returns>The WebApplicationBuilder</returns>
     */
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        var secret =AppSettingsEntrance.GetFromAppSetting("JwtSettings:Secret");
        var issuer =AppSettingsEntrance.GetFromAppSetting("JwtSettings:Issuer");
        var audience =AppSettingsEntrance.GetFromAppSetting("JwtSettings:Audience");
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        return services;
    }
    
   
    public static LoginResponseDto UserLogin(List<Claim> authClaims)
    {
        try
        {
            var tokenLiveTime = AppSettingsEntrance.GetFromAppSetting("JwtSettings:TokenLiveTime");
            var refreshTokenLiveTime = AppSettingsEntrance.GetFromAppSetting("JwtSettings:RefreshTokenLiveTime");
            var issuer =AppSettingsEntrance.GetFromAppSetting("JwtSettings:Issuer");
            var audience =AppSettingsEntrance.GetFromAppSetting("JwtSettings:Audience");
            var secret =AppSettingsEntrance.GetFromAppSetting("JwtSettings:Secret");

            var tokenExpires = DateTime.UtcNow.AddHours(Convert.ToInt32(tokenLiveTime));
            var refreshTokenExpires = DateTime.UtcNow.AddHours(Convert.ToInt32(refreshTokenLiveTime));
            var claimIdentity = new ClaimsIdentity(authClaims);
            var tokenKey = Encoding.UTF8.GetBytes(secret);
            
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);
            
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = audience,
                Issuer = issuer,
                Subject = claimIdentity,
                Expires = tokenExpires,
                SigningCredentials = signingCredentials
            };
            
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);
            
            return new LoginResponseDto
            {
                Token = token,
                ExpiredAt = tokenExpires,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiredAt = refreshTokenExpires
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"token generating error => {e.Message}");
        }
        
        throw new Exception("Error in generating token");
    }
    
    

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}