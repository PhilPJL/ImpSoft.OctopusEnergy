namespace ImpSoft.OctopusEnergy
{
    public static class ClientFactory
    {
        /// <summary>
        /// Creates a client with access to public and private data.
        /// </summary>
        /// <param name="apiKey">An API key allocated at tbd</param>
        public static IPrivateClient Create(string apiKey)
        {
            return new PrivateClient(apiKey);
        }

        /// <summary>
        /// Creates a client with access to public data.
        /// </summary>
        /// <returns></returns>
        public static IPublicClient Create()
        {
            return new PublicClient();
        }
    }
}