using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SoundsLike
{
    [XmlRoot("config", Namespace = "", IsNullable = false)]
    public class Config
    {
        public Config()
        {
            Settings = new List<ConfigSet>();
        }

        [XmlElement("codify", typeof(ConfigSet))]
        public List<ConfigSet> Settings { get; set; }

        /// <summary>
        /// ConvertStringToBytes: Convert a string to a byte array
        /// </summary>
        /// <param name="text">The text to be converted</param>
        /// <returns></returns>
        private static byte[] ConvertStringToBytes(string text)
        {
            var ms = new MemoryStream();
            using (var writer = new StreamWriter(ms) { AutoFlush = true })
            {
                writer.Write(text);
            }
            return ms.ToArray();
        }

        /// <summary>
        /// Deserialize: Deserialize Xml to Fetch Object
        /// </summary>
        /// <param name="xml">Xml String</param>
        /// <returns>Fetch Object</returns>
        public static Config Deserialize(string xml)
        {
            var byteArray = ConvertStringToBytes(xml);
            var xs = new XmlSerializer(typeof(Config));
            var ms = new MemoryStream(byteArray);
            return (Config)xs.Deserialize(ms);
        }        
    }
}
