using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ILNEditor.TypeExpanders;
using ILNumerics.Drawing;

namespace ILNEditor.Drawing
{
    [TypeConverter(typeof(GroupConverter))]
    public class GroupWrapper : NodeWrapper
    {
        private readonly Group source;

        public GroupWrapper(Group source, PanelEditor editor, string path, string name = null, string label = null)
            : base(source, editor, path, BuildName(name, editor.Panel, source, "Group"), label)
        {
            this.source = source;
        }

        #region Group

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

        internal override void Traverse(IEnumerable<Node> nodes = null)
        {
            foreach (Node node in nodes ?? source.Children)
            {
                Type nodeType = node.GetType();
                var childGroup = node as Group;

                // Hide the editor itself
                if (childGroup != null && node is PanelEditor)
                    continue;

                if (WrapperMap.ContainsKey(nodeType)) // NodeType is mapped
                {
                    var wrapper = (WrapperBase) Activator.CreateInstance(WrapperMap[nodeType], node, Editor, Path, null, null);
                    if (childGroup != null && childGroup.Children.Count > 0)
                        wrapper.Traverse();
                }
                else if (nodeType.BaseType != null && nodeType.BaseType != typeof(object) && WrapperMap.ContainsKey(nodeType.BaseType)) // Only BaseType is mapped
                {
                    var wrapper = (WrapperBase) Activator.CreateInstance(WrapperMap[nodeType.BaseType], node, Editor, Path, nodeType.Name, null);
                    if (childGroup != null && childGroup.Children.Count > 0)
                        wrapper.Traverse();
                }
                else if (childGroup != null && childGroup.Children.Count > 0)
                    new GroupWrapper(childGroup, Editor, Path).Traverse();
            }
        }

        #endregion

        #region Nested type: GroupConverter

        private class GroupConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is GroupWrapper)
                    return ((GroupWrapper) value).Label;

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
