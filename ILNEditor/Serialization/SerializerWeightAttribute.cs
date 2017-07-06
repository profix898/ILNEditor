using System;

namespace ILNEditor.Serialization
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class SerializerWeightAttribute : Attribute
    {
        public SerializerWeightAttribute(int weight)
        {
            Weight = weight;
        }

        public int Weight { get; }
    }
}
