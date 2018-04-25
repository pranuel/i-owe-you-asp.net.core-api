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

        // GET api/debtsgroups
        [HttpGet]
        public async Task<List<DebtsGroup>> GetAll()
        {
            var debtsGroups = await _debtsGroupRepo.GetAllDebtsSummariesForMeAsync(UserSub);
            return debtsGroups;
        }

        // GET api/debtsgroups/5
        [HttpGet("{id}")]
        public async Task<DebtsGroup> Get(int id)
        {
            var debtsGroup = await _debtsGroupRepo.GetDebtsSummaryByIdAsync(id, UserSub);
            return debtsGroup;
        }

    }
}
