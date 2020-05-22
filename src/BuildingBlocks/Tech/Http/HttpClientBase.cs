using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Gherkin.BuildingBlocks.Tech.Http
{
    public abstract class HttpClientBase
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;

        private readonly Func<string> _generateToken;

        public HttpClientBase(HttpClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;           
        }

        protected async Task<U> GetAsync<U>(string requestUrl, IEnumerable<Claim> claims)
        {
            AddClaimsToRequest(claims);
            if (_generateToken != null)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _generateToken());
            }
            var response = await Client.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            EnsureResponseSuccess(response);
            return await response.Content.ReadAsAsync<U>();
        }

        protected async Task<U> GetAsync<U>(string requestUrl)
        {
            if (_generateToken != null)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _generateToken());
            }
            var response = await Client.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            EnsureResponseSuccess(response);
            return await response.Content.ReadAsAsync<U>();
        }

        private void AddClaimsToRequest(IEnumerable<Claim> claims)
        {
            // add header etc.
        }

        protected async Task<string> GetPlainStringAsync(string requestUrl)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
            var response = await Client.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            EnsureResponseSuccess(response);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<U> PostAsync<U, X>(string relativePath, X model)
        {
            if (_generateToken != null)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _generateToken());
            }
            var response = await Client.PostAsync(relativePath, SerializeContent(model));
            EnsureResponseSuccess(response);
            return await response.Content.ReadAsAsync<U>();
        }

        protected async Task<U> PutAsync<U, X>(string requestUrl, X model)
        {
            var response = await Client.PutAsync(requestUrl, SerializeContent(model));
            EnsureResponseSuccess(response);
            return await response.Content.ReadAsAsync<U>();
        }

        protected async Task PutAsync<T>(string requestUrl, T model)
        {
            var response = await Client.PutAsync(requestUrl, SerializeContent(model));
            EnsureResponseSuccess(response);
        }     

        protected virtual void EnsureResponseSuccess(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
        }

        private HttpContent SerializeContent<U>(U model)
        {
            //TODO: Inject as Strategy
            return new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
        }

        protected HttpClient Client
        {
            get { return _client; }
        }
        protected ILogger Logger
        {
            get { return _logger; }
        }
    }
}