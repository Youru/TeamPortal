using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TeamPortal.Constant;
using TeamPortal.Extension;
using TeamPortal.Models;

namespace TeamPortal.Services
{
    public class GitService : IGitService
    {
        private readonly IConfiguration Configuration;
        private List<(string, string)> ProjectRepositoryList => new List<(string, string)>
        {

        };
        public GitService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<IEnumerable<MergeRequestModel>> GetMergeRequestInformations() =>
            await ContextExecutionMultiRepo(async (client, url, projectRepository) =>
            {
                var currentMergeRequestList = await client.GetObjectAsync<IEnumerable<MergeRequestModel>>(url);
                await currentMergeRequestList.UpdateApprovedParameter(projectRepository, client);

                return currentMergeRequestList;
            }, Url.Git.GitBaseUrl);
        public async Task<IEnumerable<BranchModel>> GetBranches()
        {
            var branches = await ContextExecutionMultiRepo(async (client, url, projectRepository) =>
            {
                var currentBranches = await client.GetObjectAsync<IEnumerable<BranchModel>>(url);
                currentBranches.ToList().ForEach(cb => cb.Repository = projectRepository.Item2);

                return currentBranches.Where(b => IsFeatureBranches(b.name));
            }, Url.Git.GitBrancheUrl);

            return branches.OrderBy(b => b.commit.created_at);

            bool IsFeatureBranches(string name) => !BranchToAvoid().Any(bta => bta.ToLower().Contains(name.ToLower()));
            List<string> BranchToAvoid() => new List<string>() { "dev", "master" };
        }
        
        private async Task<IEnumerable<T>> ContextExecutionMultiRepo<T>(Func<HttpClient, string, (string, string), Task<IEnumerable<T>>> action, string url)
        {
            List<T> result = new List<T>();
            foreach (var projectRepository in ProjectRepositoryList)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("PRIVATE-TOKEN", Configuration["Git:AccessToken"]);
                    var currentUrl = string.Format(url, projectRepository.Item1, projectRepository.Item2);
                    result.AddRange(await action(client, currentUrl, projectRepository));
                }
            }
            return result;
        }
    }
}
