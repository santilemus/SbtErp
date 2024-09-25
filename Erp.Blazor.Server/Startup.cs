using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.Circuits;
using SBT.Apps.Erp.Blazor.Server.Services;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.DashboardAspNetCore;
using DevExpress.AspNetCore.Reporting;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Erp.Blazor.Server.Middleware;
using DevExpress.Blazor.Configuration;
using SBT.Apps.Base.Module;
using Microsoft.AspNetCore.HttpOverrides;
using SBT.Apps.Erp.Blazor.Server.Editors;
using SBT.Apps.Empleado.Module;

namespace SBT.Apps.Erp.Blazor.Server;

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
        services.AddSingleton(typeof(Microsoft.AspNetCore.SignalR.HubConnectionHandler<>), typeof(ProxyHubConnectionHandler<>));

        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddHttpContextAccessor();
        services.AddScoped<CircuitHandler, CircuitHandlerProxy>();
        services.Configure<GlobalOptions>(options => options.SizeMode = DevExpress.Blazor.SizeMode.Small);
        services.AddXaf(Configuration, builder =>
        {
            builder.UseApplication<BlazorBlazorApplication>();
            builder.Modules
                .AddAuditTrailXpo(options => { 
                    options.ObjectAuditingMode = DevExpress.Persistent.AuditTrail.ObjectAuditingMode.Lightweight; 
                    options.Enabled = false; 
                })
                .AddCloningXpo()
                .AddConditionalAppearance()
                .AddDashboards(options =>
                {
                    options.DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData);
                    options.HideDirectDataSourceConnections = false; // false para mostrar el enlace Create data source en el wizard
                    // agregado por SELM 13/07/2023
                    // mas info en https://docs.devexpress.com/eXpressAppFramework/117720/analytics/dashboards/dashboard-performance-with-large-data-sources
                    options.SetupDashboardConfigurator = (dashboardConfigurator, serviceProvider) =>
                    {
                        dashboardConfigurator.AllowExecutingCustomSql = true;
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        dashboardConfigurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(configuration));
                    };
                })
                .AddReports(options =>
                {
                    options.EnableInplaceReports = true;
                    options.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportDataV2);
                    options.ReportStoreMode = DevExpress.ExpressApp.ReportsV2.ReportStoreModes.XML;
                    options.ShowAdditionalNavigation = true;
                })
                .AddValidation()
                .AddViewVariants(options =>
                {
                    options.ShowAdditionalNavigation = true;
                })
                .Add<SBT.Apps.Erp.Module.ErpModule>()
                .Add<BlazorBlazorModule>();
            builder.ObjectSpaceProviders
                .AddSecuredXpo((serviceProvider, options) =>
                {
                    string connectionString = null;
                    if (Configuration.GetConnectionString("ConnectionString") != null)
                    {
                        connectionString = Configuration.GetConnectionString("ConnectionString");
                    }
#if EASYTEST
                    if(Configuration.GetConnectionString("EasyTestConnectionString") != null) {
                        connectionString = Configuration.GetConnectionString("EasyTestConnectionString");
                    }
#endif
                    ArgumentNullException.ThrowIfNull(connectionString);
                    options.ConnectionString = connectionString;
                    options.ThreadSafe = true;
                    options.UseSharedDataStoreProvider = true;
                    // agregado por SELM el 13/07/2023 para permitir ejecutar sentencias sql via Session
                    options.AllowICommandChannelDoWithSecurityContext = true;
                })
                .AddNonPersistent();
            builder.Security
                .UseIntegratedMode(options =>
                {
                    options.RoleType = typeof(PermissionPolicyRole);
                    // ApplicationUser descends from PermissionPolicyUser and supports the OAuth authentication. For more information, refer to the following topic: https://docs.devexpress.com/eXpressAppFramework/402197
                    // If your application uses PermissionPolicyUser or a custom user type, set the UserType property as follows:
                    options.UserType = typeof(SBT.Apps.Base.Module.BusinessObjects.Usuario);
                    // ApplicationUserLoginInfo is only necessary for applications that use the ApplicationUser user type.
                    // If you use PermissionPolicyUser or a custom user type, comment out the following line:
                    options.UserLoginInfoType = typeof(SBT.Apps.Base.Module.BusinessObjects.ApplicationUserLoginInfo);
                    options.UseXpoPermissionsCaching();
                    // agregado para permisos de empresa, sucursal y usuario
                    options.Events.OnSecurityStrategyCreated = securityStrategyBase =>
                    {
                        var securityStrategy = (SecurityStrategy)securityStrategyBase;
                        securityStrategy.AnonymousAllowedTypes.Add(typeof(SBT.Apps.Base.Module.BusinessObjects.Empresa));
                        securityStrategy.AnonymousAllowedTypes.Add(typeof(SBT.Apps.Base.Module.BusinessObjects.EmpresaUnidad));
                        securityStrategy.PermissionsReloadMode = PermissionsReloadMode.CacheOnFirstAccess;
                    };
                    options.Events.OnCustomizeSecurityCriteriaOperator = context =>
                    {
                        if (EmpresaActualOidFunction.CanEvaluate(context))
                        {
                            EmpresaActualOidFunction.Evaluate(context);
                            return;
                        }
                        if (AgenciaActualOidFunction.CanEvaluate(context))
                        {
                            AgenciaActualOidFunction.Evaluate(context);
                            return;
                        }
                        if (EmpleadoActualOidFunction.CanEvaluate(context))
                        {
                            EmpleadoActualOidFunction.Evaluate(context);
                            return;
                        }
                        // agregar aqui las otras funciones personalizadas y que son similares a la anterior (van a la bd)
                    };
                })
                .AddAuthenticationProvider<AuthenticationStandardProviderOptions, CustomAuthenticationStandardProvider>(options =>
                {
                    options.IsSupportChangePassword = true;
                    options.LogonParametersType = typeof(CustomLogonParameters);
                })
                .AddPasswordAuthentication(options =>
                {
                    options.IsSupportChangePassword = true;
                });
        });
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LoginPath = "/LoginPage";
            //options.Events.OnSignedIn = DoSignedIn;
        });

        // agregado por SELM el 13/07/2023, para habilitar consultas personalizadas en el diseñador de informes
        // más info: https://supportcenter.devexpress.com/ticket/details/t1062867/how-to-enable-custom-sql-in-xaf-blazor-report-designer
        services.ConfigureReportingServices(configurator =>
        {
            configurator.ConfigureReportDesigner(designer =>
            {
                designer.RegisterDataSourceWizardConfigFileConnectionStringsProvider();
                designer.EnableCustomSql();
                designer.RegisterDataSourceWizardConfigFileConnectionStringsProvider();
                designer.RegisterSqlDataSourceWizardCustomizationService<CustomSqlDataSourceWizardCustomizationService>();
            });
        });

        services.AddCors(options =>
        {
            // esta politica no es correcta para produccion
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
            /// politica especifica de ejemplo, adecuarla si se va a utilizar
            options.AddPolicy("Upload", policy =>
                policy.WithOrigins("http://localhost")
                .WithMethods("GET", "POST")
                .WithHeaders("Header1", "Header2")
            );
        });

        // Agregado el 06/sept/2024 por SELM
        // XPO Profiller, solo para pruebas de identificación de problemas de desempeño
        DevExpress.Xpo.Logger.ILogger logger = new DevExpress.Xpo.Logger.LoggerBase(5000);
        DevExpress.Xpo.Logger.LogManager.SetTransport(logger);
        services.AddSingleton((DevExpress.Xpo.Logger.Transport.ILogSource)logger);
    }

    private Task DoSignedIn(CookieSignedInContext context)
    {
        var xyz = context.Properties.Items;
        return Task.CompletedTask;
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
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
        app.UseCors();  // agregado por SELM 18/ago/2024
        app.UseAuthorization();
        app.UseXaf();
        app.UseMiddleware<ExportMiddleware>();
        app.UseMiddleware<UploadFileMiddleware>();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapXafEndpoints();
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
            endpoints.MapControllers();
        });
    }
}
