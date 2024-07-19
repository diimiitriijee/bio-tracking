using System.Text;
using API.Services;
using Domain;
using Domain.src;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<Korisnik>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.User.RequireUniqueEmail = true;

        }).AddRoles<AppRole>()
          .AddRoleManager<RoleManager<AppRole>>()
          .AddEntityFrameworkStores<DataContext>() //da kreira sve tabele povezane sa identitijem
          .AddSignInManager<SignInManager<Korisnik>>() // NOVO: Dodajemo SignInManager
          .AddDefaultTokenProviders(); // NOVO: Dodajemo default token providere

        //NOVI deo
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = IdentityConstants.ApplicationScheme; // Dodajemo ovo za Identity.Application
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            opt.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chat")))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        })
        .AddCookie(IdentityConstants.ApplicationScheme, options =>
        {
            options.LoginPath = "/account/login";
            options.LogoutPath = "/account/logout";
            options.AccessDeniedPath = "/account/access-denied";
        });

        //pravim polise za autorizaciju koje ce da se koriste iznad funkcija da se odredi ko sme da pozove fju
        services.AddAuthorization(options => {
            options.AddPolicy("RequireAdministratorRole", policy => 
                policy.RequireRole("Administrator"));
            options.AddPolicy("RequireTuristickiVodicRole", policy => 
                policy.RequireRole("TuristickiVodic"));
            options.AddPolicy("RequireObicanKorisnikRole", policy => 
                policy.RequireRole("ObicanKorisnik"));
            options.AddPolicy("RequireObicanKorisnikRoleOrTuristickiVodicRole", policy => 
                policy.RequireRole("ObicanKorisnik", "TuristickiVodic"));
            options.AddPolicy("RequireAdministratorRoleOrTuristickiVodicRole", policy => 
                policy.RequireRole("Administrator", "TuristickiVodic"));
        });

        services.AddScoped<TokenService>();
        return services;
    }
}