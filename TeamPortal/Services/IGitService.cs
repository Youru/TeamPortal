using System.Collections.Generic;
using System.Threading.Tasks;
using TeamPortal.Models;

namespace TeamPortal.Services
{
    public interface IGitService
    {
        Task<List<MergeRequestModel>> GetMergeRequestInformations();
        Task<List<BranchModel>> GetBranches();
    }
}
