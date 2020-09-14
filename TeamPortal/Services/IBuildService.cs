using System.Collections.Generic;
using System.Threading.Tasks;
using TeamPortal.Models;

namespace TeamPortal.Services
{
    public interface IBuildService
    {
        Task<IEnumerable<BuildDefinitionModel>> GetBuildInformations();
        Task<IEnumerable<ReleaseModel>> GetReleaseInformations();
    }
}