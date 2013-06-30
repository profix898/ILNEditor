using System;
using System.Collections.Generic;
using System.Diagnostics;
using ILNEditor.Drawing.Plotting;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace ILNEditor
{
    public class ILPanelEditor
    {
        private readonly ILPanel ilPanel;
        private readonly WrapperMap wrapperMap = new WrapperMap();
        private readonly List<object> wrappers = new List<object>();

        private EditorForm propertyForm;

        public ILPanelEditor(ILPanel ilPanel)
        {
            this.ilPanel = ilPanel;

            Update();
        }

        public void Update()
        {
            wrappers.Clear();
            Traverse(ilPanel.Scene.First<ILGroup>());
        }

        private void Traverse(ILGroup group)
        {
            foreach (ILNode node in group.Childs)
            {
                if (node is ILPlotCube)
                    wrappers.Add(new ILPlotCubeWrapper(node as ILPlotCube, this));
                else if (node is ILGroup)
                    Traverse(node as ILGroup);
                else
                {
                    Type nodeType = node.GetType();

                    if (wrapperMap.ContainsKey(nodeType))
                        wrappers.Add(Activator.CreateInstance(wrapperMap[nodeType], node, this));
                    else if (nodeType.BaseType != null && nodeType.BaseType != typeof(object) && wrapperMap.ContainsKey(nodeType.BaseType))
                        wrappers.Add(Activator.CreateInstance(wrapperMap[nodeType.BaseType], node, this));
                }
            }
        }

        #region Internals

        internal ILPanel Panel
        {
            [DebuggerStepThrough]
            get { return ilPanel; }
        }

        internal void MouseDoubleClickPropertyForm(object sender, string label, ILMouseEventArgs e)
        {
            if (propertyForm != null)
                propertyForm.Dispose();

            propertyForm = new EditorForm(sender, label);
            propertyForm.Closed += (o, args) => ilPanel.Refresh();
            propertyForm.Show();

            e.Cancel = true;
        }

        #endregion

        #region Static

        public static ILPanelEditor AttachTo(ILPanel ilPanel)
        {
            return new ILPanelEditor(ilPanel);
        }

        #endregion
    }
}
