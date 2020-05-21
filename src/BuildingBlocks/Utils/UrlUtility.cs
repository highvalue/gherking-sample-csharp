using System;

namespace Gherkin.BuildingBlocks.Utils
{
	// inspiration: https://stackoverflow.com/a/33465715

	public static class UrlUtility
    {
		const char PATH_DELIMITER = '/';

		public static string Combine(string path, string relative)
        {
			if (relative == null)
				relative = String.Empty;

			if (path == null)
				path = String.Empty;

			if (relative.Length == 0 && path.Length == 0)
				return String.Empty;

			if (relative.Length == 0)
				return path;

			if (path.Length == 0)
				return relative;

			path = path.Replace('\\', PATH_DELIMITER);
			relative = relative.Replace('\\', PATH_DELIMITER);

			return path.TrimEnd(PATH_DELIMITER) + PATH_DELIMITER + relative.TrimStart(PATH_DELIMITER);
		}
        public static string Combine(string server, string path, string relative, int port, bool useHttps)
        {
            string url = useHttps ? "https://" : "http://";
            string p = Combine(path, relative);

            url = $"{url}{server}:{port.ToString()}/{p}";
            return url;
        }
    }

}