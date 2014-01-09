using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [DebuggerDisplay("Path = {Path}, WrapperType = {GetType().Name}")]
    public abstract class ILWrapperBase : IDisposable
    {
        private readonly ILPanelEditor editor;
        private readonly string label;
        private readonly string name;
        private readonly string path;
        private readonly object source;

        private bool disposed;

        protected ILWrapperBase(object source, ILPanelEditor editor, string path, string name, string label = null)
        {
            this.source = source;
            this.editor = editor;
            this.path = String.IsNullOrEmpty(path) ? name : path + ":" + name;
            this.name = name;
            this.label = String.IsNullOrEmpty(label) ? name : label;

            editor.Wrappers.Add(this);
        }

        #region Editor

        protected ILPanelEditor Editor
        {
            [DebuggerStepThrough]
            get { return editor; }
        }

        protected ILPanel Panel
        {
            [DebuggerStepThrough]
            get { return editor.Panel; }
        }

        protected List<ILWrapperBase> Wrappers
        {
            [DebuggerStepThrough]
            get { return editor.Wrappers; }
        }

        protected WrapperMap WrapperMap
        {
            [DebuggerStepThrough]
            get { return editor.WrapperMap; }
        }

        protected ILWrapperBase FindWrapper(object item)
        {
            return editor.FindWrapper(item);
        }

        protected ILWrapperBase FindWrapperById(int id)
        {
            return editor.FindWrapperById(id);
        }

        protected void MouseDoubleClickShowEditor(object sender, ILMouseEventArgs args)
        {
            editor.MouseDoubleClickShowEditor(sender, args);
        }

        protected void ShowEditor(string node = null)
        {
            editor.ShowEditor(node);
        }

        #endregion

        #region Wrapper

        [Browsable(false)]
        public object Source
        {
            [DebuggerStepThrough]
            get { return source; }
        }

        [Browsable(false)]
        public string Path
        {
            [DebuggerStepThrough]
            get { return path; }
        }

        [Browsable(false)]
        public string Name
        {
            [DebuggerStepThrough]
            get { return name; }
        }

        [Browsable(false)]
        public string Label
        {
            [DebuggerStepThrough]
            get { return label; }
        }

        internal virtual void Traverse(IEnumerable<ILNode> nodes = null)
        {
        }

        #endregion

        #region Helpers

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

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region DisposeFinalize

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Release managed resources
                    editor.Wrappers.Remove(this);
                }

                // Release unmanaged resources
            }

            disposed = true;
        }

        ~ILWrapperBase()
        {
            Dispose(false);
        }

        #endregion
    }
}
