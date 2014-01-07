using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            DisposeWrappers();

            // Traverse scene
            new ILGroupWrapper(ilPanel.Scene.First<ILGroup>(), this, String.Empty, "ROOT").Traverse();

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

        internal ILWrapperBase FindWrapper(object item)
        {
            var ilNode = item as ILNode;
            if (ilNode != null)
                return FindWrapperById(ilNode.ID);

            return wrappers.FirstOrDefault(wrapper => (wrapper.Source == item));
        }

        internal ILWrapperBase FindWrapperById(int id)
        {
            return wrappers.FirstOrDefault(wrapper =>
            {
                var wrappedNode = wrapper.Source as ILNode;
                if (wrappedNode != null)
                    return (wrappedNode.ID == id);

                return false;
            });
        }

        internal void MouseDoubleClickShowEditor(object sender, ILMouseEventArgs args)
        {
            // 1) In a 'standard' scene, the MouseDoubleClick handler of the original item is invoked -> use sender
            // 2) In a ILPlotCube scene, the MouseDoubleClick handler of ILPlotCube is invoke (with original item in the ILMouseEventArgs.Target) -> lookup args.Target
            object item = (FindWrapperById(args.Target.ID) ?? sender) as ILWrapperBase;
            if (item == null)
                return;

            ShowEditor(((ILWrapperBase) item).FullName);
            args.Cancel = true;
        }

        internal void ShowEditor(string node = null)
        {
            if (editor == null)
                return;

            if (!String.IsNullOrEmpty(node))
                editor.SelectNode(node);

            editor.Show();
        }

        #endregion

        #region Private

        private void DisposeWrappers()
        {
            // Dispose wrappers (unsubscribing events)
            foreach (ILWrapperBase wrapper in wrappers.ToList())
                wrapper.Dispose();
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    editor.Hide();
                    editor.Dispose();

                    DisposeWrappers();
                }

                editor = null;
                wrapperMap = null;
                wrappers = null;
            }

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
