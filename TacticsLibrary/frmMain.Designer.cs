namespace TacticsLibrary
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.selectClass = new System.Windows.Forms.ListBox();
            this.selectType = new System.Windows.Forms.ListBox();
            this.plotPanel = new System.Windows.Forms.Panel();
            this.lblCurrentWidth = new System.Windows.Forms.Label();
            this.lblCurrentHeight = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(13, 13);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.selectClass);
            this.splitContainer1.Panel1.Controls.Add(this.selectType);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.plotPanel);
            this.splitContainer1.Size = new System.Drawing.Size(745, 555);
            this.splitContainer1.SplitterDistance = 248;
            this.splitContainer1.TabIndex = 1;
            // 
            // selectClass
            // 
            this.selectClass.FormattingEnabled = true;
            this.selectClass.Items.AddRange(new object[] {
            "Unknown",
            "Friendly",
            "Enemy"});
            this.selectClass.Location = new System.Drawing.Point(3, 104);
            this.selectClass.Name = "selectClass";
            this.selectClass.Size = new System.Drawing.Size(120, 95);
            this.selectClass.TabIndex = 1;
            // 
            // selectType
            // 
            this.selectType.FormattingEnabled = true;
            this.selectType.Items.AddRange(new object[] {
            "Air",
            "Surface",
            "Sub",
            "Missile"});
            this.selectType.Location = new System.Drawing.Point(3, 3);
            this.selectType.Name = "selectType";
            this.selectType.Size = new System.Drawing.Size(120, 95);
            this.selectType.TabIndex = 0;
            // 
            // plotPanel
            // 
            this.plotPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.plotPanel.BackColor = System.Drawing.Color.Black;
            this.plotPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.plotPanel.Location = new System.Drawing.Point(-4, 27);
            this.plotPanel.MaximumSize = new System.Drawing.Size(836, 571);
            this.plotPanel.Name = "plotPanel";
            this.plotPanel.Size = new System.Drawing.Size(500, 500);
            this.plotPanel.TabIndex = 1;
            this.plotPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.PlotPanel_Paint);
            this.plotPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.plotPanel_MouseDoubleClick);
            // 
            // lblCurrentWidth
            // 
            this.lblCurrentWidth.AutoSize = true;
            this.lblCurrentWidth.Location = new System.Drawing.Point(767, 40);
            this.lblCurrentWidth.Name = "lblCurrentWidth";
            this.lblCurrentWidth.Size = new System.Drawing.Size(0, 13);
            this.lblCurrentWidth.TabIndex = 2;
            // 
            // lblCurrentHeight
            // 
            this.lblCurrentHeight.AutoSize = true;
            this.lblCurrentHeight.Location = new System.Drawing.Point(268, 571);
            this.lblCurrentHeight.Name = "lblCurrentHeight";
            this.lblCurrentHeight.Size = new System.Drawing.Size(0, 13);
            this.lblCurrentHeight.TabIndex = 3;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 595);
            this.Controls.Add(this.lblCurrentHeight);
            this.Controls.Add(this.lblCurrentWidth);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmMain";
            this.Text = "Main Plot";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel plotPanel;
        private System.Windows.Forms.Label lblCurrentWidth;
        private System.Windows.Forms.Label lblCurrentHeight;
        private System.Windows.Forms.ListBox selectClass;
        private System.Windows.Forms.ListBox selectType;
    }
}

