using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Gherkin.Testing.Utils.Extensions
{
  public static class SerializationExtensions
    {   
        public static string ToJson<T>(this T source)
        {
            if (source is string)
                return source as string;

            return JsonConvert.SerializeObject(source);
        }

        public static T FromJsonToType<T>(this string source)
        {
            return JsonConvert.DeserializeObject<T>(source);
        }

        public static T FromXmlToType<T>(this string source) where T : class
        {
          using var reader =  new StringReader(source);

          return  new XmlSerializer(typeof(T))
           .Deserialize(reader) as T;
        }

        public static string ToXml<T>(this T source)
        {
            var encoding = new UTF8Encoding();
            using var memStrm = new MemoryStream();            
            using var xmlSink = new XmlTextWriter(memStrm, encoding);
          
            new XmlSerializer(typeof(T)).Serialize(xmlSink, source);           
            return encoding.GetString(memStrm.ToArray());          
        }
    }
}
