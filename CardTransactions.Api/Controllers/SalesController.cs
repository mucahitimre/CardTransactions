using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CardTransactions.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] SalesRequestModel request)
        {

        }
    }

    public class SalesRequestModel
    {
        public string CardNumber { get; set; }

        public string CardFullName { get; set; }

        public string CardEndDate { get; set; }

        public int CardSecurityNumber { get; set; }

        public double Price { get; set; }
    }
}

