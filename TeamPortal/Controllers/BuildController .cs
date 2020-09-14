using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeamPortal.Back.Models;
using TeamPortal.Services;

namespace TeamPortal.Back.Controllers
{
    [Authorize]
    public class BuildController : Controller
    {
        private readonly ILogger<BuildController> _logger;
        private readonly IBuildService _buildService;

        public BuildController(ILogger<BuildController> logger, IBuildService buildService)
        {
            _logger = logger;
            _buildService = buildService;
        }

        [ResponseCache(CacheProfileName = "Default1min")]
        public IActionResult Index()
        {
            var buildInformations = _buildService.GetBuildInformations().Result;
            var releaseInformations = _buildService.GetReleaseInformations().Result;
            return View("index", (buildInformations, releaseInformations));
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
