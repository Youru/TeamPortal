using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeamPortal.Back.Models;
using TeamPortal.Services;
using static TeamPortal.Startup;

namespace TeamPortal.Back.Controllers
{
    [Authorize]
    public class GitController : Controller
    {
        private readonly ILogger<GitController> _logger;
        private readonly IGitService _gitLabService;
        private readonly IGitService _gitDevOpsService;

        public GitController(ILogger<GitController> logger, ServiceResolver service)
        {
            _logger = logger;
            _gitLabService = service(nameof(GitLabService));
            _gitDevOpsService = service(nameof(GitDevopsService));
        }

        [ResponseCache(CacheProfileName = "Default1min")]
        public IActionResult Index()
        {
            var mergeRequests = _gitLabService.GetMergeRequestInformations().Result.ToList();
            mergeRequests.AddRange(_gitDevOpsService.GetMergeRequestInformations().Result);


            var branches = _gitLabService.GetBranches().Result.ToList();
            branches.AddRange(_gitDevOpsService.GetBranches().Result);
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
