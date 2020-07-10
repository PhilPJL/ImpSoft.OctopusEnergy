using System;

namespace ImpSoft.OctopusEnergy.Api
{
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

}