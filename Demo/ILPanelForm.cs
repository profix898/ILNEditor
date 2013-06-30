using System;
using System.Drawing;
using System.Windows.Forms;
using ILNEditor;
using ILNumerics;
using ILNumerics.Drawing;
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
            // create some data 
            ILArray<float> A = ILMath.ones<float>(1, 10);
            // add plot cube with legend
            ILPlotCube plotCube = ilPanel.Scene.Add(new ILPlotCube
            {
                new ILLegend
                {
                    Location = new PointF(1, 0.02f),
                    Anchor = new PointF(1, 0)
                }
            });
            // get enumerator for all marker styles
            Array styles = Enum.GetValues(typeof(MarkerStyle));
            float offset = 0;
            // iterate marker styles 
            foreach (MarkerStyle style in styles)
            {
                // add new line plot for every marker style
                plotCube.Add(new ILLinePlot(
                                 A + offset++, style, markerStyle: style));
            }

            //// Create plotcube
            //plotCube = ilPanel.Scene.Add(new ILPlotCube());

            //// Add random data lineplots (for demonstration purposes)
            //plotCube.Add(new ILLinePlot(ILMath.tosingle(ILMath.randn(2, 200))));
            //plotCube.Add(new ILLinePlot(ILMath.tosingle(ILMath.randn(2, 200) + 5.0)));

            //// Attach editor
            ILPanelEditor.AttachTo(ilPanel);
        }
    }
}
