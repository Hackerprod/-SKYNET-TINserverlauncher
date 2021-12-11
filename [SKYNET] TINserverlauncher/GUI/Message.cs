using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;                    // Para Stream
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace SKYNET
{
    public partial class Message : Form
    {
        public Message()
        {
            InitializeComponent();
        }
        public string _Boton;
        public string _Boton2;
        public string _Texto;
        public string _Texto2;
        public string _Configurar;
        public string theme;
        public string Aprobar; 
        public string mapa;
        public string Nusuario;
        public string Idioma;
        public string ExportarLS;
        private bool mouseDown;   
        private Point lastLocation;
        public string _Dota2DirPath;


        private void Message_Load(object sender, EventArgs e)
        {
            ConfigurarTXT.Text = _Configurar;
            AplicarThema();
            Mensage();
            if (!string.IsNullOrEmpty(ExportarLS))
            {
                Texto2.Text = _Texto;
                Boton.Hide();
                Boton2.Hide();
            }
        }

        #region   Themas...
        private void AplicarThema()
        {

            if (theme == "Oscuro" | theme == "Dark")
            {
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(29)))), ((int)(((byte)(33)))));
                this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
                Texto.BackColor = this.BackColor;
                Texto.ForeColor = this.ForeColor;
                this.Boton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(48)))));
                this.Boton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
                this.Boton.FlatAppearance.BorderSize = 0;
                this.Boton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(48)))));
                this.Boton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
                this.Boton2.FlatAppearance.BorderSize = 0;

            }
            if (theme == "Normal")
            {
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(51)))));
                this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(169)))), ((int)(((byte)(239)))));
                Texto.BackColor = this.BackColor;
                Texto.ForeColor = this.ForeColor;
                Boton.BackColor = this.BackColor;
                Boton.ForeColor = this.ForeColor;
                Boton2.BackColor = this.BackColor;
                Boton2.ForeColor = this.ForeColor;

            }
            if (theme == "Claro" | theme == "Aqua")
            {
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
                this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(38)))), ((int)(((byte)(43)))));
                Texto.BackColor = this.BackColor;
                Texto.ForeColor = this.ForeColor;
                Boton.BackColor = this.BackColor;
                Boton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(169)))), ((int)(((byte)(239)))));
                Boton2.BackColor = this.BackColor;
                Boton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(169)))), ((int)(((byte)(239)))));

            }
        }
        #endregion

        private void Mensage()
        {   
            //////////////////////////////////////////////////////////////////////////
            //Mensages en Español
            if (Idioma == "Español")
                {
                if (ConfigurarTXT.Text == "Cliente")
                    {
                        Texto.Text = "No haz configurado la dirección del Cliente...";
                        Texto2.Text = "Quieres hacerlo ahora";
                        Boton.Text = "Si";
                        Boton2.Text = "No";
                    }
                if (ConfigurarTXT.Text == "Servidor")
                {
                    Texto.Text = "No haz configurado la dirección del Servidor...";
                    Texto2.Text = "Quieres hacerlo ahora";
                    Boton.Text = "Si";
                    Boton2.Text = "No";
                }
                if (ConfigurarTXT.Text == "//MapaDir")
                {
                    Texto.Text = "No haz seleccionado la carpeta que contiene los mapas...";
                    Texto2.Text = "Quieres hacerlo ahora";
                    Boton.Text = "Si";
                    Boton2.Text = "No";
                }
                if (ConfigurarTXT.Text == "Mapa")
                {
                    if (mapa == "The King's New Journey")
                        {
                        Texto.Text = "El mapa " + mapa + " se ha aplicado ";
                        Texto2.Text = "correctamente";
                        Boton.Hide();
                        Boton2.Text = "Aceptar";
                        }
                    else
                    {
                        Texto.Text = "El mapa " + mapa + " se ha aplicado correctamente";
                        Boton.Hide();
                        Boton2.Text = "Aceptar";
                    }
                }
                if (ConfigurarTXT.Text == "ServerIP")
                {
                    Texto.Text = "Debes poner el número IP del servidor";
                    Boton.Hide();
                    Boton2.Text = "Aceptar";
                }
                if (ConfigurarTXT.Text == "MapaErr")
                {
                    Texto.Text = "Los Mapas no se pudieron importar";
                    Boton.Hide();
                    Boton2.Text = "Aceptar";
                }
                if (ConfigurarTXT.Text == "UserErr")
                {
                    Texto.Text = "Debes poner un nombre de usuario ";
                    Boton.Hide();
                    Boton2.Text = "Aceptar";
                }
                if (ConfigurarTXT.Text == "PassErr")
                {
                    Texto.Text = "Debes poner una contraseña";
                    Boton.Hide();
                    Boton2.Text = "Aceptar";
                }
                if (ConfigurarTXT.Text == "UserError")
                {
                    Texto.Text = "Error al crear nuevo usuario... Verifique la dirección ";
                    Texto2.Text = "del cliente o si el usuario "+ Nusuario +" existe";
                    Boton.Hide();
                    Boton2.Text = "Aceptar";
                }
                if (ConfigurarTXT.Text == "UserAdd")
                {
                    Texto.Text = "El usuario " + Nusuario + " se agregó correctamente";
                    Boton.Hide();
                    Boton2.Text = "Aceptar";
                }
                if (ConfigurarTXT.Text == "UserAddErr")
                {
                    Texto.Text = "No se pudo agregar el usuario " + Nusuario + "";
                    Boton.Hide();
                    Boton2.Text = "Aceptar";
                }
                if (ConfigurarTXT.Text == "SteamErr")
                {
                    Texto.Text = "Debes configurar los datos de autentificación de Steam";
                    Boton.Hide();
                    Boton2.Text = "Aceptar";
                }
                if (ConfigurarTXT.Text == "ErrorMaps")
                {
                    Texto.Text = "Error al aplicar el mapa... verifique la dirección del";
                    Texto2.Text = "cliente de Steam";
                    Boton.Hide();
                    Boton2.Text = "Aceptar";
                }
                if (ConfigurarTXT.Text == "MapaLoc")
                {
                    Texto.Text = "No haz seleccionado la carpeta que contiene los mapas...";
                    Texto2.Text = "Quieres hacerlo ahora";
                    Boton.Text = "Si";
                    Boton2.Text = "No";
                }
                if (ConfigurarTXT.Text == "MapaImport")
                {
                    Texto.Text = "Los mapas se importaron correctamente";
                    Boton.Hide();
                    Boton2.Text = "Aceptar";
                }
                if (ConfigurarTXT.Text == "UserExist")
                {
                    Texto.Text = "El usuario " + Nusuario + " existe";
                    Boton.Hide();
                    Boton2.Text = "Aceptar";
                }

            }




            //////////////////////////////////////////////////////////////////////
            //Mensages en Ingles
            else 
            {
                if (ConfigurarTXT.Text == "Cliente")
                {
                    Texto.Text = "You have not configured the client address...";
                    Texto2.Text = "You want to make it now";
                    Boton.Text = "Yes";
                    Boton2.Text = "No";
                }
                if (ConfigurarTXT.Text == "Servidor")
                {
                    Texto.Text = "You have not configured the server address...";
                    Texto2.Text = "You want to make it now";
                    Boton.Text = "Yes";
                    Boton2.Text = "No";
                }
                if (ConfigurarTXT.Text == "//MapaDir")
                {
                    Texto.Text = "You have not selected the folder that contains the maps...";
                    Texto2.Text = "You want to make it now";
                    Boton.Text = "Yes";
                    Boton2.Text = "No";
                }
                if (ConfigurarTXT.Text == "Mapa")
                {
                    if (mapa == "The King's New Journey")
                    {
                        Texto.Text = "The map " + mapa + " has been applied";
                        Texto2.Text = "correctly";
                        Boton.Hide();
                        Boton2.Text = "Ok";
                    }
                    else
                    {
                        Texto.Text = "El mapa " + mapa + " has been applied correctly";
                        Boton.Hide();
                        Boton2.Text = "Ok";
                    }
                }
                if (ConfigurarTXT.Text == "ServerIP")
                {
                    Texto.Text = "You should put the IP number of the server";
                    Boton.Hide();
                    Boton2.Text = "Ok";
                }
                if (ConfigurarTXT.Text == "MapaErr")
                {
                    Texto.Text = "The maps could not be imported";
                    Boton.Hide();
                    Boton2.Text = "Ok";
                }
                if (ConfigurarTXT.Text == "UserErr")
                {
                    Texto.Text = "You should put the user's name ";
                    Boton.Hide();
                    Boton2.Text = "Ok";
                }
                if (ConfigurarTXT.Text == "PassErr")
                {
                    Texto.Text = "You should put the password";
                    Boton.Hide();
                    Boton2.Text = "Ok";
                }
                if (ConfigurarTXT.Text == "UserError")
                {
                    Texto.Text = "Error when creating new user... Verify the address ";
                    Texto2.Text = "of the client or if the user " + Nusuario + " exists";
                    Boton.Hide();
                    Boton2.Text = "Ok";
                }
                if (ConfigurarTXT.Text == "UserAdd")
                {
                    Texto.Text = "The user " + Nusuario + " has been added correctly";
                    Boton.Hide();
                    Boton2.Text = "Ok";
                }
                if (ConfigurarTXT.Text == "UserAddErr")
                {
                    Texto.Text = "The user " + Nusuario + " could not be added";
                    Boton.Hide();
                    Boton2.Text = "Ok";
                }
                if (ConfigurarTXT.Text == "SteamErr")
                {
                    Texto.Text = "You should configure the user's name and password";
                    Texto2.Text = "of real Steam account";
                    Boton.Hide();
                    Boton2.Text = "Ok";
                }
                if (ConfigurarTXT.Text == "ErrorMaps")
                {
                    Texto.Text = "Error when applying the map... verify the address of the";
                    Texto2.Text = "Steam client";
                    Boton.Hide();
                    Boton2.Text = "Ok";
                }
                if (ConfigurarTXT.Text == "MapaLoc")
                {
                    Texto.Text = "You have not selected the folder that contains the maps...";
                    Texto2.Text = "You want to make it now";
                    Boton.Text = "Yes";
                    Boton2.Text = "No";
                }
                if (ConfigurarTXT.Text == "MapaImport")
                {
                    Texto.Text = "The maps were imported correctly";
                    Boton.Hide();
                    Boton2.Text = "Ok";
                }
                if (ConfigurarTXT.Text == "UserExist")
                {
                    Texto.Text = "The user " + Nusuario + " exists";
                    Boton.Hide();
                    Boton2.Text = "Ok";
                }
            }
        }


        private void Message_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Message_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }

        private void Message_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void Boton_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.BackColor == System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(29)))), ((int)(((byte)(33))))))
            {
                this.Boton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                this.Boton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            }
        }

        private void Boton_MouseLeave(object sender, EventArgs e)
        {
            AplicarThema();
        }
        private void Boton2_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.BackColor == System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(29)))), ((int)(((byte)(33))))))
            {
                this.Boton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                this.Boton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            }
        }

        private void Boton2_MouseLeave(object sender, EventArgs e)
        {
            AplicarThema();
        }

        private void Boton2_Click(object sender, EventArgs e)
        {

        }
    }
}
