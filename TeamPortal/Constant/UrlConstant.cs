namespace TeamPortal.Constant
{
    public static class Url
    {
        public static class AzureDevOps
        {
            public static string GitPrApiBaseUrl => Startup.StaticConfig["AzureDevOps:BaseUrl"] + "/{0}/_apis/git/repositories/{1}/pullrequests?api-version=6.0";
            public static string GitPrBaseUrl => Startup.StaticConfig["AzureDevOps:BaseUrl"] + "/{0}/_git/{1}/pullrequest/{2}";
            public static string GitPrPolicyApiUrl => Startup.StaticConfig["AzureDevOps:BaseUrl"] + "/{0}/_apis/policy/evaluations?artifactId=vstfs:///CodeReview/CodeReviewId/{1}/{2}";
            public static string GitBrancheApiUrl => Startup.StaticConfig["AzureDevOps:BaseUrl"] + "/{0}/_apis/git/repositories/{1}/stats/branches";
            public static string GitBrancheUrl => Startup.StaticConfig["AzureDevOps:BaseUrl"] + "/{0}/_git/{1}?version=GB{2}";
            public static string BuildSonarqubeUrl => Startup.StaticConfig["AzureDevOps:BaseUrl"] + @"/CRM/_apis/build/definitions?path=\SonarQube&api-version=6.0";
            public static string BuildDefinitionUrl => Startup.StaticConfig["AzureDevOps:BaseUrl"] + @"/CRM/_apis/build/latest/{0}?api-version=6.0-preview.1";
            public static string ReleaseAppsUrl => @"https://vsrm.dev.azure.com/corum-am/CRM/_apis/release/definitions?path=\Apps&api-version=6.0";
            public static string ReleaseDefinitionUrl => @"https://vsrm.dev.azure.com/corum-am/CRM/_apis/release/definitions/{0}?api-version=6.0";
            public static string ReleaseUrl => @"https://vsrm.dev.azure.com/corum-am/CRM/_apis/release/releases/{0}?api-version=6.0";
        }

        public static class GitLab
        {

            public static string GitBaseUrl => Startup.StaticConfig["Git:BaseUrl"] + "/api/v4/projects/{0}%2F{1}/merge_requests?state=opened";
            public static string GitMergeApprovalUrl => Startup.StaticConfig["Git:BaseUrl"] + "/api/v4/projects/{0}%2F{1}/merge_requests/{2}/approvals";
            public static string GitBrancheUrl => Startup.StaticConfig["Git:BaseUrl"] + "/api/v4/projects/{0}%2F{1}/repository/branches";
        }
    }
}
