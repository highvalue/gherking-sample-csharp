using System.Net.Http;
using System.Net.Http.Headers;

namespace Gherkin.Testing.Utils.Extensions
{
    public static class HttpClientHandlerExtensions
    {
        public static HttpResponseMessage PostRequest(this HttpClient client, string requestUrl, StringContent stringContent, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = client.PostAsync(requestUrl, stringContent).GetAwaiter().GetResult();
            return response;

        }
        public static Either<RequestResult, T> PostRequest<T>(this HttpClient client, string requestUrl, StringContent stringContent, string token, TestingSerializationOptions serializerOptions = TestingSerializationOptions.Json) where T : class
        {

            HttpResponseMessage response = client.PostRequest(requestUrl, stringContent, token);

            var result = response.ToRequestResult();

            return response.IsSuccessStatusCode switch
            {
                false => result,
                true => serializerOptions switch
                {
                    TestingSerializationOptions.Json => result.Content.FromJsonToType<T>(),
                    TestingSerializationOptions.Xml => result.Content.FromXmlToType<T>()
                }
            };
        }


        public static HttpResponseMessage GetRequest(this HttpClient client, string requestUrl, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = client.GetAsync(requestUrl).GetAwaiter().GetResult();

            return response;
        }

        public static Either<RequestResult, T> GetRequest<T>(this HttpClient client, string requestUrl, string token, TestingSerializationOptions serializerOptions = TestingSerializationOptions.Json) where T : class
        {
            HttpResponseMessage response = client.GetRequest(requestUrl, token);
            
            var result = response.ToRequestResult();

            return response.IsSuccessStatusCode switch
            {
                false => result,
                true => serializerOptions switch
                {
                    TestingSerializationOptions.Json => result.Content.FromJsonToType<T>(),
                    TestingSerializationOptions.Xml => result.Content.FromXmlToType<T>()
                }
            };
        }

        public static RequestResult ToRequestResult(this HttpResponseMessage response)
        {
            var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return new RequestResult() { StatusCode = response.StatusCode, Content = result };
        }
    }
}
