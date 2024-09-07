
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json; 
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WallBank.Application.Exceptions;
using WallBank.Application.Interfaces;
using WallBank.Application.Wrappers;

namespace WallBank.Application.HelperServices
{
    [ExcludeFromCodeCoverage]
    public class HttpHelperClient : IhttpClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<HttpHelperClient> _logger;

        public const string ErrorMessage = "An Error Occured";
        public HttpHelperClient(HttpClient client, ILogger<HttpHelperClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<T> GetData<T>(string url)
        {
            var response = await _client.GetAsync(url);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ApiException("No matching record found");

            var datastring = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(datastring);
            return data;
        }

        public async Task<object> GetDataObject(string url)
        {
            var response = await _client.GetAsync(url);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApiException("No matching record found");
            }

            var datastring = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<object>(datastring);
            return data;
        }

        public async Task<PagedResponse<IEnumerable<T>>> GetPaginatedData<T>(string url)
        {
            return await _client.GetFromJsonAsync<PagedResponse<IEnumerable<T>>>(url);
        }

        public async Task<(bool status, string message, object data)> PostData(object model, string url)
        {

            var response = await _client.PostAsJsonAsync(url, model);
            var data = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responsedata = JsonConvert.DeserializeObject<Response<object>>(data);
                return (true, responsedata?.Message ?? "Success", responsedata?.Data);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                throw new ApiException("Invalid data sent");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new ApiException("You are not authorized to view this endpoint");
            }
            throw new ApiException(ErrorMessage);


        }
         
        public async Task<(bool status, string message, object data)> PutData(object model, string url)
        {
            var response = await _client.PutAsJsonAsync(url, model);
            var data = await response.Content.ReadFromJsonAsync<Response<object>>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return (true, data?.Message ?? "Success", data?.Data);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new ApiException(data?.Message ?? "Error occured, Bad request");
            }
            throw new ApiException(data?.Message);
        }

       

        public async Task<T> PostData<T>(object model, string url, string key)
        {
            try
            {
                if (_client.DefaultRequestHeaders.Authorization == null)
                    _client.DefaultRequestHeaders.Add("Authorization", key);


                var response = await _client.PostAsJsonAsync(url, model);
                var datastring = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("path: {Path}, payload: {@Payload}, response:{Response}", url, model, datastring);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new ApiException("You are not authorised");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new ApiException("Could not locate endpoint");

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                   throw new ApiException("An error occurred", datastring);

                }


                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<T>(datastring);
                    return data;

                    //var data = JsonConvert.DeserializeObject<T>(datastring, new JsonSerializerSettings()
                    //{
                    //	Error = (sender, error) => error.ErrorContext.Handled = true

                    //});
                    //return data;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "path: {Path}, payload: {@Payload}", url, model);

                throw new ApiException(ex.Message);
            }

            throw new ApiException("Remote server returned an error occured");
        }

        public void AddHeader(Dictionary<string, string> header)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                foreach (var item in header)
                {
                    _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }


        }

    }
    
}
