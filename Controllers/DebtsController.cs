using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using I.Owe.You.Api.Model;
using I.Owe.You.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace I.Owe.You.Api.Controllers
{
    public class DebtsController : BaseController
    {
        private DebtsRepo _debtsRepo { get; }

        public DebtsController(DebtsRepo debtsRepo)
        {
            _debtsRepo = debtsRepo;
        }

        // GET api/debts
        [HttpGet]
        public async Task<Debt[]> GetAll()
        {
            var debts = await _debtsRepo.GetAllDebtsAsync();
            return debts;
        }

        // GET api/debts/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/debts
        [HttpPost]
        public async Task Post(Debt debt)
        {
            await _debtsRepo.AddDebtAsync(debt);
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
