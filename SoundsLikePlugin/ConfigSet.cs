using System.ComponentModel;
using System.Xml.Serialization;

namespace SoundsLike
{
    [XmlType(AnonymousType = true)]
    public enum CodificationMethod
    {
        [XmlEnum("soundex")]
        Soundex,
        [XmlEnum("soundex-original")]
        SoundexOriginal,
        [XmlEnum("metaphone")]
        Metaphone
    }

    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class ConfigSet
    {
        [XmlAttribute("method"),DefaultValue(CodificationMethod.Metaphone)]
        public CodificationMethod Method { get; set; }

        [XmlAttribute("source"),DefaultValue("name")]
        public string Source { get; set; }

        [XmlAttribute("target")]
        public string Target { get; set; }

        [XmlAttribute("disable-manual-update"),DefaultValue(false)]
        public bool DisableUpdate { get; set; }

        [XmlAttribute("min"),DefaultValue(0)]
        public int MinLength { get; set; }

        [XmlAttribute("max"),DefaultValue(6)]
        public int MaxLength { get; set; }
    }
}
