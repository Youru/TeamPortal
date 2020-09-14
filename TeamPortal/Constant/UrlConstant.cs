using Microsoft.Extensions.Configuration;

namespace TeamPortal.Constant
{
    public class Url
    {
        private static IConfiguration Configuration;
        public Url(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static class AzureDevOps
        {
            public static string BuildSonarqubeUrl => Configuration["AzureDevOps:BaseUrl"] + @"/{Project}/_apis/build/definitions?path=\SonarQube&api-version=6.0";
            public static string BuildDefinitionUrl => Configuration["AzureDevOps:BaseUrl"] + @"/{Project}/_apis/build/latest/{0}?api-version=6.0-preview.1";
            public static string ReleaseAppsUrl => @"https://vsrm.dev.azure.com/{orga}/{Project}/_apis/release/definitions?path=\Apps&api-version=6.0";
            public static string ReleaseDefinitionUrl => @"https://vsrm.dev.azure.com/{orga}/{Project}/_apis/release/definitions/{0}?api-version=6.0";
            public static string ReleaseUrl => @"https://vsrm.dev.azure.com/{orga}/{Project}/_apis/release/releases/{0}?api-version=6.0";
        }

        public static class Git
        {

            public static string GitBaseUrl => Configuration["Git:BaseUrl"] + "/api/v4/projects/{0}%2F{1}/merge_requests?state=opened";
            public static string GitMergeApprovalUrl => Configuration["Git:BaseUrl"] + "/api/v4/projects/{0}%2F{1}/merge_requests/{2}/approvals";
            public static string GitBrancheUrl => Configuration["Git:BaseUrl"] + "/api/v4/projects/{0}%2F{1}/repository/branches";
        }
    }
}
