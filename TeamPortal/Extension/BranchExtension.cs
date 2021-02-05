using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TeamPortal.Constant;
using TeamPortal.Models;

namespace TeamPortal.Extension
{
    public static class BranchExtension
    {
        public async static Task UpdateWebSite(this IEnumerable<BranchModel> branchesModel, (string, string) projectRepository)
        {
            foreach (var branch in branchesModel)
            {
                branch.web_url = string.Format(Url.AzureDevOps.GitBrancheUrl, projectRepository.Item1, projectRepository.Item2, branch.name);
            }
        }
    }
}
