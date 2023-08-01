using CardTransactions.Api.Abstractions;
using CardTransactions.Domain.Models;
using CardTransactions.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CardTransactions.Web.Controllers
{
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

        public IActionResult Index()
        {
            var list = _salesManager.GetListAsync(new SalesListFilter());

            return View(list);
        }

        public async Task<IActionResult> GetData()
        {
            var list = await _salesManager.GetListAsync(new SalesListFilter());

            return Json(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}