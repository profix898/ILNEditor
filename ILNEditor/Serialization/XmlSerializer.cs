using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;

namespace ILNEditor.Serialization
{
    public class XmlSerializer : ISerializer
    {
        private readonly XmlDocument document;

        public XmlSerializer()
        {
            document = new XmlDocument();
            document.AppendChild(document.CreateElement("SceneSettings"));
            document.DocumentElement.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            document.DocumentElement.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
        }

        public XmlDocument Document
        {
            [DebuggerStepThrough]
            get { return document; }
        }

        #region ISerializer Members

        public void Set(string[] path, string name, object value)
        {
            if (value == null)
                return;

            XmlNode parent = path.Aggregate<string, XmlNode>(document.DocumentElement,
                                                             (current, part) => current.SelectSingleNode(part) ?? current.AppendChild(document.CreateElement(part)));

            Serialize(parent, name, value);
        }

        #endregion

        public string SaveToString()
        {
            using (TextWriter textWriter = new StringWriter())
            {
                document.Save(textWriter);
                return textWriter.ToString();
            }
        }

        public void SaveToFile(string filename)
        {
            document.Save(filename);
        }

        private void Serialize(XmlNode parent, string name, object value)
        {
            XPathNavigator navigator = parent.AppendChild(document.CreateElement(name)).CreateNavigator();
            using (XmlWriter writer = navigator.AppendChild())
            {
                // Handle non-serializable types
                if (value is Font)
                    value = new SerializableFont((Font) value);
                else if (value is Color)
                    value = new SerializableColor((Color) value);

                var serializer = new System.Xml.Serialization.XmlSerializer(value.GetType());
                writer.WriteWhitespace(String.Empty);
                serializer.Serialize(writer, value);
                writer.Close();
            }
        }

        #region StaticUtility

        public static string SerializeToString(ILPanelEditor editor)
        {
            var serializer = new XmlSerializer();
            editor.Serialize(serializer);

            return serializer.SaveToString();
        }

        public static void SerializeToFile(ILPanelEditor editor, string filename)
        {
            var serializer = new XmlSerializer();
            editor.Serialize(serializer);

            serializer.SaveToFile(filename);
        }

        #endregion
    }
}
