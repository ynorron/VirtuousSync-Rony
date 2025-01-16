using AutoMapper;
using RestSharp;
using Sync.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sync
{
    /// <summary>
    /// Service to interact with the Virtuous API for fetching and managing contact data.
    /// </summary>
    internal class VirtuousService
    {
        private readonly RestClient _restClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtuousService"/> class.
        /// Configures the RestClient to authenticate with the Virtuous API.
        /// </summary>
        /// <param name="configuration">The application configuration containing API base URL and API key.</param>
        /// <exception cref="Exception">Thrown if the client initialization fails.</exception>
        public VirtuousService(IConfiguration configuration)
        {
            try
            {
                var apiBaseUrl = configuration.GetValue("VirtuousApiBaseUrl");
                var apiKey = configuration.GetValue("VirtuousApiKey");

                var options = new RestClientOptions(apiBaseUrl)
                {
                    Authenticator = new RestSharp.Authenticators.OAuth2.OAuth2AuthorizationRequestHeaderAuthenticator(apiKey)
                };

                _restClient = new RestClient(options);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to initialize VirtuousService: {ex.Message}");
                throw; // Optionally rethrow to signal critical failure
            }
        }

        /// <summary>
        /// Fetches a paged list of contacts from the Virtuous API without query conditions.
        /// </summary>
        /// <param name="skip">The number of contacts to skip (for pagination).</param>
        /// <param name="take">The number of contacts to fetch in a single call.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PagedResult{AbbreviatedContactDTO}"/> with the contacts.</returns>
        /// <exception cref="Exception">Thrown if the API call fails or returns an invalid response.</exception>
        public async Task<PagedResult<AbbreviatedContactDTO>> GetContactsAsync(int skip, int take)
        {
            try
            {
                var request = new RestRequest("/api/Contact/Query", Method.Post);
                request.AddQueryParameter("Skip", skip.ToString());
                request.AddQueryParameter("Take", take.ToString());

                var body = new ContactQueryRequest();
                request.AddJsonBody(body);

                var response = await _restClient.PostAsync<PagedResult<AbbreviatedContactDTO>>(request);

                if (response == null)
                {
                    throw new Exception("Received null response from the API.");
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching contacts (Skip: {skip}, Take: {take}): {ex.Message}");
                throw; 
            }
        }

        /// <summary>
        /// Fetches a paged list of contacts from the Virtuous API with specified query conditions.
        /// </summary>
        /// <param name="skip">The number of contacts to skip (for pagination).</param>
        /// <param name="take">The number of contacts to fetch in a single call.</param>
        /// <param name="queryGroups">A list of query conditions grouped logically for filtering contacts.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PagedResult{AbbreviatedContactDTO}"/> with the filtered contacts.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="queryGroups"/> is null or empty.</exception>
        /// <exception cref="Exception">Thrown if the API call fails or returns an invalid response.</exception>
        public async Task<PagedResult<AbbreviatedContactDTO>> GetContactsAsync(int skip, int take, List<Group> queryGroups)
        {
            if (queryGroups == null || queryGroups.Count == 0)
            {
                throw new ArgumentNullException(nameof(queryGroups), "Query groups cannot be null or empty.");
            }

            try
            {
                var request = new RestRequest("/api/Contact/Query", Method.Post);
                request.AddQueryParameter("Skip", skip.ToString());
                request.AddQueryParameter("Take", take.ToString());

                var body = new ContactQueryRequest();
                body.Groups.AddRange(queryGroups);
                request.AddJsonBody(body);

                var response = await _restClient.PostAsync<PagedResult<AbbreviatedContactDTO>>(request);

                if (response == null)
                {
                    throw new Exception("Received null response from the API.");
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching contacts with query groups (Skip: {skip}, Take: {take}): {ex.Message}");
                throw; 
            }
        }
    }
}
