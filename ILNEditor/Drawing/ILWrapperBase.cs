using System;
using System.Diagnostics;

namespace ILNEditor.Drawing
{
    [DebuggerDisplay("FullName = {FullName}, WrapperType = {GetType().Name}")]
    public abstract class ILWrapperBase
    {
        private readonly ILPanelEditor editor;
        private readonly string name;
        private readonly string path;
        private readonly object source;

        protected ILWrapperBase(object source, ILPanelEditor editor, string path, string name)
        {
            this.source = source;
            this.editor = editor;
            this.path = path;
            this.name = name;

            editor.Wrappers.Add(this);
        }

        internal object Source
        {
            [DebuggerStepThrough]
            get { return source; }
        }

        internal ILPanelEditor Editor
        {
            [DebuggerStepThrough]
            get { return editor; }
        }

        internal string Path
        {
            [DebuggerStepThrough]
            get { return path; }
        }

        internal string Name
        {
            [DebuggerStepThrough]
            get { return name; }
        }

        internal string FullName
        {
            [DebuggerStepThrough]
            get { return String.IsNullOrEmpty(path) ? name : path + ":" + name; }
        }

        internal virtual bool TraverseChildren
        {
            get { return true; }
        }
    }
}
