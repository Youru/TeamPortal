using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TeamPortal.Constant;
using TeamPortal.Extension;
using TeamPortal.Models;

namespace TeamPortal.Services
{
    public class BuildService : IBuildService
    {
        private readonly IConfiguration Configuration;
        private string AuthenticationToken => Convert.ToBase64String(Encoding.ASCII.GetBytes($":{Configuration["AzureDevOps:AccessToken"]}"));
        public BuildService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<IEnumerable<BuildDefinitionModel>> GetBuildInformations()
        {
            var buildList = await ContextExecution<BuildDefinitionModel>(async (client) =>
            {
                var SonarBuildIds = await GetSonarQubeBuild(client);
                return await GetBuildInformations(client, SonarBuildIds);
            });

            return buildList;
        }
        public async Task<IEnumerable<ReleaseModel>> GetReleaseInformations()
        {
            var releaseList = await ContextExecution<ReleaseModel>(async (client) =>
            {
                var releaseDefinitionIds = await GetReleaseDefinitionIds(client);
                return await GetReleaseInformations(client, releaseDefinitionIds);
            });

            return releaseList;
        }
        
        private async Task<List<int>> GetSonarQubeBuild(HttpClient client)
        {
            var buildDefinitionIdsList = new List<int>();

            var currentBuildList = await client.GetObjectAsync<BuildResponseModel>(Url.AzureDevOps.BuildSonarqubeUrl);
            var buildDefinitionIds = currentBuildList.value.Select(b => b.id).ToList();
            buildDefinitionIdsList.AddRange(buildDefinitionIds);

            return buildDefinitionIdsList;
        }
        private async Task<List<BuildDefinitionModel>> GetBuildInformations(HttpClient client, List<int> SonarBuildIds)
        {
            var buildList = new List<BuildDefinitionModel>();

            foreach (var sonarBuildId in SonarBuildIds)
            {
                var url = string.Format(Url.AzureDevOps.BuildDefinitionUrl, sonarBuildId);
                var currentBuild = await client.GetObjectAsync<BuildDefinitionModel>(url);
                buildList.Add(currentBuild);
            }

            return buildList;
        }
        private async Task<List<int>> GetReleaseDefinitionIds(HttpClient client)
        {
            var buildDefinitionIdsList = new List<int>();

            var currentBuildList = await client.GetObjectAsync<ReleaseResponseModel>(Url.AzureDevOps.ReleaseAppsUrl);
            var buildDefinitionIds = currentBuildList.value.Select(b => b.id).ToList();

            buildDefinitionIdsList.AddRange(buildDefinitionIds);

            return buildDefinitionIdsList;
        }
        private async Task<List<ReleaseModel>> GetReleaseInformations(HttpClient client, List<int> releaseDefinitionIds)
        {
            var releaseList = new List<ReleaseModel>();

            foreach (var releaseDefinitionId in releaseDefinitionIds)
            {
                var url = string.Format(Url.AzureDevOps.ReleaseDefinitionUrl, releaseDefinitionId);

                var releaseDefinition = await client.GetObjectAsync<ReleaseDefinitionModel>(url);
                var currentRelease = await GetReleaseFinistTime(client, releaseDefinition);

                releaseList.Add(currentRelease);
            }


            return releaseList;
        }
        private async Task<ReleaseModel> GetReleaseFinistTime(HttpClient client, ReleaseDefinitionModel releaseDefinition)
        {
            var statusNotStarted = "notStarted";

            var url = string.Format(Url.AzureDevOps.ReleaseUrl, releaseDefinition.lastRelease.id);
            var currentRelease = await client.GetObjectAsync<ReleaseModel>(url);
            currentRelease.environments = currentRelease.environments.Where(e => e.status != statusNotStarted);

            return currentRelease;
        }
        private async Task<IEnumerable<T>> ContextExecution<T>(Func<HttpClient, Task<IEnumerable<T>>> action)
        {
            List<T> result = new List<T>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthenticationToken);
                result.AddRange(await action(client));
            }

            return result;
        }
    }
}