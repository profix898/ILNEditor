using System;
using System.Collections.Generic;
using System.Diagnostics;
using ILNEditor.Drawing;
using ILNEditor.Editors;
using ILNumerics.Drawing;

namespace ILNEditor
{
    public sealed class ILPanelEditor : IDisposable
    {
        private readonly ILPanel ilPanel;
        private bool disposed;
        private IILPanelEditor editor;

        private WrapperMap wrapperMap = new WrapperMap();
        private List<ILWrapperBase> wrappers = new List<ILWrapperBase>();

        public ILPanelEditor(ILPanel ilPanel, IILPanelEditor editor = null)
        {
            this.ilPanel = ilPanel;
            this.editor = editor ?? new ILPanelEditorForm(this);
            this.editor.PropertyChanged += (o, args) => ilPanel.Refresh();
        }

        public WrapperMap WrapperMap
        {
            [DebuggerStepThrough]
            get { return wrapperMap; }
        }

        public void Update()
        {
            wrappers.Clear();

            Traverse(String.Empty, ilPanel.Scene.First<ILGroup>());

            editor.UpdateNodes();
        }

        #region Internals

        internal ILPanel Panel
        {
            [DebuggerStepThrough]
            get { return ilPanel; }
        }

        internal List<ILWrapperBase> Wrappers
        {
            [DebuggerStepThrough]
            get { return wrappers; }
        }

        internal void MouseDoubleClickShowEditor(object sender, ILMouseEventArgs e)
        {
            if (!(sender is ILWrapperBase))
                return;

            ShowEditor(((ILWrapperBase) sender).FullName);
            e.Cancel = true;
        }

        internal void ShowEditor(string node = null)
        {
            if (!String.IsNullOrEmpty(node))
                editor.SelectNode(node);

            if (!editor.Visible)
                editor.Show();
        }

        #endregion

        #region Private

        private void Traverse(string path, ILGroup group)
        {
            foreach (ILNode node in group.Children)
            {
                Type nodeType = node.GetType();
                var childGroup = node as ILGroup;

                if (wrapperMap.ContainsKey(nodeType)) // NodeType is mapped
                {
                    var wrapper = (ILWrapperBase) Activator.CreateInstance(wrapperMap[nodeType], node, this, path, null);
                    if (childGroup != null && wrapper.TraverseChildren && childGroup.Children.Count > 0)
                        Traverse(wrapper.FullName, childGroup);
                }
                else if (nodeType.BaseType != null && nodeType.BaseType != typeof(object) && wrapperMap.ContainsKey(nodeType.BaseType)) // Only BaseType is mapped
                {
                    var wrapper = (ILWrapperBase) Activator.CreateInstance(wrapperMap[nodeType.BaseType], node, this, path, nodeType.Name);
                    if (childGroup != null && wrapper.TraverseChildren && childGroup.Children.Count > 0)
                        Traverse(wrapper.FullName, childGroup);
                }
                else if (childGroup != null && childGroup.Children.Count > 0)
                    Traverse(String.IsNullOrEmpty(path) ? nodeType.Name : path + ":" + nodeType.Name, childGroup);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                editor.Hide();
                editor.Dispose();
            }

            editor = null;
            wrapperMap = null;
            wrappers = null;

            disposed = true;
        }

        ~ILPanelEditor()
        {
            Dispose(false);
        }

        #endregion

        #region Static

        public static ILPanelEditor AttachTo(ILPanel ilPanel)
        {
            var editor = new ILPanelEditor(ilPanel);
            editor.Update();

            return editor;
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
