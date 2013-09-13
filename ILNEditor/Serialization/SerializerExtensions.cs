using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using ILNEditor.Drawing;

namespace ILNEditor.Serialization
{
    public static class SerializerExtensions
    {
        public static void Serialize(this ILPanelEditor editor, ISerializer serializer)
        {
            foreach (ILWrapperBase wrapper in editor.Wrappers)
                SerializeInternal(editor, serializer, wrapper, GetProperties(wrapper), ToIdentifier(wrapper.FullName));
        }

        private static void SerializeInternal(ILPanelEditor editor, ISerializer serializer, object instance, IEnumerable<PropertyInfo> properties, string path)
        {
            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead)
                {
                    object value = property.GetValue(instance, null);

                    if (value != null && editor.WrapperMap.Values.Contains(value.GetType()))
                    {
                        PropertyInfo[] childProperties = GetProperties(value);
                        if (childProperties.Length > 0)
                        {
                            SerializeInternal(editor, serializer, value, childProperties, String.Format("{0}:{1}", path, ToIdentifier(property.Name)));
                            continue;
                        }
                    }

                    if (property.CanWrite)
                        serializer.Set(path.Split(':'), ToIdentifier(property.Name), value);
                }
            }
        }

        public static void Deserialize(this ILPanelEditor editor, IDeserializer deserializer)
        {
            foreach (ILWrapperBase wrapper in editor.Wrappers)
                DeserializeInternal(editor, deserializer, wrapper, GetProperties(wrapper), ToIdentifier(wrapper.FullName));
        }

        private static void DeserializeInternal(ILPanelEditor editor, IDeserializer deserializer, object instance, IEnumerable<PropertyInfo> properties, string path)
        {
            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    object value = property.GetValue(instance, null);

                    string configPrefixChild = String.Format("{0}:{1}", path, ToIdentifier(property.Name));
                    if (deserializer.Contains(configPrefixChild.Split(':')))
                    {
                        PropertyInfo[] childProperties = GetProperties(value);
                        if (childProperties.Length > 0 && editor.WrapperMap.Values.Contains(value.GetType()))
                        {
                            DeserializeInternal(editor, deserializer, value, childProperties, String.Format("{0}:{1}", path, ToIdentifier(property.Name)));
                            continue;
                        }
                    }

                    try
                    {
                        property.SetValue(instance, deserializer.Get(path.Split(':'), property.Name, property.PropertyType), null);
                    }
                    catch (XmlException)
                    {
                        // Exception in xml deserialization (e.g. element not found or not deserializable)
                        int i = 0;
                    }
                }
            }
        }

        private static PropertyInfo[] GetProperties(object instance)
        {
            // Get properties (skip those with SerializerIgnore attribute)
            PropertyInfo[] instanceProperties = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            IEnumerable<PropertyInfo> properties = instanceProperties.Where(property => !property.GetCustomAttributes(typeof(SerializerIgnoreAttribute), false).Any()
                                                                                        && property.CanRead && property.CanWrite
                                                                                        && property.GetAccessors().Any(accessor => accessor.GetParameters().Length == 0));

            // Sort properties by weight (via SerializerWeightAttribute)
            return properties.OrderBy(property =>
            {
                object[] orderAttribute = property.GetCustomAttributes(typeof(SerializerWeightAttribute), false);
                return (orderAttribute.Length > 0) ? ((SerializerWeightAttribute) orderAttribute[0]).Weight : 0;
            }).ToArray();
        }

        private static string ToIdentifier(string input)
        {
            return Regex.Replace(Regex.Replace(input, @"[^\u0000-\u007F]", ""), @"[^\w]", "").Replace("#", "");
        }
    }
}
