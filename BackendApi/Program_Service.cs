
using BackendApi.Service;
using DbEf;
using FluentValidation;
using Ges;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model.Util;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;

namespace BackendApi
{
    public class Program_Service
    {
        public static void Configure(WebApplicationBuilder builder)
        {
          

            //json, configuracion de ignore cycles
            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            //cors, declaracion de politicas
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(configuration =>
                {
                    configuration.AllowAnyOrigin()
                           .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


         

            //entity framework
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                var dataBaseConnection = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(dataBaseConnection,b => b.MigrationsAssembly("BackendApi"));
            });


            //automapper
            builder.Services.AddAutoMapper(Assembly.Load("Dto"));


            //fluent validation
            builder.Services.AddValidatorsFromAssembly(Assembly.Load("Ges"));


            //mediator
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(Assembly.Load("Ges"));

                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });


            //swagger
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new()
                    {
                        Title = "MAUI - Backend",
                        Version = "v1",
                        Description = "How to use minimal API in NET 9",
                        Contact = new OpenApiContact
                        {
                            Email = "hachch1984@gmail.com",
                            Name = "Henry Chavez",
                            Url = new System.Uri("https://www.linkedin.com/in/hachch1984/")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT",
                            Url = new System.Uri("https://opensource.org/licenses/MIT")
                        }
                    });

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Bearer {token}"
                    });



                    options.OperationFilter<SwaggerEndPointFilter>();

                });
            }





            //token
            builder.Services.AddAuthentication()
                .AddJwtBearer(opciones =>
                {

                    var jwtTokenSigningKey = builder.Configuration["JwtTokenSigningKey"]!;
                    var key = new SymmetricSecurityKey(Convert.FromBase64String(jwtTokenSigningKey));

                    opciones.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
            //token policies
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Authorization_CustomPolicy.IsAdmin, policy => policy.RequireClaim(Authorization_CustomPolicy.IsAdmin));
                options.AddPolicy(Authorization_CustomPolicy.IsUser, policy => policy.RequireClaim(Authorization_CustomPolicy.IsUser));
            });




            //signalR
            builder.Services.AddSignalR();

            builder.Services.AddHostedService<NotificationBackground_Service>();




            //configuracion de output cache
            builder.Services.AddOutputCache();

        }
    }



    public class SwaggerEndPointFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().Any() == false)
            {
                return;
            }

            operation.Security = new List<OpenApiSecurityRequirement>{new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    } };

        }
    }


}
