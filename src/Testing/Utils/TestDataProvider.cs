using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Gherkin.Testing.Utils.Extensions;

namespace Gherkin.Testing.Utils
{
  public static class TestDataProvider
    {

        public static string[] LinesFromFile<TEntryPoint>(string pathWithFile)
        {
            var @assembly = Assembly.GetAssembly(typeof(TEntryPoint));
            var path = Path.GetDirectoryName(@assembly.Location);

            var fullPath = Path.Combine(path, pathWithFile);
           
            return File.ReadAllLines(fullPath);
        }

        public static string FromFile<TEntryPoint>(string pathWithFile)
        {
            var @assembly = Assembly.GetAssembly(typeof(TEntryPoint));
            var path = Path.GetDirectoryName(@assembly.Location);

            var fullPath = Path.Combine(path, pathWithFile);

            return File.ReadAllText(fullPath);
        }

        public static IEnumerable<T> FromFile<TEntryPoint,T>(string pathWithFile, int count, Action<Exception> onFail)
        { 
            return LinesFromFile<TEntryPoint>(pathWithFile)                                   
                  .SelectTry(x => x.FromJsonToType<T>())
                  .OnFail(x => onFail(x))
                  .SelectSuccess()
                  .Take(count);
        }

    }
}
