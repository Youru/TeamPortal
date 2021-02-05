using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TeamPortal.Constant;
using TeamPortal.Models;

namespace TeamPortal.Extension
{
    public static class MergeRequestExtension
    {
        private const string APPROVED_STATUS = "approved";

        public async static Task UpdateApprovedParameter(this IEnumerable<MergeRequestModel> MergeRequestsModel, (string, string) projectRepository, HttpClient client)
        {
            foreach (var currentMergeRequest in MergeRequestsModel)
            {
                var urlApproval = string.Format(Url.GitLab.GitMergeApprovalUrl, projectRepository.Item1, projectRepository.Item2, currentMergeRequest.iid);
                var approval = await client.GetObjectAsync<ApprovalModel>(urlApproval);
                currentMergeRequest.IsApproved = approval.approved;
            }
        }
        public async static Task UpdateApprovedDevOpsParameter(this MergeRequestModelDevops MergeRequestsModel, (string, string) projectRepository, HttpClient client)
        {
            foreach (var currentMergeRequest in MergeRequestsModel.Value)
            {
                var urlApproval = string.Format(Url.AzureDevOps.GitPrPolicyApiUrl, projectRepository.Item1, currentMergeRequest.repository.project.id, currentMergeRequest.pullRequestId);
                var approvals = await client.GetObjectAsync<ValueApprovalDevopsModel>(urlApproval);
                currentMergeRequest.IsApproved = !approvals.value.Any(a => a.status != APPROVED_STATUS);
            }
        }

        public async static Task UpdateWebSiteDevOpsParameter(this IEnumerable<MergeRequestModel> MergeRequestsModel, (string, string) projectRepository)
        {
            foreach (var currentMergeRequest in MergeRequestsModel)
            {
                currentMergeRequest.web_url = string.Format(Url.AzureDevOps.GitPrBaseUrl, projectRepository.Item1, projectRepository.Item2, currentMergeRequest.iid);
            }
        }
    }
}
