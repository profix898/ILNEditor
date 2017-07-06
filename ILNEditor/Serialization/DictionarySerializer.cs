using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ILNEditor.Serialization
{
    public class DictionarySerializer : ISerializer
    {
        private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

        private readonly string keySeparator;
        private readonly string pathSeparator;

        public DictionarySerializer(string pathSeparator = ":", string keySeparator = "::")
        {
            this.pathSeparator = pathSeparator;
            this.keySeparator = keySeparator;
        }

        public DictionarySerializer(Dictionary<string, object> dictionary, string pathSeparator = ":", string keySeparator = "::")
        {
            this.dictionary = dictionary;
            this.pathSeparator = pathSeparator;
            this.keySeparator = keySeparator;
        }

        public Dictionary<string, object> Dictionary
        {
            [DebuggerStepThrough]
            get { return dictionary; }
        }

        #region Implementation of ISerializer

        public void Set(string[] path, string name, object value)
        {
            string key = PathToKeyString(path, name, pathSeparator, keySeparator);

            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }

        #endregion

        internal static string PathToKeyString(string[] path, string name = null, string pathSeparator = ":", string keySeparator = "::")
        {
            if (String.IsNullOrEmpty(name))
                return String.Join(pathSeparator, path);

            return $"{String.Join(pathSeparator, path)}{keySeparator}{name}";
        }
    }
}
