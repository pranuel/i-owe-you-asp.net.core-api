using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using I.Owe.You.Api.Model;
using I.Owe.You.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace I.Owe.You.Api.Controllers
{
    public class UsersController : BaseController
    {
        private UsersRepo UsersRepo { get; }

        public UsersController(UsersRepo usersRepo)
        {
            UsersRepo = usersRepo;
        }

        // GET api/users/me
        [HttpGet("me")]
        public async Task<User> Me()
        {
            var me = await UsersRepo.GetUserBySubAsync(UserSub);
            return me;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
