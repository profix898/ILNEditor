using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using ILNumerics.Drawing;

namespace ILNEditor.TypeExpanders
{
    [TypeConverter(typeof(Matrix4ExpanderConverter))]
    internal class Matrix4Expander
    {
        private readonly string member;
        private readonly object parent;

        public Matrix4Expander(object parent, string member)
        {
            this.parent = parent;
            this.member = member;
        }

        #region Matrix4

        [Category("Matrix4")]
        public string M1x
        {
            get
            {
                Matrix4 matrix = GetMatrix4();

                return String.Format("{0:F1}; {1:F1}; {2:F1}; {3:F1}", matrix.M11, matrix.M12, matrix.M13, matrix.M14);
            }
            set
            {
                float c1, c2, c3, c4;
                if (!ParseRow(value, out c1, out c2, out c3, out c4))
                    return;

                Matrix4 matrix = GetMatrix4();
                matrix.M11 = c1;
                matrix.M12 = c2;
                matrix.M13 = c3;
                matrix.M14 = c4;
                SetMatrix4(matrix);
            }
        }

        [Category("Matrix4")]
        public string M2x
        {
            get
            {
                Matrix4 matrix = GetMatrix4();

                return String.Format("{0:F1}; {1:F1}; {2:F1}; {3:F1}", matrix.M21, matrix.M22, matrix.M23, matrix.M24);
            }
            set
            {
                float c1, c2, c3, c4;
                if (!ParseRow(value, out c1, out c2, out c3, out c4))
                    return;

                Matrix4 matrix = GetMatrix4();
                matrix.M21 = c1;
                matrix.M22 = c2;
                matrix.M23 = c3;
                matrix.M24 = c4;
                SetMatrix4(matrix);
            }
        }

        [Category("Matrix4")]
        public string M3x
        {
            get
            {
                Matrix4 matrix = GetMatrix4();

                return String.Format("{0:F1}; {1:F1}; {2:F1}; {3:F1}", matrix.M31, matrix.M32, matrix.M33, matrix.M34);
            }
            set
            {
                float c1, c2, c3, c4;
                if (!ParseRow(value, out c1, out c2, out c3, out c4))
                    return;

                Matrix4 matrix = GetMatrix4();
                matrix.M31 = c1;
                matrix.M32 = c2;
                matrix.M33 = c3;
                matrix.M34 = c4;
                SetMatrix4(matrix);
            }
        }

        [Category("Matrix4")]
        public string M4x
        {
            get
            {
                Matrix4 matrix = GetMatrix4();

                return String.Format("{0:F1}; {1:F1}; {2:F1}; {3:F1}", matrix.M41, matrix.M42, matrix.M43, matrix.M44);
            }
            set
            {
                float c1, c2, c3, c4;
                if (!ParseRow(value, out c1, out c2, out c3, out c4))
                    return;

                Matrix4 matrix = GetMatrix4();
                matrix.M41 = c1;
                matrix.M42 = c2;
                matrix.M43 = c3;
                matrix.M44 = c4;
                SetMatrix4(matrix);
            }
        }

        [Category("Matrix4")]
        public Matrix4ExpanderOperations Operations
        {
            get { return new Matrix4ExpanderOperations(GetMatrix4, SetMatrix4); }
        }

        #endregion

        #region Nested type: Matrix4ExpanderConverter

        private class Matrix4ExpanderConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is Matrix4Expander)
                    return "Matrix4";

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #region Private

        private Matrix4 GetMatrix4()
        {
            return (Matrix4) parent.GetType().GetProperty(member).GetValue(parent, null);
        }

        private void SetMatrix4(Matrix4 rectangle)
        {
            parent.GetType().GetProperty(member).SetValue(parent, rectangle, null);
        }

        private bool ParseRow(string value, out float c1, out float c2, out float c3, out float c4)
        {
            Match match = new Regex("([\\d.,-]+); ([\\d.,-]+); ([\\d.,-]+); ([\\d.,-]+)").Match(value.Trim());

            c1 = match.Success ? Single.Parse(match.Groups[1].Value) : 0f;
            c2 = match.Success ? Single.Parse(match.Groups[2].Value) : 0f;
            c3 = match.Success ? Single.Parse(match.Groups[3].Value) : 0f;
            c4 = match.Success ? Single.Parse(match.Groups[4].Value) : 0f;

            return match.Success;
        }

        #endregion
    }
}
