using System;
using System.Drawing;
using System.Windows.Forms;
using ILNEditor;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace Demo
{
    public partial class DemoForm : Form
    {
        #region DemoEnum enum

        public enum DemoEnum
        {
            RandomLinePlot,
            SincSurface,
            GreenSphere
        }

        #endregion

        private ILPanelEditor editor;

        public DemoForm()
        {
            InitializeComponent();
        }

        private void ILPanelForm_Load(object sender, EventArgs e)
        {
            comboBoxDemo.Items.AddRange(Enum.GetNames(typeof(DemoEnum)));
            comboBoxDemo.SelectedIndex = 1; // Select first demo by default
        }

        private void comboBoxDemo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDemo.SelectedIndex == -1)
                return;

            // Dispose editor and reset scene
            if (editor != null)
                editor.Dispose();
            foreach (ILNode node in ilPanel.Scene.Find<ILNode>())
                ilPanel.Scene.Remove(node);

            // Add and render demo content
            switch ((DemoEnum) comboBoxDemo.SelectedIndex)
            {
                case DemoEnum.RandomLinePlot:
                {
                    ILPlotCube plotCube = ilPanel.Scene.Add(new ILPlotCube());
                    plotCube.Add(new ILLinePlot(ILMath.tosingle(ILMath.randn(2, 200))));
                    plotCube.Add(new ILLinePlot(ILMath.tosingle(ILMath.randn(2, 200) + 5.0)));
                    plotCube.Add(new ILLegend("Line 1", "Line 2")); // TODO: Legend not rendered/visible. Why?
                }
                    break;
                case DemoEnum.SincSurface:
                {
                    ILPlotCube plotCube = ilPanel.Scene.Add(new ILPlotCube(null, false));
                    plotCube.Add(new ILSurface(ILSpecialData.sincf(40, 60, 2.5f))
                    {
                        Wireframe = { Color = Color.FromArgb(50, Color.LightGray) },
                        Colormap = Colormaps.Jet
                    });
                    ilPanel.Scene.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(1f, 0.23f, 1), 0.7f);
                }
                    break;
                case DemoEnum.GreenSphere:
                {
                    ilPanel.Scene.Add(new ILSphere());
                    // TODO: Sphere not rendered at all
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Attach ILNEditor
            editor = ILPanelEditor.AttachTo(ilPanel);
        }
    }
}
