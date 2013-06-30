using System.Windows.Forms;

namespace ILNEditor
{
    public sealed partial class EditorForm : Form
    {
        public EditorForm(object objectProperty, string title = "Properties")
        {
            InitializeComponent();

            Text = title;

            propertyGrid.SelectedObject = objectProperty;
        }
    }
}
