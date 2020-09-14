using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeamPortal.Back.Models;
using TeamPortal.Services;

namespace TeamPortal.Back.Controllers
{
    [Authorize]
    public class GitController : Controller
    {
        private readonly ILogger<GitController> _logger;
        private readonly IGitService _gitService;

        public GitController(ILogger<GitController> logger, IGitService gitService)
        {
            _logger = logger;
            _gitService = gitService;
        }

        [ResponseCache(CacheProfileName = "Default1min")]
        public IActionResult Index()
        {
            var mergeRequests = _gitService.GetMergeRequestInformations().Result;
            var branches = _gitService.GetBranches().Result;
            return View("index", (mergeRequests, branches));
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
