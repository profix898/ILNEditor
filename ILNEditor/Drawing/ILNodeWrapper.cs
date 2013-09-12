﻿using System;
using System.ComponentModel;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    internal abstract class ILNodeWrapper : ILWrapperBase
    {
        private readonly ILNode source;

        protected ILNodeWrapper(ILNode source, ILPanelEditor editor, string path, string name = null)
            : base(editor, path, String.IsNullOrEmpty(name) ? "Node" : name)
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

        internal ILGroup Parent
        {
            get { return source.Parent; }
        }
    }
}
