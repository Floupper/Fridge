namespace SAE2._4
{
    partial class Pseudo
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
            this.lblUtilisateur = new System.Windows.Forms.Label();
            this.lblNvPseudo = new System.Windows.Forms.Label();
            this.btnIgnorerNvPseudo = new System.Windows.Forms.Button();
            this.btnValiderPseudo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblUtilisateur
            // 
            this.lblUtilisateur.AutoSize = true;
            this.lblUtilisateur.Font = new System.Drawing.Font("Comic Sans MS", 16F, System.Drawing.FontStyle.Bold);
            this.lblUtilisateur.Location = new System.Drawing.Point(44, 67);
            this.lblUtilisateur.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUtilisateur.Name = "lblUtilisateur";
            this.lblUtilisateur.Size = new System.Drawing.Size(564, 45);
            this.lblUtilisateur.TabIndex = 0;
            this.lblUtilisateur.Text = "Voulez vous ajouter l\'utilisateur : ";
            // 
            // lblNvPseudo
            // 
            this.lblNvPseudo.AutoSize = true;
            this.lblNvPseudo.Font = new System.Drawing.Font("Comic Sans MS", 14F);
            this.lblNvPseudo.Location = new System.Drawing.Point(47, 157);
            this.lblNvPseudo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNvPseudo.Name = "lblNvPseudo";
            this.lblNvPseudo.Size = new System.Drawing.Size(38, 39);
            this.lblNvPseudo.TabIndex = 1;
            this.lblNvPseudo.Text = "...";
            // 
            // btnIgnorerNvPseudo
            // 
            this.btnIgnorerNvPseudo.BackgroundImage = global::SAE2._4.Properties.Resources.png_clipart_x_icon_american_red_cross_international_red_cross_and_red_crescent_movement_red_cross_text_logo;
            this.btnIgnorerNvPseudo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIgnorerNvPseudo.Location = new System.Drawing.Point(461, 225);
            this.btnIgnorerNvPseudo.Margin = new System.Windows.Forms.Padding(2);
            this.btnIgnorerNvPseudo.Name = "btnIgnorerNvPseudo";
            this.btnIgnorerNvPseudo.Size = new System.Drawing.Size(86, 84);
            this.btnIgnorerNvPseudo.TabIndex = 8;
            this.btnIgnorerNvPseudo.UseVisualStyleBackColor = true;
            this.btnIgnorerNvPseudo.Click += new System.EventHandler(this.btnIgnorerNvPseudo_Click);
            // 
            // btnValiderPseudo
            // 
            this.btnValiderPseudo.BackgroundImage = global::SAE2._4.Properties.Resources.Check_Mark_PNG_Clipart;
            this.btnValiderPseudo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnValiderPseudo.Location = new System.Drawing.Point(341, 225);
            this.btnValiderPseudo.Margin = new System.Windows.Forms.Padding(2);
            this.btnValiderPseudo.Name = "btnValiderPseudo";
            this.btnValiderPseudo.Size = new System.Drawing.Size(86, 84);
            this.btnValiderPseudo.TabIndex = 7;
            this.btnValiderPseudo.UseVisualStyleBackColor = true;
            this.btnValiderPseudo.Click += new System.EventHandler(this.btnValiderPseudo_Click);
            // 
            // Pseudo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SAE2._4.Properties.Resources.pseudo;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(656, 343);
            this.Controls.Add(this.btnIgnorerNvPseudo);
            this.Controls.Add(this.btnValiderPseudo);
            this.Controls.Add(this.lblNvPseudo);
            this.Controls.Add(this.lblUtilisateur);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Pseudo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pseudo";
            this.Load += new System.EventHandler(this.Pseudo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUtilisateur;
        private System.Windows.Forms.Label lblNvPseudo;
        private System.Windows.Forms.Button btnValiderPseudo;
        private System.Windows.Forms.Button btnIgnorerNvPseudo;
    }
}