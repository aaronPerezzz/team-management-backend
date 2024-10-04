﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.AccessControl;
using Microsoft.Identity.Client;

namespace team_management_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));


            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(
            //            Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
            //        ClockSkew = TimeSpan.Zero
            //    });


            /**
             *Codigo para autenticar en Azure
             **/
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(options =>
                {
                    Configuration.Bind("AzureAd", options);
                    options.Events = new JwtBearerEvents();
                    options.TokenValidationParameters.NameClaimType = "name";
                },
                options => { Configuration.Bind("AzureAd", options); });

            services.Configure<OpenIdConnectOptions>(
                OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters.ValidateLifetime = true;
                    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                });

            services.AddSwaggerGen(swagger =>
            {
                var scopes = Configuration.GetValue<string>("AzureAd:Scopes");
                swagger.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Description = "API protegida",
                   Flows = new OpenApiOAuthFlows
                   {
                       Implicit = new OpenApiOAuthFlow
                       {
                           AuthorizationUrl = new Uri("https://login.microsoftonline.com/102d3653-c8a4-4711-a5a3-7dc0ab963878/oauth2/v2.0/authorize"),
                           Scopes = new Dictionary<string, string>
                           {
                               { "api://b65e313f-5391-4991-9dd4-ac957a196bac/WriteOnly", "Write" },
                               { "api://b65e313f-5391-4991-9dd4-ac957a196bac/ReadOnly", "Read"}
                           }

                       }
                   }
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        },
                        Scheme = "oautg2",
                        Name = "oautg2",
                        In = ParameterLocation.Header

                    },
                    new string[]{"Id","Description"}
                    }
                });
            });
            
            services.AddControllers();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //codigo para azure active identity
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
