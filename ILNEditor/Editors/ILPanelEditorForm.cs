using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ILNEditor.Drawing;

namespace ILNEditor.Editors
{
    public sealed partial class ILPanelEditorForm : Form, IILPanelEditor
    {
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
            base.Show();
            BringToFront();
        }

        public void UpdateNodes()
        {
            treeView.Nodes.Clear();
            treeView.PathSeparator = @":";
            foreach (ILWrapperBase wrapper in editor.Wrappers)
            {
                TreeNode node = null;
                foreach (string part in wrapper.FullName.Split(':'))
                {
                    string key = part.Replace(" ", String.Empty); // Remove whitespaces from identifier
                    if (node == null)
                        node = treeView.Nodes.ContainsKey(key) ? treeView.Nodes[key] : treeView.Nodes.Add(key, part);
                    else
                        node = node.Nodes.ContainsKey(key) ? node.Nodes[key] : node.Nodes.Add(key, part);
                }
            }
        }

        public void SelectNode(string node)
        {
            treeView.SelectedNode = FindNodeByFullPath(treeView.Nodes.Cast<TreeNode>(), node);
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.Expand();
                treeView.SelectedNode.EnsureVisible();
            }
        }

        #endregion

        private TreeNode FindNodeByFullPath(IEnumerable<TreeNode> nodes, string path)
        {
            if (nodes == null)
                return null;

            TreeNode node = nodes.FirstOrDefault(treeNode => path.StartsWith(treeNode.FullPath));
            if (node != null && node.FullPath != path)
                node = FindNodeByFullPath(node.Nodes.Cast<TreeNode>(), path);

            return node;
        }

        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;

            propertyGrid.SelectedObject = editor.Wrappers.FirstOrDefault(wrapper => wrapper.FullName == treeView.SelectedNode.FullPath);
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
            //            ToolStripItem item = miAdd.DropDownItems.Add(type.Name);
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
    }
}
