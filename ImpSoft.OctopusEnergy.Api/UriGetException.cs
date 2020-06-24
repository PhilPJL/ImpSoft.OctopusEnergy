using ImpSoft.OctopusEnergy.Api.Properties;
using System;
using System.Globalization;
using System.Net.Http;

namespace ImpSoft.OctopusEnergy.Api
{
    public class UriGetException : Exception
    {
        public string UriString { get; }

        public UriGetException(string message, Uri uri) : base(message)
        {
            Preconditions.IsNotNull(uri, nameof(uri));

            UriString = uri.ToString();
        }

        public UriGetException()
        {
        }

        public UriGetException(string caller, HttpResponseMessage response, Uri uri) : base(ConstructMessage(caller, response))
        {
            Preconditions.IsNotNull(response, nameof(response));
            Preconditions.IsNotNull(uri, nameof(uri));

            UriString = uri.ToString();
        }

        private static string ConstructMessage(string caller, HttpResponseMessage response)
        {
            caller = caller?.StripAsyncSuffix() ?? Resources.UnknownMethod;

            return string.Format(CultureInfo.CurrentCulture,
                Resources.HttpRequestFailed, caller, response.StatusCode, response.ReasonPhrase);
        }

        public UriGetException(string message) : base(message)
        {
        }

        public UriGetException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}