using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;

namespace ILNEditor.Serialization
{
    public class SerializableFont
    {
        // Ctor for Deserialization
        private SerializableFont()
        {
            Font = null;
        }

        public SerializableFont(Font font)
        {
            Font = font;
        }

        [XmlIgnore]
        public Font Font { get; private set; }

        [XmlText]
        public string SerializeFont
        {
            get
            {
                try
                {
                    if (Font == null)
                        return null;

                    return TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(Font);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    Font = (Font) TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(value);
                }
                catch
                {
                    Font = null;
                }
            }
        }

        public static implicit operator Font(SerializableFont serializeableFont)
        {
            if (serializeableFont == null)
                return null;

            return serializeableFont.Font;
        }

        public static implicit operator SerializableFont(Font font)
        {
            return new SerializableFont(font);
        }
    }
}
