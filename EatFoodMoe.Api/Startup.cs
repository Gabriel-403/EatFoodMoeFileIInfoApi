using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EatFoodMoe.Api.Data;
using EatFoodMoe.Api.Extends;
using EatFoodMoe.Api.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace EatFoodMoe.Api
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment HostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<UserDbContext>(options => options.UseSqlite("Data Source=eat_food_user_db.sqlite3"));
            services.AddDbContext<FileDbContext>(options => options.UseSqlite("Data Source=eat_food_db.sqlite3"));
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IClaimsService, EatFoodClaimService>();

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<AppUser>();

            services.Configure<IdentityOptions>(option =>
            {
                option.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
            });

            if (HostEnvironment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins, configure =>
                {
                    configure.WithOrigins("http://localhost:3000", "http://localhost:3001");
                    configure.AllowAnyHeader();
                    configure.AllowAnyMethod();
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = "Token";
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:5000";
                options.Audience = "user_api";
                options.SaveToken = true;
                options.MapInboundClaims = true;
            }).AddCookie("token");

            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
