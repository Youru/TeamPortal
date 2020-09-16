using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace TeamPortal.Back
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) =>
        {
            var builtConfig = config.Build();

            KeyVaultClient kvClient = new KeyVaultClient(async (authority, resource, scope) =>
            {
                var adCredential = new ClientCredential(builtConfig["KeyVault:ClientId"], builtConfig["KeyVault:ClientSecret"]);
                var authenticationContext = new AuthenticationContext(authority, null);
                return (await authenticationContext.AcquireTokenAsync(resource, adCredential)).AccessToken;
            });
            config.AddAzureKeyVault(new AzureKeyVaultConfigurationOptions
            {
                Vault = $"https://{builtConfig["KeyVault:Vault"]}.vault.azure.net/",
                Client = kvClient,
                ReloadInterval = TimeSpan.FromSeconds(20)
            });
        })
        .UseStartup<Startup>();
    }
}
