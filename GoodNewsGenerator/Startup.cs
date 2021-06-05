using GoodNewsGenerator.Models.Data;
using GoodNewsGenerator.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Interfaces_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using GoodNewsGenerator_Implementation_Services;
using EntityGeneratorNews.Data;
using Serilog;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using GoodNewsGenerator_Implementation_Services.RulesForeAutoMapper;
using DTO_Models_For_GoodNewsGenerator;
using Microsoft.Extensions.Options;

namespace GoodNewsGenerator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfConectionString = new ConfigurationBuilder().AddJsonFile("ConnectionString.json").Build();
        }

        
        private IConfiguration Configuration { get; }
        private IConfiguration ConfConectionString { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => new CookieAuthenticationOptions()
                {
                    LoginPath = new PathString("/Account/Login"),
                    ExpireTimeSpan = TimeSpan.FromMinutes(5),
                    AccessDeniedPath = new PathString("/Account/Login")
                });// настройка авторизациии на основе куки и  указание пути в случае если identituClame User == null

            services.AddDbContext<DbContextNewsGenerator>(options => options.UseSqlServer(ConfConectionString.GetConnectionString("DefaultConnection")));

            services.AddScoped<IRepository<News>, NewsRepository>();
            services.AddScoped<IRepository<Comment>, CommentRepository>();
            services.AddScoped<IRepository<Role>, RoleRepository>();
            services.AddScoped<IRepository<Source>, SourceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddTransient<SputnikParser>(); // выбераем Transient так как при каждом запросе нам необходим новый объект TutByParser
            services.AddTransient<OnlinerParser>();
            services.AddTransient<belta>();

            MapperConfiguration mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping()); // настраиваем конфигурации мапера передавая фаил с правилами мапинга
            });

            IMapper mapper = mapperConfig.CreateMapper(); // передаём маперу настройки конфигурации

            services.AddSingleton(mapper); // добавляем настроеный объект автомапера в DI  



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
