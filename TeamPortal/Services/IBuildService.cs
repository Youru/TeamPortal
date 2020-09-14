using System.Collections.Generic;
using System.Threading.Tasks;
using TeamPortal.Models;

namespace TeamPortal.Services
{
    public interface IBuildService
    {
        Task<List<BuildDefinitionModel>> GetBuildInformations();
        Task<List<ReleaseModel>> GetReleaseInformations();
    }
}