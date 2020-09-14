using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TeamPortal.Constant;
using TeamPortal.Models;

namespace TeamPortal.Extension
{
    public static class MergeRequestExtension
    {
        public async static Task UpdateApprovedParameter(this IEnumerable<MergeRequestModel> MergeRequestsModel, (string, string) projectRepository, HttpClient client)
        {
            foreach (var currentMergeRequest in MergeRequestsModel)
            {
                var urlApproval = string.Format(Url.Git.GitMergeApprovalUrl, projectRepository.Item1, projectRepository.Item2, currentMergeRequest.iid);
                var approval = await client.GetObjectAsync<ApprovalModel>(urlApproval);
                currentMergeRequest.IsApproved = approval.approved;
            }
        }
    }
}
