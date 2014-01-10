using System;
using System.Drawing;
using System.Xml.Serialization;

namespace ILNEditor.Serialization
{
    public class SerializableColor
    {
        // Ctor for Deserialization
        private SerializableColor()
        {
            Color = Color.Empty;
        }

        public SerializableColor(Color color)
        {
            Color = color;
        }

        [XmlIgnore]
        public Color Color { get; private set; }

        [XmlText]
        public string SerializeColor
        {
            get
            {
                try
                {
                    if (Color.IsEmpty)
                        return null;

                    return String.Format("{0:X2}{1}", Color.A, ColorTranslator.ToHtml(Color));
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
                    Color = Color.FromArgb(Convert.ToByte(value.Substring(0, 2), 16), ColorTranslator.FromHtml(value.Substring(2)));
                }
                catch
                {
                    Color = Color.Empty;
                }
            }
        }

        public static implicit operator Color(SerializableColor serializeableColor)
        {
            if (serializeableColor == null)
                return Color.Empty;

            return serializeableColor.Color;
        }

        public static implicit operator SerializableColor(Color? color)
        {
            return new SerializableColor(color.HasValue ? color.Value : Color.Empty);
        }
    }
}
