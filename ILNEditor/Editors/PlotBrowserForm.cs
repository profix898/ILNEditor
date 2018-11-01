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
        private readonly PanelEditor editor;
        private readonly Dictionary<TreeNode, Action<bool>> nodeCallbacks = new Dictionary<TreeNode, Action<bool>>();

        private bool isSuspended;

        public PlotBrowserForm(PanelEditor editor)
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
            foreach (PlotCube plotCube in editor.Panel.Scene.Find<PlotCube>())
            {
                TreeNode node = treeView.Nodes.Add($"PlotCube#{plotCubeIdx}");

                int scaleGroupIdx = 0;
                foreach (PlotCubeScaleGroup scaleGroup in plotCube.Find<PlotCubeScaleGroup>())
                {
                    node = node.Nodes.Add($"ScaleGroup#{scaleGroupIdx}");

                    int dataGroupIdx = 0;
                    foreach (PlotCubeDataGroup dataGroup in scaleGroup.Find<PlotCubeDataGroup>())
                    {
                        node = node.Nodes.Add($"DataGroup#{dataGroupIdx}");

                        // Capture plots (add plots and attach callbacks)
                        CapturePlot<LinePlot>(dataGroup, node, LinePlot.LinePlotTag);
                        CapturePlot<ImageSCPlot>(dataGroup, node, ImageSCPlot.ImageSCTag);
                        CapturePlot<Surface>(dataGroup, node, Surface.SurfaceDefaultTag);
                        CapturePlot<ContourPlot>(dataGroup, node, ContourPlot.DefaultContourTag);

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

        private void CapturePlot<T>(PlotCubeDataGroup dataGroup, TreeNode parentNode, string defaultTag) where T : Node
        {
            int itemIdx = 0;
            foreach (T plot in dataGroup.Find<T>())
            {
                T plotClosure = plot;

                // Find (or build) label
                string label = $"{defaultTag}#{itemIdx++}";
                var legend = dataGroup.First<Legend>();
                if (legend != null)
                {
                    LegendItem legendItem = legend.Items.Find<LegendItem>().FirstOrDefault(item => item.GetProvider().GetID() == plotClosure.ID);
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
