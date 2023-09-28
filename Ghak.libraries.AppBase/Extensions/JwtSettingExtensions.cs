using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ghak.libraries.AppBase.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Ghak.libraries.AppBase.Extensions;

public static class JwtSettingExtensions
{
    /**
     * <summary>
     * This method is used to add jwt token to the application
     * </summary>
     * <param name="builder">The WebApplicationBuilder</param>
     * <returns>The WebApplicationBuilder</returns>
     */
    public static WebApplicationBuilder JwtSetting(this WebApplicationBuilder builder)
    {
        var settingsSection = builder.Configuration.GetSection("JwtSettings");
        if (settingsSection == null)
        {
            throw new Exception("JwtSettings section in appsettings.json is not configured");
        }

        var tokenLiveTime = settingsSection.GetValue<string>("TokenLiveTime");
        var secret = settingsSection.GetValue<string>("Secret");
        var issuer = settingsSection.GetValue<string>("Issuer");
        var audience = settingsSection.GetValue<string>("Audience");

        if (string.IsNullOrEmpty(tokenLiveTime)
            || string.IsNullOrEmpty(secret)
            || string.IsNullOrEmpty(issuer)
            || string.IsNullOrEmpty(audience))
        {
            throw new Exception("JwtSettings should contain: TokenLiveTime, Secret, Issuer, Audience");
        }

        builder.Services.AddAuthentication(options =>
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
                };
            });

        return builder;
    }
    
   
    public static LoginResponseDto Login(string audience, string tokenLiveTime, List<Claim> authClaims)
    {
        try
        {
            var expires = DateTime.UtcNow.AddHours(Convert.ToInt32(tokenLiveTime));
            var claimIdentity = new ClaimsIdentity(authClaims);
            var tokenKey = Encoding.UTF8.GetBytes(audience);
            
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);
            
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimIdentity,
                Expires = expires,
                SigningCredentials = signingCredentials
            };
            
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);
            
            return new LoginResponseDto
            {
                Token = token,
                ExpiredAt = expires,
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"token generating error => {e.Message}");
        }
        
        throw new Exception("Error in generating token");
    }
}