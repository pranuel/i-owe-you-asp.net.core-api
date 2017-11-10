using System.Threading.Tasks;
using I.Owe.You.Api.Model;
using I.Owe.You.Api.Repository;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace I.Owe.You.Api.Tests
{

    public class UsersRepoTests
    {

        [Fact]
        public async Task Create_me()
        {
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase("Create_me")
                .Options;

            var sub = "test sub!";
            // Run the test against one instance of the context
            using (var context = new ApiContext(options))
            {
                var sut = new UsersRepo(context);
                var me = new User
                {
                    Sub = sub,
                    Name = "Yoda"
                };
                await sut.AddMeAsync(me, sub);
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApiContext(options))
            {
                var sut = new UsersRepo(context);
                var me = await sut.GetUserBySubAsync(sub);
                me.Name.Should().Be("Yoda");
            }
        }

    }

}