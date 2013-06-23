namespace Demo
{
    partial class ILPanelForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ILPanelForm));
            this.ilPanel = new ILNumerics.Drawing.ILPanel();
            this.SuspendLayout();
            // 
            // ilPanel
            // 
            this.ilPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.ilPanel.Editor = null;
            this.ilPanel.Location = new System.Drawing.Point(0, 0);
            this.ilPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ilPanel.Name = "ilPanel";
            this.ilPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("ilPanel.Rectangle")));
            this.ilPanel.ShowUIControls = false;
            this.ilPanel.Size = new System.Drawing.Size(684, 461);
            this.ilPanel.TabIndex = 0;
            // 
            // ILPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.ilPanel);
            this.Name = "ILPanelForm";
            this.Text = "ILN Editor Demo";
            this.Load += new System.EventHandler(this.ILPanelForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ILNumerics.Drawing.ILPanel ilPanel;
    }
}

