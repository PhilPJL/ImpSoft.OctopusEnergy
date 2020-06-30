namespace ImpSoft.OctopusEnergy.Api
{
    public static class ClientFactory
    {
        /// <summary>
        ///     Creates a client with access to public and private data.
        /// </summary>
        /// <param name="apiKey">An API key allocated at tbd</param>
        /// <param name="enableAutomaticCompression">By default this will attempt to enable GZip and Deflate compression.  Some HttpClientHandler implementations (Blazor WASM) don't support the AutomaticCompression property. 
        /// Set the property in those cases.  Set to false to disable compression where AutomaticCompression is supported.</param>
        public static IPrivateClient Create(string apiKey, bool? enableAutomaticCompression = true)
        {
            return new PrivateClient(apiKey, enableAutomaticCompression);
        }

        /// <summary>
        ///     Creates a client with access to public data.
        /// </summary>
        /// <param name="enableAutomaticCompression">By default this will attempt to enable GZip and Deflate compression.  Some HttpClientHandler implementations (Blazor WASM) don't support the AutomaticCompression property. 
        /// Set the property to null in those cases.  Set to false to disable compression where AutomaticCompression is supported.</param>
        /// <returns></returns>
        public static IPublicClient Create(bool? enableAutomaticCompression = true)
        {
            return new PublicClient(enableAutomaticCompression);
        }

        /// <summary>
        /// The base URL of the Octopus Energy API
        /// </summary>
        public static string BaseUrl { get; } = "https://api.octopus.energy";
    }
}