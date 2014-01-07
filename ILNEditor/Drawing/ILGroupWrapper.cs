using System;
using System.ComponentModel;
using System.Globalization;
using ILNEditor.TypeExpanders;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(ILGroupConverter))]
    internal class ILGroupWrapper : ILNodeWrapper
    {
        private readonly ILGroup source;

        public ILGroupWrapper(ILGroup source, ILPanelEditor editor, string path, string name = null)
            : base(source, editor, path, String.IsNullOrEmpty(name) ? "Group" : name)
        {
            this.source = source;
        }

        #region ILGroup

        [Category("Format")]
        public Matrix4Expander Transform
        {
            get { return new Matrix4Expander(source, "Transform"); }
        }

        [Category("Format")]
        public float? Alpha
        {
            get { return source.Alpha; }
            set { source.Alpha = value; }
        }

        #endregion

        internal override void Traverse()
        {
            foreach (ILNode node in source.Children)
            {
                Type nodeType = node.GetType();
                var childGroup = node as ILGroup;

                if (Editor.WrapperMap.ContainsKey(nodeType)) // NodeType is mapped
                {
                    var wrapper = (ILWrapperBase) Activator.CreateInstance(Editor.WrapperMap[nodeType], node, Editor, FullName, null);
                    if (childGroup != null && childGroup.Children.Count > 0)
                        wrapper.Traverse();
                }
                else if (nodeType.BaseType != null && nodeType.BaseType != typeof(object) && Editor.WrapperMap.ContainsKey(nodeType.BaseType)) // Only BaseType is mapped
                {
                    var wrapper = (ILWrapperBase) Activator.CreateInstance(Editor.WrapperMap[nodeType.BaseType], node, Editor, FullName, nodeType.Name);
                    if (childGroup != null && childGroup.Children.Count > 0)
                        wrapper.Traverse();
                }
                else if (childGroup != null && childGroup.Children.Count > 0)
                    new ILGroupWrapper(childGroup, Editor, FullName).Traverse();
            }
        }

        protected void TraverseILGroupOnly()
        {
            foreach (ILNode node in source.Children)
            {
                var childGroup = node as ILGroup;
                if (childGroup != null && childGroup.Children.Count > 0)
                    new ILGroupWrapper(childGroup, Editor, FullName).Traverse();
            }
        }

        #region Nested type: ILGroupConverter

        private class ILGroupConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is ILGroupWrapper)
                    return ((ILGroupWrapper) value).Name;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
