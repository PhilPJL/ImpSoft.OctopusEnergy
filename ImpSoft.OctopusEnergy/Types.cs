using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ImpSoft.OctopusEnergy
{
    [DataContract(Name = "")]
    public class PagedResults<TResult>
    {
        [DataMember(Name = "count")] public int Count { get; set; }
        [DataMember(Name = "next")] public string Next { get; set; }
        [DataMember(Name = "previous")] public string Previous { get; set; }
        [DataMember(Name = "results")] public IEnumerable<TResult> Results { get; set; }
    }

    [DataContract(Name = "")]
    public class Consumption
    {
        [DataMember(Name = "consumption")] public decimal Quantity { get; set; }
        [DataMember(Name = "interval_start")] public DateTimeOffset Start { get; set; }
        [DataMember(Name = "interval_end")] public DateTimeOffset End { get; set; }
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

    [DataContract(Name = "")]
    public class ProductBase
    {
        [DataMember(Name = "code")] public string Code { get; set; }
        [DataMember(Name = "full_name")] public string FullName { get; set; }
        [DataMember(Name = "display_name")] public string DisplayName { get; set; }
        [DataMember(Name = "description")] public string Description { get; set; }
        [DataMember(Name = "is_variable")] public bool IsVariable { get; set; }
        [DataMember(Name = "is_green")] public bool IsGreen { get; set; }
        [DataMember(Name = "is_tracker")] public bool IsTracker { get; set; }
        [DataMember(Name = "is_prepay")] public bool IsPrepay { get; set; }
        [DataMember(Name = "is_business")] public bool IsBusiness { get; set; }
        [DataMember(Name = "is_restricted")] public bool IsRestricted { get; set; }
        [DataMember(Name = "term")] public int? Term { get; set; }
        [DataMember(Name = "brand")] public string Brand { get; set; }
        [DataMember(Name = "available_from")] public DateTimeOffset AvailableFrom { get; set; }
        [DataMember(Name = "available_to")] public DateTimeOffset? AvailableTo { get; set; }

        [DataMember(Name = "links")] public IEnumerable<Link> Links { get; set; }
    }

    public class Product : ProductBase
    {
        [DataMember(Name = "direction")] public string Direction { get; set; }
    }

    [DataContract(Name = "")]
    public class ProductDetail : ProductBase
    {
        [DataMember(Name = "tariffs_active_at")]
        public DateTimeOffset ActiveAt { get; set; }

        [DataMember(Name = "single_register_electricity_tariffs")]
        public Dictionary<string, TariffsByPeriod> SingleRegisterElectricityTariffs { get; set; }

        [DataMember(Name = "dual_register_electricity_tariffs")]
        public Dictionary<string, TariffsByPeriod> DualRegisterElectricityTariffs { get; set; }

        [DataMember(Name = "single_register_gas_tariffs")]
        public Dictionary<string, TariffsByPeriod> SingleRegisterGasTariffs { get; set; }

        [DataMember(Name = "sample_quotes")]
        public Dictionary<string, SampleQuotesByPeriod> SampleQuotes { get; set; }

        [DataMember(Name = "sample_consumption")]
        public SampleConsumptionByRate SampleConsumption { get; set; }
    }

    [DataContract(Name = "")]
    public class SampleConsumptionByRate
    {
        [DataMember(Name = "electricity_single_rate")] public SampleConsumption ElectricitySingleRate { get; set; }
        [DataMember(Name = "electricity_dual_rate")] public SampleConsumption ElectricityDualRate { get; set; }
        [DataMember(Name = "dual_fuel_single_rate")] public SampleConsumption DualFuelSingleRate { get; set; }
        [DataMember(Name = "dual_fuel_dual_rate")] public SampleConsumption DualFuelDualRate { get; set; }
    }

    [DataContract(Name = "")]
    public class SampleConsumption
    {
        [DataMember(Name = "electricity_standard")] public decimal? ElectricityStandard { get; set; }
        [DataMember(Name = "electricity_day")] public decimal? ElectricityDay { get; set; }
        [DataMember(Name = "electricity_night")] public decimal? ElectricityNight { get; set; }
        [DataMember(Name = "gas_standard")] public decimal? GasStandard { get; set; }
    }

    [DataContract(Name = "")]
    public class TariffsByPeriod
    {
        [DataMember(Name = "direct_debit_monthly")]
        public Tariff Monthly { get; set; }

        [DataMember(Name = "direct_debit_quarterly")]
        public Tariff Quarterly { get; set; }
    }

    [DataContract(Name = "")]
    public class Tariff
    {
        [DataMember(Name = "code")] public string Code { get; set; }

        [DataMember(Name = "standing_charge_exc_vat")]
        public decimal StandingChargeExcVAT { get; set; }

        [DataMember(Name = "standing_charge_inc_vat")]
        public decimal StandingChargeIncVAT { get; set; }

        [DataMember(Name = "online_discount_exc_vat")]
        public decimal OnlineDiscountExcVAT { get; set; }

        [DataMember(Name = "online_discount_inc_vat")]
        public decimal OnlineDiscountIncVAT { get; set; }

        [DataMember(Name = "dual_fuel_discount_exc_vat")]
        public decimal DualFuelDiscountExcVAT { get; set; }

        [DataMember(Name = "dual_fuel_discount_inc_vat")]
        public decimal DualFuelDiscountIncVAT { get; set; }

        [DataMember(Name = "exit_fees_exc_vat")]
        public decimal ExitFeesExcVAT { get; set; }

        [DataMember(Name = "exit_fees_inc_vat")]
        public decimal ExitFeesIncVAT { get; set; }

        [DataMember(Name = "standard_unit_rate_exc_vat")]
        public decimal? StandardUnitRateExcVAT { get; set; }

        [DataMember(Name = "standard_unit_rate_inc_vat")]
        public decimal? StandardUnitRateIncVAT { get; set; }

        [DataMember(Name = "day_unit_rate_exc_vat")]
        public decimal? DayUnitRateExcVAT { get; set; }

        [DataMember(Name = "day_unit_rate_inc_vat")]
        public decimal? DayUnitRateIncVAT { get; set; }

        [DataMember(Name = "night_unit_rate_exc_vat")]
        public decimal? NightUnitRateExcVAT { get; set; }

        [DataMember(Name = "night_unit_rate_inc_vat")]
        public decimal? NightUnitRateIncVAT { get; set; }

        [DataMember(Name = "links")] public IEnumerable<Link> Links { get; set; }
    }

    [DataContract(Name = "")]
    public class SampleQuotesByPeriod
    {
        [DataMember(Name = "direct_debit_monthly")]
        public SampleQuotes Monthly { get; set; }

        [DataMember(Name = "direct_debit_quarterly")]
        public SampleQuotes Quarterly { get; set; }
    }

    [DataContract(Name = "")]
    public class SampleQuotes
    {
        [DataMember(Name = "electricity_single_rate")]
        public SampleQuote ElectricitySingleRate { get; set; }

        [DataMember(Name = "electricity_dual_rate")]
        public SampleQuote ElectricityDualRate { get; set; }

        [DataMember(Name = "dual_fuel_single_rate")]
        public SampleQuote DualFuelSingleRate { get; set; }

        [DataMember(Name = "dual_fuel_dual_rate")]
        public SampleQuote DualFuelDualRate { get; set; }
    }

    [DataContract(Name = "")]
    public class SampleQuote
    {
        [DataMember(Name = "annual_cost_inc_vat")] public decimal? AnnualCostIncVAT { get; set; }
        [DataMember(Name = "annual_cost_exc_vat")] public decimal? AnnualCostExcVAT { get; set; }
    }

    [DataContract(Name = "")]
    public class Link
    {
        [DataMember(Name = "href")] public string HRef { get; set; }
        [DataMember(Name = "method")] public string Method { get; set; }
        [DataMember(Name = "rel")] public string Rel { get; set; }
    }

    [DataContract(Name = "")]
    public class MeterPointGridSupplyPoint
    {
        [DataMember(Name = "gsp")] public string GroupId { get; set; }
        [DataMember(Name = "mpan")] public string MPan { get; set; }
        [DataMember(Name = "profile_class")] public int ProfileClass { get; set; }
    }

    [DataContract(Name = "")]
    public class GridSupplyPoint
    {
        [DataMember(Name = "group_id")] public string GroupId { get; set; }
    }

    [DataContract(Name = "")]
    public class Charge
    {
        [DataMember(Name = "value_exc_vat")] public decimal ValueExcludingVAT { get; set; }
        [DataMember(Name = "value_inc_vat")] public decimal ValueIncludingVAT { get; set; }
        [DataMember(Name = "valid_from")] public DateTimeOffset? ValidFrom { get; set; }
        [DataMember(Name = "valid_to")] public DateTimeOffset? ValidTo { get; set; }

        public DateTimeOffset ValidFromUTC => ValidFrom?.ToUniversalTime() ?? DateTimeOffset.MinValue;
        public DateTimeOffset ValidToUTC => ValidTo?.ToUniversalTime() ?? DateTimeOffset.MaxValue;
    }
}