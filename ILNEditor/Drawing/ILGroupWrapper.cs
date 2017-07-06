using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ILNEditor.TypeExpanders;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILGroupConverter))]
    public class ILGroupWrapper : ILNodeWrapper
    {
        private readonly ILGroup source;

        public ILGroupWrapper(ILGroup source, ILPanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Group"), label)
        {
            this.source = source;
        }

        #region ILGroup

        [Category("Format")]
        public Matrix4Expander Transform
        {
            get { return new Matrix4Expander(source, "Transform"); }
        }

        [Browsable(false)]
        public Matrix4 TransformMatrix
        {
            get { return GetSyncNode(source).Transform; }
            set { source.Transform = value; }
        }

        [Category("Format")]
        public float? Alpha
        {
            get { return source.Alpha; }
            set { source.Alpha = value; }
        }

        #endregion

        #region Traversal

        internal override void Traverse(IEnumerable<ILNode> nodes = null)
        {
            foreach (ILNode node in nodes ?? source.Children)
            {
                Type nodeType = node.GetType();
                var childGroup = node as ILGroup;

                // Hide the editor itself
                if (childGroup != null && node is ILPanelEditor)
                    continue;

                if (WrapperMap.ContainsKey(nodeType)) // NodeType is mapped
                {
                    var wrapper = (ILWrapperBase) Activator.CreateInstance(WrapperMap[nodeType], node, Editor, Path, null, null);
                    if (childGroup != null && childGroup.Children.Count > 0)
                        wrapper.Traverse();
                }
                else if (nodeType.BaseType != null && nodeType.BaseType != typeof(object) && WrapperMap.ContainsKey(nodeType.BaseType)) // Only BaseType is mapped
                {
                    var wrapper = (ILWrapperBase) Activator.CreateInstance(WrapperMap[nodeType.BaseType], node, Editor, Path, nodeType.Name, null);
                    if (childGroup != null && childGroup.Children.Count > 0)
                        wrapper.Traverse();
                }
                else if (childGroup != null && childGroup.Children.Count > 0)
                    new ILGroupWrapper(childGroup, Editor, Path).Traverse();
            }
        }

        #endregion

        #region Nested type: ILGroupConverter

        private class ILGroupConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILGroupWrapper)
                    return ((ILGroupWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
