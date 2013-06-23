using System;
using System.Windows.Forms;

namespace ILNEditor
{
    public sealed partial class ObjectPropertyForm : Form
    {
        public ObjectPropertyForm(object objectProperty, string title = "Properties")
        {
            InitializeComponent();

            Text = title;

            propertyGridObject.SelectedObject = objectProperty;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
