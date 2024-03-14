namespace SAE2._4
{
    partial class ConsulterAvis
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
            this.lblbTitreAvis = new System.Windows.Forms.Label();
            this.lblFont = new System.Windows.Forms.Label();
            this.pnlAvis = new System.Windows.Forms.Panel();
            this.btnMenu5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblbTitreAvis
            // 
            this.lblbTitreAvis.AutoSize = true;
            this.lblbTitreAvis.Font = new System.Drawing.Font("Comic Sans MS", 16F, System.Drawing.FontStyle.Bold);
            this.lblbTitreAvis.Location = new System.Drawing.Point(74, 59);
            this.lblbTitreAvis.Name = "lblbTitreAvis";
            this.lblbTitreAvis.Size = new System.Drawing.Size(477, 60);
            this.lblbTitreAvis.TabIndex = 5;
            this.lblbTitreAvis.Text = "Nom de la Recette : ";
            // 
            // lblFont
            // 
            this.lblFont.AutoSize = true;
            this.lblFont.Enabled = false;
            this.lblFont.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold);
            this.lblFont.Location = new System.Drawing.Point(649, 534);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(0, 45);
            this.lblFont.TabIndex = 6;
            // 
            // pnlAvis
            // 
            this.pnlAvis.AutoScroll = true;
            this.pnlAvis.BackColor = System.Drawing.Color.Transparent;
            this.pnlAvis.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlAvis.Location = new System.Drawing.Point(125, 206);
            this.pnlAvis.Name = "pnlAvis";
            this.pnlAvis.Size = new System.Drawing.Size(1555, 877);
            this.pnlAvis.TabIndex = 7;
            // 
            // btnMenu5
            // 
            this.btnMenu5.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold);
            this.btnMenu5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(4)))));
            this.btnMenu5.Location = new System.Drawing.Point(1493, 49);
            this.btnMenu5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMenu5.Name = "btnMenu5";
            this.btnMenu5.Size = new System.Drawing.Size(187, 86);
            this.btnMenu5.TabIndex = 37;
            this.btnMenu5.Text = "Quitter";
            this.btnMenu5.UseVisualStyleBackColor = true;
            this.btnMenu5.Click += new System.EventHandler(this.btnMenu5_Click);
            // 
            // ConsulterAvis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SAE2._4.Properties.Resources.Consulter;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1774, 1129);
            this.Controls.Add(this.btnMenu5);
            this.Controls.Add(this.pnlAvis);
            this.Controls.Add(this.lblFont);
            this.Controls.Add(this.lblbTitreAvis);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1800, 1200);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1800, 1200);
            this.Name = "ConsulterAvis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConsulterAvis";
            this.Load += new System.EventHandler(this.ConsulterAvis_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblbTitreAvis;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.Panel pnlAvis;
        private System.Windows.Forms.Button btnMenu5;
    }
}