using System;
using System.Windows.Forms;
using ILNEditor;
using ILNumerics;
using ILNumerics.Drawing.Plotting;

namespace Demo
{
    public partial class ILPanelForm : Form
    {
        private ILPlotCube plotCube;

        public ILPanelForm()
        {
            InitializeComponent();
        }

        private void ILPanelForm_Load(object sender, EventArgs e)
        {
            // Create plotcube
            plotCube = ilPanel.Scene.Add(new ILPlotCube());

            // Add random data lineplots (for demonstration purposes)
            plotCube.Add(new ILLinePlot(ILMath.tosingle(ILMath.randn(2, 200))));
            plotCube.Add(new ILLinePlot(ILMath.tosingle(ILMath.randn(2, 200) + 5.0)));

            // Attach editor
            ILPanelEditor.AttachTo(ilPanel);
        }
    }
}
