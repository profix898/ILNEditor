﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    internal abstract class ILShapeWrapper : ILDrawableWrapper
    {
        private readonly ILShape source;

        protected ILShapeWrapper(ILShape source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Shape" : name)
        {
            this.source = source;
        }

        #region ILShape

        [Category("Format")]
        public bool Selectable
        {
            get { return source.Selectable; }
            set { source.Selectable = value; }
        }

        [Category("Format")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color SpecularColor
        {
            get { return source.SpecularColor; }
            set { source.SpecularColor = value; }
        }

        [Category("Format")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color EmissionColor
        {
            get { return source.EmissionColor; }
            set { source.EmissionColor = value; }
        }

        [Category("Format")]
        public float Shininess
        {
            get { return source.Shininess; }
            set { source.Shininess = value; }
        }

        [Category("Format")]
        public bool AutoNormals
        {
            get { return source.AutoNormals; }
            set { source.AutoNormals = value; }
        }

        #endregion
    }
}