using System;
using System.ComponentModel;
using System.Globalization;
using ILNEditor.TypeConverters;
using ILNumerics.Drawing;

namespace ILNEditor.TypeExpanders
{
    [TypeConverter(typeof(Matrix4OperationsConverter))]
    public class Matrix4ExpanderOperations
    {
        private readonly Func<Matrix4> getMatrix;
        private readonly Action<Matrix4> setMatrix;

        public Matrix4ExpanderOperations(Func<Matrix4> getMatrix, Action<Matrix4> setMatrix)
        {
            this.getMatrix = getMatrix;
            this.setMatrix = setMatrix;
        }

        #region Operations

        [Category("Matrix4 Operation")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Scale
        {
            get { return new Vector3(1.0, 1.0, 1.0); }
            set { setMatrix(getMatrix().Scale(value.X, value.Y, value.Z)); }
        }

        [Category("Matrix4 Operation")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Translate
        {
            get { return new Vector3(0.0, 0.0, 0.0); }
            set { setMatrix(getMatrix().Translate(value.X, value.Y, value.Z)); }
        }

        [Category("Matrix4 Operation")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Rotate
        {
            get { return new Vector3(0.0, 0.0, 0.0); }
            set
            {
                setMatrix(getMatrix().Rotate(Vector3.UnitX, value.X));
                setMatrix(getMatrix().Rotate(Vector3.UnitY, value.Y));
                setMatrix(getMatrix().Rotate(Vector3.UnitZ, value.Z));
            }
        }

        #endregion

        #region Nested type: Matrix4OperationsConverter

        private class Matrix4OperationsConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(string) && value is Matrix4ExpanderOperations)
                    return "Matrix4 Operations";

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion
    }
}
