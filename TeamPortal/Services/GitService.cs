using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TeamPortal.Models;

namespace TeamPortal.Services
{
    public class GitService : IGitService
    {
        private readonly IConfiguration Configuration;
        private string gitBaseUrl => Configuration["Git:BaseUrl"]+"/api/v4/projects/{0}%2F{1}/merge_requests?state=opened";
        private string gitMergeApprovalUrl => Configuration["Git:BaseUrl"]+"/api/v4/projects/{0}%2F{1}/merge_requests/{2}/approvals";
        private string gitBrancheUrl => Configuration["Git:BaseUrl"]+"/api/v4/projects/{0}%2F{1}/repository/branches";
        private List<(string, string)> ProjectRepositoryList => new List<(string, string)>
        {
            
        };

        public GitService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<List<MergeRequestModel>> GetMergeRequestInformations()
        {
            var mergeRequestList = new List<MergeRequestModel>();
            foreach (var projectRepository in ProjectRepositoryList)
            {
                using (var client = new HttpClient())
                {
                    var url = string.Format(gitBaseUrl, projectRepository.Item1, projectRepository.Item2);
                    client.DefaultRequestHeaders.Add("PRIVATE-TOKEN", Configuration["Git:AccessToken"]);
                    var response = await client.GetStringAsync(url);
                    var currentMergeRequestList = JsonConvert.DeserializeObject<List<MergeRequestModel>>(response);
                    await SetApprovedParameter(projectRepository, client, currentMergeRequestList);
                    mergeRequestList.AddRange(currentMergeRequestList);
                }
            }

            return mergeRequestList;
        }

        public async Task<List<BranchModel>> GetBranches()
        {
            var branches = new List<BranchModel>();
            foreach (var projectRepository in ProjectRepositoryList)
            {
                using (var client = new HttpClient())
                {
                    var url = string.Format(gitBrancheUrl, projectRepository.Item1, projectRepository.Item2);
                    client.DefaultRequestHeaders.Add("PRIVATE-TOKEN", Configuration["Git:AccessToken"]);
                    var response = await client.GetStringAsync(url);
                    var currentBranches = JsonConvert.DeserializeObject<List<BranchModel>>(response);
                    currentBranches.ForEach(cb => cb.Repository = projectRepository.Item2);

                    branches.AddRange(currentBranches);
                }
            }

            branches = branches.Where(b => IsFeatureBranches(b.name)).OrderBy(b => b.commit.created_at).ToList();

            return branches;

            bool IsFeatureBranches(string name) => !BranchToAvoid().Any(bta => bta.ToLower().Contains(name.ToLower()));
            List<string> BranchToAvoid() => new List<string>() { "dev", "master" };
        }

        private async Task SetApprovedParameter((string, string) projectRepository, HttpClient client, List<MergeRequestModel> currentMergeRequestList)
        {
            foreach (var currentMergeRequest in currentMergeRequestList)
            {

                var urlApproval = string.Format(gitMergeApprovalUrl, projectRepository.Item1, projectRepository.Item2, currentMergeRequest.iid);
                var responseApproval = await client.GetStringAsync(urlApproval);
                var approval = JsonConvert.DeserializeObject<ApprovalModel>(responseApproval);
                currentMergeRequest.IsApproved = approval.approved;
            }
        }
    }
}
