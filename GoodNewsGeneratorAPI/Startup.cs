using AutoMapper;
using CQRSandMediatorForApi.Command;
using CQRSandMediatorForApi.CommandHandlers;
using CQRSandMediatorForApi.Queries;
using CQRSandMediatorForApi.QueryHandlers;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.Data;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Implementation_Services;
using GoodNewsGenerator_Implementation_Services.RulesForeAutoMapper;
using GoodNewsGenerator_Interfaces_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

           
            //News Hendler
            services.AddScoped<IRequestHandler<GetNewsByIdQueriy, NewsModelDTO> , GetNewsByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllNewsQueriy, IEnumerable<NewsModelDTO>>, GetAllNewsQueryHandler>();
            services.AddScoped<IRequestHandler<GetUrlFromNewsQueriy, IEnumerable<string>>, GetUrlFromNewsQueriyHendler>();
            services.AddScoped<IRequestHandler<AddNewsCommand, int>, AddNewsCommandHendler>();
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
            //Service
            services.AddScoped<INewsCQRService, NewsCQRService>();
            services.AddScoped<ISourceCQRService, SourceSQRService>();
            services.AddScoped<IRoleCQRService, RoleCQRService>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GoodNewsGeneratorAPI", Version = "v1" });
            });

           

            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoodNewsGeneratorAPI v1"));
            }

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
