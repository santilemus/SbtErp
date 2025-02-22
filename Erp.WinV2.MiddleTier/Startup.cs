﻿using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using DevExpress.ExpressApp.Security.Authentication.ClientServer;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using SBT.Apps.Erp.Module;
using System.Text.Json.Serialization;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Erp.WinV2.MiddleTier.JWT;

namespace SBT.Apps.Erp.WinV2.MiddleTier;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IAuthenticationTokenProvider, JwtTokenProviderService>();

        services.AddXafMiddleTier(Configuration, builder =>
        {
            builder.ConfigureDataServer(options =>
            {
                options.UseConnectionString(Configuration.GetConnectionString("ConnectionString"));
                options.AllowICommandChannelDoWithSecurityContext(true);
                options.UseDataStorePool(true);
            });

            builder.Modules
                .AddReports(options =>
                {
                    options.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportDataV2);
                })
                .Add<ErpModule>();

            builder.Security
                .UseIntegratedMode(options =>
                {
                    options.RoleType = typeof(PermissionPolicyRole);
                    // ApplicationUser descends from PermissionPolicyUser and supports the OAuth authentication. For more information, refer to the following topic: https://docs.devexpress.com/eXpressAppFramework/402197
                    // If your application uses PermissionPolicyUser or a custom user type, set the UserType property as follows:
                    options.UserType = typeof(Usuario);
                    // ApplicationUserLoginInfo is only necessary for applications that use the ApplicationUser user type.
                    // If you use PermissionPolicyUser or a custom user type, comment out the following line:
                    options.UserLoginInfoType = typeof(ApplicationUserLoginInfo);
                    options.UseXpoPermissionsCaching();
                    options.Events.OnSecurityStrategyCreated = securityStrategy =>
                    {
                        ((SecurityStrategy)securityStrategy).RegisterXPOAdapterProviders();
                        ((SecurityStrategy)securityStrategy).AnonymousAllowedTypes.Add(typeof(Empresa));
                        ((SecurityStrategy)securityStrategy).AnonymousAllowedTypes.Add(typeof(EmpresaUnidad));
                        //((SecurityStrategy)securityStrategy).AnonymousAllowedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifference));
                        //((SecurityStrategy)securityStrategy).AnonymousAllowedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifferenceAspect));
                        ((SecurityStrategy)securityStrategy).AnonymousAllowedTypes.Add(typeof(Usuario));
                        ((SecurityStrategy)securityStrategy).AnonymousAllowedTypes.Add(typeof(ZonaGeografica));
                        ((SecurityStrategy)securityStrategy).AnonymousAllowedTypes.Add(typeof(Moneda));
                        ((SecurityStrategy)securityStrategy).AnonymousAllowedTypes.Add(typeof(ClaseSociedad));
                        ((SecurityStrategy)securityStrategy).Authentication = new CustomAuthentication();
                        securityStrategy.ReloadPermissions();
                    };
                })
                // agregado SELM 22/07/2023
                .AddAuthenticationProvider<AuthenticationStandardProviderOptions, CustomAuthenticationStandardProvider>(options =>
                {
                    options.IsSupportChangePassword = true;
                    options.LogonParametersType = typeof(CustomLogonParameters);
                })
                .AddPasswordAuthentication(options =>
                {
                    options.IsSupportChangePassword = true;
                });

            builder.AddBuildStep(application =>
            {
                application.ApplicationName = "SBT ERP Middle Tier Security";
                application.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
                //application.CreateCustomModelDifferenceStore += += (s, e) => {
                //    e.Store = new ModelDifferenceDbStore((XafApplication)sender!, typeof(ModelDifference), true, "Win");
                //    e.Handled = true;
                //};
#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached && application.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
                {
                    application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
                    application.DatabaseVersionMismatch += (s, e) =>
                    {
                        e.Updater.Update();
                        e.Handled = true;
                    };
                }
#endif
            });
        });

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = Configuration["Authentication:Jwt:Issuer"],
                    //ValidAudience = Configuration["Authentication:Jwt:Audience"],
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:Jwt:IssuerSigningKey"]))
                };
            });

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireXafAuthentication()
                    .Build();
        });

        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SBT ERP Middle Tier API",
                Version = "v1",
                Description = @"MiddleTier"
            });
            c.AddSecurityDefinition("JWT", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.Http,
                Name = "Bearer",
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme() {
                            Reference = new OpenApiReference() {
                                Type = ReferenceType.SecurityScheme,
                                Id = "JWT"
                            }
                        },
                        new string[0]
                    },
            });
        });
        // agregado por SELM
        services.AddControllersWithViews()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true;
            }
            );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SBT.Apps.Erp.WinV2 Api v1");
            });
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. To change this for production scenarios, see: https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseRequestLocalization();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseXafMiddleTier();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
