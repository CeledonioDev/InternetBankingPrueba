using Core.Models;
using InternetBanking.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using System;

using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;
using Core.Manager;
using Core.Ports.Repositories;
using Persistence.Repositories;
using Core.Services;
using InternetBanking.Services;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using InternetBanking.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;


namespace InternetBanking
{
    public class Startup
    {
        private const string SecretKey = "ea2cd8b52c122734c4c4efe9a8a2aaac"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
              .AddJsonOptions(opts =>
              {
                  opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
              });

            services.AddSingleton(Configuration);

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwIssuerOptions));
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddControllersWithViews()
                .AddNewtonsoftJson(setupAction: options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    });

            services.AddDbContext<InternetBankingContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("InternetBankingDatabase"), builder => builder.EnableRetryOnFailure(maxRetryCount: 5)));

            services.Configure<JwIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.AccountNumber, Constants.Strings.JwtClaims.ApiAccess));
            });

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            AddSwagger(services);
            BuildManagerToScope(services);
            BuildRepositoriesToScope(services);
            //BuildServicesToScope(services);


        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Internet Banking {groupName}",
                    Version = groupName,
                    Description = "Internet Banking API",
                });
            });
        }

    

        private void BuildRepositoriesToScope(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<IAccountLogRepository, AccountLogRepository>();
            services.AddScoped<IJwtFactory, JwtFactory>();
        }

        private void BuildManagerToScope(IServiceCollection services)
        {
            services.AddScoped<UserManager, UserManager>();
            services.AddScoped<UserAccountManager, UserAccountManager>();
            services.AddScoped<JwIssuerOptions, JwIssuerOptions>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(
           builder =>
           {
               builder.Run(
                   async context =>
                   {
                       context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                       context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                       var error = context.Features.Get<IExceptionHandlerFeature>();
                       if (error != null)
                       {
                           context.Response.AddApplicationError(error.Error.Message);
                           await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                       }
                   });
           });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Internet API V1");
            });

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
