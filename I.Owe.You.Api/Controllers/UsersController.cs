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
        private UsersRepo _usersRepo { get; }

        public UsersController(UsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        // GET api/users/me
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var me = await _usersRepo.GetUserBySubAsync(UserSub);
            if (me == null)
            {
                return NotFound();
            }
            return Ok(me);
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _usersRepo.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST api/users/me
        [HttpPost("me")]
        public async Task<User> PostAsync(User user)
        {
            await this._usersRepo.AddMeAsync(user, UserSub);
            var me = await _usersRepo.GetUserBySubAsync(UserSub);
            return me;
        }

        // GET api/users
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name)
        {
            var users = await _usersRepo.GetAllUsers();
            return Ok(users);
        }
    }
}
