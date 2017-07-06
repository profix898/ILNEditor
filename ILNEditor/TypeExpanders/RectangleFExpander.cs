using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace ILNEditor.TypeExpanders
{
    [TypeConverter(typeof(RectangleFExpanderConverter))]
    public class RectangleFExpander
    {
        private readonly string member;
        private readonly object parent;

        public RectangleFExpander(object parent, string member)
        {
            this.parent = parent;
            this.member = member;
        }

        //[Category("Rectangle")]
        //[TypeConverter(typeof(PointFConverter))]
        //public PointF Location
        //{
        //    get { return GetRectangle().Location; }
        //    set
        //    {
        //        RectangleF rectangle = GetRectangle();
        //        rectangle.Location = value;
        //        SetRectangle(rectangle);
        //    }
        //}

        //[Category("Rectangle")]
        //[TypeConverter(typeof(SizeFConverter))]
        //public SizeF Size
        //{
        //    get { return GetRectangle().Size; }
        //    set
        //    {
        //        RectangleF rectangle = GetRectangle();
        //        rectangle.Size = value;
        //        SetRectangle(rectangle);
        //    }
        //}

        [Category("Rectangle")]
        public float X
        {
            get { return GetRectangle().X; }
            set
            {
                RectangleF rectangle = GetRectangle();
                rectangle.X = value;
                SetRectangle(rectangle);
            }
        }

        [Category("Rectangle")]
        public float Y
        {
            get { return GetRectangle().Y; }
            set
            {
                RectangleF rectangle = GetRectangle();
                rectangle.Y = value;
                SetRectangle(rectangle);
            }
        }

        [Category("Rectangle")]
        public float Width
        {
            get { return GetRectangle().Width; }
            set
            {
                RectangleF rectangle = GetRectangle();
                rectangle.Width = value;
                SetRectangle(rectangle);
            }
        }

        [Category("Rectangle")]
        public float Height
        {
            get { return GetRectangle().Height; }
            set
            {
                RectangleF rectangle = GetRectangle();
                rectangle.Height = value;
                SetRectangle(rectangle);
            }
        }

        #region Private

        private RectangleF GetRectangle()
        {
            return (RectangleF) parent.GetType().GetProperty(member).GetValue(parent, null);
        }

        private void SetRectangle(RectangleF rectangle)
        {
            parent.GetType().GetProperty(member).SetValue(parent, rectangle, null);
        }

        #endregion

        #region Nested type: RectangleFExpanderConverter

        private class RectangleFExpanderConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is RectangleFExpander)
                {
                    var expander = (RectangleFExpander) value;

                    return $"Rectangle ({expander.Width:F} x {expander.Height:F} @ X={expander.X:F}; Y={expander.Y:F})";
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
