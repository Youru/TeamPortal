namespace TeamPortal.Constant
{
    public static class Url
    {
        public static class AzureDevOps
        {
            public static string BuildSonarqubeUrl => Startup.StaticConfig["AzureDevOps:BaseUrl"] + @"/{project}/_apis/build/definitions?path=\SonarQube&api-version=6.0";
            public static string BuildDefinitionUrl => Startup.StaticConfig["AzureDevOps:BaseUrl"] + @"/{project}/_apis/build/latest/{0}?api-version=6.0-preview.1";
            public static string ReleaseAppsUrl => @"https://vsrm.dev.azure.com/{orga}/{project}/_apis/release/definitions?path=\Apps&api-version=6.0";
            public static string ReleaseDefinitionUrl => @"https://vsrm.dev.azure.com/{orga}/{project}/_apis/release/definitions/{0}?api-version=6.0";
            public static string ReleaseUrl => @"https://vsrm.dev.azure.com/{orga}/{project}/_apis/release/releases/{0}?api-version=6.0";
        }

        public static class Git
        {

            public static string GitBaseUrl => Startup.StaticConfig["Git:BaseUrl"] + "/api/v4/projects/{0}%2F{1}/merge_requests?state=opened";
            public static string GitMergeApprovalUrl => Startup.StaticConfig["Git:BaseUrl"] + "/api/v4/projects/{0}%2F{1}/merge_requests/{2}/approvals";
            public static string GitBrancheUrl => Startup.StaticConfig["Git:BaseUrl"] + "/api/v4/projects/{0}%2F{1}/repository/branches";
        }
    }
}
