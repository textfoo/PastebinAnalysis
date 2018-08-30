using HttpService.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpService.Services
{
    public class HttpService : IHttpService
    {
        private HttpClientHandler _handler = new HttpClientHandler(); 
        private static HttpClient _client;
        private readonly ILogger<HttpService> _log;

        public HttpService(ILogger<HttpService> log)
        {
            _log = log;
            //wet, wild, and wacky
            _handler.ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
            _client = new HttpClient(_handler);
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint, List<KeyValuePair<string, string>> headers = null)
        {
            AddClientHeaders(headers);
            return await _client.GetAsync(endpoint);
        }

        public async Task<HttpResponseMessage> GetFormUrlEncodedAsync(string endpoint, List<KeyValuePair<string, string>> headers = null)
        {
            return await GetAsync(endpoint, headers);
        }

        /// <summary>
        /// Probably will break stuff. 
        /// </summary>
        /// <param name="credentialCache"></param>
        /// <param name="endpoint"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetWithCredentialAsync(CredentialCache credentialCache, string endpoint, List<KeyValuePair<string, string>> headers = null)
        {

            _handler.Credentials = credentialCache;
                _client = new HttpClient(_handler);
            if (headers != null)
                foreach (var pair in headers)
                    _client.DefaultRequestHeaders.TryAddWithoutValidation(pair.Key, pair.Value);
            return await _client.GetAsync(endpoint); 
        }

        public async Task<HttpResponseMessage> PostFormUrlEncodedAsync(string endpoint, List<KeyValuePair<string, string>> headers, List<KeyValuePair<string, string>> data)
        {
            AddClientHeaders(headers);
            using (var content = new FormUrlEncodedContent(data))
            {
                content.Headers.Clear();
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                return await _client.PostAsync(endpoint, content);
            }
        }

        public async Task<HttpResponseMessage> PostJsonAsync(string endpoint, List<KeyValuePair<string, string>> headers, string data)
        {
            AddClientHeaders(headers);
            using (var content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(data)))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return await _client.PostAsync(endpoint, content);
            }
        }

        public async Task<HttpResponseMessage> PostMultiPartFormData(string endpoint, List<KeyValuePair<string, string>> headers, string data, string fileName)
        {
            AddClientHeaders(headers);
            using (var content = new MultipartFormDataContent())
            using (var stringData = new StringContent(data))
            {
                content.Add(stringData, "file", fileName);
                return await _client.PostAsync(endpoint, content); 
            }

        }

        public async Task<HttpResponseMessage> PostPlainTextAsync(string endpoint, List<KeyValuePair<string, string>> headers, string data = null)
        {
            AddClientHeaders(headers);
            using (var content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(data)))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
                return await _client.PostAsync(endpoint, content);
            }
        }

        public async Task<HttpResponseMessage> PutJsonAsync(string endpoint, List<KeyValuePair<string, string>> headers, string data)
        {
            AddClientHeaders(headers);
            using (var content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(data)))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return await _client.PutAsync(endpoint, content);
            }
        }

        private void AddClientHeaders(List<KeyValuePair<string, string>> headers)
        {
            headers = headers ?? new List<KeyValuePair<string, string>>();
            _client.DefaultRequestHeaders.Clear();
            headers.ForEach(x =>
            {
                _client.DefaultRequestHeaders.TryAddWithoutValidation(x.Key, x.Value);
            });
        }

        public string ReturnHtmlEncodedText(string input)
        {
            input = input ?? "";
            //return System.Web.HttpUtility.UrlEncode(input); 
            return System.Uri.EscapeDataString(input); 
        }

        public void Dispose()
        {
            _client.CancelPendingRequests();
            _client.Dispose();
        }
    }
}
