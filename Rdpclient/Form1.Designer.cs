namespace Rdpclient
{
    partial class Form1
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.ConnectionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgBox = new System.Windows.Forms.PictureBox();
            this.grupScreen = new System.Windows.Forms.GroupBox();
            this.lblStats = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).BeginInit();
            this.grupScreen.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectionMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(786, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // ConnectionMenuItem
            // 
            this.ConnectionMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectMenuItem,
            this.disconnectMenuItem});
            this.ConnectionMenuItem.Name = "ConnectionMenuItem";
            this.ConnectionMenuItem.Size = new System.Drawing.Size(62, 20);
            this.ConnectionMenuItem.Text = "Bağlantı";
            // 
            // connectMenuItem
            // 
            this.connectMenuItem.Name = "connectMenuItem";
            this.connectMenuItem.Size = new System.Drawing.Size(147, 22);
            this.connectMenuItem.Text = "Bağlan";
            this.connectMenuItem.Click += new System.EventHandler(this.connectMenuItem_Click);
            // 
            // disconnectMenuItem
            // 
            this.disconnectMenuItem.Name = "disconnectMenuItem";
            this.disconnectMenuItem.Size = new System.Drawing.Size(147, 22);
            this.disconnectMenuItem.Text = "Bağlantıyı Kes";
            this.disconnectMenuItem.Click += new System.EventHandler(this.disconnectMenuItem_Click);
            // 
            // imgBox
            // 
            this.imgBox.Location = new System.Drawing.Point(6, 19);
            this.imgBox.Name = "imgBox";
            this.imgBox.Size = new System.Drawing.Size(750, 357);
            this.imgBox.TabIndex = 7;
            this.imgBox.TabStop = false;
            // 
            // grupScreen
            // 
            this.grupScreen.Controls.Add(this.lblStats);
            this.grupScreen.Controls.Add(this.imgBox);
            this.grupScreen.Location = new System.Drawing.Point(12, 27);
            this.grupScreen.Name = "grupScreen";
            this.grupScreen.Size = new System.Drawing.Size(762, 382);
            this.grupScreen.TabIndex = 8;
            this.grupScreen.TabStop = false;
            this.grupScreen.Text = "groupBox1";
            // 
            // lblStats
            // 
            this.lblStats.BackColor = System.Drawing.Color.Transparent;
            this.lblStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblStats.ForeColor = System.Drawing.Color.Black;
            this.lblStats.Location = new System.Drawing.Point(17, 29);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(470, 40);
            this.lblStats.TabIndex = 8;
            this.lblStats.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 532);
            this.Controls.Add(this.grupScreen);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).EndInit();
            this.grupScreen.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem ConnectionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectMenuItem;
        private System.Windows.Forms.PictureBox imgBox;
        private System.Windows.Forms.GroupBox grupScreen;
        private System.Windows.Forms.Label lblStats;
    }
}

