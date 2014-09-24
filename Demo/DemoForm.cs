using System;
using System.Drawing;
using System.Windows.Forms;
using ILNEditor;
using ILNEditor.Serialization;
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
            LinePlot,
            SincSurface,
            Points,
            Gear
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
            comboBoxDemo.SelectedIndex = (int) DemoEnum.LinePlot;
        }

        private void comboBoxDemo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDemo.SelectedIndex == -1)
                return;

            // Reset scene
            ilPanel.Scene = new ILScene();

            // Add and render demo content
            switch ((DemoEnum) comboBoxDemo.SelectedIndex)
            {
                case DemoEnum.LinePlot:
                {
                    ILPlotCube plotCube = ilPanel.Scene.Add(new ILPlotCube());
                    plotCube.Add(new ILLinePlot(ILMath.tosingle(ILMath.randn(1, 20))));
                    plotCube.Add(new ILLinePlot(ILMath.tosingle(ILMath.randn(1, 20) + 2.0), lineStyle: DashStyle.Dashed, markerStyle: MarkerStyle.Square));
                    plotCube.Add(new ILLegend("Line 1", "Line 2"));
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
                case DemoEnum.Points:
                    ilPanel.Scene.Add(new ILPoints
                    {
                        Positions = ILMath.tosingle(ILMath.randn(3, 100)),
                        Color = Color.Green,
                        Size = 10
                    });
                    break;
                case DemoEnum.Gear:
                {
                    ILGear gear = ilPanel.Scene.Add(new ILGear());
                    gear.Rotate(Vector3.UnitY, -0.2);
                    gear.Transform = gear.Transform.Scale(0.7, 0.7, 0.7);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ilPanel.Scene.Configure();
            ilPanel.Refresh();

            // Attach ILNEditor
            editor = ILPanelEditor.AttachTo(ilPanel);
        }

        private void btnToXml_Click(object sender, EventArgs e)
        {
            var fileDialog = new SaveFileDialog();
            fileDialog.DefaultExt = "*.xml";
            fileDialog.Filter = "XML Files (*.xml)|*.xml";
            fileDialog.FileName = "scene-settings.xml";
            if (fileDialog.ShowDialog() == DialogResult.OK)
                XmlSerializer.SerializeToFile(editor, fileDialog.FileName);
        }

        private void btnFromXml_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = "*.xml";
            fileDialog.Filter = "XML Files (*.xml)|*.xml";
            fileDialog.FileName = "scene-settings.xml";
            fileDialog.CheckFileExists = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
                XmlDeserializer.DeserializeFromFile(editor, fileDialog.FileName);
        }
    }
}
