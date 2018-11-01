using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace ILNEditor.Serialization
{
    public class XmlDeserializer : IDeserializer
    {
        private readonly XmlDocument document;

        public XmlDeserializer()
        {
            document = new XmlDocument();
        }

        public XmlDocument Document
        {
            [DebuggerStepThrough]
            get { return document; }
        }

        public void LoadFromString(string xmlString)
        {
            using (TextReader reader = new StringReader(xmlString))
            {
                document.Load(reader);
            }
        }

        public void LoadFromFile(string filename)
        {
            document.Load(filename);
        }

        private object Deserialize(XmlNode node, string name, Type type)
        {
            node = node.SelectSingleNode(name);
            if (node == null)
                throw new XmlException("XmlElement does not exist.");

            XPathNavigator navigator = node.FirstChild.CreateNavigator();
            using (XmlReader reader = navigator.ReadSubtree())
            {
                // Handle not-serializable types
                if (type == typeof(Font))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(SerializableFont));
                    return ((SerializableFont) serializer.Deserialize(reader)).Font;
                }
                if (type == typeof(Color) || type == typeof(Color?))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(SerializableColor));
                    return ((SerializableColor) serializer.Deserialize(reader)).Color;
                }

                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(type);
                    return serializer.Deserialize(reader);
                }
            }
        }

        private XmlNode GetNodeByPath(string[] path)
        {
            XmlNode parent = document.DocumentElement;
            foreach (string part in path)
            {
                parent = parent?.SelectSingleNode(part);
                if (parent == null)
                    return null;
            }

            return parent;
        }

        #region StaticUtility

        public static void DeserializeFromString(PanelEditor editor, string xmlString)
        {
            var deserializer = new XmlDeserializer();
            deserializer.LoadFromString(xmlString);
            editor.Deserialize(deserializer);
        }

        public static void DeserializeFromFile(PanelEditor editor, string filename)
        {
            var deserializer = new XmlDeserializer();
            deserializer.LoadFromFile(filename);
            editor.Deserialize(deserializer);
        }

        #endregion

        #region Implementation of IDeserializer

        public bool Contains(string[] path)
        {
            return (GetNodeByPath(path) != null);
        }

        public bool Contains(string[] path, string name)
        {
            XmlNode node = GetNodeByPath(path);
            if (node == null)
                return false;

            return (node.SelectSingleNode(name) != null);
        }

        public object Get(string[] path, string name, Type type)
        {
            XmlNode node = GetNodeByPath(path);
            if (node == null)
                throw new XmlException("XmlNode does not exist.");

            return Deserialize(node, name, type);
        }

        #endregion
    }
}
