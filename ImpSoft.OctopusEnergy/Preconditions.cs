﻿using JetBrains.Annotations;
using System;

namespace ImpSoft.OctopusEnergy
{
    public static class Preconditions
    {
        [ContractAnnotation("halt <= paramValue : null")]
        [AssertionMethod]
        public static void IsNotNull<T>([NotNull, NoEnumeration] T paramValue, [InvokerParameterName] string paramName) where T : class
        {
            if (paramValue == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        [ContractAnnotation("halt <= paramValue : null")]
        [AssertionMethod]
        public static void IsNotNullOrEmpty([NotNull] string paramValue, [InvokerParameterName] string paramName)
        {
            if (string.IsNullOrEmpty(paramValue))
            {
                throw new ArgumentException("The parameter must not be null or empty.", paramName);
            }
        }

        [ContractAnnotation("halt <= paramValue : null")]
        [AssertionMethod]
        public static void IsNotNullOrWhiteSpace([NotNull] string paramValue, [InvokerParameterName] string paramName)
        {
            if (string.IsNullOrWhiteSpace(paramValue))
            {
                throw new ArgumentException("The parameter must not be null or whitespace.", paramName);
            }
        }
    }
}