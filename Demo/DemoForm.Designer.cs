namespace Demo
{
    partial class DemoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DemoForm));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxDemo = new System.Windows.Forms.ComboBox();
            this.ilPanel = new ILNumerics.Drawing.ILPanel();
            this.btnFromXml = new System.Windows.Forms.Button();
            this.btnToXml = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.btnToXml);
            this.splitContainer.Panel1.Controls.Add(this.btnFromXml);
            this.splitContainer.Panel1.Controls.Add(this.label1);
            this.splitContainer.Panel1.Controls.Add(this.comboBoxDemo);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.ilPanel);
            this.splitContainer.Size = new System.Drawing.Size(684, 461);
            this.splitContainer.SplitterDistance = 30;
            this.splitContainer.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Demo";
            // 
            // comboBoxDemo
            // 
            this.comboBoxDemo.FormattingEnabled = true;
            this.comboBoxDemo.Location = new System.Drawing.Point(53, 6);
            this.comboBoxDemo.Name = "comboBoxDemo";
            this.comboBoxDemo.Size = new System.Drawing.Size(186, 21);
            this.comboBoxDemo.TabIndex = 0;
            this.comboBoxDemo.SelectedIndexChanged += new System.EventHandler(this.comboBoxDemo_SelectedIndexChanged);
            // 
            // ilPanel
            // 
            this.ilPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilPanel.Driver = ILNumerics.Drawing.RendererTypes.GDI;
            this.ilPanel.Editor = null;
            this.ilPanel.Location = new System.Drawing.Point(0, 0);
            this.ilPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ilPanel.Name = "ilPanel";
            this.ilPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("ilPanel.Rectangle")));
            this.ilPanel.ShowUIControls = false;
            this.ilPanel.Size = new System.Drawing.Size(684, 427);
            this.ilPanel.TabIndex = 0;
            // 
            // btnFromXml
            // 
            this.btnFromXml.Location = new System.Drawing.Point(597, 4);
            this.btnFromXml.Name = "btnFromXml";
            this.btnFromXml.Size = new System.Drawing.Size(75, 23);
            this.btnFromXml.TabIndex = 2;
            this.btnFromXml.Text = "FromXml";
            this.btnFromXml.UseVisualStyleBackColor = true;
            this.btnFromXml.Click += new System.EventHandler(this.btnFromXml_Click);
            // 
            // btnToXml
            // 
            this.btnToXml.Location = new System.Drawing.Point(516, 4);
            this.btnToXml.Name = "btnToXml";
            this.btnToXml.Size = new System.Drawing.Size(75, 23);
            this.btnToXml.TabIndex = 2;
            this.btnToXml.Text = "ToXml";
            this.btnToXml.UseVisualStyleBackColor = true;
            this.btnToXml.Click += new System.EventHandler(this.btnToXml_Click);
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.splitContainer);
            this.Name = "DemoForm";
            this.Text = "ILN Editor Demo";
            this.Load += new System.EventHandler(this.ILPanelForm_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxDemo;
        private ILNumerics.Drawing.ILPanel ilPanel;
        private System.Windows.Forms.Button btnToXml;
        private System.Windows.Forms.Button btnFromXml;

    }
}

