using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ILNEditor.Drawing;

namespace ILNEditor.Serialization
{
    public static class SerializerExtensions
    {
        public static void Serialize(this ILPanelEditor editor, ISerializer serializer)
        {
            foreach (ILWrapperBase wrapper in editor.Wrappers)
                SerializeInternal(editor, serializer, wrapper, GetProperties(wrapper), wrapper.Path);
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
                            SerializeInternal(editor, serializer, value, childProperties, $"{path}:{ToIdentifier(property.Name)}");
                            continue;
                        }
                    }

                    if (property.CanWrite)
                        serializer.Set(SplitPath(path), ToIdentifier(property.Name), value);
                }
            }
        }

        public static void Deserialize(this ILPanelEditor editor, IDeserializer deserializer)
        {
            foreach (ILWrapperBase wrapper in editor.Wrappers)
                DeserializeInternal(editor, deserializer, wrapper, GetProperties(wrapper), wrapper.Path);

            editor.Panel.Configure();
            editor.Update();
        }

        private static void DeserializeInternal(ILPanelEditor editor, IDeserializer deserializer, object instance, IEnumerable<PropertyInfo> properties, string path)
        {
            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    object value = property.GetValue(instance, null);

                    string configPrefixChild = $"{path}:{ToIdentifier(property.Name)}";
                    if (deserializer.Contains(SplitPath(configPrefixChild)))
                    {
                        PropertyInfo[] childProperties = GetProperties(value);
                        if (childProperties.Length > 0 && editor.WrapperMap.Values.Contains(value.GetType()))
                        {
                            DeserializeInternal(editor, deserializer, value, childProperties, $"{path}:{ToIdentifier(property.Name)}");
                            continue;
                        }
                    }

                    try
                    {
                        string[] pathParts = SplitPath(path);
                        if (deserializer.Contains(pathParts, property.Name))
                            property.SetValue(instance, deserializer.Get(pathParts, property.Name, property.PropertyType), null);
                    }
                    catch
                    {
                        // Exception in deserialization (e.g. ElementNotFound or not deserializable)
                    }
                }
            }
        }

        private static PropertyInfo[] GetProperties(object instance)
        {
            Type type = instance.GetType();

            // Skip types decorated with SerializerIgnore attribute
            if (type.GetCustomAttributes(typeof(SerializerIgnoreAttribute), false).Any())
                return new PropertyInfo[] { };

            // Get properties (skip those with SerializerIgnore attribute)
            PropertyInfo[] instanceProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            IEnumerable<PropertyInfo> properties = instanceProperties.Where(property => !property.GetCustomAttributes(typeof(SerializerIgnoreAttribute), false).Any()
                                                                                        && !property.PropertyType.GetCustomAttributes(typeof(SerializerIgnoreAttribute), false).Any()
                                                                                        && property.CanRead && property.CanWrite
                                                                                        && property.GetAccessors().Any(accessor => accessor.GetParameters().Length == 0));

            // Sort properties by weight (via SerializerWeightAttribute)
            return properties.OrderBy(property =>
            {
                object[] orderAttribute = property.GetCustomAttributes(typeof(SerializerWeightAttribute), false);
                return (orderAttribute.Length > 0) ? ((SerializerWeightAttribute) orderAttribute[0]).Weight : 0;
            }).ToArray();
        }

        private static string[] SplitPath(string path)
        {
            return path.Split(':').Select(ToIdentifier).ToArray();
        }

        private static string ToIdentifier(string input)
        {
            return Regex.Replace(Regex.Replace(input, @"[^\u0000-\u007F^:]", ""), @"[^\w]", "").Replace("#", "");
        }
    }
}
