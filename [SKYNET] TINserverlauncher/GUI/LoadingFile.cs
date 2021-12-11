using System.Windows.Forms;

namespace SKYNET
{
    public partial class LoadingFile : UserControl
    {
        public LoadingFile(string theme)
        {
            InitializeComponent();
            AplicarThema(theme);
        }
        private void AplicarThema(string theme)
        {

            if (theme == "Oscuro" | theme == "Dark")
            {
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(29)))), ((int)(((byte)(33)))));
                this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
                label1.BackColor = this.BackColor;
                label1.ForeColor = this.ForeColor;

            }
            if (theme == "Normal")
            {
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(51)))));
                this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(169)))), ((int)(((byte)(239)))));
                label1.BackColor = this.BackColor;
                label1.ForeColor = this.ForeColor;
            }
            if (theme == "Claro" | theme == "Aqua")
            {
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
                this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(38)))), ((int)(((byte)(43)))));
                label1.BackColor = this.BackColor;
                label1.ForeColor = this.ForeColor;
            }
        }

    }
}
