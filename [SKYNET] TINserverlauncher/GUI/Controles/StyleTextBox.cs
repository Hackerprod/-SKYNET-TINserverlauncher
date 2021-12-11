using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace SKYNET
{
    public class StyleTextBox : UserControl
    {
        private IContainer components;
        private string _waterMarkText;

        private TextBox TBox;

        private Label Label1;

        [Category("Appearance")]
        public string WaterMarkText
        {
            get
            {
                return this._waterMarkText;
            }
            set
            {
                this.TBox.Text = value;
                this._waterMarkText = value;
                if (string.IsNullOrEmpty(value))
                    return;
                this.TBox.ForeColor = Color.Gray;
            }
        }

        [Category("Behavior")]
        public string PasswordChar
        {
            get
            {
                return Conversions.ToString(this.TBox.PasswordChar);
            }
            set
            {
                this.TBox.PasswordChar = Conversions.ToChar(value);
            }
        }

        public StyleTextBox()
        {
            this.InitializeComponent();
            TBox.GotFocus += new EventHandler(this.TBox_GotFocus);
            TBox.LostFocus += new EventHandler(this.TBox_LostFocus);

        }

        [DebuggerNonUserCode]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!disposing || this.components == null)
                    return;
                this.components.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.TBox = new TextBox();
            this.Label1 = new Label();
            this.SuspendLayout();
            this.TBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.TBox.BackColor = Color.WhiteSmoke;
            this.TBox.BorderStyle = BorderStyle.None;
            this.TBox.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.TBox.Location = new Point(20, 6);
            this.TBox.Margin = new Padding(4);
            this.TBox.Name = "TBox";
            this.TBox.Size = new Size(367, 27);
            this.TBox.TabIndex = 91;
            this.Label1.BackColor = Color.FromArgb(66, 139, 202);
            this.Label1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.Label1.Location = new Point(0, 0);
            this.Label1.Margin = new Padding(0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new Size(8, 38);
            this.Label1.TabIndex = 93;
            this.Label1.Text = " ";
            this.AutoScaleDimensions = new SizeF(8f, 16f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.WhiteSmoke;
            this.Controls.Add((Control)this.Label1);
            this.Controls.Add((Control)this.TBox);
            this.Margin = new Padding(4);
            this.Name = "StyleTextBox";
            this.Size = new Size(387, 38);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void TBox_GotFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.WaterMarkText) || !this.TBox.Text.Equals(this.WaterMarkText))
                return;
            this.TBox.Text = string.Empty;
            this.TBox.ForeColor = Color.Black;
        }

        private void TBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.WaterMarkText) || !string.IsNullOrEmpty(this.TBox.Text))
                return;
            this.TBox.Text = this.WaterMarkText;
            this.TBox.ForeColor = Color.Gray;
        }
    }
}
