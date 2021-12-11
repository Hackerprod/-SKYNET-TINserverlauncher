namespace SKYNET
{
    partial class Message
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Message));
            this.Texto = new System.Windows.Forms.Label();
            this.Boton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.Texto2 = new System.Windows.Forms.Label();
            this.Boton2 = new System.Windows.Forms.Button();
            this.ConfigurarTXT = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // Texto
            // 
            this.Texto.AutoSize = true;
            this.Texto.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Texto.Location = new System.Drawing.Point(18, 46);
            this.Texto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Texto.Name = "Texto";
            this.Texto.Size = new System.Drawing.Size(0, 16);
            this.Texto.TabIndex = 223;
            // 
            // Boton
            // 
            this.Boton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Boton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Boton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Boton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(169)))), ((int)(((byte)(239)))));
            this.Boton.Location = new System.Drawing.Point(212, 100);
            this.Boton.Name = "Boton";
            this.Boton.Size = new System.Drawing.Size(92, 31);
            this.Boton.TabIndex = 224;
            this.Boton.UseVisualStyleBackColor = true;
            this.Boton.MouseLeave += new System.EventHandler(this.Boton_MouseLeave);
            this.Boton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Boton_MouseMove);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(169)))), ((int)(((byte)(239)))));
            this.pictureBox1.Location = new System.Drawing.Point(-5, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(495, 3);
            this.pictureBox1.TabIndex = 225;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(169)))), ((int)(((byte)(239)))));
            this.pictureBox2.Location = new System.Drawing.Point(-5, 140);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(495, 3);
            this.pictureBox2.TabIndex = 226;
            this.pictureBox2.TabStop = false;
            // 
            // Texto2
            // 
            this.Texto2.AutoSize = true;
            this.Texto2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Texto2.Location = new System.Drawing.Point(18, 65);
            this.Texto2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Texto2.Name = "Texto2";
            this.Texto2.Size = new System.Drawing.Size(0, 16);
            this.Texto2.TabIndex = 227;
            // 
            // Boton2
            // 
            this.Boton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Boton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Boton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Boton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(169)))), ((int)(((byte)(239)))));
            this.Boton2.Location = new System.Drawing.Point(310, 100);
            this.Boton2.Name = "Boton2";
            this.Boton2.Size = new System.Drawing.Size(92, 31);
            this.Boton2.TabIndex = 228;
            this.Boton2.UseVisualStyleBackColor = true;
            this.Boton2.Click += new System.EventHandler(this.Boton2_Click);
            this.Boton2.MouseLeave += new System.EventHandler(this.Boton2_MouseLeave);
            this.Boton2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Boton2_MouseMove);
            // 
            // ConfigurarTXT
            // 
            this.ConfigurarTXT.Location = new System.Drawing.Point(12, 108);
            this.ConfigurarTXT.Name = "ConfigurarTXT";
            this.ConfigurarTXT.Size = new System.Drawing.Size(0, 26);
            this.ConfigurarTXT.TabIndex = 229;
            // 
            // Message
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(414, 143);
            this.Controls.Add(this.ConfigurarTXT);
            this.Controls.Add(this.Boton2);
            this.Controls.Add(this.Texto2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Boton);
            this.Controls.Add(this.Texto);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(169)))), ((int)(((byte)(239)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Message";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Message";
            this.Load += new System.EventHandler(this.Message_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Message_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Message_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Message_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Texto;
        private System.Windows.Forms.Button Boton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label Texto2;
        private System.Windows.Forms.Button Boton2;
        private System.Windows.Forms.TextBox ConfigurarTXT;
    }
}