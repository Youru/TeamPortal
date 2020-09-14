using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TeamPortal.Models;

namespace TeamPortal.Services
{
    public class BuildService : IBuildService
    {
        private readonly IConfiguration Configuration;
        private string buildSonarqubeUrl => Configuration["AzureDevOps:BaseUrl"] + @"/{Project}/_apis/build/definitions?path=\SonarQube&api-version=6.0";
        private string buildDefinitionUrl => Configuration["AzureDevOps:BaseUrl"] + @"/{Project}/_apis/build/latest/{0}?api-version=6.0-preview.1";
        private string releaseAppsUrl => @"https://vsrm.dev.azure.com/{orga}/{Project}/_apis/release/definitions?path=\Apps&api-version=6.0";
        private string releaseDefinitionUrl => @"https://vsrm.dev.azure.com/{orga}/{Project}/_apis/release/definitions/{0}?api-version=6.0";
        private string releaseUrl => @"https://vsrm.dev.azure.com/{orga}/{Project}/_apis/release/releases/{0}?api-version=6.0";
        private string AuthenticationToken => Convert.ToBase64String(Encoding.ASCII.GetBytes($":{Configuration["AzureDevOps:AccessToken"]}"));
        public BuildService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<List<BuildDefinitionModel>> GetBuildInformations()
        {
            var SonarBuildIds = await GetSonarQubeBuild();
            var buildList = await GetBuildInformations(SonarBuildIds);

            return buildList;
        }
        public async Task<List<ReleaseModel>> GetReleaseInformations()
        {
            var releaseDefinitionIds = await GetReleaseDefinitionIds();
            var releaseList = await GetReleaseInformations(releaseDefinitionIds);
            return releaseList;
        }
        private async Task<List<int>> GetSonarQubeBuild()
        {
            var buildDefinitionIdsList = new List<int>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthenticationToken);
                var response = await client.GetStringAsync(buildSonarqubeUrl);
                var currentBuildList = JsonConvert.DeserializeObject<BuildResponseModel>(response);
                var buildDefinitionIds = currentBuildList.value.Select(b => b.id).ToList();
                buildDefinitionIdsList.AddRange(buildDefinitionIds);
            }

            return buildDefinitionIdsList;
        }
        private async Task<List<BuildDefinitionModel>> GetBuildInformations(List<int> SonarBuildIds)
        {
            var buildList = new List<BuildDefinitionModel>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthenticationToken);
                foreach (var sonarBuildId in SonarBuildIds)
                {
                    var url = string.Format(buildDefinitionUrl, sonarBuildId);
                    var response = await client.GetStringAsync(url);
                    var currentBuild = JsonConvert.DeserializeObject<BuildDefinitionModel>(response);
                    buildList.Add(currentBuild);
                }
            }
            return buildList;
        }
        private async Task<List<int>> GetReleaseDefinitionIds()
        {
            var buildDefinitionIdsList = new List<int>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthenticationToken);
                var response = await client.GetStringAsync(releaseAppsUrl);
                var currentBuildList = JsonConvert.DeserializeObject<ReleaseResponseModel>(response);
                var buildDefinitionIds = currentBuildList.value.Select(b => b.id).ToList();
                buildDefinitionIdsList.AddRange(buildDefinitionIds);
            }

            return buildDefinitionIdsList;
        }
        private async Task<List<ReleaseModel>> GetReleaseInformations(List<int> releaseDefinitionIds)
        {
            var releaseList = new List<ReleaseModel>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthenticationToken);
                foreach (var releaseDefinitionId in releaseDefinitionIds)
                {
                    var url = string.Format(releaseDefinitionUrl, releaseDefinitionId);
                    var response = await client.GetStringAsync(url);
                    var releaseDefinition = JsonConvert.DeserializeObject<ReleaseDefinitionModel>(response);
                    var currentRelease = await GetReleaseFinistTime(releaseDefinition);
                    releaseList.Add(currentRelease);
                }
            }

            return releaseList;
        }
        private async Task<ReleaseModel> GetReleaseFinistTime(ReleaseDefinitionModel releaseDefinition)
        {
            var statusNotStarted = "notStarted";
            ReleaseModel currentRelease;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthenticationToken);
                var url = string.Format(releaseUrl, releaseDefinition.lastRelease.id);
                var response = await client.GetStringAsync(url);
                currentRelease = JsonConvert.DeserializeObject<ReleaseModel>(response);
                currentRelease.environments = currentRelease.environments.Where(e => e.status != statusNotStarted);
            }
            return currentRelease;
        }
    }
}


