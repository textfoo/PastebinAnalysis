using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpService.Interfaces
{
    public interface IHttpService : IDisposable
    {
        Task<HttpResponseMessage> GetAsync(string endpoint, List<KeyValuePair<string, string>> headers = null);
        Task<HttpResponseMessage> GetFormUrlEncodedAsync(string endpoint, List<KeyValuePair<string, string>> headers = null);
        Task<HttpResponseMessage> PostFormUrlEncodedAsync(string endpoint, List<KeyValuePair<string, string>> headers, List<KeyValuePair<string, string>> data);
        Task<HttpResponseMessage> GetWithCredentialAsync(CredentialCache credentialCache, string endpoint, List<KeyValuePair<string, string>> headers = null);
        Task<HttpResponseMessage> PostJsonAsync(string endpoint, List<KeyValuePair<string, string>> headers, string data);
        Task<HttpResponseMessage> PostPlainTextAsync(string endpoint, List<KeyValuePair<string, string>> headers, string data = null);
        Task<HttpResponseMessage> PutJsonAsync(string endpoint, List<KeyValuePair<string, string>> headers, string data);
        Task<HttpResponseMessage> PostMultiPartFormData(string endpoint, List<KeyValuePair<string, string>> headers, string data, string fileName);
        string ReturnHtmlEncodedText(string input); 
    }
}
