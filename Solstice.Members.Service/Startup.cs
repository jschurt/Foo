using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using Solstice.Infrastructure;
using Solstice.Utilities.Email;
using System;

namespace Solstice.Members.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public bool UseOpenApi { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            UseOpenApi = env.IsDevelopment() || env.IsQualityAnalysis();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSolsticeServiceStandardServices(authenticationOptions =>
            {
                authenticationOptions.TokenValidationOptions = Configuration
                    .GetSection("Jwt")
                    .Get<TokenValidationOptions>();
            });

            services.AddControllers();

            if (UseOpenApi)
            {
                // https://github.com/RicoSuter/NSwag/wiki/AspNetCore-Middleware
                services.AddOpenApiDocument(configure =>
                {
                    var assemblyName = GetType().Assembly.GetName();
                    configure.Title = assemblyName.Name;
                    configure.Version = assemblyName.Version.ToString();

                    configure.AddSecurity("Bearer", new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.OAuth2,
                        Flow = OpenApiOAuth2Flow.Password,
                        In = OpenApiSecurityApiKeyLocation.Header,
                        TokenUrl = Configuration["Jwt:TokenUrl"]
                    });

                    configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());
                });
            }

            services.AddSolsticeDalServices();
            services.AddMarketplaceDbServices(Configuration["ConnectionStrings:Marketplace"]);
            services.AddWorkbenchDbServices(Configuration["ConnectionStrings:Workbench"]);
            services.AddMySolsticeDbServices(Configuration["ConnectionStrings:MySolstice"]);
            services.AddMembersBusinessLogicServices(Configuration.GetSection("Settings:Members"));
            services.AddCommsBusinessLogicServices();
            services.AddApplicationInsightsTelemetry();

            services.AddProductsServiceClients(options =>
            {
                options.InheritAuthorizationHeader = true;
                options.BaseAddress = new Uri(Configuration["ServicesBaseAddress:Products"]);
            });

            services.Configure<SmtpClientOptions>(Configuration.GetSection(nameof(SmtpClientOptions)));
            services.AddScoped<SmtpClientWrapper>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSolsticeAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (UseOpenApi)
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }
        }
    }
}
