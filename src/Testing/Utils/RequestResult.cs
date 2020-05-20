using System.Net;

namespace Gherkin.Testing.Utils
{
   public class RequestResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
    }
}
