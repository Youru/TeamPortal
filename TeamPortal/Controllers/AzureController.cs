using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace TeamPortal.Controllers
{
    public class AzureController : Controller
    {
        private readonly ITokenAcquisition _tokenAcquisition;

        public AzureController(ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
        }

        public IActionResult Index()
        {
            readToken();
            return View();
        }

        private async Task readToken()
        {
            //var test = AzureEnvironment
            string[] scopes = new string[] { "https://management.azure.com/user_impersonation" };
            //string[] scopes = new string[] { "https://graph.windows.net/User.Read" };
            try
            {
                //var accessToken = new TokenAcquisitionTokenCredential(_tokenAcquisition);
                var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
                var toto = accessToken;
            }
            catch (Exception ex)
            {
                var toto = ex;
            }
        }

        public void Restart()
        {
            var url = @"https://management.azure.com/subscriptions/02958f63-63db-46fc-8cd1-3174cbbc25b1/providers/Microsoft.Web/sites?api-version=2019-08-01";
            GetAllSite(url);
        }

        private async Task GetAllSite(string url)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            using (var client = new HttpClient())
            {
                var sites = await client.GetAsync(url);
            }
        }
    }
}
