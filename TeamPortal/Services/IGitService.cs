using System.Collections.Generic;
using System.Threading.Tasks;
using TeamPortal.Models;

namespace TeamPortal.Services
{
    public interface IGitService
    {
        Task<IEnumerable<MergeRequestModel>> GetMergeRequestInformations();
        Task<IEnumerable<BranchModel>> GetBranches();
    }
}
