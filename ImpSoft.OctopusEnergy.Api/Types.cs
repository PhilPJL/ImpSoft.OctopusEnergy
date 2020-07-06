using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("ImpSoft.OctopusEnergy.Api.Tests")]

namespace ImpSoft.OctopusEnergy.Api
{
    public class PagedResults<TResult>
    {
        [JsonPropertyName("count")] public int Count { get; set; }
        [JsonPropertyName("next")] public string Next { get; set; }
        [JsonPropertyName("previous")] public string Previous { get; set; }
        [JsonPropertyName("results")] public IEnumerable<TResult> Results { get; set; }
    }

    public class Consumption
    {
        [JsonPropertyName("consumption")] public decimal Quantity { get; set; }
        [JsonPropertyName("interval_start")] public DateTimeOffset Start { get; set; }
        [JsonPropertyName("interval_end")] public DateTimeOffset End { get; set; }
    }

#pragma warning disable CA1027 // Mark enums with FlagsAttribute
    public enum Interval
#pragma warning restore CA1027 // Mark enums with FlagsAttribute
    {
        Default,
        HalfHour = Default,
        Hour,
        Day,
        Week,
        Month,
        Quarter
    }

    public enum ElectricityUnitRate
    {
        Standard,
        Day,
        Night
    }

    public class ProductBase
    {
        [JsonPropertyName("code")] public string Code { get; set; }
        [JsonPropertyName("full_name")] public string FullName { get; set; }
        [JsonPropertyName("display_name")] public string DisplayName { get; set; }
        [JsonPropertyName("description")] public string Description { get; set; }
        [JsonPropertyName("is_variable")] public bool IsVariable { get; set; }
        [JsonPropertyName("is_green")] public bool IsGreen { get; set; }
        [JsonPropertyName("is_tracker")] public bool IsTracker { get; set; }
        [JsonPropertyName("is_prepay")] public bool IsPrepay { get; set; }
        [JsonPropertyName("is_business")] public bool IsBusiness { get; set; }
        [JsonPropertyName("is_restricted")] public bool IsRestricted { get; set; }
        [JsonPropertyName("term")] public int? Term { get; set; }
        [JsonPropertyName("brand")] public string Brand { get; set; }
        [JsonPropertyName("available_from")] public DateTimeOffset AvailableFrom { get; set; }
        [JsonPropertyName("available_to")] public DateTimeOffset? AvailableTo { get; set; }

        [JsonPropertyName("links")] public IEnumerable<Link> Links { get; set; }
    }

    public class Product : ProductBase
    {
        [JsonPropertyName("direction")] public string Direction { get; set; }
    }

    public class ProductDetail : ProductBase
    {
        [JsonPropertyName("tariffs_active_at")]
        public DateTimeOffset ActiveAt { get; set; }

        [JsonPropertyName("single_register_electricity_tariffs")]
        public Dictionary<string, TariffsByPeriod> SingleRegisterElectricityTariffs { get; set; }

        [JsonPropertyName("dual_register_electricity_tariffs")]
        public Dictionary<string, TariffsByPeriod> DualRegisterElectricityTariffs { get; set; }

        [JsonPropertyName("single_register_gas_tariffs")]
        public Dictionary<string, TariffsByPeriod> SingleRegisterGasTariffs { get; set; }

        [JsonPropertyName("sample_quotes")] public Dictionary<string, SampleQuotesByPeriod> SampleQuotes { get; set; }

        [JsonPropertyName("sample_consumption")]
        public SampleConsumptionByRate SampleConsumption { get; set; }
    }

    public class SampleConsumptionByRate
    {
        [JsonPropertyName("electricity_single_rate")]
        public SampleConsumption ElectricitySingleRate { get; set; }

        [JsonPropertyName("electricity_dual_rate")]
        public SampleConsumption ElectricityDualRate { get; set; }

        [JsonPropertyName("dual_fuel_single_rate")]
        public SampleConsumption DualFuelSingleRate { get; set; }

        [JsonPropertyName("dual_fuel_dual_rate")]
        public SampleConsumption DualFuelDualRate { get; set; }
    }

    public class SampleConsumption
    {
        [JsonPropertyName("electricity_standard")]
        public decimal? ElectricityStandard { get; set; }

        [JsonPropertyName("electricity_day")] public decimal? ElectricityDay { get; set; }

        [JsonPropertyName("electricity_night")]
        public decimal? ElectricityNight { get; set; }

        [JsonPropertyName("gas_standard")] public decimal? GasStandard { get; set; }
    }

    public class TariffsByPeriod
    {
        [JsonPropertyName("direct_debit_monthly")]
        public Tariff Monthly { get; set; }

        [JsonPropertyName("direct_debit_quarterly")]
        public Tariff Quarterly { get; set; }
    }

    public class Tariff
    {
        [JsonPropertyName("code")] public string Code { get; set; }

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

        [JsonPropertyName("links")] public IEnumerable<Link> Links { get; set; }
    }

    public class SampleQuotesByPeriod
    {
        [JsonPropertyName("direct_debit_monthly")]
        public SampleQuotes Monthly { get; set; }

        [JsonPropertyName("direct_debit_quarterly")]
        public SampleQuotes Quarterly { get; set; }
    }

    public class SampleQuotes
    {
        [JsonPropertyName("electricity_single_rate")]
        public SampleQuote ElectricitySingleRate { get; set; }

        [JsonPropertyName("electricity_dual_rate")]
        public SampleQuote ElectricityDualRate { get; set; }

        [JsonPropertyName("dual_fuel_single_rate")]
        public SampleQuote DualFuelSingleRate { get; set; }

        [JsonPropertyName("dual_fuel_dual_rate")]
        public SampleQuote DualFuelDualRate { get; set; }
    }

    public class SampleQuote
    {
        [JsonPropertyName("annual_cost_inc_vat")]
        public decimal? AnnualCostIncludingVAT { get; set; }

        [JsonPropertyName("annual_cost_exc_vat")]
        public decimal? AnnualCostExcludingVAT { get; set; }
    }

    public class Link
    {
        [JsonPropertyName("href")] public string HRef { get; set; }
        [JsonPropertyName("method")] public string Method { get; set; }
        [JsonPropertyName("rel")] public string Rel { get; set; }
    }

    internal class MeterPointGridSupplyPoint
    {
        [JsonPropertyName("gsp")] public string GroupId { get; set; }
        [JsonPropertyName("mpan")] public string MPan { get; set; }
        [JsonPropertyName("profile_class")] public int ProfileClass { get; set; }
    }

    internal class GridSupplyPoint
    {
        [JsonPropertyName("group_id")] public string GroupId { get; set; }
    }

    public class GridSupplyPointInfo
    {
        public string GroupId { get; set; }
        public string AreaId { get; set; }
        public string Area { get; set; }

        public static IEnumerable<GridSupplyPointInfo> GetAll()
        {
            return new List<GridSupplyPointInfo>
            {
                new GridSupplyPointInfo{ GroupId = "_A", AreaId = "10", Area = "East England" },
                new GridSupplyPointInfo{ GroupId = "_B", AreaId = "11", Area = "East Midlands" },
                new GridSupplyPointInfo{ GroupId = "_C", AreaId = "12", Area = "London" },
                new GridSupplyPointInfo{ GroupId = "_D", AreaId = "13", Area = "North Wales, Merseyside and Cheshire" },
                new GridSupplyPointInfo{ GroupId = "_E", AreaId = "14", Area = "West Midlands" },
                new GridSupplyPointInfo{ GroupId = "_F", AreaId = "15", Area = "North East England" },
                new GridSupplyPointInfo{ GroupId = "_G", AreaId = "16", Area = "North West England" },
                new GridSupplyPointInfo{ GroupId = "_H", AreaId = "17", Area = "North Scotland" },
                new GridSupplyPointInfo{ GroupId = "_I", AreaId = "18", Area = "South Scotland" },
                new GridSupplyPointInfo{ GroupId = "_J", AreaId = "19", Area = "South East England" },
                new GridSupplyPointInfo{ GroupId = "_K", AreaId = "20", Area = "Southern England" },
                new GridSupplyPointInfo{ GroupId = "_L", AreaId = "21", Area = "South Wales" },
                new GridSupplyPointInfo{ GroupId = "_M", AreaId = "22", Area = "South West England" },
                new GridSupplyPointInfo{ GroupId = "_N", AreaId = "23", Area = "Yorkshire" },
            };
        }
    }

    public class Charge
    {
        [JsonPropertyName("value_exc_vat")] public decimal ValueExcludingVAT { get; set; }
        [JsonPropertyName("value_inc_vat")] public decimal ValueIncludingVAT { get; set; }
        [JsonPropertyName("valid_from")] public DateTimeOffset? ValidFrom { get; set; }
        [JsonPropertyName("valid_to")] public DateTimeOffset? ValidTo { get; set; }

        public DateTimeOffset ValidFromUTC => ValidFrom?.ToUniversalTime() ?? DateTimeOffset.MinValue;
        public DateTimeOffset ValidToUTC => ValidTo?.ToUniversalTime() ?? DateTimeOffset.MaxValue;
    }
}