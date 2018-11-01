namespace ILNEditor.Serialization
{
    public interface ISerializer
    {
        void Set(string[] path, string name, object value);
    }
}
