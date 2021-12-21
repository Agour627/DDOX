using AutoMapper;
using DDOX.API.Core.Services;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Helpers;
using DDOX.API.Infrastructure.Configurations;
using DDOX.API.Repository.Data;
using DDOX.API.Repository.Repositories;
using DDOX.API.Repository.Repositories.Interfaces;
using DDOX.API.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API
{
    public static class ServicesConfigurations
    {
        public static void ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration Configuration)
        {
            var builder = new NpgsqlConnectionStringBuilder()
            {
                Database = Configuration["DatabaseString:Database"],
                Host = Configuration["DatabaseString:Server"],
                Port = Int32.Parse(Configuration["DatabaseString:Port"]),
                Username = Configuration["DatabaseString:Username"],
                Password = Configuration["DatabaseString:Password"],
            };
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(builder.ConnectionString,
                                  x => x.MigrationsAssembly("DDOX.API.Repository"));
            });
            services.AddHealthChecks().AddNpgSql(builder.ConnectionString);
        }
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<TokenData>();
            services.AddScoped<ExtensionsValidator>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IFolderService, FolderService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IExtensionService, ExtensionService>();
            services.AddTransient<IIndexService, IndexService>();
            services.AddTransient<IPartitionSevice, PartitionSevice>(); 
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPartitionSevice, PartitionSevice>(); 
            services.AddTransient<IExtensionService, ExtensionService>();
            services.AddTransient<IPageService, PageService>();
            services.AddTransient<IRestrictionService, RestrictionService>(); 

        }
        public static void ConfigureEncryptSettings(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<EncryptSettings>(options => options.EncryptionKey = Configuration["EncryptionKey"]);

        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DDOX",
                    Version = "v1",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Authorization",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
       
        }


        
    }
}
