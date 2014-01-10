using System;

namespace ILNEditor.Serialization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class SerializerIgnoreAttribute : Attribute
    {
    }
}
