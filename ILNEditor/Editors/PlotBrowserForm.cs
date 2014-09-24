using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor.Editors
{
    public partial class PlotBrowserForm : Form, IPlotBrowser
    {
        private readonly ILPanelEditor editor;
        private readonly Dictionary<TreeNode, Action<bool>> nodeCallbacks = new Dictionary<TreeNode, Action<bool>>();

        private bool isSuspended;

        public PlotBrowserForm(ILPanelEditor editor)
        {
            this.editor = editor;

            InitializeComponent();
        }

        #region IPlotBrowser Members

        public new void Show()
        {
            if (!Visible)
                base.Show();

            BringToFront();
        }

        public void UpdateList()
        {
            treeView.Nodes.Clear();
            nodeCallbacks.Clear();
            isSuspended = true;

            int plotCubeIdx = 0;
            foreach (ILPlotCube plotCube in editor.Panel.Scene.Find<ILPlotCube>())
            {
                TreeNode node = treeView.Nodes.Add(String.Format("PlotCube#{0}", plotCubeIdx));

                int scaleGroupIdx = 0;
                foreach (ILPlotCubeScaleGroup scaleGroup in plotCube.Find<ILPlotCubeScaleGroup>())
                {
                    node = node.Nodes.Add(String.Format("ScaleGroup#{0}", scaleGroupIdx));

                    int dataGroupIdx = 0;
                    foreach (ILPlotCubeDataGroup dataGroup in scaleGroup.Find<ILPlotCubeDataGroup>())
                    {
                        node = node.Nodes.Add(String.Format("DataGroup#{0}", dataGroupIdx));

                        // Capture plots (add plots and attach callbacks)
                        CapturePlot<ILLinePlot>(dataGroup, node, ILLinePlot.LinePlotTag);
                        CapturePlot<ILImageSCPlot>(dataGroup, node, ILImageSCPlot.ImageSCTag);
                        CapturePlot<ILSurface>(dataGroup, node, ILSurface.SurfaceDefaultTag);
                        CapturePlot<ILContourPlot>(dataGroup, node, ILContourPlot.DefaultContourTag);

                        node = node.Parent;
                        dataGroupIdx++;
                    }

                    node = node.Parent;
                    scaleGroupIdx++;
                }

                plotCubeIdx++;
            }

            isSuspended = false;
            treeView.ExpandAll();
        }

        #endregion

        private void PlotBrowserForm_Load(object sender, EventArgs e)
        {
        }

        private void PlotBrowserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isSuspended)
                return;

            // Ignore collapse/expand events
            if (e.Action == TreeViewAction.Collapse || e.Action == TreeViewAction.Expand)
                return;

            // Check/Uncheck child nodes
            if (e.Node.Nodes.Count > 0)
                UnCheckChildNodes(e.Node, e.Node.Checked);

            // Invoke callback
            if (nodeCallbacks.ContainsKey(e.Node))
                nodeCallbacks[e.Node](e.Node.Checked);

            // Uncheck parents
            if (!e.Node.Checked && e.Node.Parent != null && e.Node.Parent.Checked)
            {
                isSuspended = true;
                UnCheckParentNodes(e.Node, e.Node.Checked);
                isSuspended = false;
            }
        }

        #region Helpers

        private void UnCheckChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;

                if (node.Nodes.Count > 0)
                    UnCheckChildNodes(node, nodeChecked);
            }
        }

        private void UnCheckParentNodes(TreeNode treeNode, bool nodeChecked)
        {
            while (treeNode.Parent != null)
            {
                // ReSharper disable once RedundantCheckBeforeAssignment
                if (treeNode.Parent.Checked != nodeChecked)
                    treeNode.Parent.Checked = nodeChecked;

                treeNode = treeNode.Parent;
            }
        }

        private void CapturePlot<T>(ILPlotCubeDataGroup dataGroup, TreeNode parentNode, string defaultTag) where T : ILNode
        {
            int itemIdx = 0;
            foreach (T plot in dataGroup.Find<T>())
            {
                T plotClosure = plot;

                // Find (or build) label
                string label = String.Format("{0}#{1}", defaultTag, itemIdx++);
                var legend = dataGroup.First<ILLegend>();
                if (legend != null)
                {
                    ILLegendItem legendItem = legend.Items.Find<ILLegendItem>().FirstOrDefault(item => item.GetProvider().GetID() == plotClosure.ID);
                    if (legendItem != null)
                        label = legendItem.Text;
                }

                // Add node to tree ...
                TreeNode plotNode = parentNode.Nodes.Add(plot.ID.ToString(CultureInfo.InvariantCulture), label);
                plotNode.Checked = plot.Visible;

                // ... and attach callback
                nodeCallbacks.Add(plotNode, visible =>
                {
                    plotClosure.Visible = visible;

                    editor.Panel.Configure();
                    editor.Panel.Refresh();
                });
            }
        }

        #endregion
    }
}
