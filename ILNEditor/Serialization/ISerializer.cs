using System;

namespace ILNEditor.Serialization
{
    public interface ISerializer
    {
        void Set(string[] path, string name, object value);
    }

    public interface IDeserializer
    {
        bool Contains(string[] path);

        bool Contains(string[] path, string name);

        object Get(string[] path, string name, Type type);
    }
}
