using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("ImpSoft.OctopusEnergy.Api.Tests")]
[assembly: InternalsVisibleTo("TestHarness")]

namespace ImpSoft.OctopusEnergy.Api;

[JsonSerializable(typeof(ProductDetail))]
[JsonSerializable(typeof(GridSupplyPoint))]
[JsonSerializable(typeof(MeterPointGridSupplyPoint))]
[JsonSerializable(typeof(GridSupplyPointInfo))]
[JsonSerializable(typeof(PagedResults<Consumption>))]
[JsonSerializable(typeof(PagedResults<GridSupplyPointInfo>))]
[JsonSerializable(typeof(PagedResults<GridSupplyPoint>))]
[JsonSerializable(typeof(PagedResults<Charge>))]
[JsonSerializable(typeof(PagedResults<Product>))]
[JsonSerializable(typeof(Account))]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal partial class OctopusEnergyApiJsonContext : JsonSerializerContext
{
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class PagedResults<TResult>
{
    [JsonPropertyName("count")] public int Count { get; set; }
    [JsonPropertyName("next")] public string Next { get; set; } = string.Empty;
    [JsonPropertyName("previous")] public string Previous { get; set; } = string.Empty;
    [JsonPropertyName("results")] public ICollection<TResult> Results { get; set; } = Array.Empty<TResult>();
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class Account
{
    [JsonPropertyName("number")] public string Number { get; set; } = string.Empty;
    [JsonPropertyName("properties")] public ICollection<Property> Properties { get; set; } = Array.Empty<Property>();
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "<Pending>")]
public class Property
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("moved_in_at")] public DateTimeOffset MovedInAt { get; set; }
    [JsonPropertyName("moved_out_at")] public DateTimeOffset? MovedOutAt { get; set; }
    [JsonPropertyName("address_line_1")] public string AddressLine1 { get; set; } = string.Empty;
    [JsonPropertyName("address_line_2")] public string AddressLine2 { get; set; } = string.Empty;
    [JsonPropertyName("address_line_3")] public string AddressLine3 { get; set; } = string.Empty;
    [JsonPropertyName("town")] public string Town { get; set; } = string.Empty;
    [JsonPropertyName("county")] public string County { get; set; } = string.Empty;
    [JsonPropertyName("postcode")] public string Postcode { get; set; } = string.Empty;
    [JsonPropertyName("electricity_meter_points")] public ICollection<ElectricityMeterPoint> ElectricityMeterPoints { get; set; } = Array.Empty<ElectricityMeterPoint>();
    [JsonPropertyName("gas_meter_points")] public ICollection<GasMeterPoint> GasMeterPoints { get; set; } = Array.Empty<GasMeterPoint>();
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class ElectricityMeterPoint
{
    [JsonPropertyName("mpan")] public string Mpan { get; set; } = string.Empty;
    [JsonPropertyName("profile_class")] public int ProfileClass { get; set; }
    [JsonPropertyName("consumption_day")] public int ConsumptionDay { get; set; }
    [JsonPropertyName("consumption_night")] public int ConsumptionNight { get; set; }
    [JsonPropertyName("meters")] public ICollection<ElectricityMeter> Meters { get; set; } = Array.Empty<ElectricityMeter>();
    [JsonPropertyName("agreements")] public ICollection<Agreement> Agreements { get; set; } = Array.Empty<Agreement>();
    [JsonPropertyName("is_export")] public bool IsExport { get; set; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class Meter
{
    [JsonPropertyName("serial_number")] public string SerialNumber { get; set; } = string.Empty;
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class ElectricityMeter : Meter
{
    [JsonPropertyName("registers")] public ICollection<Register> Registers { get; set; } = Array.Empty<Register>();
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class GasMeterPoint
{
    [JsonPropertyName("mprn")] public string Mprn { get; set; } = string.Empty;
    [JsonPropertyName("consumption_standard")] public int ConsumptionStandard { get; set; }
    [JsonPropertyName("meters")] public ICollection<GasMeter> Meters { get; set; } = Array.Empty<GasMeter>();
    [JsonPropertyName("agreements")] public ICollection<Agreement> Agreements { get; set; } = Array.Empty<Agreement>();
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class GasMeter : Meter
{
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class Register
{
    [JsonPropertyName("identifier")] public string Identifier { get; set; } = string.Empty;

    [JsonPropertyName("rate")]
#if NET8_0_OR_GREATER
    [JsonConverter(typeof(JsonStringEnumConverter<ElectricityUnitRate>))] 
#else
    [JsonConverter(typeof(JsonStringEnumConverter))] 
#endif

    public ElectricityUnitRate Rate { get; set; }
    [JsonPropertyName("is_settlement_register")] public bool IsSettlementRegister { get; set; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class Agreement
{
    [JsonPropertyName("tariff_code")] public string TariffCode { get; set; } = string.Empty;
    [JsonPropertyName("valid_from")] public DateTimeOffset ValidFrom { get; set; }
    [JsonPropertyName("valid_to")] public DateTimeOffset? ValidTo { get; set; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class Consumption
{
    [JsonPropertyName("consumption")] public decimal Quantity { get; set; }
    [JsonPropertyName("interval_start")] public DateTimeOffset Start { get; set; }
    [JsonPropertyName("interval_end")] public DateTimeOffset End { get; set; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public enum Interval
{
    Default,
    HalfHour = Default,
    Hour,
    Day,
    Week,
    Month,
    Quarter
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public enum ElectricityUnitRate
{
    None = 0,
    Standard,
    Day,
    Night
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class ProductBase
{
    [JsonPropertyName("code")] public string Code { get; set; } = string.Empty;
    [JsonPropertyName("full_name")] public string FullName { get; set; } = string.Empty;
    [JsonPropertyName("display_name")] public string DisplayName { get; set; } = string.Empty;
    [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;
    [JsonPropertyName("is_variable")] public bool IsVariable { get; set; }
    [JsonPropertyName("is_green")] public bool IsGreen { get; set; }
    [JsonPropertyName("is_tracker")] public bool IsTracker { get; set; }
    [JsonPropertyName("is_prepay")] public bool IsPrepay { get; set; }
    [JsonPropertyName("is_business")] public bool IsBusiness { get; set; }
    [JsonPropertyName("is_restricted")] public bool IsRestricted { get; set; }
    [JsonPropertyName("term")] public int? Term { get; set; }
    [JsonPropertyName("brand")] public string Brand { get; set; } = string.Empty;
    [JsonPropertyName("available_from")] public DateTimeOffset AvailableFrom { get; set; }
    [JsonPropertyName("available_to")] public DateTimeOffset? AvailableTo { get; set; }

    [JsonPropertyName("links")] public IEnumerable<Link> Links { get; set; } = Array.Empty<Link>();
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class Product : ProductBase
{
    [JsonPropertyName("direction")] public string Direction { get; set; } = string.Empty;
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class ProductDetail : ProductBase
{
    [JsonPropertyName("tariffs_active_at")]
    public DateTimeOffset ActiveAt { get; set; }

    [JsonPropertyName("single_register_electricity_tariffs")]
    public Dictionary<string, TariffsByPeriod>? SingleRegisterElectricityTariffs { get; set; }

    [JsonPropertyName("dual_register_electricity_tariffs")]
    public Dictionary<string, TariffsByPeriod>? DualRegisterElectricityTariffs { get; set; }

    [JsonPropertyName("single_register_gas_tariffs")]
    public Dictionary<string, TariffsByPeriod>? SingleRegisterGasTariffs { get; set; }

    [JsonPropertyName("sample_quotes")] public Dictionary<string, SampleQuotesByPeriod>? SampleQuotes { get; set; }

    [JsonPropertyName("sample_consumption")]
    public SampleConsumptionByRate? SampleConsumption { get; set; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class SampleConsumptionByRate
{
    [JsonPropertyName("electricity_single_rate")]
    public SampleConsumption? ElectricitySingleRate { get; set; }

    [JsonPropertyName("electricity_dual_rate")]
    public SampleConsumption? ElectricityDualRate { get; set; }

    [JsonPropertyName("dual_fuel_single_rate")]
    public SampleConsumption? DualFuelSingleRate { get; set; }

    [JsonPropertyName("dual_fuel_dual_rate")]
    public SampleConsumption? DualFuelDualRate { get; set; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class SampleConsumption
{
    [JsonPropertyName("electricity_standard")]
    public decimal? ElectricityStandard { get; set; }

    [JsonPropertyName("electricity_day")] public decimal? ElectricityDay { get; set; }

    [JsonPropertyName("electricity_night")]
    public decimal? ElectricityNight { get; set; }

    [JsonPropertyName("gas_standard")] public decimal? GasStandard { get; set; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class TariffsByPeriod
{
    [JsonPropertyName("direct_debit_monthly")]
    public Tariff? Monthly { get; set; }

    [JsonPropertyName("direct_debit_quarterly")]
    public Tariff? Quarterly { get; set; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class Tariff
{
    [JsonPropertyName("code")] public string Code { get; set; } = string.Empty;

    [JsonPropertyName("standing_charge_exc_vat")]
    public decimal StandingChargeExcludingVAT { get; set; }

    [JsonPropertyName("standing_charge_inc_vat")]
    public decimal StandingChargeIncludingVAT { get; set; }

    [JsonPropertyName("online_discount_exc_vat")]
    public decimal OnlineDiscountExcludingVAT { get; set; }

    [JsonPropertyName("online_discount_inc_vat")]
    public decimal OnlineDiscountIncludingVAT { get; set; }

    [JsonPropertyName("dual_fuel_discount_exc_vat")]
    public decimal DualFuelDiscountExcludingVAT { get; set; }

    [JsonPropertyName("dual_fuel_discount_inc_vat")]
    public decimal DualFuelDiscountIncludingVAT { get; set; }

    [JsonPropertyName("exit_fees_exc_vat")]
    public decimal ExitFeesExcludingVAT { get; set; }

    [JsonPropertyName("exit_fees_inc_vat")]
    public decimal ExitFeesIncludingVAT { get; set; }

    [JsonPropertyName("standard_unit_rate_exc_vat")]
    public decimal? StandardUnitRateExcludingVAT { get; set; }

    [JsonPropertyName("standard_unit_rate_inc_vat")]
    public decimal? StandardUnitRateIncludingVAT { get; set; }

    [JsonPropertyName("day_unit_rate_exc_vat")]
    public decimal? DayUnitRateExcludingVAT { get; set; }

    [JsonPropertyName("day_unit_rate_inc_vat")]
    public decimal? DayUnitRateIncludingVAT { get; set; }

    [JsonPropertyName("night_unit_rate_exc_vat")]
    public decimal? NightUnitRateExcludingVAT { get; set; }

    [JsonPropertyName("night_unit_rate_inc_vat")]
    public decimal? NightUnitRateIncludingVAT { get; set; }

    [JsonPropertyName("links")] public IEnumerable<Link> Links { get; set; } = Array.Empty<Link>();
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class SampleQuotesByPeriod
{
    [JsonPropertyName("direct_debit_monthly")]
    public SampleQuotes? Monthly { get; set; }

    [JsonPropertyName("direct_debit_quarterly")]
    public SampleQuotes? Quarterly { get; set; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class SampleQuotes
{
    [JsonPropertyName("electricity_single_rate")]
    public SampleQuote? ElectricitySingleRate { get; set; }

    [JsonPropertyName("electricity_dual_rate")]
    public SampleQuote? ElectricityDualRate { get; set; }

    [JsonPropertyName("dual_fuel_single_rate")]
    public SampleQuote? DualFuelSingleRate { get; set; }

    [JsonPropertyName("dual_fuel_dual_rate")]
    public SampleQuote? DualFuelDualRate { get; set; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class SampleQuote
{
    [JsonPropertyName("annual_cost_inc_vat")]
    public decimal? AnnualCostIncludingVAT { get; set; }

    [JsonPropertyName("annual_cost_exc_vat")]
    public decimal? AnnualCostExcludingVAT { get; set; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class Link
{
    [JsonPropertyName("href")] public string HRef { get; set; } = string.Empty;
    [JsonPropertyName("method")] public string Method { get; set; } = string.Empty;
    [JsonPropertyName("rel")] public string Rel { get; set; } = string.Empty;
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal class MeterPointGridSupplyPoint
{
    [JsonPropertyName("gsp")] public string GroupId { get; set; } = string.Empty;
    [JsonPropertyName("mpan")] public string MPan { get; set; } = string.Empty;
    [JsonPropertyName("profile_class")] public int ProfileClass { get; set; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal class GridSupplyPoint
{
    [JsonPropertyName("group_id")] public string GroupId { get; set; } = string.Empty;
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class GridSupplyPointInfo
{
    public string? GroupId { get; set; }
    public string? AreaId { get; set; }
    public string? Area { get; set; }

    public static IEnumerable<GridSupplyPointInfo> GetAll()
    {
        return new List<GridSupplyPointInfo>
        {
            new() { GroupId = "_A", AreaId = "10", Area = "East England" },
            new() { GroupId = "_B", AreaId = "11", Area = "East Midlands" },
            new() { GroupId = "_C", AreaId = "12", Area = "London" },
            new() { GroupId = "_D", AreaId = "13", Area = "North Wales, Merseyside and Cheshire" },
            new() { GroupId = "_E", AreaId = "14", Area = "West Midlands" },
            new() { GroupId = "_F", AreaId = "15", Area = "North East England" },
            new() { GroupId = "_G", AreaId = "16", Area = "North West England" },
            new() { GroupId = "_H", AreaId = "17", Area = "North Scotland" },
            new() { GroupId = "_I", AreaId = "18", Area = "South Scotland" },
            new() { GroupId = "_J", AreaId = "19", Area = "South East England" },
            new() { GroupId = "_K", AreaId = "20", Area = "Southern England" },
            new() { GroupId = "_L", AreaId = "21", Area = "South Wales" },
            new() { GroupId = "_M", AreaId = "22", Area = "South West England" },
            new() { GroupId = "_N", AreaId = "23", Area = "Yorkshire" }
        };
    }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class Charge
{
    [JsonPropertyName("value_exc_vat")] public decimal ValueExcludingVAT { get; set; }
    [JsonPropertyName("value_inc_vat")] public decimal ValueIncludingVAT { get; set; }
    [JsonPropertyName("valid_from")] public DateTimeOffset? ValidFrom { get; set; }
    [JsonPropertyName("valid_to")] public DateTimeOffset? ValidTo { get; set; }

    [JsonIgnore] public DateTimeOffset ValidFromUTC => ValidFrom?.ToUniversalTime() ?? DateTimeOffset.MinValue;
    [JsonIgnore] public DateTimeOffset ValidToUTC => ValidTo?.ToUniversalTime() ?? DateTimeOffset.MaxValue;
}