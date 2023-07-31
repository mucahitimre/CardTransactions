using CardTransactions.Api.Abstractions;
using CardTransactions.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CardTransactions.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SalesController : ControllerBase
    {
        private readonly ISalesManager _salesManager;

        public SalesController(ISalesManager salesManager)
        {
            _salesManager = salesManager;
        }

        [HttpPost]
        public async Task Post([FromBody] SalesInsertModel request)
        {
            await _salesManager.SaveAsync(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] SalesListModel request)
        {
            var documents = await _salesManager.GetListAsync(request);

            return Ok(documents);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("id parameter is null or empyt, please check the request.");
            }

            var document = await _salesManager.GetAsync(id);

            return Ok(document);
        }
    }
}