using System;
using System.Collections.Generic;
using ILNEditor.Drawing;
using ILNumerics.Drawing;

namespace ILNEditor
{
    public class WrapperMap : Dictionary<Type, Type>
    {
        public WrapperMap()
        {
            AddDefaultMapping();
        }

        public WrapperMap(IDictionary<Type, Type> dictionary)
            : base(dictionary)
        {
        }

        private void AddDefaultMapping()
        {
            Add(typeof(ILLabel), typeof(ILLabelWrapper));
            Add(typeof(ILLines), typeof(ILLinesWrapper));
        }
    }
}
