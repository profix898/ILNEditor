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

        [XmlElement("Alpha")]
        public byte SerializeAlpha
        {
            get { return Color.A; }
            set { Color = Color.FromArgb(value, Color); }
        }

        [XmlElement("Color")]
        public string SerializeColor
        {
            get
            {
                try
                {
                    if (Color.IsEmpty)
                        return null;

                    return ColorTranslator.ToHtml(Color);
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
                    Color = Color.FromArgb(Color.A, ColorTranslator.FromHtml(value));
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
