namespace ILNEditor
{
    public interface IDialog
    {
        bool Visible { get; }

        void Show();

        void Hide();
    }
}
