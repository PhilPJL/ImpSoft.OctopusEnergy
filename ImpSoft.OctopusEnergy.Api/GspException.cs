using System;
using JetBrains.Annotations;

namespace ImpSoft.OctopusEnergy.Api;

[UsedImplicitly]
public class GspException : ApplicationException
{
    public GspException()
    {
    }

    public GspException(string message) : base(message)
    {
    }

    public GspException(string message, Exception innerException) : base(message, innerException)
    {
    }
}