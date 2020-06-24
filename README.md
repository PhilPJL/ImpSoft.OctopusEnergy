# Octopus Energy client API

A simple .NET Core and .NET Standard client for the Octopus Energy's API https://developer.octopus.energy/docs/api/.

## Usage
``` C#
		var key = "<APIKEY>";

    // create a private client API with access to electricity and gas consumption
		var api = ClientFactory.Create(key);

		var from = new DateTimeOffset(2020, 05, 01, 00, 00, 00, TimeSpan.FromHours(1));
		var to = new DateTimeOffset(2020, 05, 11, 23, 59, 00, TimeSpan.FromHours(1));

		var consumption = await api.GetElectricityConsumptionAsync("<mpan>", "<meter serial>", from, to, Interval.Day);
```
