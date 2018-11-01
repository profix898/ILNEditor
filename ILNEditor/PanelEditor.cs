using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using ILNEditor.Drawing;
using ILNEditor.Drawing.Plotting;
using ILNEditor.Editors;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using MouseEventArgs = ILNumerics.Drawing.MouseEventArgs;
using Panel = ILNumerics.Drawing.Panel;

namespace ILNEditor
{
    public class PanelEditor : Group
    {
        private readonly IPanelEditor editor;
        private readonly Panel ilPanel;

        private readonly WrapperMap wrapperMap = new WrapperMap();
        private readonly List<WrapperBase> wrappers = new List<WrapperBase>();

        private PanelEditor(Panel ilPanel, IPanelEditor editor = null)
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
            new GroupWrapper(ilPanel.Scene.First<Group>(), this, String.Empty, "ROOT").Traverse();

            editor.UpdateNodes();
        }

        public void ShowEditor()
        {
            if (editor == null)
                return;

            ShowEditor(null);
        }

        #region PlotCube

        public void ShowPlotBrowser()
        {
            if (editor == null)
                return;

            if (!wrapperMap.ContainsKey(typeof(PlotCube)))
                return;

            Editor.PlotBrowser.Show();
        }

        public Menu.MenuItemCollection GetPlotCubeMenu(PlotCube plotCube = null)
        {
            if (editor == null)
                return null;

            if (!wrapperMap.ContainsKey(typeof(PlotCube)))
                return null;

            plotCube = plotCube ?? ilPanel.Scene.First<PlotCube>();

            return (FindWrapper(plotCube) as PlotCubeWrapper)?.MenuItems;
        }

        #endregion

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
            foreach (WrapperBase wrapper in wrappers.ToList())
                wrapper.Dispose();
        }

        #endregion

        #region Internals

        internal Panel Panel
        {
            [DebuggerStepThrough]
            get { return ilPanel; }
        }

        internal IPanelEditor Editor
        {
            [DebuggerStepThrough]
            get { return editor; }
        }

        internal List<WrapperBase> Wrappers
        {
            [DebuggerStepThrough]
            get { return wrappers; }
        }

        internal WrapperBase FindWrapper(object item)
        {
            var ilNode = item as Node;
            if (ilNode != null)
                return FindWrapperById(ilNode.ID);

            return wrappers.FirstOrDefault(wrapper => (wrapper.Source == item));
        }

        internal WrapperBase FindWrapperById(int id)
        {
            return wrappers.FirstOrDefault(wrapper => (wrapper.Source as Node)?.ID == id);
        }

        internal void MouseDoubleClickShowEditor(object sender, MouseEventArgs args)
        {
            // 1) In a 'standard' scene, the MouseDoubleClick handler of the original item is invoked -> use sender
            // 2) In a PlotCube scene, the MouseDoubleClick handler of PlotCube is invoked (with original item in the MouseEventArgs.Target) -> lookup args.Target
            object item = (FindWrapperById(args.Target.ID) ?? sender) as WrapperBase;
            if (item == null)
                return;

            ShowEditor(((WrapperBase) item).Path);
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

        #region Factory

        public static PanelEditor AttachTo(Panel ilPanel, IPanelEditor editor = null)
        {
            var editorInstance = new PanelEditor(ilPanel, editor);
            editorInstance.Update();

            return editorInstance;
        }

        #endregion
    }
}
