using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using I.Owe.You.Api.Model;
using I.Owe.You.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace I.Owe.You.Api.Controllers
{
    public class DebtsGroupsController : BaseController
    {
        private DebtsGroupRepo _debtsGroupRepo { get; }

        public DebtsGroupsController(DebtsGroupRepo debtsGroupRepo)
        {
            _debtsGroupRepo = debtsGroupRepo;
        }

        // GET api/debtssummaries
        [HttpGet]
        public async Task<List<DebtsGroup>> GetAll()
        {
            return await _debtsGroupRepo.GetAllDebtsSummariesForMeAsync(UserSub);
        }

        // GET api/debtssummaries/5
        [HttpGet("{id}")]
        public async Task<DebtsGroup> GetAsync(int id)
        {
            return await _debtsGroupRepo.GetDebtsSummaryByIdAsync(id, UserSub);
        }

    }
}
