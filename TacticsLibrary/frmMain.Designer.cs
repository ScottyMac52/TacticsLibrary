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
            this.lblPolarPosition = new System.Windows.Forms.Label();
            this.lblPositionRelative = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPosition = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.contactCourse = new System.Windows.Forms.NumericUpDown();
            this.contactSpeed = new System.Windows.Forms.NumericUpDown();
            this.selectClass = new System.Windows.Forms.ListBox();
            this.selectType = new System.Windows.Forms.ListBox();
            this.plotPanel = new System.Windows.Forms.Panel();
            this.lblCurrentWidth = new System.Windows.Forms.Label();
            this.lblCurrentHeight = new System.Windows.Forms.Label();
            this.gridViewContacts = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contactCourse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contactSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewContacts)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(13, 13);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblPolarPosition);
            this.splitContainer1.Panel1.Controls.Add(this.lblPositionRelative);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.lblPosition);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.contactCourse);
            this.splitContainer1.Panel1.Controls.Add(this.contactSpeed);
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
            // lblPolarPosition
            // 
            this.lblPolarPosition.AutoSize = true;
            this.lblPolarPosition.Location = new System.Drawing.Point(51, 305);
            this.lblPolarPosition.Name = "lblPolarPosition";
            this.lblPolarPosition.Size = new System.Drawing.Size(0, 13);
            this.lblPolarPosition.TabIndex = 9;
            // 
            // lblPositionRelative
            // 
            this.lblPositionRelative.AutoSize = true;
            this.lblPositionRelative.Location = new System.Drawing.Point(51, 286);
            this.lblPositionRelative.Name = "lblPositionRelative";
            this.lblPositionRelative.Size = new System.Drawing.Size(0, 13);
            this.lblPositionRelative.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 265);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Position";
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point(51, 267);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(0, 13);
            this.lblPosition.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 236);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Course";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 209);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Velocity";
            // 
            // contactCourse
            // 
            this.contactCourse.Location = new System.Drawing.Point(122, 232);
            this.contactCourse.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.contactCourse.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.contactCourse.Name = "contactCourse";
            this.contactCourse.Size = new System.Drawing.Size(120, 20);
            this.contactCourse.TabIndex = 3;
            this.contactCourse.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // contactSpeed
            // 
            this.contactSpeed.Location = new System.Drawing.Point(122, 205);
            this.contactSpeed.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.contactSpeed.Name = "contactSpeed";
            this.contactSpeed.Size = new System.Drawing.Size(120, 20);
            this.contactSpeed.TabIndex = 2;
            this.contactSpeed.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
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
            this.plotPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.plotPanel_MouseClick);
            this.plotPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.plotPanel_MouseDoubleClick);
            this.plotPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.plotPanel_MouseMove);
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
            // gridViewContacts
            // 
            this.gridViewContacts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewContacts.Location = new System.Drawing.Point(13, 571);
            this.gridViewContacts.Name = "gridViewContacts";
            this.gridViewContacts.Size = new System.Drawing.Size(745, 150);
            this.gridViewContacts.TabIndex = 9;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 724);
            this.Controls.Add(this.gridViewContacts);
            this.Controls.Add(this.lblCurrentHeight);
            this.Controls.Add(this.lblCurrentWidth);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmMain";
            this.Text = "Main Plot";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contactCourse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contactSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewContacts)).EndInit();
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
        private System.Windows.Forms.NumericUpDown contactSpeed;
        private System.Windows.Forms.NumericUpDown contactCourse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label lblPositionRelative;
        private System.Windows.Forms.DataGridView gridViewContacts;
        private System.Windows.Forms.Label lblPolarPosition;
    }
}

