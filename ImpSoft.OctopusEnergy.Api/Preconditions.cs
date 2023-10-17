using System;
using ImpSoft.OctopusEnergy.Api.Properties;
using JetBrains.Annotations;

namespace ImpSoft.OctopusEnergy.Api;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class Preconditions
{
    [ContractAnnotation("halt <= paramValue : null")]
    [AssertionMethod]
    public static void IsNotNull<T>([NoEnumeration] T paramValue, [InvokerParameterName] string paramName)
        where T : class
    {
        if (paramValue == null) throw new ArgumentNullException(paramName);
    }

    [ContractAnnotation("halt <= paramValue : null")]
    [AssertionMethod]
    public static void IsNotNullOrEmpty(string paramValue, [InvokerParameterName] string paramName)
    {
        if (string.IsNullOrEmpty(paramValue))
            throw new ArgumentException(Resources.ParameterMustNotBeNullOrEmpty, paramName);
    }

    [ContractAnnotation("halt <= paramValue : null")]
    [AssertionMethod]
    public static void IsNotNullOrWhiteSpace(string paramValue, [InvokerParameterName] string paramName)
    {
        if (string.IsNullOrWhiteSpace(paramValue))
            throw new ArgumentException(Resources.ParameterMustNotBeNullOrWhitespace, paramName);
    }
}