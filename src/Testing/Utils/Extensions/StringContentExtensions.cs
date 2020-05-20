using System.Net.Http;
using System.Text;

namespace Gherkin.Testing.Utils.Extensions
{ 
    public static class StringContentExtensions
    {
        public static StringContent ToJsonContent(this string json)
        {
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            return content;
        }

        public static StringContent ToJsonContent<T>(this T source)
        {            
            if (source is string)
                return source.ToJsonContent();
           
            StringContent content = new StringContent(source.ToJson(), UnicodeEncoding.UTF8, "application/json");

            return content;
        }       

        public static StringContent ToXmlContent(this string xml)
        {
            StringContent content = new StringContent(xml, UnicodeEncoding.UTF8, "application/xml");

            return content;
        }

        public static StringContent ToXmlContent<T>(this T source)
        {
            StringContent content = new StringContent(source.ToXml(), UnicodeEncoding.UTF8, "application/xml");

            return content;
        }

    }
}
