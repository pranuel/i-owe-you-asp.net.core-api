using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using I.Owe.You.Api.Model;
using I.Owe.You.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace I.Owe.You.Api.Controllers
{
    public class DebtsSummariesController : BaseController
    {
        private DebtsSummariesRepo _debtsSummariesRepo { get; }

        public DebtsSummariesController(DebtsSummariesRepo debtsSummariesRepo)
        {
            _debtsSummariesRepo = debtsSummariesRepo;
        }

        // GET api/debtssummaries
        [HttpGet]
        public async Task<List<DebtsSummary>> GetAll()
        {
            return await _debtsSummariesRepo.GetAllDebtsSummariesForMeAsync(UserSub);
        }

        // GET api/debtssummaries/5
        [HttpGet("{id}")]
        public async Task<DebtsSummary> GetAsync(int id)
        {
            return await _debtsSummariesRepo.GetDebtsSummaryByIdAsync(id);
        }

    }
}
