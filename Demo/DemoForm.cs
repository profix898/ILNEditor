using System;
using System.Drawing;
using System.Windows.Forms;
using ILNEditor;
using ILNEditor.Serialization;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using XmlSerializer = ILNEditor.Serialization.XmlSerializer;

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

        private PanelEditor editor;

        public DemoForm()
        {
            InitializeComponent();
        }

        private void PanelForm_Load(object sender, EventArgs e)
        {
            comboBoxDemo.Items.AddRange(Enum.GetNames(typeof(DemoEnum)));
            comboBoxDemo.SelectedIndex = (int) DemoEnum.LinePlot;
        }

        private void comboBoxDemo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDemo.SelectedIndex == -1)
                return;

            // Reset scene
            ilPanel.Scene = new Scene();

            // Add and render demo content
            switch ((DemoEnum) comboBoxDemo.SelectedIndex)
            {
                case DemoEnum.LinePlot:
                {
                    PlotCube plotCube = ilPanel.Scene.Add(new PlotCube());
                    plotCube.Add(new LinePlot(ILMath.tosingle(ILMath.randn(1, 20))));
                    plotCube.Add(new LinePlot(ILMath.tosingle(ILMath.randn(1, 20) + 2.0), lineStyle: DashStyle.Dashed, markerStyle: MarkerStyle.Square));
                    plotCube.Add(new Legend("Line 1", "Line 2"));
                }
                    break;
                case DemoEnum.SincSurface:
                {
                    PlotCube plotCube = ilPanel.Scene.Add(new PlotCube(null, false));
                    plotCube.Add(new Surface(SpecialData.sincf(40, 60, 2.5f))
                    {
                        Wireframe = { Color = Color.FromArgb(50, Color.LightGray) },
                        Colormap = Colormaps.Jet
                    });
                    ilPanel.Scene.First<PlotCube>().Rotation = Matrix4.Rotation(new Vector3(1f, 0.23f, 1), 0.7f);
                }
                    break;
                case DemoEnum.Points:
                    ilPanel.Scene.Add(new Points
                    {
                        Positions = ILMath.tosingle(ILMath.randn(3, 100)),
                        Color = Color.Green,
                        Size = 10
                    });
                    break;
                case DemoEnum.Gear:
                {
                    Gear gear = ilPanel.Scene.Add(new Gear());
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
            editor = PanelEditor.AttachTo(ilPanel);
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
