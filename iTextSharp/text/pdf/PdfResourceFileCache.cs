using System.Collections.Concurrent;

namespace iTextSharp.text.pdf
{
    public class PdfResourceFileCache
    {
        private static readonly ConcurrentDictionary<string, byte[]> s_Cache = new ConcurrentDictionary<string, byte[]>();

        public static bool TrySet(string key, byte[] value)
        {
            return s_Cache.TryAdd(key, value);
        }

        public static byte[] Get(string key)
        {
            byte[] ret = null;
            s_Cache.TryGetValue(key, out ret);
            return ret;
        }

        public static bool ContainsKey(string key)
        {
            return s_Cache.ContainsKey(key);
        }
    }
}
