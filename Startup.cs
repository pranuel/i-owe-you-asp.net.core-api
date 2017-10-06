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
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("debts_database"));
            services.AddScoped<DebtsRepo>();
            services.AddScoped<UsersRepo>();

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
            }

            app.UseCors(builder => builder
                .WithOrigins("http://localhost:8888")
                .AllowAnyMethod()
                .AllowAnyHeader()); ;

            // 2. Enable authentication middleware
            app.UseAuthentication();

            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                var debtsRepo = serviceProvider.GetService<DebtsRepo>();
                var usersRepo = serviceProvider.GetService<UsersRepo>();
                AddTestDataAsync(debtsRepo, usersRepo).Wait();
            }
        }

        private async Task AddTestDataAsync(DebtsRepo debtsRepo, UsersRepo usersRepo)
        {
            var testUser1 = new User
            {
                Id = 1,
                FirstName = "Luke",
                LastName = "Skywalker",
                Sub = "facebook|1080925881970593"
            };
            await usersRepo.AddUserAsync(testUser1);

            var testUser2 = new User
            {
                Id = 2,
                FirstName = "Darth",
                LastName = "Vader",
                Sub = "test sub 2"
            };
            await usersRepo.AddUserAsync(testUser2);

            var testDebt1 = new Debt
            {
                Id = 1,
                DebtorId = testUser1.Id,
                CreditorId = testUser2.Id,
                Amount = 100,
                Reason = "build death star"
            };

            await debtsRepo.AddDebtAsync(testDebt1);
        }
    }
}
