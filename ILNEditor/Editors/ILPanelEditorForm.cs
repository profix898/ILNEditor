using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ILNEditor.Drawing;

namespace ILNEditor.Editors
{
    public sealed partial class ILPanelEditorForm : Form, IILPanelEditor
    {
        private const string PathSeparator = @":";
        private const char PathSeparatorChar = ':';

        private readonly ILPanelEditor editor;

        public ILPanelEditorForm(ILPanelEditor editor)
        {
            this.editor = editor;

            InitializeComponent();
        }

        #region IILPanelEditor Members

        public event PropertyChangedEventHandler PropertyChanged;

        public new void Show()
        {
            if (!Visible)
                base.Show();

            BringToFront();
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

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = true;

            //    // Hide context menu if no item is selected
            //    if (propertyGrid.SelectedObject == null)
            //    {
            //        e.Cancel = true;
            //        return;
            //    }

            //    // Get selected ILNode (equivalent to SelectedObject on propertyGrid, see treeView_AfterSelect)
            //    var node = ((ILWrapperBase) propertyGrid.SelectedObject).Source as ILNode;

            //    // Add/Remove is valid on ILNode objects only
            //    if (node == null)
            //    {
            //        e.Cancel = true;
            //        MessageBox.Show("Only ILNode objects can be added/removed.");
            //        return;
            //    }

            //    // Add
            //    var group = node as ILGroup;
            //    if (group != null) // Can only add items to ILGroup nodes
            //    {
            //        miAdd.DropDownItems.Clear();
            //        foreach (Type type in editor.WrapperMap.Keys.Where(type => type.GetConstructor(Type.EmptyTypes) != null))
            //        {
            //            Type typeClosure = type;
            //            ToolStripItem item = miAdd.DropDownItems.Add(type.WrapperName);
            //            item.Click += (o, args) => AddNode(group, typeClosure);
            //        }
            //    }
            //    else
            //        miAdd.Enabled = false;

            //    // Remove
            //    miRemove.Click += (o, args) => RemoveNode(node);
        }

        //private void RemoveNode(ILNode node)
        //{
        //    propertyGrid.SelectedObject = null;
        //    editor.Panel.Scene.Remove(node);
        //    editor.Panel.Refresh();
        //    editor.Update();
        //}

        //private void AddNode(ILGroup parent, Type type)
        //{
        //    parent.Children.Add((ILNode) Activator.CreateInstance(type));
        //}

        #endregion
    }
}
