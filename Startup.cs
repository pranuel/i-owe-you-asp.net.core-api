using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using I.Owe.You.Api.Model;
using I.Owe.You.Api.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace I.Owe.You.Api
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
            // Use SQL Database if in Azure, otherwise, use in memory
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                services.AddDbContext<ApiContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Database")));
                // Automatically perform database migration
                services.BuildServiceProvider().GetService<ApiContext>().Database.Migrate();
            }
            else
            {
                services.AddDbContext<ApiContext>(options =>
                    options.UseInMemoryDatabase("debts_database"));
            }

            services.AddScoped<DebtsRepo>();
            services.AddScoped<UsersRepo>();
            services.AddScoped<DebtsSummariesRepo>();

            services.AddMvc();

            // Details on authentication: https://auth0.com/docs/quickstart/backend/aspnet-core-webapi
            // 1. Add Authentication Services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = "https://pranuel.eu.auth0.com/";
                options.Audience = "https://iou.de";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var serviceProvider = serviceScope.ServiceProvider;
                    SeedData.Initialize(serviceProvider);
                }
            }

            app.UseCors(builder => builder
                .WithOrigins("http://localhost:8888")
                .AllowAnyMethod()
                .AllowAnyHeader()); ;

            // 2. Enable authentication middleware
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
