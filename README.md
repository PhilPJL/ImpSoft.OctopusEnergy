# Octopus Energy client API

A simple .NET Core and .NET Standard client for the Octopus Energy's API https://developer.octopus.energy/docs/api/.

## Usage
``` C#
async Task GetElectricityConsumption()
{
  var key = "<APIKEY>";

  // create an authenticated client API with access to electricity and gas consumption
  // and all public information
  var api = ClientFactory.Create(key);

  var from = new DateTimeOffset(2020, 05, 01, 00, 00, 00, TimeSpan.FromHours(1));
  var to = new DateTimeOffset(2020, 05, 11, 23, 59, 00, TimeSpan.FromHours(1));

  var consumption = await api.GetElectricityConsumptionAsync(
    "<mpan>", "<meter serial>", from, to, Interval.Day);
    
  consumption.ToList()
    .ForEach(c => Console.WriteLine($"[{c.Start.ToLocalTime()}-{c.End.ToLocalTime()}), {c.Quantity:00.00}"));
}

async Task GetAgileRates()
{
  // create a non-authenticated client with access to public information only
  var api = ClientFactory.Create();
	
  // there should be one GSP (in this case '_C')
  //var gsp = (await api.GetGridSupplyPointsAsync("SW16 2GY")).SingleOrDefault();
	
  // alternatively retrieve the GSP using the 'mpan'
  var gsp = await api.GetGridSupplyPointAsync("1030051828896");
	
  if(gsp == null){
    // handle error
  }
	
  // the current agile tariff
  var productCode = "AGILE-18-02-21";
  var agile = await api.GetProductAsync(productCode);

  // get the tariff for GSP _C
  var tariffCode = agile.SingleRegisterElectricityTariffs.ForGsp(gsp.GroupId).Monthly.Code;

  var from = new DateTimeOffset(2020, 03, 12, 00, 00, 00, TimeSpan.FromHours(0));
  var to = new DateTimeOffset(2020, 03, 12, 23, 59, 00, TimeSpan.FromHours(0));

  // retrieve agile tariff rates for desired period
  var agileRates = await api.GetElectricityUnitRatesAsync(
    productCode, tariffCode, ElectricityUnitRate.Standard, from, to);
	
  agileRates
    .Select(r => new {r.ValidFrom, r.ValueIncludingVAT})
    .OrderBy(r => r.ValidFrom)
    .ToList()
    .ForEach(r => Console.WriteLine($"{r.ValidFrom} - {(r.ValueIncludingVAT/100):C4}"));
}

```
