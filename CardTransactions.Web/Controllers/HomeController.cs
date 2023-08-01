using CardTransactions.Api.Abstractions;
using CardTransactions.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CardTransactions.Web.Controllers
{
    /// <summary>
    /// The home controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class HomeController : Controller
    {
        private readonly ISalesManager _salesManager;
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="salesManager">The sales manager.</param>
        /// <param name="logger">The logger.</param>
        public HomeController(
            ISalesManager salesManager,
            ILogger<HomeController> logger)
        {
            _salesManager = salesManager;
            _logger = logger;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var list = await _salesManager.GetListAsync(new SalesListFilter());

            return View(list);
        }

        /// <summary>
        /// Details the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(string id)
        {
            var item = (await _salesManager.GetListAsync(new SalesListFilter() { Id = id })).FirstOrDefault();

            return View(item);
        }
    }
}