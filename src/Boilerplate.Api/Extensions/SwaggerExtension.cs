using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Boilerplate.Api.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddApiDoc(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Boilerplate.Api",
                        Version = "v1",
                        Description = "API Boilerplate",
                        Contact = new OpenApiContact
                        {
                            Name = "Yan Pitangui",
                            Url = new Uri("https://github.com/yanpitangui")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT",
                            Url = new Uri("https://github.com/yanpitangui/dotnet-api-boilerplate/blob/main/LICENSE")
                        }
                    });
                c.DescribeAllParametersInCamelCase();
                c.OrderActionsBy(x => x.RelativePath);

                var xmlfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlfile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                // To Enable authorization using Swagger (JWT)    
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });

            });
            return services;
        }

        public static IApplicationBuilder UseApiDoc(this IApplicationBuilder app)
        {
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.RoutePrefix = "api-docs";
                   c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                   c.DocExpansion(DocExpansion.List);
               });
            return app;
        }
    }
}
