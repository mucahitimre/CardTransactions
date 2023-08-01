using CardTransactions.Api.Abstractions;
using CardTransactions.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CardTransactions.Api.Controllers
{
    /// <summary>
    /// The sales controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SalesController : ControllerBase
    {
        private readonly ISalesManager _salesManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesController"/> class.
        /// </summary>
        /// <param name="salesManager">The sales manager.</param>
        public SalesController(ISalesManager salesManager)
        {
            _salesManager = salesManager;
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        [HttpPost]
        public async Task<IActionResult> DoPayment([FromBody] SalesInsertModel request)
        {
            var response = await _salesManager.SaveAsync(request);

            return Ok(response);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] SalesListFilter request)
        {
            var documents = await _salesManager.GetListAsync(request);

            return Ok(documents);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">id parameter is null or empyt, please check the request.</exception>
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("id parameter is null or empty, please check the request.");
            }

            var document = await _salesManager.GetAsync(id);

            return Ok(document);
        }
    }
}