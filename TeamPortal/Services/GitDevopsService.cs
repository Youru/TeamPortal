using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TeamPortal.Constant;
using TeamPortal.Extension;
using TeamPortal.Models;

namespace TeamPortal.Services
{
    public class GitDevopsService : IGitService
    {
        private readonly IConfiguration Configuration;
        private string AuthenticationToken => Convert.ToBase64String(Encoding.ASCII.GetBytes($":{Configuration["AzureDevOps:AccessToken"]}"));
        private List<(string, string)> ProjectRepositoryList => new List<(string, string)>
        {
            ("Corum.Data","Corum.Data"),
            ("CRM","CRM"),
            ("Abraham","Abraham"),
            ("CRM.Dynamics","CRM.Dynamics"),
            ("dynamics-onprem","dynamics-onprem")
        };
        public GitDevopsService(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public async Task<IEnumerable<MergeRequestModel>> GetMergeRequestInformations() =>
            await ContextExecution(async (client, url, projectRepository) =>
            {
                var mergeRequestDevops = await client.GetObjectAsync<MergeRequestModelDevops>(url);
                await mergeRequestDevops.UpdateApprovedDevOpsParameter(projectRepository, client);
                var mergeRequest = await GetMergeRequestModel(mergeRequestDevops);
                await mergeRequest.UpdateWebSiteDevOpsParameter(projectRepository);

                return mergeRequest;
            }, Url.AzureDevOps.GitPrApiBaseUrl);

        private async Task<IEnumerable<MergeRequestModel>> GetMergeRequestModel(MergeRequestModelDevops mergeRequestModelDevops)
        {
            var mergeRequest = new List<MergeRequestModel>();

            foreach (var pr in mergeRequestModelDevops.Value)
            {
                mergeRequest.Add(pr.MapValueToMergeRequest());
            }
            return mergeRequest;
        }

        public async Task<IEnumerable<BranchModel>> GetBranches() =>
            await ContextExecution(async (client, url, projectRepository) =>
            {
                var branchesRequest = await client.GetObjectAsync<BranchDevOpsModel>(url);
                var branch = await GetBranchModel(branchesRequest);
                await branch.UpdateWebSite(projectRepository);
                return branch;
            }, Url.AzureDevOps.GitBrancheApiUrl);

        private async Task<IEnumerable<BranchModel>> GetBranchModel(BranchDevOpsModel branchModelDevops)
        {
            var branches = new List<BranchModel>();

            foreach (var br in branchModelDevops.Value)
            {
                branches.Add(br.MapValueToBranch());
            }
            return branches;
        }

        private async Task<IEnumerable<T>> ContextExecution<T>(Func<HttpClient, string, (string, string), Task<IEnumerable<T>>> action, string url)
        {
            List<T> result = new List<T>();


            foreach (var projectRepository in ProjectRepositoryList)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthenticationToken);
                    var currentUrl = string.Format(url, projectRepository.Item1, projectRepository.Item2);
                    result.AddRange(await action(client, currentUrl, projectRepository));
                }
            }

            return result;
        }
    }
}
