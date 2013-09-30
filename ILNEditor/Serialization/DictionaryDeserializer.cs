using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ILNEditor.Serialization
{
    public class DictionaryDeserializer : IDeserializer
    {
        private readonly string keySeparator;
        private readonly string pathSeparator;

        private Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public DictionaryDeserializer(string pathSeparator = ":", string keySeparator = "::")
        {
            this.pathSeparator = pathSeparator;
            this.keySeparator = keySeparator;
        }

        public DictionaryDeserializer(Dictionary<string, object> dictionary, string pathSeparator = ":", string keySeparator = "::")
        {
            this.dictionary = dictionary;
            this.pathSeparator = pathSeparator;
            this.keySeparator = keySeparator;
        }

        #region Implementation of IDeserializer

        public bool Contains(string[] path)
        {
            string key = DictionarySerializer.PathToKeyString(path, pathSeparator, keySeparator);

            return dictionary.Any(item => item.Key.StartsWith(key));
        }

        public object Get(string[] path, string name, Type type)
        {
            return dictionary[DictionarySerializer.PathToKeyString(path, name, pathSeparator, keySeparator)];
        }

        #endregion

        public Dictionary<string, object> Dictionary
        {
            [DebuggerStepThrough]
            get { return dictionary; }
            [DebuggerStepThrough]
            set { dictionary = value; }
        }
    }
}
