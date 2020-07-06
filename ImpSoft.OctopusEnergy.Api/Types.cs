using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("ImpSoft.OctopusEnergy.Api.Tests")]

namespace ImpSoft.OctopusEnergy.Api
{
    public class PagedResults<TResult>
    {
        [JsonProperty("count")] public int Count { get; set; }
        [JsonProperty("next")] public string Next { get; set; }
        [JsonProperty("previous")] public string Previous { get; set; }
        [JsonProperty("results")] public IEnumerable<TResult> Results { get; set; }
    }

    public class Consumption
    {
        [JsonProperty("consumption")] public decimal Quantity { get; set; }
        [JsonProperty("interval_start")] public DateTimeOffset Start { get; set; }
        [JsonProperty("interval_end")] public DateTimeOffset End { get; set; }
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
        [JsonProperty("code")] public string Code { get; set; }
        [JsonProperty("full_name")] public string FullName { get; set; }
        [JsonProperty("display_name")] public string DisplayName { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("is_variable")] public bool IsVariable { get; set; }
        [JsonProperty("is_green")] public bool IsGreen { get; set; }
        [JsonProperty("is_tracker")] public bool IsTracker { get; set; }
        [JsonProperty("is_prepay")] public bool IsPrepay { get; set; }
        [JsonProperty("is_business")] public bool IsBusiness { get; set; }
        [JsonProperty("is_restricted")] public bool IsRestricted { get; set; }
        [JsonProperty("term")] public int? Term { get; set; }
        [JsonProperty("brand")] public string Brand { get; set; }
        [JsonProperty("available_from")] public DateTimeOffset AvailableFrom { get; set; }
        [JsonProperty("available_to")] public DateTimeOffset? AvailableTo { get; set; }

        [JsonProperty("links")] public IEnumerable<Link> Links { get; set; }
    }

    public class Product : ProductBase
    {
        [JsonProperty("direction")] public string Direction { get; set; }
    }

    public class ProductDetail : ProductBase
    {
        [JsonProperty("tariffs_active_at")]
        public DateTimeOffset ActiveAt { get; set; }

        [JsonProperty("single_register_electricity_tariffs")]
        public Dictionary<string, TariffsByPeriod> SingleRegisterElectricityTariffs { get; set; }

        [JsonProperty("dual_register_electricity_tariffs")]
        public Dictionary<string, TariffsByPeriod> DualRegisterElectricityTariffs { get; set; }

        [JsonProperty("single_register_gas_tariffs")]
        public Dictionary<string, TariffsByPeriod> SingleRegisterGasTariffs { get; set; }

        [JsonProperty("sample_quotes")] public Dictionary<string, SampleQuotesByPeriod> SampleQuotes { get; set; }

        [JsonProperty("sample_consumption")]
        public SampleConsumptionByRate SampleConsumption { get; set; }
    }

    public class SampleConsumptionByRate
    {
        [JsonProperty("electricity_single_rate")]
        public SampleConsumption ElectricitySingleRate { get; set; }

        [JsonProperty("electricity_dual_rate")]
        public SampleConsumption ElectricityDualRate { get; set; }

        [JsonProperty("dual_fuel_single_rate")]
        public SampleConsumption DualFuelSingleRate { get; set; }

        [JsonProperty("dual_fuel_dual_rate")]
        public SampleConsumption DualFuelDualRate { get; set; }
    }

    public class SampleConsumption
    {
        [JsonProperty("electricity_standard")]
        public decimal? ElectricityStandard { get; set; }

        [JsonProperty("electricity_day")] public decimal? ElectricityDay { get; set; }

        [JsonProperty("electricity_night")]
        public decimal? ElectricityNight { get; set; }

        [JsonProperty("gas_standard")] public decimal? GasStandard { get; set; }
    }

    public class TariffsByPeriod
    {
        [JsonProperty("direct_debit_monthly")]
        public Tariff Monthly { get; set; }

        [JsonProperty("direct_debit_quarterly")]
        public Tariff Quarterly { get; set; }
    }

    public class Tariff
    {
        [JsonProperty("code")] public string Code { get; set; }

        [JsonProperty("standing_charge_exc_vat")]
        public decimal StandingChargeExcludingVAT { get; set; }

        [JsonProperty("standing_charge_inc_vat")]
        public decimal StandingChargeIncludingVAT { get; set; }

        [JsonProperty("online_discount_exc_vat")]
        public decimal OnlineDiscountExcludingVAT { get; set; }

        [JsonProperty("online_discount_inc_vat")]
        public decimal OnlineDiscountIncludingVAT { get; set; }

        [JsonProperty("dual_fuel_discount_exc_vat")]
        public decimal DualFuelDiscountExcludingVAT { get; set; }

        [JsonProperty("dual_fuel_discount_inc_vat")]
        public decimal DualFuelDiscountIncludingVAT { get; set; }

        [JsonProperty("exit_fees_exc_vat")]
        public decimal ExitFeesExcludingVAT { get; set; }

        [JsonProperty("exit_fees_inc_vat")]
        public decimal ExitFeesIncludingVAT { get; set; }

        [JsonProperty("standard_unit_rate_exc_vat")]
        public decimal? StandardUnitRateExcludingVAT { get; set; }

        [JsonProperty("standard_unit_rate_inc_vat")]
        public decimal? StandardUnitRateIncludingVAT { get; set; }

        [JsonProperty("day_unit_rate_exc_vat")]
        public decimal? DayUnitRateExcludingVAT { get; set; }

        [JsonProperty("day_unit_rate_inc_vat")]
        public decimal? DayUnitRateIncludingVAT { get; set; }

        [JsonProperty("night_unit_rate_exc_vat")]
        public decimal? NightUnitRateExcludingVAT { get; set; }

        [JsonProperty("night_unit_rate_inc_vat")]
        public decimal? NightUnitRateIncludingVAT { get; set; }

        [JsonProperty("links")] public IEnumerable<Link> Links { get; set; }
    }

    public class SampleQuotesByPeriod
    {
        [JsonProperty("direct_debit_monthly")]
        public SampleQuotes Monthly { get; set; }

        [JsonProperty("direct_debit_quarterly")]
        public SampleQuotes Quarterly { get; set; }
    }

    public class SampleQuotes
    {
        [JsonProperty("electricity_single_rate")]
        public SampleQuote ElectricitySingleRate { get; set; }

        [JsonProperty("electricity_dual_rate")]
        public SampleQuote ElectricityDualRate { get; set; }

        [JsonProperty("dual_fuel_single_rate")]
        public SampleQuote DualFuelSingleRate { get; set; }

        [JsonProperty("dual_fuel_dual_rate")]
        public SampleQuote DualFuelDualRate { get; set; }
    }

    public class SampleQuote
    {
        [JsonProperty("annual_cost_inc_vat")]
        public decimal? AnnualCostIncludingVAT { get; set; }

        [JsonProperty("annual_cost_exc_vat")]
        public decimal? AnnualCostExcludingVAT { get; set; }
    }

    public class Link
    {
        [JsonProperty("href")] public string HRef { get; set; }
        [JsonProperty("method")] public string Method { get; set; }
        [JsonProperty("rel")] public string Rel { get; set; }
    }

    internal class MeterPointGridSupplyPoint
    {
        [JsonProperty("gsp")] public string GroupId { get; set; }
        [JsonProperty("mpan")] public string MPan { get; set; }
        [JsonProperty("profile_class")] public int ProfileClass { get; set; }
    }

    internal class GridSupplyPoint
    {
        [JsonProperty("group_id")] public string GroupId { get; set; }
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
        [JsonProperty("value_exc_vat")] public decimal ValueExcludingVAT { get; set; }
        [JsonProperty("value_inc_vat")] public decimal ValueIncludingVAT { get; set; }
        [JsonProperty("valid_from")] public DateTimeOffset? ValidFrom { get; set; }
        [JsonProperty("valid_to")] public DateTimeOffset? ValidTo { get; set; }

        public DateTimeOffset ValidFromUTC => ValidFrom?.ToUniversalTime() ?? DateTimeOffset.MinValue;
        public DateTimeOffset ValidToUTC => ValidTo?.ToUniversalTime() ?? DateTimeOffset.MaxValue;
    }
}