using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ILNEditor.Drawing;
using ILNEditor.Editors;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor
{
    public class ILPanelEditor : ILGroup
    {
        private readonly IPanelEditor editor;
        private readonly ILPanel ilPanel;

        private readonly WrapperMap wrapperMap = new WrapperMap();
        private readonly List<ILWrapperBase> wrappers = new List<ILWrapperBase>();

        private ILPanelEditor(ILPanel ilPanel, IPanelEditor editor = null)
        {
            this.ilPanel = ilPanel;
            this.editor = editor ?? new PanelEditorForm(this);
            this.editor.PropertyChanged += (o, args) =>
            {
                OnPropertyChanged(args.PropertyName);

                ilPanel.Configure();
                ilPanel.Refresh();
            };

            ilPanel.Scene.Add(this);
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

        public void ShowEditor()
        {
            ShowEditor(null);
        }

        public void ShowPlotBrowser()
        {
            if (editor == null)
                return;

            if (!wrapperMap.ContainsKey(typeof(ILPlotCube)))
                return;

            PanelEditor.PlotBrowser.Show();
        }

        public override void Dispose()
        {
            editor.Hide();
            editor.Dispose();

            DisposeWrappers();

            base.Dispose();
        }

        #region Private

        private void DisposeWrappers()
        {
            // Dispose wrappers (unsubscribing events)
            foreach (ILWrapperBase wrapper in wrappers.ToList())
                wrapper.Dispose();
        }

        #endregion

        #region Internals

        internal ILPanel Panel
        {
            [DebuggerStepThrough]
            get { return ilPanel; }
        }

        internal IPanelEditor PanelEditor
        {
            [DebuggerStepThrough]
            get { return editor; }
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

            ShowEditor(((ILWrapperBase) item).Path);
            args.Cancel = true;
        }

        internal void ShowEditor(string node)
        {
            if (editor == null)
                return;

            if (!String.IsNullOrEmpty(node))
                editor.SelectNode(node);

            editor.Show();
        }

        #endregion

        #region Static

        public static ILPanelEditor AttachTo(ILPanel ilPanel, IPanelEditor editor = null)
        {
            var editorInstance = new ILPanelEditor(ilPanel, editor);
            editorInstance.Update();

            return editorInstance;
        }

        #endregion
    }
}
