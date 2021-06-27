using AutoMapper;
using CQRSandMediatorForApi.Command;
using CQRSandMediatorForApi.CommandHandlers;
using CQRSandMediatorForApi.Queries;
using CQRSandMediatorForApi.QueryHandlers;
using DTO_Models;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.Data;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Implementation_Services;
using GoodNewsGenerator_Implementation_Services.RulesForeAutoMapper;
using GoodNewsGenerator_Interfaces_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using GoodNewsGeneratorAPI.JWT;
using GoodNewsGeneratorAPI.Models.JwtModel;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGeneratorAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<DbContextNewsGenerator>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            var JwtSection = Configuration.GetSection("JWT"); // �������� ������ � ����������� ��� jwt
            services.Configure<JwtOptions>(JwtSection); // ������������ ��������� ������������ � ������� ����������� �������� ������ JwtOptions � �������� json JwtSection

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // ������������� ����������� �� ��������� "Bearer"
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // ������������� ����� ������� ����� ���������� �� �������� "Bearer"
            })
             .AddJwtBearer(options =>
             {
                
                 options.RequireHttpsMetadata = false;//�������� ������������� https 
                 options.SaveToken = true; // ��������� ��������� ����� � ������� HttpClient
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = false, // ��������� ���� ������� ��������
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"])),//������������� ����� ���������� ��� ��������
                     ValidateIssuer = true, //������� ��� ����� ������������ ��������
                     ValidateAudience = true,// ������� ��� ����� ������������ ����������
                     ValidateLifetime = true,// ��������� ����� ����� ������
                     ValidIssuer = "NewsGenerator", // ������������� ��� ��������
                     ValidAudience = "User",// ������������� ��� ���������� ������
                     ClockSkew = TimeSpan.Zero,// ������� ����� �������� ����� ����� ��������� �������� � �������������
                 };//��������� ��������� ���������� ������
             });

            //��������� hangfire
            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
            {
              CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),//������������ ����� �������� ������ ������
              SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5), // �������� ������ ������� ������
              QueuePollInterval = TimeSpan.Zero, // �������� ������ �������
              UseRecommendedIsolationLevel = true, //������������ ������������� ����������� ���
              DisableGlobalLocks = true // ��������� ���������� ���������� 
            }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            //News Hendler
            services.AddScoped<IRequestHandler<GetNewsByIdQueriy, NewsModelDTO>, GetNewsByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllNewsQueriy, IEnumerable<NewsModelDTO>>, GetAllNewsQueryHandler>();
            services.AddScoped<IRequestHandler<GetUrlFromNewsQueriy, IEnumerable<string>>, GetUrlFromNewsQueriyHendler>();
            services.AddScoped<IRequestHandler<AddNewsCommand, int>, AddNewsCommandHendler>();
            services.AddScoped<IRequestHandler<UpdateNewsCommand, int>, UpdateNewsCommandHendler>();
            //rssSource Hendler
            services.AddScoped<IRequestHandler<GetAllRssSourceQueriy, IEnumerable<SourceModelDTO>>, GetAllRssSourceQueriyHendler>();
            services.AddScoped<IRequestHandler<GetRssSourceByIdQueriy, SourceModelDTO>, GetRssSourceByIdQueriyHendler>();
            services.AddScoped<IRequestHandler<AddRssSourceCommand, int>, AddRssSourceCommandHendler>();
            services.AddScoped<IRequestHandler<DeleteRssSourceByIdCommand, int>, DeleteRssSourceByIdCommandHendler>();
            //RoleHendler
            services.AddScoped<IRequestHandler<GetAllUserWithRole, IEnumerable<UserModelDTO>>, GetAllUserWithRoleHendler>();
            services.AddScoped<IRequestHandler<GetRoleByIdQueriy, RoleModelDTO>, GetRoleByIdQueriyHendler>();
            services.AddScoped<IRequestHandler<AddRoleCommand, int>, AddRoleCommandHendler>();
            services.AddScoped<IRequestHandler<DeleteRoleCommand, int>, DeleteRoleCommandHendler>();
            services.AddScoped<IRequestHandler<GetUserByIdQueriy, UserModelDTO>, GetUserByIdQueriyHendler>();
            //UserHendler
            services.AddScoped<IRequestHandler<AddUserCommand, int>, AddUserCommandHendler>();
            services.AddScoped <IRequestHandler<GetUserByQueriy, UserModelDTO>, GetUserByQueriyHendler>();
            services.AddScoped<IRequestHandler<GetEmailUserByRefresgTokenKeyQueriy, string>, GetEmailUserByRefresgTokenKeyQueriyHendler>();
            //RefreshTokenHendler
            services.AddScoped<IRequestHandler<GetRefreshTokenByKeyQueriy, RefreshTokenModelDTO>, GetRefreshTokenByKeyQueriyHendler>();
            services.AddScoped<IRequestHandler<AddRefreshTokenCommand, int>, AddRefreshTokenCommandHendler>();
            //Service
            services.AddScoped<INewsCQRService, NewsCQRService>();
            services.AddScoped<ISourceCQRService, SourceSQRService>();
            services.AddScoped<IRoleCQRService, RoleCQRService>();
            services.AddScoped<IUserCQRService, UserCQRService>();
            services.AddScoped<IJwtAuthManager, GenerateTokens>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GoodNewsGeneratorAPI", Version = "v1" });
            });



            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping()); // ������������� ����� �������� � ���� ��������� ������� ��� ����������
            });
            IMapper mapper = mappingConfig.CreateMapper(); // ������ ������ ����������

            services.AddSingleton(mapper); // ������� ������ ���������� �  IoC ���������
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoodNewsGeneratorAPI v1"));
            }

            var NewsCQRService = provider.GetService<INewsCQRService>();
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            RecurringJob.AddOrUpdate(() => NewsCQRService.CoefficientPositivity(), "* * * * *");
            RecurringJob.AddOrUpdate(() => NewsCQRService.GetNewsFromRssSource(), "0 12 * * *");

            app.UseStaticFiles();//������������� ����������� ������ �� wwwroot
            app.UseHttpsRedirection();// ������������� https ��������� 
            app.UseRouting();// ������������ �������������
            app.UseAuthentication();// ������������ ��������������               
            app.UseAuthorization();// ����������� 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });//������������ �������� ����� � ������������� 

        }
    }
}
