using System;
using System.ComponentModel;
using System.Linq;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    public abstract class ILNodeWrapper : ILWrapperBase
    {
        private readonly ILNode source;

        protected ILNodeWrapper(ILNode source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Node"), label)
        {
            this.source = source;
        }

        #region ILNode

        [Category("Format")]
        public bool Visible
        {
            get { return source.Visible; }
            set { source.Visible = value; }
        }

        [Category("Format")]
        public bool Markable
        {
            get { return source.Markable; }
            set { source.Markable = value; }
        }

        #endregion

        #region ILNodeHelpers

        protected static string BuildName<T>(string name, ILPanel panel, T node, string defaultName) where T : ILNode
        {
            if (!String.IsNullOrEmpty(name))
                return name;

            if (node != null)
            {
                var nodeTag = node.Tag as String;
                if (!String.IsNullOrEmpty(nodeTag) && !Equals(node.Tag, defaultName))
                    return (string) node.Tag;
            }

            return BuildDefaultName(panel, node, defaultName);
        }

        protected static string BuildDefaultName<T>(ILPanel panel, T node, string defaultName) where T : ILNode
        {
            return String.Format("{0}#{1}", defaultName, GetNodeIndex(panel, node));
        }

        protected static int GetNodeIndex<T>(ILPanel panel, T node) where T : ILNode
        {
            int index = panel.Scene.Find<T>().ToList().IndexOf(node);
            if (index == -1) // Try SceneSyncRoot next
                index = panel.SceneSyncRoot.Find<T>().ToList().IndexOf(node);

            return index;
        }

        protected T GetSyncNode<T>(T node) where T : ILNode
        {
            return Editor.Panel.SceneSyncRoot.FindById<T>(node.ID) ?? node;
        }

        #endregion
    }
}
