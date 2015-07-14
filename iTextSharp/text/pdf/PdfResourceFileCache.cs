using System.Collections.Generic;

namespace iTextSharp.text.pdf
{
    public class PdfResourceFileCache
    {
        private static Dictionary<string, byte[]> m_Cache = new Dictionary<string, byte[]>();

        public static void Set(string key, byte[] value)
        {
            if (m_Cache.ContainsKey(key))
            {
                m_Cache[key] = value;
            }
            else
            {
                m_Cache.Add(key, value);
            }
        }

        public static byte[] Get(string key)
        {
            return m_Cache.ContainsKey(key) ? m_Cache[key] : null;
        }

        public static bool ContainsKey(string key)
        {
            return m_Cache.ContainsKey(key);
        }
    }
}
