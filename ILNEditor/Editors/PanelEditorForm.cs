using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ILNEditor.Drawing;

namespace ILNEditor.Editors
{
    public sealed partial class PanelEditorForm : Form, IPanelEditor
    {
        private const string PathSeparator = @":";
        private const char PathSeparatorChar = ':';

        private readonly ILPanelEditor editor;
        private readonly PlotBrowserForm plotBrowser;

        public PanelEditorForm(ILPanelEditor editor)
        {
            this.editor = editor;
            plotBrowser = new PlotBrowserForm(editor);

            InitializeComponent();
        }

        #region IPanelEditor Members

        public IPlotBrowser PlotBrowser
        {
            get { return plotBrowser; }
        }

        public new void Show()
        {
            if (!Visible)
                base.Show();

            BringToFront();
        }

        public new void Hide()
        {
            if (Visible)
                base.Hide();

            if (plotBrowser.Visible)
                plotBrowser.Hide();
        }

        public void UpdateNodes()
        {
            string selectedNode = (treeView.SelectedNode != null) ? GetNodePath(treeView.SelectedNode) : null;

            treeView.Nodes.Clear();
            treeView.PathSeparator = PathSeparator;
            foreach (ILWrapperBase wrapper in editor.Wrappers.OrderBy(wrapper => wrapper.Path))
            {
                TreeNode node = null;
                foreach (string part in wrapper.Path.Split(PathSeparatorChar))
                {
                    if (node == null)
                        node = treeView.Nodes.ContainsKey(part) ? treeView.Nodes[part] : treeView.Nodes.Add(part, wrapper.Label);
                    else
                        node = node.Nodes.ContainsKey(part) ? node.Nodes[part] : node.Nodes.Add(part, wrapper.Label);
                }
            }

            if (!String.IsNullOrEmpty(selectedNode))
                SelectNode(selectedNode);

            plotBrowser.UpdateList();
        }

        public void SelectNode(string node)
        {
            treeView.SelectedNode = FindNodeByPath(node);
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.Expand();
                treeView.SelectedNode.EnsureVisible();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Helpers

        private string GetNodePath(TreeNode node)
        {
            var pathParts = new string[node.Level + 1];
            for (int level = node.Level; level >= 0; level--)
            {
                pathParts[level] = node.Name;
                node = node.Parent;
            }

            return String.Join(PathSeparator, pathParts);
        }

        private TreeNode FindNodeByPath(string path)
        {
            TreeNode node = null;
            foreach (string part in path.Split(PathSeparatorChar))
            {
                if (node == null)
                    node = treeView.Nodes.ContainsKey(part) ? treeView.Nodes[part] : null;
                else
                    node = node.Nodes.ContainsKey(part) ? node.Nodes[part] : null;

                if (node == null) // Node not found
                    break;
            }

            return node;
        }

        #endregion

        #region FormInternals

        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(String.Empty));

            Hide();
            e.Cancel = true;
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;

            string selectedPath = GetNodePath(treeView.SelectedNode);
            propertyGrid.SelectedObject = editor.Wrappers.FirstOrDefault(wrapper => wrapper.Path == selectedPath);
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(e.ChangedItem.Label));
        }

        #endregion

        private void btnPlotBrowser_Click(object sender, EventArgs e)
        {
            plotBrowser.Show();
        }

        private void btnCollapseAll_Click(object sender, EventArgs e)
        {
            treeView.CollapseAll();
        }
    }
}
