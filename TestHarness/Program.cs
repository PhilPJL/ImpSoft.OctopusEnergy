// See https://aka.ms/new-console-template for more information
using CommunityToolkit.Diagnostics;
using ImpSoft.OctopusEnergy.Api;
using Microsoft.Extensions.Configuration;
using System.Reflection;

var configuration = new ConfigurationBuilder().AddUserSecrets(Assembly.GetExecutingAssembly()).Build();

var apiKey = configuration["ApiKey"];
Guard.IsNotNullOrWhiteSpace(apiKey);

var electricityMprn = configuration["ElectricityMprn"];
Guard.IsNotNullOrWhiteSpace(electricityMprn);

using var httpClient = new HttpClient();

// Optionally set the base address. If not set it will default to https://api.octopus.energy
//httpClient.BaseAddress = OctopusEnergyClient.DefaultBaseAddress;

// Optionally configure authentication using your api key.  If not set api methods that require it will fail (gas/elec consumption).
httpClient.SetAuthenticationHeaderFromApiKey(apiKey);

var octopusClient = new OctopusEnergyClient(httpClient);

var result = await octopusClient.GetGridSupplyPointByMpanAsync(electricityMprn);

Console.WriteLine(result.ToString());
