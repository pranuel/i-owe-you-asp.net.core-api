using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using I.Owe.You.Api.Repository;

namespace I.Owe.You.Api.Model
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApiContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApiContext>>()))
            {
                // Look for any users.
                if (context.Users.Any())
                {
                    return; // DB has been seeded
                }

                context.Users.Add(new User
                {
                    Name = "Darth Vader",
                    Sub = "test sub 2",
                    PhotoUrl = "https://pbs.twimg.com/profile_images/680042601693732865/rW4LJDkG_400x400.jpg"
                });
                context.SaveChanges();
            }
        }
    }
}