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

        object Get(string[] path, string name, Type type);
    }
}
