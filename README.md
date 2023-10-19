# Octopus Energy client API

A simple .NET Core and .NET Standard client for the Octopus Energy's API https://developer.octopus.energy/docs/api/.

## Usage
``` C#
// Simple console app example
public static async Task Main()
{
    // Get your api key, maybe from user secrets
    var apiKey = "abcdefg....";

    using var httpClient = new HttpClient();

    // Optionally set the HttpClient base address. 
    // If not set it will default to OctopusEnergyClient.DefaultBaseAddress = https://api.octopus.energy
    httpClient.BaseAddress = OctopusEnergyClient.DefaultBaseAddress;

    // Optionally configure authentication using your api key.  
    // If the key is not set then api methods that require it will fail (gas/electricity consumption).
    httpClient.SetAuthenticationHeaderFromApiKey(apiKey);

    // Create the api wrapper
    var octopusClient = new OctopusEnergyClient(httpClient);

    var account = await octopusClient.GetAccountAsync(accountId);

    ...
}

async Task GetElectricityConsumption(IOctopusEnergyClient client)
{
  var from = new DateTimeOffset(2020, 05, 01, 00, 00, 00, TimeSpan.FromHours(1));
  var to = new DateTimeOffset(2020, 05, 11, 23, 59, 00, TimeSpan.FromHours(1));

  var consumption = await client.GetElectricityConsumptionAsync(
    "<mpan>", "<meter serial>", from, to, Interval.Day);
    
  consumption.ToList()
    .ForEach(c => Console.WriteLine(
      $"[{c.Start.ToLocalTime()}-{c.End.ToLocalTime()}), {c.Quantity:00.00}"));
}

async Task GetAgileRates(IOctopusEnergyClient client)
{
  // Retrieve GSP for postcode (in this case "_C".  If 0 or more than 1 GSP is returned an exception will be thrown.
  var gsp = (await client.GetGridSupplyPointByPostcodeAsync("SW16 2GY"));
	
  // Alternatively retrieve the GSP using the 'mpan'.
  //var gsp = await client.GetGridSupplyPointByMpanAsync("<mpan>");
	
  // the current agile tariff
  var productCode = "AGILE-18-02-21";
  var agile = await client.GetProductAsync(productCode);

  // get the tariff for GSP _C
  var tariffCode = agile.SingleRegisterElectricityTariffs.ForGsp(gsp).Monthly.Code;

  // get sample quote info
  var quote = agile.SampleQuotes.ForGsp(gsp);

  var from = new DateTimeOffset(2020, 03, 12, 00, 00, 00, TimeSpan.FromHours(0));
  var to = new DateTimeOffset(2020, 03, 12, 23, 59, 00, TimeSpan.FromHours(0));

  // retrieve agile tariff rates for desired period
  var agileRates = await client.GetElectricityUnitRatesAsync(
    productCode, tariffCode, from, to, ElectricityUnitRate.Standard);
	
  agileRates
    .Select(r => new {r.ValidFrom, r.ValueIncludingVAT})
    .OrderBy(r => r.ValidFrom)
    .ToList()
    .ForEach(r => Console.WriteLine($"{r.ValidFrom} - {(r.ValueIncludingVAT/100):C4}"));
}

```

The project uses one or more icons made by <a href="https://www.flaticon.com/authors/flat-icons" title="Flat Icons">Flat Icons</a> from <a href="https://www.flaticon.com/" title="Flaticon"> www.flaticon.com</a>
