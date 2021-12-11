using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using SKYNET.Properties;
using System.Data;
using System.Linq;
using System.ComponentModel;
using Microsoft.Win32;
using System.Threading;
using System.Data.SQLite;
using System.Security.Principal;
using System.IO;                
using System.Net;     

namespace SKYNET
{
    public partial class frmMain : Form
    {
        public static frmMain frm;
        public frmMain()
        {
            InitializeComponent();
            frm = this;

            PruebasEnMiPc();
            SaveSettings.LoadAllSettings(Application.ProductName, this);
            FirstLaunch();
            ComprobarDLL();
            CheckForIllegalCrossThreadCalls = false;  

            CpuUtil c = new CpuUtil();
            c.PerformanceCounterEventHandler += CpuUsage;

            RamUtil r = new RamUtil();
            r.PerformanceCounterEventHandler += PerformanceCounterEventHandler;
        }

        //variables
        public string _txtuserINI;
        public string _txtuserDB3;
        public string _Cerrarlauncher;
        public string _Minimlauncher;
        public string _MinimBlauncher;
        public string _opendotaBox;
        public string _NewuserAmbos;
        public string _Testbox;
        public string _testBox;
        public string MapaLauncher;
        public string Clientdir;
        public string Serverloc;
        public string theme;
        public string DotaDir;
        public string x86x64Box;
        public string _Serverdir;
        public string Mapdir;
        public string fileName;
        public string destFile;
        public string _clientdir;
        public System.Media.SoundPlayer shopA;  //Sonido
        public System.Media.SoundPlayer shop1;  //Sonido
        public System.Media.SoundPlayer shop2;  //Sonido
        public System.Media.SoundPlayer shop3;  //Sonido
        public System.Media.SoundPlayer shop4;  //Sonido
        private bool mouseDown;     //Mover ventana
        private Point lastLocation; //Mover ventana
        //private static bool IsDbRecentlyCreated = false;
        private string PingIP;
        private string IPstats;
 //       dotaGSI GSI;



        private void PruebasEnMiPc()
        {
            // Borrar INI para errores de websokets
            if (Environment.CurrentDirectory == @"D:\Instaladores\Programación\Projects\TINserverlauncher\TINserverlauncher\bin\x86\Debug")
            {
                //if (File.Exists((Environment.CurrentDirectory + @"/TINserverlauncher/TINserverlauncherIP.ini")))
                //File.Delete(Environment.CurrentDirectory + @"/TINserverlauncher/TINserverlauncherIP.ini");
            }
        }



        private void MainDark_Load(object sender, EventArgs e)
        {
            AplicarThema();
            DotaMenu();
            CargarIp();
            GenerarDirectorio();
            generarPatch();
            FirstLaunch();
            CargarBotones();
            IpBox.Text = "";
            OpcionesNulas();

            //Cargar el IP de txtip
            RegistryKey IPview = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
            IPview.GetValue("txtip", RegistryValueKind.String);
            String Ipshow = (String)IPview.GetValue("txtip");
            txtip.Text = Ipshow;

        }
        private void FirstLaunch()
        {
            //Ver si First Run esta vacio
            RegistryKey FirstR = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
            FirstR.GetValue("FirstRun", RegistryValueKind.String);
            String FirstRun = (String)FirstR.GetValue("FirstRun");
            if (string.IsNullOrEmpty(FirstRun))
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TINserverlauncher");
                key.SetValue("Cerrarlauncher", "False");
                key.SetValue("mantenerA", "True");
                key.SetValue("MinimBlauncher", "False");
                key.SetValue("Minimlauncher", "False");
                key.SetValue("NewuserAmbos", "False");
                key.SetValue("opendotaBox", "False");
                key.SetValue("remoteserver", "False");
                key.SetValue("txtuserDB3", "False");
                key.SetValue("txtuserINI", "True");
                key.SetValue("userBox", "TINserver.users.ini");
                key.SetValue("x86x64", "x86");
                key.SetValue("ShopSound", "1");
                key.SetValue("FirstRun", "Ya iniciado");
                key.Close();
                txtip.Clear();
                localserver.Checked = true;
                mantenerA.Checked = true;
                LaunchOption.Checked = true;
                userBox.Text = "TINserver.users.ini";
                historialiP.Text = "Desactivar";
                Thema.Text = "Normal";
                mapaBox.Text = "Default";
                ComprobarDLL();
                dotaMenu.Checked = false;
                DotaBtn.Hide();
                opendotaBox.Hide();
                if (Thread.CurrentThread.CurrentCulture.Name == "es-ES")
                {
                    LanguajeBox.Text = "Español";
                }
                else
                {
                    LanguajeBox.Text = "English";
                }
                SaveSettings.SaveAllSettings(Application.ProductName, this);
            }
        }
		public bool IsVerified()
        {
			if(string.IsNullOrEmpty(txtip.Text))
            {
				//MessageBox.Show("Debes Poner el número IP del servidor","");
                Message Mensage = new Message(); //creamos un objeto del formulario 2
                Mensage._Configurar = "ServerIP";
                string Themes = Thema.Text;
                Mensage.theme = Themes;
                string Idioma = LanguajeBox.Text;
                Mensage.Idioma = Idioma;
                DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
				return false;
			}
			return true;
        }
        private void OpcionesNulas()
        {
            try{

            if (string.IsNullOrEmpty(LanguajeBox.Text))    //Idioma
            {
                if (Idiomalbl.Text=="Idioma")
                {
                    LanguajeBox.Text = "Español";
                }
                else if (Idiomalbl.Text == "Language")
                {
                    LanguajeBox.Text = "English";
                }
            }

            if (string.IsNullOrEmpty(userBox.Text))    //UserBox
            {
                userBox.Text = "TINserver.users.ini";
            }

            //Menu de Dota
            RegistryKey DotaMenu = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
            DotaMenu.GetValue("DotaMenu", RegistryValueKind.String);
            string menuDota = (String)DotaMenu.GetValue("DotaMenu");
            if (menuDota == null)
            {
                dotaMenu.Checked = true;
            }

            if (string.IsNullOrEmpty(historialiP.Text)) //Historial IP
            {

                    if (Idiomalbl.Text == "Idioma")
                    {
                        historialiP.Text = "Desactivar";
                    }
                    else if (Idiomalbl.Text == "Language")
                    {
                        historialiP.Text = "Disable";
                    }

            }

            if (string.IsNullOrEmpty(Thema.Text))   // Thema
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    if (Idiomalbl.Text == "Idioma")
                    {
                        Thema.Text = "Oscuro";
                    }
                    else if (Idiomalbl.Text == "Language")
                    {
                        Thema.Text = "Dark";
                    }
                }
                else if (BackColor == Color.FromArgb(41, 44, 51))
                {
                    Thema.Text = "Normal";
                }
                else if (BackColor == Color.FromArgb(240, 243, 248))
                {
                    if (Idiomalbl.Text == "Idioma")
                    {
                        Thema.Text = "Claro";
                    }
                    else if (Idiomalbl.Text == "Language")
                    {
                        Thema.Text = "Acua";
                    }
                }
            }
                // Opciones de Lanzamiento
                if (LaunchOption.Checked == false)
                {
                    FontBack1.Hide();
                    Cerrarlauncher.Hide();
                    Minimlauncher.Hide();
                    MinimBlauncher.Hide();
                    mantenerA.Hide();
                    opendotaBox.Hide();
                    TINserverO.Hide();
                    GCExterno.Hide();
                    GCfilename.Hide();
                    SearchServerGC.Hide();
                }
                    else
                {
                    FontBack1.Show();
                    Cerrarlauncher.Show();
                    Minimlauncher.Show();
                    MinimBlauncher.Show();
                    mantenerA.Show();
                    GCExterno.Show();
                    TINserverO.Show();
                    if (GCExterno.Checked == true)
                    {
                        GCfilename.Show();
                        SearchServerGC.Show();
                    }

                    if (dotaMenu.Checked == true)
                    {
                    opendotaBox.Show();
                    }
                }

        }catch (Exception exc) { Crearlog(exc.Message); }
    }
        private void Borrardll()
        {
            string Acceso = Environment.CurrentDirectory;
            if (Acceso != @"D:\Instaladores\Programación\Projects\TINserverlauncher\TINserverlauncher\bin\x86\Debug")
            {
               //ValvePak
                try {
                    if (File.Exists("ValvePak.dll"))
                    {
                        File.Delete("ValvePak.dll");   
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }
                //ScreenCapture
                try {
                    if (File.Exists("ScreenCapture.dll"))
                    {
                        File.Delete("ScreenCapture.dll");
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }

                //MyVideoClass
                try {
                    if (File.Exists("MyVideoClass.dll"))
                    {
                        File.Delete("MyVideoClass.dll");
                    }
                } catch { }
                try {
                //ValveResourceFormat.dll
                    if (File.Exists("ValveResourceFormat.dll"))
                    {
                        File.Delete("ValveResourceFormat.dll");
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }
                try {
                //System_Data_SQLite.dll
                    if (File.Exists("System.Data.SQLite.dll"))
                    {
                        File.Delete("System.Data.SQLite.dll");
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }
                try {
                //TcpComm
                    if (File.Exists("TcpComm.dll"))
                    {
                        File.Delete("TcpComm.dll");
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }
                /*
                try {
                    //CapturarEstadisticas.exe
                    if (File.Exists("CapturarEstadisticas.exe"))
                    {
                        File.Delete("CapturarEstadisticas.exe");
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }
                try {
                    //Dota2StatsGrapper.exe
                    if (File.Exists("Dota2StatsGrapper.exe"))
                    {
                        File.Delete("Dota2StatsGrapper.exe");
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }
                try {
                //MySql_Data.dll
                    if (File.Exists("MySql.Data.dll"))
                    {
                        //File.Delete("MySql.Data.dll");
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }
                try {
                    //Newtonsoft_Json.dll
                    if (File.Exists("Newtonsoft.Json.dll"))
                    {
                        //File.Delete("Newtonsoft.Json.dll");
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }
                */
            }
        }


        private void ComprobarDLL()
        {
            //////////////////////////////////////////
            //Estadisticas

            //Newtonsoft_Json.dll
            try
            {
                if (!File.Exists(@"Newtonsoft.Json.dll"))
                {
                    File.WriteAllBytes(@"Newtonsoft.Json.dll", Resources.Newtonsoft_Json);
                    File.SetAttributes(@"Newtonsoft.Json.dll", FileAttributes.Hidden);
                }
            }
            catch (Exception exc) { Crearlog(exc.Message);  }
            //////////////////////////////////////////
            //System.Data.SQLite.dll
            try
            {
                if (!File.Exists(@"System.Data.SQLite.dll"))
                {
                    File.WriteAllBytes(@"System.Data.SQLite.dll", Resources.System_Data_SQLite);
                    File.SetAttributes(@"System.Data.SQLite.dll", FileAttributes.Hidden);
                }
            }
            catch (Exception exc) { Crearlog(exc.Message);  }
            //MySql.Data.dll
            try
            {
                if (!File.Exists(@"MySql.Data.dll"))
                {
                    File.WriteAllBytes(@"MySql.Data.dll", Resources.MySql_Data);
                    File.SetAttributes(@"MySql.Data.dll", FileAttributes.Hidden);
                }
            }
            catch (Exception exc) { Crearlog(exc.Message);  }
            //MyVideoClass.dll
            try
            {
                if (!File.Exists(@"MyVideoClass.dll"))
                {
                    File.WriteAllBytes(@"MyVideoClass.dll", Resources.MyVideoClass);
                    File.SetAttributes(@"MyVideoClass.dll", FileAttributes.Hidden);
                }
            }
            catch (Exception exc) { Crearlog(exc.Message);  }
        }
        private void CargarIp()
            {   
                try
                    {
                string[] SavedIP = new string[] { };

                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TINserverlauncher");
                key.GetValue("SavedIP", RegistryValueKind.MultiString);
                string[] Ipshow = (string[])key.GetValue("SavedIP");

                if (Ipshow == null)
                {
                    key.SetValue("SavedIP", SavedIP, RegistryValueKind.MultiString);
                }
                else
                {
                    foreach (string ip in Ipshow)
                    {
                        if (!string.IsNullOrEmpty(ip))
                        {
                            IpBox.Items.Add(ip);
                        }
                    }
                    cargarIPinMain();
                }

            }
            catch {}
            }
        private void cargarIPinMain()
        {
            RegistryIP.Clear();
            foreach (string ip in IpBox.Items)
            {
                if (!string.IsNullOrEmpty(ip))
                {
                    RegistryIP.Text = RegistryIP.Text + Environment.NewLine + ip;
                }
            }
        }

        private void CargarBotones()
            {
                //Oscuro
                try { if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    CerrarBox.Image = Resources.CerrarOscuro_1;
                }
                //Normal
                if (BackColor == Color.FromArgb(41, 44, 51))
                {
                    CerrarBox.Image = Resources.CerrarNormal_1;
                }
                //Claro
                if (BackColor == Color.FromArgb(240, 243, 248))
                {
                    CerrarBox.Image = Resources.CerrarClaro_1;
                }
                }
            catch (Exception exc) { Crearlog(exc.Message); }

                try { if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    MiniBox.Image = Resources.MinimizeOscuro_1;
                }
                //Normal
                if (BackColor == Color.FromArgb(41, 44, 51))
                {
                    MiniBox.Image = Resources.MinimizeNormal_1;
                }
                //Claro
                if (BackColor == Color.FromArgb(240, 243, 248))
                {
                    MiniBox.Image = Properties.Resources.MinimizeClaro_1;
                }
                }
            catch (Exception exc) { Crearlog(exc.Message); }
            }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region   Mapas...

        private void AplicarMapa()
        {
            //Directorio Patch
            RegistryKey Patch = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
            Patch.GetValue("Patch", RegistryValueKind.String);
            String lauchPatch = (String)Patch.GetValue("Patch");
            Clientdir = clientloctxt.Text;
            string nombremapa = mapaBox.Text;
            string fileName = nombremapa + ".vpk";
            string sourcefile = lauchPatch + @"\Tinserverlauncher\Maps\";
            string targetfile = Clientdir + @"\SteamApps\common\dota 2 beta\game\dota\maps\dota.vpk";
            string target = Clientdir + @"\SteamApps\common\dota 2 beta\game\dota\maps\";

            if (Directory.Exists(sourcefile))
            {
                try
                   {
                //Immortal Gardens
                if (nombremapa == "Immortal Gardens")
                {                         
                    //programacion del mapa
                    File.Delete(targetfile);
                    File.Copy(sourcefile + "immortalgarden.vpk", targetfile);
                    //MessageBox.Show("El mapa " + nombremapa + " se ha aplicado correctamente");
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Mapa";
                    string Themes = Thema.Text;
                    string Mapa = mapaBox.Text;
                    Mensage.theme = Themes;
                    Mensage.mapa = Mapa;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                }

                //The King's New Journey
                else if (nombremapa == "The King's New Journey")
                {
                    //programacion del mapa
                    File.Delete(targetfile);
                    File.Copy(sourcefile + "kingsjourney.vpk", targetfile);
                    //MessageBox.Show("El mapa " + nombremapa + " se ha aplicado correctamente");
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Mapa";
                    string Themes = Thema.Text;
                    string Mapa = mapaBox.Text;
                    Mensage.theme = Themes;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    Mensage.mapa = Mapa;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                }

                //Reef's Edge 
                else if (nombremapa == "Reef's Edge")
                {
                    //programacion del mapa
                    File.Delete(targetfile);
                    File.Copy(sourcefile + "reefsedge.vpk", targetfile);
                    //MessageBox.Show("El mapa " + nombremapa + " se ha aplicado correctamente");
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Mapa";
                    string Themes = Thema.Text;
                    string Mapa = mapaBox.Text;
                    Mensage.theme = Themes;
                    Mensage.mapa = Mapa;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                }

                //Autumn 
                else if (nombremapa == "Autumn")
                {
                    //programacion del mapa
                    File.Delete(targetfile);
                    File.Copy(sourcefile + "autumn.vpk", targetfile);
                    //MessageBox.Show("El mapa " + nombremapa + " se ha aplicado correctamente");
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Mapa";
                    string Themes = Thema.Text;
                    string Mapa = mapaBox.Text;
                    Mensage.theme = Themes;
                    Mensage.mapa = Mapa;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                }

                //default 
                else if (nombremapa == "Default")
                {
                    //programacion del mapa
                    File.Delete(targetfile);
                    File.Copy(sourcefile + "default.vpk", targetfile);
                    //MessageBox.Show("El mapa " + nombremapa + " se ha aplicado correctamente");
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Mapa";
                    string Themes = Thema.Text;
                    string Mapa = mapaBox.Text;
                    Mensage.theme = Themes;
                    Mensage.mapa = Mapa;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                }

                //desert
                else if (nombremapa == "Desert")
                {
                    //programacion del mapa
                    File.Delete(targetfile);
                    File.Copy(sourcefile + "desert.vpk", targetfile);
                    //MessageBox.Show("El mapa " + nombremapa + " se ha aplicado correctamente");
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Mapa";
                    string Themes = Thema.Text;
                    string Mapa = mapaBox.Text;
                    Mensage.theme = Themes;
                    Mensage.mapa = Mapa;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                }

                //spring 
                else if (nombremapa == "Spring")
                {
                    //programacion del mapa
                    File.Delete(targetfile);
                    File.Copy(sourcefile + "spring.vpk", targetfile);
                    //MessageBox.Show("El mapa " + nombremapa + " se ha aplicado correctamente");
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Mapa";
                    string Themes = Thema.Text;
                    string Mapa = mapaBox.Text;
                    Mensage.theme = Themes;
                    Mensage.mapa = Mapa;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                }

                //winter 
                else if (nombremapa == "Winter")
                {
                    //programacion del mapa
                    File.Delete(targetfile);
                    File.Copy(sourcefile + "winter.vpk", targetfile);
                    //MessageBox.Show("El mapa " + nombremapa + " se ha aplicado correctamente");
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Mapa";
                    string Themes = Thema.Text;
                    string Mapa = mapaBox.Text;
                    Mensage.theme = Themes;
                    Mensage.mapa = Mapa;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                }
                    }
                catch 
                {
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "ErrorMaps";
                    string Themes = Thema.Text;
                    Mensage.theme = Themes;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                }

            }
        }

        private void mapaBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nombremapa = mapaBox.Text;
            //Immortal Gardens
            if (nombremapa == "Immortal Gardens")
            {
                pictureMapBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureMapBox.Image = Resources.immortalgarden;
            }

                //The King's New Journey
            else if (nombremapa == "The King's New Journey")
            {
                pictureMapBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureMapBox.Image = Resources.kingsjourney;
            }
            //Default
            else if (nombremapa == "Default")
            {
                pictureMapBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureMapBox.Image = Properties.Resources.defaul;
            }
            //Desert
            else if (nombremapa == "Desert")
            {
                pictureMapBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureMapBox.Image = Properties.Resources.desert;
            }
            //Reef's Edge
            else if (nombremapa == "Reef's Edge")
            {
                pictureMapBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureMapBox.Image = Resources.reefsedge;
            }
            //Spring
            else if (nombremapa == "Spring")
            {
                pictureMapBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureMapBox.Image = Resources.spring;
            }
            //Winter
            else if (nombremapa == "Winter")
            {
                pictureMapBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureMapBox.Image = Resources.winter;
            }
            //Autumn
            else if (nombremapa == "Autumn")
            {
                pictureMapBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureMapBox.Image = Resources.autumn;
            }

        }

        private void Aplicmap_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(clientloctxt.Text))
            {
                Message Mensage = new Message(); //creamos un objeto del formulario 2
                Mensage._Configurar = "Cliente";
                string Themes = Thema.Text;
                Mensage.theme = Themes;
                string Idioma = LanguajeBox.Text;
                Mensage.Idioma = Idioma;
                DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                if (res == DialogResult.OK)
                {
                    clienlocBtn.PerformClick();
                    AplicarMapa();
                }
                if (res == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                AplicarMapa();
            }

        }


            

            

        /*
         * 
        */
        private void SelectMapaDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog clientloc_patch = new FolderBrowserDialog();
            clientloc_patch.ShowDialog();
        }


        #endregion
        private void BtnHome_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage1);
        }
        private void UserBtn_Click(object sender, EventArgs e)
        {
            dataGrid.MouseWheel += new MouseEventHandler(dataGrid_MouseWheel);
            mainTabs.SelectTab(tabPage2);
            clean();
            ComprobarDLL();

            if (AdminUsers.Text == "Administrar" || AdminUsers.Text == "Manage")
            {
                dataGrid.Hide();
            }
            SaveSettings.SaveAllSettings(Application.ProductName, this);

        }
        //Vínculo de los botones




        private void generarPatch()
        { //Generar la direccion de la App
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TINserverlauncher");
            key.SetValue("Patch", Environment.CurrentDirectory);
            key.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage3);
            tabPage3.MouseWheel += new MouseEventHandler(tabPage_MouseWheel);
            tabPage3_2.MouseWheel += new MouseEventHandler(tabPage_MouseWheel);
            UpdateSteam();
            INIeditorID.Text = "";
            SaveSettings.SaveAllSettings(Application.ProductName, this);
        }

        private void tabPage_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta.Equals(120))
                mainTabs.SelectTab(tabPage3_2);

            else if (!e.Delta.Equals(120))
                mainTabs.SelectTab(tabPage3);
        }


        private void ConfigBtn_Click(object sender, EventArgs e)
        {
            Display display = new Display();

            foreach (var r in display.GetResolutions())
            {
                ResolBox.Items.Add(r.Width + " x " + r.Height);
            }
            ResolBox.Items.Add("1920 x 1080");

            mainTabs.SelectTab(tabPage4);
            //Audio
            shopA = new System.Media.SoundPlayer(Resources.shop_available);
            shopA.Play();
            cursor();
            SaveSettings.SaveAllSettings(Application.ProductName, this);

        }
        private void MainDark_MouseMove(object sender, MouseEventArgs e)
        {
            SaveSettings.SaveAllSettings(Application.ProductName, this);
        }
        private void clientlocbtn_Click(object sender, EventArgs e)
        {
            clienlocBtn.PerformClick();
        }
        private void serverlocBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog serverloc_patch = new FolderBrowserDialog();
            serverloc_patch.ShowDialog();
            serverloctxt.Text = serverloc_patch.SelectedPath;
            serverloctxt2.Text = serverloc_patch.SelectedPath;

        }
        private void button4_Click(object sender, EventArgs e)
        {
            btnGuardar.PerformClick();
        }
         /// <summary>
         /// //////
         /// </summary>





        /// <summary>
        /// ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region   Iniciar Steam... 

        private void TerminarProceso()
            {
            try 
                {
                foreach (Process proceso in Process.GetProcesses())
                    {
                        if (proceso.ProcessName == "Steam")
                        {
                            proceso.Kill();
                        }
                        if (proceso.ProcessName == "Steam Client Bootstrapper (32 bits)")
                        {
                            proceso.Kill();
                        }
                        if (proceso.ProcessName == "Steam Client WebHelper (32 bits)")
                        {
                            proceso.Kill();
                        }
                        if (proceso.ProcessName == "Steam Client Bootstrapper (64 bits)")
                        {
                            proceso.Kill();
                        }
                        if (proceso.ProcessName == "Steam Client WebHelper (64 bits)")
                        {
                            proceso.Kill();
                        }
                        if (proceso.ProcessName == "steamerrorreporter")
                        {
                            proceso.Kill();
                        }
                        if (proceso.ProcessName == "GameOverlayUI")
                        {
                            proceso.Kill();
                        }
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }
            }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //ver si TINserver esta abierto
            bool TINOpen = false;
            Process[] proceso = Process.GetProcesses();
            foreach (Process prc in proceso)
            {
                if (prc.ProcessName == "TINserver")
                {
                    TINOpen = true;
                }
            }


            TerminarProceso();
            SaveSettings.SaveAllSettings(Application.ProductName, this);
            //Guardar IP en la DB
            GuardarIp();

            Serverloc = serverloctxt.Text;
            Clientdir = clientloctxt.Text;

            if (remoteserver.Checked) //Servidor remoto
            {
                //ClientDir empty
                if (string.IsNullOrEmpty(clientloctxt.Text))
                {
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Cliente";
                    string Themes = Thema.Text;
                    Mensage.theme = Themes;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                    if (res == DialogResult.OK)
                    {
                        clientlbtn.PerformClick();
                        InsertarIP();
                        TINserverclient();
                        SaveSettings.SaveAllSettings(Application.ProductName, this);
                    }
                    if (res == DialogResult.Cancel)
                    {
                        return;
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(txtip.Text))
                        {
                            //MessageBox.Show("Debes Poner el número IP del servidor","");
                            Message Mensage = new Message(); //creamos un objeto del formulario 2
                            Mensage._Configurar = "ServerIP";
                            string Themes = Thema.Text;
                            Mensage.theme = Themes;
                            string Idioma = LanguajeBox.Text;
                            Mensage.Idioma = Idioma;
                            DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                        }
                    else
                        {
                        InsertarIP();
                        AllowSkypUpdate();
                        TINserverclient();
                        SaveSettings.SaveAllSettings(Application.ProductName, this);
                        }
                }
            }

            if (localserver.Checked) //Servidor local
            {

                if (string.IsNullOrEmpty(clientloctxt.Text))
                {
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Cliente";
                    string Themes = Thema.Text;
                    Mensage.theme = Themes;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                    if (res == DialogResult.OK)
                    {
                        clientlbtn.PerformClick();
                        SaveSettings.SaveAllSettings(Application.ProductName, this);
                    }
                    if (res == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                if (string.IsNullOrEmpty(serverloctxt.Text))
                {
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Servidor";
                    string Themes = Thema.Text;
                    Mensage.theme = Themes;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                    if (res == DialogResult.OK)
                    {
                        serverlBtn.PerformClick();
                        SaveSettings.SaveAllSettings(Application.ProductName, this);
                    }
                    if (res == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                InsertarIP();
                TINserverclient();
                SaveSettings.SaveAllSettings(Application.ProductName, this);
                if (GCExterno.Checked == false)
                {
                    if (TINOpen)
                        return;
                    
                    if (TINserverO.Checked == true)
                    {
                        TINserverOculto();
                    }
                    else if (TINserverO.Checked == false)
                    {
                        TINserver();
                    }
                }
                else if (GCExterno.Checked == true)
                {
                    Serverloc = serverloctxt.Text;
                    ProcessStartInfo Server = new ProcessStartInfo();
                    Server.FileName = GCfilename.Text;
                    Server.WorkingDirectory = Serverloc;
                    Server.WindowStyle = ProcessWindowStyle.Minimized;
                    Process.Start(Server);
                }

            }
        }


        public void TINserver()
        {
            RegistryKey TINserver = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
            TINserver.GetValue("serverloctxt", RegistryValueKind.String);
            string TINserverloc = (string)TINserver.GetValue("serverloctxt");

        if (!string.IsNullOrEmpty(TINserverloc))
            {
            try
                {
                    //Iniciar Server
                    ProcessStartInfo Server = new ProcessStartInfo();
                    Server.FileName = "TINserver.exe";
                    Server.WorkingDirectory = TINserverloc;
                    Server.WindowStyle = ProcessWindowStyle.Minimized;
                    Process.Start(Server);
                    CkeckServer.Enabled = true;
                    CkeckServer.Start();
                    ServerTimer.Enabled = true;
                    ServerTimer.Start();
                    
                }
                catch (Exception exc) { Crearlog(exc.Message); }
            }
            else { return; }
        }
        public void TINserverOculto()
        {
            RegistryKey TINserver = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
            TINserver.GetValue("serverloctxt", RegistryValueKind.String);
            string TINserverloc = (string)TINserver.GetValue("serverloctxt");

            if (!string.IsNullOrEmpty(TINserverloc))
            {
                try
                {
                    //Iniciar Server Oculto
                    Process ServerO = new Process();
                    ServerO.StartInfo.WorkingDirectory = TINserverloc;
                    ServerO.StartInfo.UseShellExecute = true;
                    ServerO.StartInfo.FileName = "TINserver.exe";
                    ServerO.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    ServerO.Start();
                    CkeckServer.Enabled = true;
                    CkeckServer.Start();
                    ServerTimer.Enabled = true;
                    ServerTimer.Start();
                }
                catch (Exception exc) { Crearlog(exc.Message); }
            }
            else { return; }
        }

        public void TINserverclient()
        {
            RegistryKey TINserverC = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
            TINserverC.GetValue("clientloctxt", RegistryValueKind.String);
            string Steamloc = (string)TINserverC.GetValue("clientloctxt");

            if (!string.IsNullOrEmpty(Steamloc))
            {
                try
                {
                    //Generar la direccion de la App
                    Process client = new Process();
                    client.StartInfo.WorkingDirectory = Steamloc;
                    client.StartInfo.FileName = "TINserverclient.exe";
                    client.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    client.Start();
                } catch (Exception exc) { Crearlog(exc.Message); }
            }
            else { return; } 
        }
        public void InsertarIP()
        {
            Serverloc = serverloctxt.Text;
            Clientdir = clientloctxt.Text;
            try
            {
            if (txtip.Enabled == false)
            {
                
                File.WriteAllLines(Clientdir + @"/TINserverClient.ini",
                             File.ReadAllLines(Clientdir + @"/TINserverClient.ini")
                                 .Select(x =>
                                 {
                                          if (x.StartsWith("serverIp")) return "serverIp = 127.0.0.1";
                                     else if (x.StartsWith("serverip")) return "serverIp = 127.0.0.1";
                                     else if (x.StartsWith("client-download.steampowered.com")) return "client-download.steampowered.com = 127.0.0.1";
                                     else if (x.StartsWith("media.steampowered.com")) return "media.steampowered.com = 127.0.0.1";
                                     else if (x.StartsWith("api.steampowered.com")) return "api.steampowered.com = 127.0.0.1";
                                     else if (x.StartsWith("steamcommunity-a.akamaihd.net")) return "steamcommunity-a.akamaihd.net = 127.0.0.1";
                                     else if (x.StartsWith("cdn.akamai.steamstatic.com")) return "cdn.akamai.steamstatic.com = 127.0.0.1";
                                     return x;
                                 }));


            }
            else if (remoteserver.Checked)
            {

                File.WriteAllLines(Clientdir + @"/TINserverClient.ini",
                         File.ReadAllLines(Clientdir + @"/TINserverClient.ini")
                             .Select(x =>
                             {
                                      if (x.StartsWith("serverIp")) return "serverIp = " + txtip.Text;
                                 else if (x.StartsWith("serverip")) return "serverIp = " + txtip.Text;
                                 else if (x.StartsWith("client-download.steampowered.com")) return "client-download.steampowered.com = " + txtip.Text;
                                 else if (x.StartsWith("media.steampowered.com")) return "media.steampowered.com = " + txtip.Text;
                                 else if (x.StartsWith("api.steampowered.com")) return "api.steampowered.com = " + txtip.Text;
                                 else if (x.StartsWith("steamcommunity-a.akamaihd.net")) return "steamcommunity-a.akamaihd.net = " + txtip.Text;
                                 else if (x.StartsWith("cdn.akamai.steamstatic.com")) return "cdn.akamai.steamstatic.com = " + txtip.Text;
                                 return x;

                             }));
            }
            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }

        #endregion

        public void Direccionclient(string Nickname)
        {
            Clientdir = clientloctxt.Text;
            string ruta = Clientdir;

        }

        private void localserver_CheckedChanged(object sender, EventArgs e)
        {
            txtip.Enabled = false;
            IpBox.Enabled = false;
        }

        private void remoteserver_CheckedChanged(object sender, EventArgs e)
        {
            txtip.Enabled = true;
            IpBox.Enabled = true;
            TINserverO.Checked = false;
            GCExterno.Checked = false;
            GCfilename.Hide();
            SearchServerGC.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage3);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage3);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage3);
        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_DoubleClick(object sender, EventArgs e)
        {
            string Acceso = Environment.UserName.ToString();
            if (Acceso == "Hackerprod")
            {
                TerminarProceso();
            }
        }


        private void ActivarOpciones()
        {
            FontBack1.Show();
            FontBack1.Show();
            Cerrarlauncher.Show();
            Minimlauncher.Show();
            MinimBlauncher.Show();
            mantenerA.Show();
            TINserverO.Show();
            GCExterno.Show();

            if (dotaMenu.Checked == true)
            {
                opendotaBox.Show();
                FontBack9.Show();
            }
            if (GCExterno.Checked == true)
            {
                GCfilename.Show();
                SearchServerGC.Show();
            }
            if (GCExterno.Checked == false)
            {
                GCfilename.Hide();
                SearchServerGC.Hide();
            }
            if (dotaMenu.Checked == false)
            {
                FontBack9.Hide();
            }


        }
        private void DesactivarOpciones()
        {
            FontBack1.Hide();
            Cerrarlauncher.Hide();
            Minimlauncher.Hide();
            MinimBlauncher.Hide();
            mantenerA.Hide();
            opendotaBox.Hide();
            TINserverO.Hide();
            GCExterno.Hide();
            GCfilename.Hide();
            SearchServerGC.Hide();
            FontBack9.Hide();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CloseBar.Checked == true)
            {
                MinToTry();
            }
            else
            {
                Application.Exit();
            }
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            if (CloseBar.Checked == true)
            {
                MinToTryBar();
            }
            else if (CloseBar.Checked == false)
            {
                Application.Exit();
            }
        }

        private void CerrarBox_MouseLeave(object sender, EventArgs e)
        {
            //Oscuro
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                CerrarBox.Image = Resources.CerrarOscuro_1;
            }
            //Normal
            if (BackColor == Color.FromArgb(41, 44, 51))
            {
                CerrarBox.Image = Resources.CerrarNormal_1;
            }
            //Claro
            if (BackColor == Color.FromArgb(240, 243, 248))
            {
                CerrarBox.Image = Resources.CerrarClaro_1;
            }
            }
            catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void CerrarBox_MouseMove(object sender, MouseEventArgs e)
        {
            //Oscuro
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                CerrarBox.Image = Resources.CerrarOscuro_2;
            }
            //Normal
            if (BackColor == Color.FromArgb(41, 44, 51))
            {
                CerrarBox.Image = Resources.CerrarNormal_2;
            }
            //Claro
            if (BackColor == Color.FromArgb(240, 243, 248))
            {
                CerrarBox.Image = Resources.CerrarClaro_2;
            }
            }
            catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void opendotaBox_CheckedChanged(object sender, EventArgs e)
        {
           Clientdir = clientloctxt.Text;
            if (opendotaBox.Checked == true)
            {
                string texto = File.ReadAllText(Clientdir + @"/TINserverClient.ini");
                if (texto.Contains("-applaunch 570") == false)
                {
                    File.WriteAllLines(Clientdir + @"/TINserverClient.ini",
                    File.ReadAllLines(Clientdir + @"/TINserverClient.ini")
                    .Select(x =>
                    {
                        if (x.StartsWith("commandLine")) return x + " -applaunch 570";
                        return x;
                    }));
                }
            }
            else if (opendotaBox.Checked == false)
            {
                string texto = File.ReadAllText(Clientdir + @"/TINserverClient.ini");
                string reemplazado = texto.Replace(" -applaunch 570", "");
                File.WriteAllText(Clientdir + @"/TINserverClient.ini", reemplazado);
            }

        }

        private void AboutBtn_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage7);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://www.facebook.com/Hackerprodlive");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://www.twitter.com/hackerprod");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://www.taringa.net/DLHprod");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"mailto:Hackerprodlive@gmail.com?subject=Hello");
        }

        
        /// <summary>
        /// Agregar Usuarios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public bool UserVerified()
        {
            if (string.IsNullOrEmpty(txtNick.Text))
            {
                Message Mensage = new Message(); //creamos un objeto del formulario 2
                Mensage._Configurar = "UserErr";
                string Themes = Thema.Text;
                Mensage.theme = Themes;
                string Idioma = LanguajeBox.Text;
                Mensage.Idioma = Idioma;
                DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                return false;
            }
            if (string.IsNullOrEmpty(txtPass.Text))
            {
                Message Mensage = new Message(); //creamos un objeto del formulario 2
                Mensage._Configurar = "PassErr";
                string Themes = Thema.Text;
                Mensage.theme = Themes;
                string Idioma = LanguajeBox.Text;
                Mensage.Idioma = Idioma;
                DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                return false;
            }
            return true;
        }
        private void AgregarBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(serverloctxt.Text))
            {
                Message Mensage = new Message(); //creamos un objeto del formulario 2
                Mensage._Configurar = "Servidor";
                string Themes = Thema.Text;
                Mensage.theme = Themes;
                string Idioma = LanguajeBox.Text;
                Mensage.Idioma = Idioma;
                DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                if (res == DialogResult.OK)
                {
                    serverlocBtn.PerformClick();
                }
                if (res == DialogResult.Cancel)
                {
                    return;
                }
            }
            else                            // backworker para DotaDir launcher
            {
                if (NewuserAmbos.Checked)
                {
                if (UserVerified())
                {
                    try
                    {   //INI
                        _Serverdir = serverloctxt.Text;
                        string Line = txtNick.Text + " = " + txtPass.Text;
                        string text = File.ReadAllText(_Serverdir + @"\Tinserver.users.ini");
                        if (text.Contains(txtNick.Text + " ="))
                            { }
                        else if (text.Contains(txtNick.Text + "="))
                            { }
                        else
                        {
                                string user = File.ReadAllText(_Serverdir + @"\Tinserver.users.ini");
                                if (user.Contains("; Accounts"))
                                {
                                    File.WriteAllLines(_Serverdir + @"\Tinserver.users.ini",
                                    File.ReadAllLines(_Serverdir + @"\Tinserver.users.ini")
                                    .Select(x =>
                                    {
                                        if (x.StartsWith("; Accounts")) return x + Environment.NewLine + Line;
                                        return x;
                                    }));
                                }
                                else if (user.Contains("[TINserver/accounts]"))
                                {
                                    File.WriteAllLines(_Serverdir + @"\Tinserver.users.ini",
                                    File.ReadAllLines(_Serverdir + @"\Tinserver.users.ini")
                                    .Select(x =>
                                    {
                                        if (x.StartsWith("[TINserver/accounts]")) return x + Environment.NewLine + Line;
                                        return x;
                                    }));
                                }
                            }                        //DB3
                        SQLiteConnection conn = null;
                        string dbPath = "Data Source=" + _Serverdir + @"\TINserver.users.db3;";
                        conn = new SQLiteConnection(dbPath);//Create database connection
                        conn.Open();//open database connection, if the data file not exist, it will create an empty one.
                        
                        SQLiteCommand cmdInsert = new SQLiteCommand(conn);
                        cmdInsert.CommandText = "INSERT INTO accounts VALUES('" + txtNick.Text + "', '" + txtPass.Text + "', 0)";
                        cmdInsert.ExecuteNonQuery();
                        conn.Close();

                        Message Mensage = new Message(); //creamos un objeto del formulario 2
                        Mensage._Configurar = "UserAdd";
                        string Themes = Thema.Text;
                        string nUsuario = txtNick.Text;
                        Mensage.Nusuario = nUsuario;
                        Mensage.theme = Themes;
                        string Idioma = LanguajeBox.Text;
                        Mensage.Idioma = Idioma;
                        DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                        clean();
                    }
                    catch 
                    {
                        //MessageBox.Show("No Pudimos agregar el usuario", "	Error de la aplicación");
                        Message Mensage = new Message(); //creamos un objeto del formulario 2
                        Mensage._Configurar = "UserError";
                        string Themes = Thema.Text;
                        string nUsuario = txtNick.Text;
                        Mensage.Nusuario = nUsuario;
                        Mensage.theme = Themes;
                        string Idioma = LanguajeBox.Text;
                        Mensage.Idioma = Idioma;
                        DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                        clean();
                    }
            }
            try
            {
                string SerDir = serverloctxt.Text;
                var UserS = File.ReadAllText(SerDir + @"\TINserver.users.ini");
                UserINIEdit.Text = UserS; CargarUsers(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
            else
            {
                if (txtuserINI.Checked)
                {
                    AgregarUsuarioINI();
                }

                else if (txtuserDB3.Checked)
                {
                    AgregarUsuarioDB3();
                }
            }
                //Actualizar lista de usuarios INI
            try{    string SerDir = serverloctxt.Text;
                    var UserS = File.ReadAllText(SerDir + @"\TINserver.users.ini");
                    UserINIEdit.Text = UserS;
                    CargarUsers(); }catch{}
            }
        }

        private void clean()
        {
            txtNick.Text = String.Empty;
            txtPass.Text = String.Empty;

        }
        private void AgregarUsuarioDB3()
        {
                if (UserVerified())
                {
                    try
                    {
                        _Serverdir = serverloctxt.Text;
                        SQLiteConnection conn = null;
                        string dbPath = "Data Source=" + _Serverdir + @"\TINserver.users.db3;";
                        conn = new SQLiteConnection(dbPath);//Create database connection
                        conn.Open();//open database connection, if the data file not exist, it will create an empty one.

                        SQLiteCommand cmdInsert = new SQLiteCommand(conn);
                        cmdInsert.CommandText = "INSERT INTO accounts VALUES('" + txtNick.Text + "', '" + txtPass.Text + "', 0)";
                        cmdInsert.ExecuteNonQuery();
                        conn.Close();

                        //Mensaje Usuario agregado
                        Message Mensage = new Message(); //creamos un objeto del formulario 2
                        Mensage._Configurar = "UserAdd";
                        string Themes = Thema.Text;
                        string nUsuario = txtNick.Text;
                        Mensage.Nusuario = nUsuario;
                        Mensage.theme = Themes;
                        string Idioma = LanguajeBox.Text;
                        Mensage.Idioma = Idioma;
                        DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                        clean();
                    }
                    catch
                    {
                        //MessageBox.Show("No Pudimos agregar el usuario", "	Error de la aplicación");
                        Message Mensage = new Message(); //creamos un objeto del formulario 2
                        Mensage._Configurar = "UserError";
                        string Themes = Thema.Text;
                        string nUsuario = txtNick.Text;
                        Mensage.Nusuario = nUsuario;
                        Mensage.theme = Themes;
                        string Idioma = LanguajeBox.Text;
                        Mensage.Idioma = Idioma;
                        DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                        clean();
                    }
            }
            try
            {
                CargarUsers();
            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void AgregarUsuarioINI()
        {
                if (UserVerified())
                {
                    //Mensaje Usuario agregado         
                    try
                    {
                        _Serverdir = serverloctxt.Text;
                        string Line = txtNick.Text + " = " + txtPass.Text;
                        string text = File.ReadAllText(_Serverdir + @"\Tinserver.users.ini");
                        if (text.Contains(txtNick.Text + " ="))
                        {
                            Message Mensage = new Message(); //creamos un objeto del formulario 2
                            Mensage._Configurar = "UserExist";
                            string nUsuario = txtNick.Text;
                            Mensage.Nusuario = nUsuario;
                            string Themes = Thema.Text;
                            Mensage.theme = Themes;
                            string Idioma = LanguajeBox.Text;
                            Mensage.Idioma = Idioma;
                            DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                            clean();
                        }
                        else if (text.Contains(txtNick.Text + "="))
                        {
                        Message Mensage = new Message(); //creamos un objeto del formulario 2
                        Mensage._Configurar = "UserExist";
                        string nUsuario = txtNick.Text;
                        Mensage.Nusuario = nUsuario;
                        string Themes = Thema.Text;
                        Mensage.theme = Themes;
                        string Idioma = LanguajeBox.Text;
                        Mensage.Idioma = Idioma;
                        DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                        clean();
                        }
                    else
                    {
                        string user = File.ReadAllText(_Serverdir + @"\Tinserver.users.ini");
                        if (user.Contains("; Accounts")) 
                        {
                            File.WriteAllLines(_Serverdir + @"\Tinserver.users.ini",
                            File.ReadAllLines(_Serverdir + @"\Tinserver.users.ini")
                            .Select(x =>
                            {
                                if (x.StartsWith("; Accounts")) return x + Environment.NewLine + Line;
                                return x;
                            }));
                        }
                        else if (user.Contains("[TINserver/accounts]"))
                        {
                            File.WriteAllLines(_Serverdir + @"\Tinserver.users.ini",
                            File.ReadAllLines(_Serverdir + @"\Tinserver.users.ini")
                            .Select(x =>
                            {
                                if (x.StartsWith("[TINserver/accounts]")) return x + Environment.NewLine + Line;
                                return x;
                            }));
                        }

                        //Mensaje usuario creado
                        Message Mensage = new Message(); 
                            Mensage._Configurar = "UserAdd";
                            string Themes = Thema.Text;
                            string nUsuario = txtNick.Text;
                            Mensage.Nusuario = nUsuario;
                            Mensage.theme = Themes;
                            string Idioma = LanguajeBox.Text;
                            Mensage.Idioma = Idioma;
                            DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                            clean();
                        }
                    }
                    catch
                    {
                        //MessageBox.Show("No Pudimos agregar el usuario", "	Error de la aplicación");
                        Message Mensage = new Message(); //creamos un objeto del formulario 2
                        Mensage._Configurar = "UserError";
                        string Themes = Thema.Text;
                        string nUsuario = txtNick.Text;
                        Mensage.theme = Themes;
                        string Idioma = LanguajeBox.Text;
                        Mensage.Idioma = Idioma;
                        Mensage.Nusuario = nUsuario;
                        DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal
                        clean();
                    }
            }
                
            try
            {
                string SerDir = serverloctxt.Text;
                var UserS = File.ReadAllText(SerDir + @"\TINserver.users.ini");
                UserINIEdit.Text = UserS; } catch (Exception exc) { Crearlog(exc.Message); }
        }

        //Administrar Usuarios
        private void AdminUsers_Click(object sender, EventArgs e)
        {
            //Si el server no esta configurado
             try
                {
            if (string.IsNullOrEmpty(serverloctxt.Text))
            {
                Message Mensage = new Message(); //creamos un objeto del formulario 2
                Mensage._Configurar = "Servidor";
                string Themes = Thema.Text;
                Mensage.theme = Themes;
                string Idioma = LanguajeBox.Text;
                Mensage.Idioma = Idioma;
                DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                if (res == DialogResult.OK)
                {
                    serverlocBtn.PerformClick();
                    AdminUsers.PerformClick();
                }
                if (res == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                _Serverdir = serverloctxt.Text;
                string useradmin = userBox.Text;
                if (useradmin == "TINserver.users.ini")
                {
                    if (AdminUsers.Text == "Administrar")
                    {
                    string SerDir = serverloctxt.Text;
                    var UserS = File.ReadAllText(SerDir + @"\TINserver.users.ini");
                    UserINIEdit.Show();
                    UserINIEdit.Text = UserS;
                    AdminUsers.Text = "Cerrar INI";
                    }
                    else if (AdminUsers.Text == "Cerrar INI")
                    {
                        //cerrar conexion
                        UserINIEdit.Hide();
                        AdminUsers.Text = "Administrar";
                    }
                    if (AdminUsers.Text == "Manage")
                    {
                        string SerDir = serverloctxt.Text;
                        var UserS = File.ReadAllText(SerDir + @"\TINserver.users.ini");
                        UserINIEdit.Show();
                        UserINIEdit.Text = UserS;
                        AdminUsers.Text = "Close INI";
                    }
                    else if (AdminUsers.Text == "Close INI")
                    {
                        //cerrar conexion
                        UserINIEdit.Hide();
                        AdminUsers.Text = "Manage";
                    }
                    else if (AdminUsers.Text == "Cerrar BD3")
                    {
                        //cerrar conexion
                        dataGrid.Hide();
                        AdminUsers.Text = "Administrar";
                    }
                    else if (AdminUsers.Text == "Close BD3")
                    {
                        //cerrar conexion
                        dataGrid.Hide();
                        AdminUsers.Text = "Manage";
                    }

                }


                else if (useradmin == "TINserver.users.db3")
                {
                    if (AdminUsers.Text == "Administrar")
                        {
                    CargarUsers();
                    dataGrid.Show();
                    dataGrid.Columns[0].Width = 88;
                    dataGrid.Columns[1].Width = 88;
                    dataGrid.Columns[2].Visible = false;
                    AdminUsers.Text = "Cerrar BD3";
                    //dataGrid.Columns(0).Width = 100;
                        }
                    else if (AdminUsers.Text == "Cerrar BD3")
                    {
                        //cerrar conexion
                        dataGrid.Hide();
                        AdminUsers.Text = "Administrar";
                    }
                    else if (AdminUsers.Text == "Manage")
                    {
                        CargarUsers();
                        dataGrid.Columns[0].Width = 88;
                        dataGrid.Columns[1].Width = 88;
                        dataGrid.Columns[2].Visible = false;
                        dataGrid.Show();
                        AdminUsers.Text = "Close BD3";
                     }
                    else if (AdminUsers.Text == "Close BD3")
                    {
                        //cerrar conexion
                        dataGrid.Hide();
                        AdminUsers.Text = "Manage";
                    }
                    else if (AdminUsers.Text == "Cerrar INI")
                    {
                        //cerrar conexion
                        UserINIEdit.Hide();
                        AdminUsers.Text = "Administrar";
                    }
                    else if (AdminUsers.Text == "Close INI")
                    {
                        //cerrar conexion
                        UserINIEdit.Hide();
                        AdminUsers.Text = "Manage";
                    }

                }
            }
        }
             catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void CargarUsers()
        {
            try{
                _Serverdir = serverloctxt.Text;
                SQLiteConnection ObjConnection = new SQLiteConnection("Data Source=" + _Serverdir + @"\TINserver.users.db3;");
                SQLiteCommand ObjCommand = new SQLiteCommand("SELECT * FROM ACCOUNTS", ObjConnection);
                ObjCommand.CommandType = CommandType.Text;
                SQLiteDataAdapter ObjDataAdapter = new SQLiteDataAdapter(ObjCommand);
                DataSet dataSet = new DataSet();
                ObjDataAdapter.Fill(dataSet, "accounts");
                dataGrid.DataSource = dataSet.Tables["accounts"];
                ObjConnection.Close(); 
                            
            } catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void GenerarDirectorio()
        {
            try
                {
                string patch = Environment.CurrentDirectory + @"\TINserverlauncher";
                    if (!Directory.Exists(MapaLauncher))
                    {
                    DirectoryInfo di = Directory.CreateDirectory(patch);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    }
                    DirectoryInfo dir = Directory.CreateDirectory(patch);
                    dir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }

        //Fin de los Usuarios //Usuarios
        private void clienlocBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog clientloc_patch = new FolderBrowserDialog();
            clientloc_patch.ShowDialog();
            clientloctxt.Text = clientloc_patch.SelectedPath;
            clientloctxt2.Text = clientloc_patch.SelectedPath;
        }


        private void TINupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(serverloctxt.Text))
                {
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Servidor";
                    string Themes = Thema.Text;
                    Mensage.theme = Themes;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                    if (res == DialogResult.OK)
                    {
                        serverlocBtn.PerformClick();
                        TINupdate.PerformClick();
                    }
                    if (res == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    _Serverdir = serverloctxt.Text;
                    ProcessStartInfo updateClient = new ProcessStartInfo();
                    updateClient.UseShellExecute = true;
                    updateClient.FileName = "_run_me_first.cmd";
                    updateClient.WorkingDirectory = _Serverdir;
                    Process.Start(updateClient);
                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void TINupdate_Click_1(object sender, EventArgs e)
        {
            try
                {
                    if (string.IsNullOrEmpty(serverloctxt.Text))
                    {
                        Message Mensage = new Message(); //creamos un objeto del formulario 2
                        Mensage._Configurar = "Servidor";
                        string Themes = Thema.Text;
                        Mensage.theme = Themes;
                        string Idioma = LanguajeBox.Text;
                        Mensage.Idioma = Idioma;
                        DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                        if (res == DialogResult.OK)
                        {
                            serverlocBtn.PerformClick();
                            TINupdate.PerformClick();
                        }
                        if (res == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                    else
                    {
                        _Serverdir = serverloctxt.Text;
                        ProcessStartInfo updateClient = new ProcessStartInfo();
                        updateClient.UseShellExecute = true;
                        updateClient.FileName = "_run_me_first.cmd";
                        updateClient.WorkingDirectory = _Serverdir;
                        Process.Start(updateClient);
                    }
                }
            catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            try
                {
                    if (string.IsNullOrEmpty(serverloctxt.Text))
            {
                Message Mensage = new Message(); //creamos un objeto del formulario 2
                Mensage._Configurar = "Servidor";
                string Themes = Thema.Text;
                Mensage.theme = Themes;
                string Idioma = LanguajeBox.Text;
                Mensage.Idioma = Idioma;
                DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                if (res == DialogResult.OK)
                {
                    serverlocBtn.PerformClick();
                    PICSupdate.PerformClick();
                }
                if (res == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {

                //Actualizar datos de Steam
                if (UserSteam.Text == "Usuario real de Steam" | PassSteam.Text == "Contraseña" | UserSteam.Text == "Real user of Steam" | PassSteam.Text == "Password" | string.IsNullOrEmpty(UserSteam.Text) | string.IsNullOrEmpty(PassSteam.Text))
                {
                    //MessageBox.Show("Debes configurar los datos de autentificación de Steam");
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "SteamErr";
                    string Themes = Thema.Text;
                    Mensage.theme = Themes;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                }
                else
                {
                    _Serverdir = serverloctxt.Text;
                    string PICSloc = _Serverdir + @"\tools\PICSdownloader\";
                    //PICSdownloader
                    File.WriteAllLines(PICSloc + "PICSdownloader.ini",
                         File.ReadAllLines(PICSloc + "PICSdownloader.ini")
                              .Select(x =>
                              {
                                  if (x.StartsWith("login")) return "login = " + UserSteam.Text;
                                  else if (x.StartsWith("Login")) return "login = " + UserSteam.Text;
                                  else if (x.StartsWith("password")) return "password = " + PassSteam.Text;
                                  else if (x.StartsWith("Password")) return "password = " + PassSteam.Text;
                                  else if (x.StartsWith("steamGuardKey")) return "steamGuardKey = " + SteamGuard.Text;
                                  else if (x.StartsWith("SteamGuardKey")) return "steamGuardKey = " + SteamGuard.Text;
                                  return x;
                              }));
                    //TINcft
                    string TINcft = _Serverdir + @"\tools\TINcft\";
                    File.WriteAllLines(TINcft + "TINcft.ini",
                    File.ReadAllLines(TINcft + "TINcft.ini")
                    .Select(x =>
                    {
                        if (x.StartsWith("username")) return "username = " + UserSteam.Text;
                        else if (x.StartsWith("Username")) return "username = " + UserSteam.Text;
                        else if (x.StartsWith("password")) return "password = " + PassSteam.Text;
                        else if (x.StartsWith("Password")) return "password = " + PassSteam.Text;
                        else if (x.StartsWith("steamGuardKey")) return "steamGuardKey = " + SteamGuard.Text;
                        else if (x.StartsWith("SteamGuardKey")) return "steamGuardKey = " + SteamGuard.Text;
                        return x;
                    }));

                    _Serverdir = serverloctxt.Text;
                    //Iniciar actualizacion
                    ProcessStartInfo updateAppInfo = new ProcessStartInfo();
                    updateAppInfo.UseShellExecute = true;
                    updateAppInfo.FileName = "updateAppInfo.cmd";
                    updateAppInfo.WorkingDirectory = _Serverdir;
                    Process.Start(updateAppInfo);
                    
                }
            }
        }
            catch (Exception exc) { Crearlog(exc.Message); }
    }

        private void ClientUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(serverloctxt.Text))
                {
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Servidor";
                    string Themes = Thema.Text;
                    Mensage.theme = Themes;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                    if (res == DialogResult.OK)
                    {
                        serverlocBtn.PerformClick();
                        ClientUpdate.PerformClick();
                    }
                    if (res == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    _Serverdir = serverloctxt.Text;
                    ProcessStartInfo updateClient = new ProcessStartInfo();
                    updateClient.UseShellExecute = true;
                    updateClient.FileName = "updateClient.cmd";
                    updateClient.WorkingDirectory = _Serverdir;
                    Process.Start(updateClient);
                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void EditServerINI_Click(object sender, EventArgs e)
        {
            try
                {
            if (string.IsNullOrEmpty(serverloctxt.Text))
            {
                Message Mensage = new Message(); //creamos un objeto del formulario 2
                Mensage._Configurar = "Servidor";
                string Themes = Thema.Text;
                Mensage.theme = Themes;
                string Idioma = LanguajeBox.Text;
                Mensage.Idioma = Idioma;
                DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                if (res == DialogResult.OK)
                {
                    serverlocBtn.PerformClick();
                    btnGuardar.PerformClick();
                }
                if (res == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                mainTabs.SelectTab(tabPage6);
                string SerDir = serverloctxt.Text;
                var UserS = File.ReadAllText(SerDir + @"\TINserver.ini");
                INIeditor.Clear();
                INIeditor.Text = UserS;
                INIeditorID.Text = "TINserver.ini";
            }
            }
            catch (Exception exc) { Crearlog(exc.Message); }
            
        }
        private void SetCancelButton(Button myCancelBtn)   
        {
            CancelButton = myCancelBtn;
        } 
        private void EditClientINI_Click(object sender, EventArgs e)
        {
            try
                {
            if (string.IsNullOrEmpty(clientloctxt.Text))
            {
                Message Mensage = new Message(); //creamos un objeto del formulario 2
                Mensage._Configurar = "Cliente";
                string Themes = Thema.Text;
                Mensage.theme = Themes;
                string Idioma = LanguajeBox.Text;
                Mensage.Idioma = Idioma;
                DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                if (res == DialogResult.OK)
                {
                    clienlocBtn.PerformClick();
                    EditClientINI.PerformClick();
                }
                if (res == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                _clientdir = clientloctxt.Text;
                mainTabs.SelectTab(tabPage6);
                var ClientINI = File.ReadAllText(_clientdir + @"\TINserverclient.ini");
                INIeditor.Text = ClientINI;
                INIeditorID.Text = "TINserverclient.ini";

            }
        }
            catch (Exception exc) { Crearlog(exc.Message); }
    }

        private void UserSteam_Click(object sender, EventArgs e)
        {
            if (UserSteam.Text == "Usuario real de Steam" | UserSteam.Text == "Real user of Steam")
                {
            UserSteam.Clear();
            UserSteam.ForeColor = Color.Black;
            UserSteam.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                }
        }

        private void PassSteam_Click(object sender, EventArgs e)
        {
            if (LanguajeBox.Text == "Español")
            {
                if (PassSteam.Text == "Contraseña")
                {
                    PassSteam.Clear();
                    PassSteam.ForeColor = Color.Black;
                    PassSteam.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);

                }
                else if (PassSteam.Text != "Contraseña")
                {
                    PassSteam.UseSystemPasswordChar = true;
                }
            }
            else if (LanguajeBox.Text == "English")
            {
                if (PassSteam.Text == "Password")
                {
                    PassSteam.Clear();
                    PassSteam.ForeColor = Color.Black;
                    PassSteam.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);

                }
                else if (PassSteam.Text != "Password")
                {
                    PassSteam.UseSystemPasswordChar = true;
                }
            }







        }

        private void SteamGuard_Click(object sender, EventArgs e)
        {
            if (LanguajeBox.Text == "Español")
            {
                if (SteamGuard.Text == "Clave de Steam Guard")
                {
                    SteamGuard.Clear();
                    SteamGuard.ForeColor = Color.Black;
                    SteamGuard.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    SteamGuard.UseSystemPasswordChar = false;
                }
            }
            else if (LanguajeBox.Text == "English")
            {
                if (SteamGuard.Text == "Steam Guard Key")
                {
                    SteamGuard.Clear();
                    SteamGuard.ForeColor = Color.Black;
                    SteamGuard.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    SteamGuard.UseSystemPasswordChar = false;
                }
            }









        }
        private void UpdateSteam()
        {   //UserSteam
            if (string.IsNullOrEmpty(UserSteam.Text))
                {
                    if (LanguajeBox.Text == "Español")
                    {
                        UserSteam.Text = "Usuario real de Steam";
                        UserSteam.Font = new Font("Verdana", 7.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
                        UserSteam.ForeColor = SystemColors.ControlDarkDark;
                    }
                    else if (LanguajeBox.Text == "English")
                    {
                        UserSteam.Text = "Real user of Steam";
                        UserSteam.Font = new Font("Verdana", 7.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
                        UserSteam.ForeColor = SystemColors.ControlDarkDark;
                    }
                }

            if (LanguajeBox.Text == "Español")
            {
                if (UserSteam.Text != "Usuario real de Steam")      
                {
                    UserSteam.ForeColor = Color.Black;
                    UserSteam.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                }
            }
            else if (LanguajeBox.Text == "English")
            {
                if (UserSteam.Text != "Real user of Steam")
                {
                    UserSteam.ForeColor = Color.Black;
                    UserSteam.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                }
            }





            //PassSteam
            if (string.IsNullOrEmpty(PassSteam.Text))
            {
                if (LanguajeBox.Text == "Español")
                {
                    PassSteam.Font = new Font("Verdana", 7.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
                    PassSteam.ForeColor = SystemColors.ControlDarkDark;
                    PassSteam.Text = "Contraseña";
                }
                else if (LanguajeBox.Text == "English")
                {
                    PassSteam.Font = new Font("Verdana", 7.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
                    PassSteam.ForeColor = SystemColors.ControlDarkDark;
                    PassSteam.Text = "Password";
                }


            }
            if (LanguajeBox.Text == "Español")
            {
                if (PassSteam.Text == "Contraseña")
                {
                    PassSteam.UseSystemPasswordChar = false;
                }
                if (PassSteam.Text != "Contraseña")
                {
                    PassSteam.ForeColor = Color.Black;
                    PassSteam.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    PassSteam.UseSystemPasswordChar = true;
                }
            }
            else if (LanguajeBox.Text == "English")
            {
                if (PassSteam.Text == "Password")
                {
                    PassSteam.UseSystemPasswordChar = false;
                }
                if (PassSteam.Text != "Password")
                {
                    PassSteam.ForeColor = Color.Black;
                    PassSteam.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    PassSteam.UseSystemPasswordChar = true;
                }
            }


            //SteamGuard
            if (string.IsNullOrEmpty(SteamGuard.Text))
            {
                if (LanguajeBox.Text == "Español")
                {
                    SteamGuard.Font = new Font("Verdana", 7.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
                    SteamGuard.ForeColor = SystemColors.ControlDarkDark;
                    SteamGuard.Text = "Clave de Steam Guard";
                }
                else if (LanguajeBox.Text == "English")
                {
                    SteamGuard.Font = new Font("Verdana", 7.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
                    SteamGuard.ForeColor = SystemColors.ControlDarkDark;
                    SteamGuard.Text = "Steam Guard Key";
                }
            }

            if (LanguajeBox.Text == "Español")
            {
                if (SteamGuard.Text == "Clave de Steam Guard")
                {
                    SteamGuard.UseSystemPasswordChar = false;
                }
                if (SteamGuard.Text != "Clave de Steam Guard")
                {
                    SteamGuard.ForeColor = Color.Black;
                    SteamGuard.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    SteamGuard.UseSystemPasswordChar = true;
                }
            }
            else if (LanguajeBox.Text == "English")
            {
                if (SteamGuard.Text == "Steam Guard Key")
                {
                    SteamGuard.UseSystemPasswordChar = false;
                }
                if (SteamGuard.Text != "Steam Guard Key")
                {
                    SteamGuard.ForeColor = Color.Black;
                    SteamGuard.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    SteamGuard.UseSystemPasswordChar = true;
                }
            }





        }

        private void PassSteam_TextChanged(object sender, EventArgs e)
        {
            if (LanguajeBox.Text == "Español")
            {
                if (PassSteam.Text != "Contraseña")
                {
                    PassSteam.UseSystemPasswordChar = true; 
                }
            }
            else if (LanguajeBox.Text == "English")
            {
                if (PassSteam.Text != "Password")
                {
                    PassSteam.UseSystemPasswordChar = true;
                }
            }
        }

        //Opciones de lanzamiento
        private void btnGuardar_MouseClick(object sender, MouseEventArgs e)
        {
            if (LaunchOption.Checked)
            {
            try
                {

            Serverloc = serverloctxt.Text;
            Clientdir = clientloctxt.Text;

            //Settings Cerrar Launcher
            if (Cerrarlauncher.Checked == true)
            {
                Application.Exit();
            }


            //Settings Minimizar Launcher
            RegistryKey Mlauncher = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
            Mlauncher.GetValue("Minimlauncher", RegistryValueKind.String);
            String minim = (String)Mlauncher.GetValue("Minimlauncher");
            if (minim != null)
            {
                if (minim.ToString() == "True")
                {
                    WindowState = FormWindowState.Minimized;
                }
            }

            //Settings Minimize to tray
            MinToTry();

            //Settings Users INI si local server es activado
            if (localserver.Checked)
            {
                SaveSettings.SaveAllSettings(Application.ProductName, this);
                RegistryKey Serverl = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
                Serverl.GetValue("serverloctxt", RegistryValueKind.String);
                String serverdir = (String)Serverl.GetValue("serverloctxt");
                //
                RegistryKey usersINI = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
                usersINI.GetValue("txtuserINI", RegistryValueKind.String);
                String userI = (String)usersINI.GetValue("txtuserINI");
                if (userI != null)
                {
                    if (userI.ToString() == "True")
                    {
                        File.WriteAllLines(serverdir + @"/TINserver.ini",
                                 File.ReadAllLines(serverdir + @"/TINserver.ini")
                                     .Select(x =>
                                     {
                                         if (x.StartsWith(";users=")) return ";users=TINserver.users.db3";
                                         else if (x.StartsWith("users=")) return "users=TINserver.users.ini";
                                         return x;
                                     }));
                    }
                }
            }

            //Settings Users DB3
            SaveSettings.SaveAllSettings(Application.ProductName, this);
            RegistryKey Serverlocation = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
            Serverlocation.GetValue("serverloctxt", RegistryValueKind.String);
            String servloc = (String)Serverlocation.GetValue("serverloctxt");
            //
            RegistryKey usersDB3 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
            usersDB3.GetValue("txtuserDB3", RegistryValueKind.String);
            String userD = (String)usersDB3.GetValue("txtuserDB3");
            if (userD != null)
            {
                if (userD.ToString() == "True")
                {
                    File.WriteAllLines(servloc + @"/TINserver.ini",
                             File.ReadAllLines(servloc + @"/TINserver.ini")
                                 .Select(x =>
                                 {
                                     if (x.StartsWith("users=")) return "users=TINserver.users.db3";
                                     else if (x.StartsWith(";users=")) return ";users=TINserver.users.ini";
                                     return x;
                                 }));
                }
            }
        }   catch (Exception exc) { Crearlog(exc.Message); }
    }
}

 

 //Opciones de lanzamiento

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            RegistryKey SecretS = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
            SecretS.GetValue("ShopSound", RegistryValueKind.String);
            String SecretShop = (String)SecretS.GetValue("ShopSound");
            if (SecretShop != null)
            {
                if (SecretShop.ToString() == "1")
                {
                    shop1 = new System.Media.SoundPlayer(Resources.shop1);
                    shop1.Play();
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TINserverlauncher");
                    key.SetValue("ShopSound", "2");
                    key.Close();
                }
                else if (SecretShop.ToString() == "2")
                {
                    shop2 = new System.Media.SoundPlayer(Resources.shop2);
                    shop2.Play();
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TINserverlauncher");
                    key.SetValue("ShopSound", "3");
                    key.Close();
                }
                else if (SecretShop.ToString() == "3")
                {
                    shop3 = new System.Media.SoundPlayer(Resources.shop3);
                    shop3.Play();
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TINserverlauncher");
                    key.SetValue("ShopSound", "4");
                    key.Close();
                }
                else if (SecretShop.ToString() == "4")
                {
                    shop4 = new System.Media.SoundPlayer(Resources.shop4);
                    shop4.Play();
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TINserverlauncher");
                    key.SetValue("ShopSound", "1");
                    key.Close();
                }
            }
        }

/// <summary>
/// Cursores
/// </summary>
        private void cursor()
        {
                        try
            {
                Icon a;
                a = Properties.Resources.cursor;
                tabPage4.Cursor = new Cursor(a.Handle);
                tabPage8_LastPlay.Cursor = new Cursor(a.Handle);

            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void MinimizeBox_MouseMove(object sender, MouseEventArgs e)
        {
            //Oscuro
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                MiniBox.Image = Resources.MinimizeOscuro_2;
            }
            //Normal
            if (BackColor == Color.FromArgb(41, 44, 51))
            {
                MiniBox.Image = Resources.MinimizeNormal_2;
            }
            //Claro
            if (BackColor == Color.FromArgb(240, 243, 248))
            {
                MiniBox.Image = Resources.MinimizeClaro_2;
            }
                    }
            catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void MinimizeBox_MouseLeave(object sender, EventArgs e)
        {
            //Oscuro
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                MiniBox.Image = Resources.MinimizeOscuro_1;
            }
            //Normal
            if (BackColor == Color.FromArgb(41, 44, 51))
            {
                MiniBox.Image = Resources.MinimizeNormal_1;
            }
            //Claro
            if (BackColor == Color.FromArgb(240, 243, 248))
            {
                MiniBox.Image = Resources.MinimizeClaro_1;
            }
            } catch (Exception exc) { Crearlog(exc.Message); }
                

        }

        private void MinimizeBox_Click(object sender, EventArgs e)
        {
            if (MinBar.Checked == true)
            {
                MinToTryBar();
            }
            else
            {
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = true;
                //
            }
        }


        //Mover ventana
        private void pictureBox10_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Location = new Point((Location.X - lastLocation.X) + e.X, (Location.Y - lastLocation.Y) + e.Y);
                Update();
                Opacity = 0.93;
            }
        }

        private void pictureBox10_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
            
        }

        private void pictureBox10_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            Opacity = 100;
        }


        #region   Idioma...
        private void LanguajeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Idiomas();
        }
        private void Idiomas()
        {
            string Idioma = LanguajeBox.Text;
            if (Idioma == "English")
                      {
                          AboutBtn.Text = "About";
                          labelTitle.Text = "TINserverLauncher";
                          Comunidadlabel.Text = "Communities";
                          TINrinRUlabel.Text = "TINserver in CS.RIN.RU";
                          TINtaringalabel.Text = "TINserver in taringa";
                          Creadolabel.Text = "Created by Hackerprod";
                          Idiomalbl.Text = "Language";
                          RealUserSteamlabel.Text = "Steam user";
                          EditClientINI.Text = "Edit";
                          EditServerINI.Text = "Edit";
                          ClientUpdate.Text = "Client";
                          EditClientlabel.Text = "Edit manually";
                          EditTINlabel.Text = "Edit manually";
                          Update2label.Text = "Update";
                          Updatelabel.Text = "Update";
                          Aplicmap.Text = "Apply";
                          SelectMapLabel.Text = "Select map";
                          AdminUsers.Text = "Manage";
                          AddUserlabel.Text = "Add new user";
                          ConfUser.Text = "Configure users";
                          btnGuardar.Text = "Start Steam";
                          NewuserAmbos.Text = "Insert in the two files";
                          AgregarBtn.Text = "Add user";
                          CreateUserLabel.Text = "When creating users:";
                          txtuserDB3.Text = "Through TINserver.users.db3";
                          PasswordLabel.Text = "Password";
                          txtuserINI.Text = "Through TINserver.users.ini";
                          Nickname.Text = "User name";
                          localserver.Text = "Local server";
                          remoteserver.Text = "Remote server";
                          mantenerA.Text = "Keep open";
                          opendotaBox.Text = "Open Dota 2";
                          MinimBlauncher.Text = "Minimize to tray";
                          Cerrarlauncher.Text = "Close the Launcher";
                          Minimlauncher.Text = "Minimize the Launcher";
                          LaunchOption.Text = "Use launch options";
                          IPutilizados.Text = "IP already used";
                          serverlBtn.Text = "Server location";
                          clientlbtn.Text = "Client location";
                          btnGuardar.Text = "Start Steam";
                          ConfigurarlauncherLabel.Text = "Configure client and server address";
                          clienlocBtn.Text = "Client location";
                          serverlocBtn.Text = "Server location";
                          //label10.Text = "IMPORT MAPS FROM DOTA TO LAUNCHER";
                          FontBack1.Text = "When Steam starts:";
                          IPlabel.Text = "IP recently used";
                          Apariencialabel.Text = "Change appearance:";
                          dotaMenu.Text = "Enable Dota 2 menu";

                          Wstart.Text = "When Windows starts";
                          WStartLauncher.Text = "Start TINserverlauncher";
                          WStartTINserver.Text = "Start TINserver";
                          WStartSteam.Text = "Start Steam client";
                          tryIcon.Text = "Try Icon";
                          MinBar.Text = "Minimize to try";
                          CloseBar.Text = "Close to try";
                          Borraralsalir.Text = "Clean when launcher closes";
                          BorrarTempC.Text = "Files generated by the client";
                          BorrarTempS.Text = "Files generated by the server";
                          Settings1.Text = "More settings >>";
                          Settings2.Text = "Back <<";
                          Settings1.Location = new Point(379, 299);
                          Settings2.Location = new Point(379, 299);


                if (UserSteam.Text == "Usuario real de Steam")
                          {
                              UserSteam.Text = "Real user of Steam";
                          }
                          if (PassSteam.Text == "Contraseña")
                          {
                              PassSteam.Text = "Password";
                          }
                          if (SteamGuard.Text == "Clave de Steam Guard")
                          {
                              SteamGuard.Text = "Steam Guard Key";
                          }
                          
                          //historialIP
                          if (historialiP.Text == "Activar")
                               {
                            historialiP.Text = "Enable";
                               }
                          if (historialiP.Text == "Desactivar")
                          {
                            historialiP.Text = "Disable";
                          }
                            historialiP.Items.Clear();
                            historialiP.Items.AddRange(new object[] {
                            "Enable",
                            "Disable"});
                            
                          //Thema
                          if (LanguajeBox.Text == "English")
                          {
                              if (Thema.Text == "Oscuro")
                              {
                                  Thema.Text = "Dark";
                              }
                              if (Thema.Text == "Normal")
                              {
                                  Thema.Text = "Normal";
                              }
                              if (Thema.Text == "Claro")
                              {
                                  Thema.Text = "Acua";
                              }
                          Thema.Items.Clear();
                          Thema.Items.AddRange(new object[] {
                            "Dark",
                            "Normal",
                            "Acua"});
                          }

                      }
            else if (Idioma == "Español")
                      {
                AboutBtn.Text = "Acerca";
                labelTitle.Text = "TINserverLauncher";
                tabPage7.Text = "About";
                Comunidadlabel.Text = "Comunidades";            
                TINrinRUlabel.Text = "TINserver en CS.RIN.RU";
                TINtaringalabel.Text = "TINserver en taringa";
                Creadolabel.Text = "Hecho por Hackerprod";
                Idiomalbl.Text = "Idioma";
                RealUserSteamlabel.Text = "Usuario de Steam";
                EditClientINI.Text = "Editar";
                EditServerINI.Text = "Editar";
                ClientUpdate.Text = "Cliente";
                EditClientlabel.Text = "Editar manualmente";
                EditTINlabel.Text = "Editar manualmente";
                Update2label.Text = "Actualizar";
                Updatelabel.Text = "Actualizar";
                Aplicmap.Text = "Aplicar";
                SelectMapLabel.Text = "Seleccionar mapa";
                AdminUsers.Text = "Administrar";
                AddUserlabel.Text = "Agregar usuario";
                ConfUser.Text = "Configurar usuarios";
                btnGuardar.Text = "Iniciar Steam";
                NewuserAmbos.Text = "Insertar en los dos ficheros";
                AgregarBtn.Text = "Agregar";
                CreateUserLabel.Text = "Al crear usuarios:";
                txtuserDB3.Text = "Mediante TINserver.users.db3";
                PasswordLabel.Text = "Contraseña";
                txtuserINI.Text = "Mediante TINserver.users.ini";
                Nickname.Text = "Nombre de usuario";
                localserver.Text = "Servidor local";
                remoteserver.Text = "Servidor remoto";
                mantenerA.Text = "Mantener abierto";
                opendotaBox.Text = "Abrir Dota 2";
                MinimBlauncher.Text = "Minimizar a la bandeja del Sistema";
                Cerrarlauncher.Text = "Cerrar el Launcher";
                Minimlauncher.Text = "Minimizar el Launcher";
                LaunchOption.Text = "Utilizar opciones de lanzamiento";
                IPutilizados.Text = "IP ya utilizados";
                serverlBtn.Text = "Ruta del servidor";
                clientlbtn.Text = "Ruta del cliente";
                btnGuardar.Text = "Iniciar Steam";
                ConfigurarlauncherLabel.Text = "Configurar dirección del Cliente y del Servidor";
                clienlocBtn.Text = "Ruta del cliente";
                serverlocBtn.Text = "Ruta del servidor";
                //label10.Text = "IMPORTAR MAPAS DE DOTA AL LAUNCHER";
                FontBack1.Text = "Al iniciar Steam:";
                IPlabel.Text = "IP usados recientemente";
                Apariencialabel.Text = "Cambiar apariencia:";
                dotaMenu.Text = "Activar menu de Dota 2";

                Wstart.Text = "Al iniciar Windows";
                WStartLauncher.Text = "Iniciar TINserverlauncher";
                WStartTINserver.Text = "Iniciar TINserver";
                WStartSteam.Text = "Iniciar cliente de Steam";
                tryIcon.Text = "Icono en la bandeja";
                MinBar.Text = "Minimizar en bandeja";
                CloseBar.Text = "Cerrar en bandeja";
                Borraralsalir.Text = "Borrar al cerrar el launcher";
                BorrarTempC.Text = "Ficheros generados por el cliente";
                BorrarTempS.Text = "Ficheros generados por el servidor";
                Settings1.Text = "Más configuraciones >>";
                Settings2.Text = "Atras <<";

                if (UserSteam.Text == "Real user of Steam")
                {
                    UserSteam.Text = "Usuario real de Steam";
                }
                if (PassSteam.Text == "Password")
                {
                    PassSteam.Text = "Contraseña";
                }
                if (SteamGuard.Text == "Steam Guard Key")
                {
                    SteamGuard.Text = "Clave de Steam Guard";
                }

                
                //historialIP
                if (historialiP.Text == "Enable")
                {
                    historialiP.Text = "Activar";
                }
                if (historialiP.Text == "Disable")
                {
                    historialiP.Text = "Desactivar";
                }
                historialiP.Items.Clear();
                historialiP.Items.AddRange(new object[] {
                            "Activar",
                            "Desactivar"});
                      }
                      
            //Thema
            if (LanguajeBox.Text == "Español")
            {
                if (Thema.Text == "Dark")
                {
                    Thema.Text = "Oscuro";
                }
                if (Thema.Text == "Normal")
                {
                    Thema.Text = "Normal";
                }
                if (Thema.Text == "Acua")
                {
                    Thema.Text = "Claro";
                }
                Thema.Items.Clear();
                Thema.Items.AddRange(new object[] {
                            "Oscuro",
                            "Normal",
                            "Claro"});
            }
        }
    

        #endregion

        private void taringadir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://www.taringa.net/posts/juegos/20132372/Crear-un-servidor-dedicado-de-Steam-mediante-TINserver.html");
        }

        private void RinRuDir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://cs.rin.ru/forum/viewtopic.php?f=29&t=66246");
        }

        private void labelTitle_Click(object sender, EventArgs e)
        {

        string Acceso = Environment.UserName.ToString();
            //if (Acceso == "Hackerprod")
            //{
            //if (SteamName.Text != "SteamName")
            //{
            //    mainTabs.SelectTab(tabPage8_Perfil);
            //}
            //else
            //{
            var tab = new TabPage();
            tab.Controls.Add(new LoadingFile(Thema.Text));

            mainTabs.TabPages.Add(tab);
            mainTabs.SelectTab(tab);
            Stats();
                /*
            var task = Task.Factory.StartNew(() => Stats());

            task..ContinueWith(
            t =>
            {
                if (SteamName.Text != "SteamName")
                    mainTabs.SelectTab(tabPage8_Perfil);
            },
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext());
            */
            //}
            //}

        }



        private void IpBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PingPicture.Hide();

            if (!string.IsNullOrEmpty(IpBox.Text))
            {
                txtip.Text = IpBox.Text;
            }

            


            ///////////////////////////////////////////
            try
            {
                PingIP = IpBox.Text;
                PingWorker.RunWorkerAsync(PingIP);

            }
            catch { PingPicture.Hide(); }
            ///////////////////////////////////////////

        }
        private void btn_ping_Click(object sender, EventArgs e) 
        { 
        } 






















        private void historialIP_SelectedIndexChanged(object sender, EventArgs e)
        {
            HistorialIP();
        }
        private void HistorialIP()
        {
            try
                {
            string Historial = historialiP.Text;
            if (Historial == "Activar" | Historial == "Enable")
            {
                IPutilizados.Show();
                IpBox.Show();
            }
            else if (Historial == "Desactivar" | Historial == "Disable")
            {
                IPutilizados.Hide();
                IpBox.Hide();
            }
                }
            catch{}
        }
        private void GuardarIp()  
        {
            if (!IpBox.Items.Contains(txtip.Text))
                { 
                    IpBox.Items.Add(txtip.Text);
                    cargarIPinMain();

                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TINserverlauncher");
                    key.SetValue("SavedIP", RegistryIP.Lines, RegistryValueKind.MultiString);
                }   
        }


        #region   Themas...
        public void AplicarThema()
            {
                theme = Thema.Text;
                if (theme == "Oscuro" | theme == "Dark")
                {
                    try
                       {
                    //Icon = Resources.TINserver_Dark;

                    BackColor = Color.FromArgb(28, 29, 33);
                    ForeColor = Color.FromArgb(147, 157, 160);
                    FontBack1.Show();
                    FontBack2.Show();
                    FontBack3.Show();
                    FontBack4.Show();
                    FontBack5.Show();
                    FontBack6.Show();
                    FontBack7.Show();
                    FontBack8.Show();
                    if (dotaMenu.Checked == true)
                    {
                        FontBack9.Show();
                    }
                    else
                    {
                        FontBack9.Hide();
                    }
                    FontBack10.Show();
                    FontBack11.Show();
                    FontBack1.BackColor = Color.FromArgb(20, 22, 22);
                    FontBack2.BackColor = Color.FromArgb(20, 22, 22);
                    FontBack3.BackColor = Color.FromArgb(20, 22, 22);
                    FontBack4.BackColor = Color.FromArgb(20, 22, 22);
                    FontBack5.BackColor = Color.FromArgb(20, 22, 22);
                    FontBack6.BackColor = Color.FromArgb(20, 22, 22);
                    FontBack7.BackColor = Color.FromArgb(20, 22, 22);
                    FontBack8.BackColor = Color.FromArgb(20, 22, 22);
                    FontBack9.BackColor = Color.FromArgb(20, 22, 22);
                    FontBack10.BackColor = Color.FromArgb(20, 22, 22);
                    FontBack11.BackColor = Color.FromArgb(20, 22, 22);
                    pictureBox10.BackColor = Color.FromArgb(28, 29, 33);
                    pictureBox6.BackColor = Color.FromArgb(28, 29, 33);
                    mainTabs.BackColor = Color.FromArgb(28, 29, 33);
                    tabPage1.BackColor = Color.FromArgb(28, 29, 33);
                    tabPage2.BackColor = Color.FromArgb(28, 29, 33);
                    tabPage4.BackColor = Color.FromArgb(28, 29, 33);
                    tabPage3.BackColor = Color.FromArgb(28, 29, 33);
                    tabPage6.BackColor = Color.FromArgb(28, 29, 33);
                    tabPage7.BackColor = Color.FromArgb(28, 29, 33);
                    tabPage3_2.BackColor = Color.FromArgb(28, 29, 33);
                    InicioColor.BackColor = Color.FromArgb(28, 29, 33);
                    UserPicture.BackColor = Color.FromArgb(28, 29, 33);
                    DotaPicture.BackColor = Color.FromArgb(28, 29, 33);
                    ConfigPicture.BackColor = Color.FromArgb(28, 29, 33);
                    pictureBox8.BackColor = Color.FromArgb(28, 29, 33);
//                    pictureBox12.BackColor = Color.FromArgb(28, 29, 33);
//                    pictureBox13.BackColor = Color.FromArgb(28, 29, 33);
                    pictureBox5.BackColor = Color.FromArgb(28, 29, 33);
//                    pictureBox11.BackColor = Color.FromArgb(28, 29, 33);
                    dataGrid.BackgroundColor = Color.FromArgb(28, 29, 33);
                    dataGrid.ForeColor = Color.FromArgb(28, 29, 33);
                    panel3.BackColor = Color.FromArgb(78, 169, 239);
                    pictureBox7.BackColor = Color.FromArgb(78, 169, 239);
                    tabPage3.ForeColor = Color.FromArgb(147, 157, 160);
                    tabPage4.ForeColor = Color.FromArgb(147, 157, 160);
                    tabPage2.ForeColor = Color.FromArgb(147, 157, 160);
                    tabPage1.ForeColor = Color.FromArgb(147, 157, 160);
                    tabPage7.ForeColor = Color.FromArgb(147, 157, 160);
                    tabPage3_2.ForeColor = Color.FromArgb(147, 157, 160);
                    Status.ForeColor = Color.FromArgb(147, 157, 160);
                    localserver.ForeColor = Color.FromArgb(147, 157, 160);
                    remoteserver.ForeColor = Color.FromArgb(147, 157, 160);
                    ConfigurarlauncherLabel.ForeColor = Color.FromArgb(147, 157, 160);
                    IPutilizados.ForeColor = Color.FromArgb(147, 157, 160);
                    AddUserlabel.ForeColor = Color.FromArgb(147, 157, 160);
                    Nickname.ForeColor = Color.FromArgb(147, 157, 160);
                    PasswordLabel.ForeColor = Color.FromArgb(147, 157, 160);
                    labelTitle.ForeColor = Color.FromArgb(147, 157, 160);
                    LaunchOption.FlatStyle = FlatStyle.Standard;
                    opendotaBox.FlatStyle = FlatStyle.Standard;
                    GCExterno.FlatStyle = FlatStyle.Standard;
                    NewuserAmbos.FlatStyle = FlatStyle.Standard;
                    CerrarBox.Image = Resources.CerrarOscuro_1;
                    MiniBox.Image = Resources.MinimizeOscuro_1;
                    Aplicmap.BackColor = Color.FromArgb(43, 47, 48);
                    Aplicmap.ForeColor = Color.FromArgb(147, 157, 160);
                    Aplicmap.FlatAppearance.BorderSize = 0;
                    AboutBtn.BackColor = Color.FromArgb(43, 47, 48);
                    AboutBtn.ForeColor = Color.FromArgb(147, 157, 160);
                    AboutBtn.FlatAppearance.BorderSize = 0;
                    clientlbtn.BackColor = Color.FromArgb(43, 47, 48);
                    clientlbtn.ForeColor = Color.FromArgb(147, 157, 160);
                    clientlbtn.FlatAppearance.BorderSize = 0;
                    serverlBtn.BackColor = Color.FromArgb(43, 47, 48);
                    serverlBtn.ForeColor = Color.FromArgb(147, 157, 160);
                    serverlBtn.FlatAppearance.BorderSize = 0;
                    SearchServerGC.BackColor = Color.FromArgb(43, 47, 48);
                    SearchServerGC.ForeColor = Color.FromArgb(147, 157, 160);
                    SearchServerGC.FlatAppearance.BorderSize = 0;
                    btnGuardar.BackColor = Color.FromArgb(43, 47, 48);
                    btnGuardar.ForeColor = Color.FromArgb(147, 157, 160);
                    btnGuardar.FlatAppearance.BorderSize = 0;
                    AgregarBtn.BackColor = Color.FromArgb(43, 47, 48);
                    AgregarBtn.ForeColor = Color.FromArgb(147, 157, 160);
                    AgregarBtn.FlatAppearance.BorderSize = 0;
                    AdminUsers.BackColor = Color.FromArgb(43, 47, 48);
                    AdminUsers.ForeColor = Color.FromArgb(147, 157, 160);
                    AdminUsers.FlatAppearance.BorderSize = 0;
                    EditServerINI.BackColor = Color.FromArgb(43, 47, 48);
                    EditServerINI.ForeColor = Color.FromArgb(147, 157, 160);
                    EditServerINI.FlatAppearance.BorderSize = 0;
                    TINupdate.BackColor = Color.FromArgb(43, 47, 48);
                    TINupdate.ForeColor = Color.FromArgb(147, 157, 160);
                    TINupdate.FlatAppearance.BorderSize = 0;
                    PICSupdate.BackColor = Color.FromArgb(43, 47, 48);
                    PICSupdate.ForeColor = Color.FromArgb(147, 157, 160);
                    PICSupdate.FlatAppearance.BorderSize = 0;
                    EditClientINI.BackColor = Color.FromArgb(43, 47, 48);
                    EditClientINI.ForeColor = Color.FromArgb(147, 157, 160);
                    EditClientINI.FlatAppearance.BorderSize = 0;
                    ClientUpdate.BackColor = Color.FromArgb(43, 47, 48);
                    ClientUpdate.ForeColor = Color.FromArgb(147, 157, 160);
                    ClientUpdate.FlatAppearance.BorderSize = 0;
                    localserver.ForeColor = Color.FromArgb(147, 157, 160);
                    LaunchOption.ForeColor = Color.FromArgb(147, 157, 160);
                    LaunchOption.BackColor = FontBack1.BackColor;
                    Cerrarlauncher.ForeColor = Color.FromArgb(147, 157, 160);
                    Cerrarlauncher.BackColor = FontBack3.BackColor;
                    Minimlauncher.ForeColor = Color.FromArgb(147, 157, 160);
                    Minimlauncher.BackColor = FontBack2.BackColor;
                    MinimBlauncher.ForeColor = tabPage1.ForeColor;
                    MinimBlauncher.BackColor = FontBack4.BackColor;
                    mantenerA.ForeColor = Color.FromArgb(147, 157, 160);
                    mantenerA.BackColor = FontBack5.BackColor;
                    opendotaBox.ForeColor = Color.FromArgb(147, 157, 160);
                    opendotaBox.BackColor = BackColor;
                    GCExterno.ForeColor = Color.FromArgb(147, 157, 160);
                    GCExterno.BackColor = BackColor;
                    txtuserINI.ForeColor = Color.FromArgb(147, 157, 160);
                    txtuserINI.BackColor = Color.FromArgb(20, 22, 22); 
                    txtuserDB3.ForeColor = Color.FromArgb(147, 157, 160);
                    txtuserDB3.BackColor = Color.FromArgb(20, 22, 22);
                    NewuserAmbos.ForeColor = Color.FromArgb(147, 157, 160);
                    NewuserAmbos.BackColor = Color.FromArgb(20, 22, 22);
                    Ayuda.ForeColor = ForeColor;
                    Ayuda.Text = "";
                    SelectMapLabel.ForeColor = ForeColor;
                    SelectMapLabel.Font = new Font("Radiance", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    Aplicmap.Font = new Font("Radiance", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    //label10.Font = new Font("Radiance", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    mapaBox.ForeColor = Color.FromArgb(0, 0, 0);
                    opendotaBox.BackColor = Color.FromArgb(20, 22, 22);
                    GCExterno.BackColor = Color.FromArgb(20, 22, 22);
                    TINserverO.ForeColor = tabPage1.ForeColor;
                    TINserverO.BackColor = FontBack10.BackColor;
                    Settings1.ForeColor = Color.FromArgb(78, 169, 239);
                    Settings2.ForeColor = Color.FromArgb(78, 169, 239);

                    DotaBtn.Image = Resources.Dota2;
                    if (LaunchOption.Checked == true)
                    {
                        FontBack1.Show();
                        FontBack2.Show();
                        FontBack3.Show();
                        FontBack4.Show();
                        FontBack5.Show();
                        FontBack6.Show();
                        FontBack7.Show();
                        FontBack8.Show();
                        if (dotaMenu.Checked == true)
                        {
                            FontBack9.Show();
                        }
                        else
                        {
                            FontBack9.Hide();
                        }
                        FontBack10.Show();
                        FontBack11.Show();
                    }
                    else
                    {
                        FontBack1.Hide();
                        FontBack2.Hide();
                        FontBack3.Hide();
                        FontBack4.Hide();
                        FontBack5.Hide();
                        FontBack6.Hide();
                        FontBack7.Hide();
                        FontBack8.Hide();
                        FontBack9.Hide();
                        FontBack10.Hide();
                        FontBack11.Hide();

                    }

                }
                catch (Exception exc) { Crearlog(exc.Message); }
                }
                if (theme == "Normal")
                {
                    try
                       {
                    //Icon = Resources.TINserver_Normal;

                    BackColor = Color.FromArgb(41, 44, 51);
                    ForeColor = Color.FromArgb(78, 169, 239);
                    FontBack1.Hide();
                    FontBack2.Hide();
                    FontBack3.Hide();
                    FontBack4.Hide();
                    FontBack5.Hide();
                    FontBack6.Hide();
                    FontBack7.Hide();
                    FontBack8.Hide();
                    FontBack9.Hide();
                    FontBack10.Hide();
                    FontBack11.Hide();
                    pictureBox10.BackColor = Color.FromArgb(41, 44, 51);
                    pictureBox6.BackColor = Color.FromArgb(41, 44, 51);
                    mainTabs.BackColor = Color.FromArgb(41, 44, 51);
                    tabPage1.BackColor = Color.FromArgb(41, 44, 51);
                    tabPage2.BackColor = Color.FromArgb(41, 44, 51);
                    tabPage4.BackColor = Color.FromArgb(41, 44, 51);
                    tabPage3.BackColor = Color.FromArgb(41, 44, 51);
                    tabPage6.BackColor = Color.FromArgb(41, 44, 51);
                    tabPage7.BackColor = Color.FromArgb(41, 44, 51);
                    tabPage3_2.BackColor = Color.FromArgb(41, 44, 51);
                    InicioColor.BackColor = Color.FromArgb(41, 44, 51);
                    UserPicture.BackColor = Color.FromArgb(41, 44, 51);
                    DotaPicture.BackColor = Color.FromArgb(41, 44, 51);
                    ConfigPicture.BackColor = Color.FromArgb(41, 44, 51);
                    pictureBox8.BackColor = Color.FromArgb(41, 44, 51);
//                   pictureBox12.BackColor = Color.FromArgb(41, 44, 51);
//                    pictureBox13.BackColor = Color.FromArgb(41, 44, 51);
//                    pictureBox11.BackColor = Color.FromArgb(41, 44, 51);
                    pictureBox5.BackColor = Color.FromArgb(41, 44, 51);
                    panel3.BackColor = Color.FromArgb(78, 169, 239);
                    pictureBox7.BackColor = Color.FromArgb(78, 169, 239);
                    dataGrid.BackgroundColor = Color.FromArgb(41, 44, 51);
                    tabPage3.ForeColor = Color.FromArgb(78, 169, 239);
                    tabPage4.ForeColor = Color.FromArgb(78, 169, 239);
                    tabPage2.ForeColor = Color.FromArgb(78, 169, 239);
                    tabPage1.ForeColor = Color.FromArgb(78, 169, 239);
                    tabPage7.ForeColor = Color.FromArgb(78, 169, 239);
                    tabPage3_2.ForeColor = Color.FromArgb(78, 169, 239);
                    Status.ForeColor = Color.FromArgb(78, 169, 239);
                    localserver.ForeColor = Color.FromArgb(78, 169, 239);
                    remoteserver.ForeColor = Color.FromArgb(78, 169, 239);
                    ConfigurarlauncherLabel.ForeColor = Color.FromArgb(78, 169, 239);
                    IPutilizados.ForeColor = Color.FromArgb(78, 169, 239);
                    AddUserlabel.ForeColor = Color.FromArgb(78, 169, 239);
                    Nickname.ForeColor = Color.FromArgb(78, 169, 239);
                    PasswordLabel.ForeColor = Color.FromArgb(78, 169, 239);
                    labelTitle.ForeColor = Color.FromArgb(78, 169, 239);
                    LaunchOption.ForeColor = Color.FromArgb(78, 169, 239);
                    LaunchOption.FlatStyle = FlatStyle.Standard;
                    opendotaBox.FlatStyle = FlatStyle.Standard;
                    GCExterno.FlatStyle = FlatStyle.Standard;
                    NewuserAmbos.FlatStyle = FlatStyle.Standard;
                    CerrarBox.Image = Resources.CerrarNormal_1;
                    MiniBox.Image = Resources.MinimizeNormal_1;
                    Aplicmap.BackColor = Color.FromArgb(41, 44, 51);
                    Aplicmap.ForeColor = Color.FromArgb(78, 169, 239);
                    Aplicmap.FlatAppearance.BorderSize = 1;
                    clientlbtn.BackColor = Color.FromArgb(41, 44, 51);
                    clientlbtn.ForeColor = Color.FromArgb(78, 169, 239);
                    clientlbtn.FlatAppearance.BorderSize = 1;
                    serverlBtn.BackColor = Color.FromArgb(41, 44, 51);
                    serverlBtn.ForeColor = Color.FromArgb(78, 169, 239);
                    serverlBtn.FlatAppearance.BorderSize = 1;
                    SearchServerGC.BackColor = Color.FromArgb(41, 44, 51);
                    SearchServerGC.ForeColor = Color.FromArgb(78, 169, 239);
                    SearchServerGC.FlatAppearance.BorderSize = 1;
                    btnGuardar.BackColor = Color.FromArgb(41, 44, 51);
                    btnGuardar.ForeColor = Color.FromArgb(78, 169, 239);
                    btnGuardar.FlatAppearance.BorderSize = 1;
                    AgregarBtn.BackColor = Color.FromArgb(41, 44, 51);
                    AgregarBtn.ForeColor = Color.FromArgb(78, 169, 239);
                    AgregarBtn.FlatAppearance.BorderSize = 1;
                    AdminUsers.BackColor = Color.FromArgb(41, 44, 51);
                    AdminUsers.ForeColor = Color.FromArgb(78, 169, 239);
                    AdminUsers.FlatAppearance.BorderSize = 1;
                    EditServerINI.BackColor = Color.FromArgb(41, 44, 51);
                    EditServerINI.ForeColor = Color.FromArgb(78, 169, 239);
                    EditServerINI.FlatAppearance.BorderSize = 1;
                    TINupdate.BackColor = Color.FromArgb(41, 44, 51);
                    TINupdate.ForeColor = Color.FromArgb(78, 169, 239);
                    TINupdate.FlatAppearance.BorderSize = 1;
                    PICSupdate.BackColor = Color.FromArgb(41, 44, 51);
                    PICSupdate.ForeColor = Color.FromArgb(78, 169, 239);
                    PICSupdate.FlatAppearance.BorderSize = 1;
                    EditClientINI.BackColor = Color.FromArgb(41, 44, 51);
                    EditClientINI.ForeColor = Color.FromArgb(78, 169, 239);
                    EditClientINI.FlatAppearance.BorderSize = 1;
                    ClientUpdate.BackColor = Color.FromArgb(41, 44, 51);
                    ClientUpdate.ForeColor = Color.FromArgb(78, 169, 239);
                    ClientUpdate.FlatAppearance.BorderSize = 1;
                    AboutBtn.BackColor = Color.FromArgb(41, 44, 51);
                    AboutBtn.ForeColor = Color.FromArgb(78, 169, 239);
                    AboutBtn.FlatAppearance.BorderSize = 1;
                    LaunchOption.BackColor = BackColor;
                    Cerrarlauncher.BackColor = BackColor;
                    Minimlauncher.BackColor = BackColor;
                    MinimBlauncher.BackColor = BackColor;
                    mantenerA.BackColor = BackColor;
                    opendotaBox.BackColor = BackColor;
                    GCExterno.BackColor = BackColor;
                    txtuserINI.BackColor = BackColor;
                    txtuserDB3.BackColor = BackColor;
                    NewuserAmbos.BackColor = BackColor;
                    NewuserAmbos.ForeColor = tabPage2.ForeColor;
                    txtuserINI.ForeColor = tabPage2.ForeColor;
                    txtuserDB3.ForeColor = tabPage2.ForeColor;
                    LaunchOption.ForeColor = tabPage1.ForeColor;
                    Cerrarlauncher.ForeColor = tabPage1.ForeColor;
                    Minimlauncher.ForeColor = tabPage1.ForeColor;
                    MinimBlauncher.ForeColor = tabPage1.ForeColor;
                    mantenerA.ForeColor = tabPage1.ForeColor;
                    opendotaBox.ForeColor = tabPage1.ForeColor;
                    GCExterno.ForeColor = tabPage1.ForeColor;
                    TINserverO.ForeColor = tabPage1.ForeColor;
                    TINserverO.BackColor = BackColor;
                    Ayuda.ForeColor = ForeColor;
                    Ayuda.Text = "";
                    SelectMapLabel.ForeColor = ForeColor;
                    SelectMapLabel.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    Aplicmap.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    mapaBox.ForeColor = Color.FromArgb(0, 0, 0);
                    DotaBtn.Image = Resources.Dota2;
                    Settings1.ForeColor = Color.FromArgb(78, 169, 239);
                    Settings2.ForeColor = Color.FromArgb(78, 169, 239);

                }
                catch (Exception exc) { Crearlog(exc.Message); }
                }
                if (theme == "Claro" | theme == "Acua")
                {
                    try
                       {
                    //Icon = Resources.TINserver_Acua;

                    BackColor = Color.FromArgb(240, 243, 248);
                    ForeColor = Color.FromArgb(35, 38, 43);
                    FontBack1.Hide();
                    FontBack2.Hide();
                    FontBack3.Hide();
                    FontBack4.Hide();
                    FontBack5.Hide();
                    FontBack6.Hide();
                    FontBack7.Hide();
                    FontBack8.Hide();
                    FontBack9.Hide();
                    FontBack10.Hide();
                    FontBack11.Hide();
                    pictureBox10.BackColor = Color.FromArgb(240, 243, 248);
                    pictureBox6.BackColor = Color.FromArgb(240, 243, 248);
                    mainTabs.BackColor = Color.FromArgb(240, 243, 248);
                    tabPage1.BackColor = Color.FromArgb(240, 243, 248);
                    tabPage2.BackColor = Color.FromArgb(240, 243, 248);
                    tabPage4.BackColor = Color.FromArgb(240, 243, 248);
                    tabPage3.BackColor = Color.FromArgb(240, 243, 248);
                    tabPage6.BackColor = Color.FromArgb(240, 243, 248);
                    tabPage7.BackColor = Color.FromArgb(240, 243, 248);
                    tabPage3_2.BackColor = Color.FromArgb(240, 243, 248);
                    InicioColor.BackColor = Color.FromArgb(240, 243, 248);
                    UserPicture.BackColor = Color.FromArgb(240, 243, 248);
                    DotaPicture.BackColor = Color.FromArgb(240, 243, 248);
                    ConfigPicture.BackColor = Color.FromArgb(240, 243, 248);
                    pictureBox8.BackColor = Color.FromArgb(240, 243, 248);
//                    pictureBox12.BackColor = Color.FromArgb(240, 243, 248);
//                    pictureBox13.BackColor = Color.FromArgb(240, 243, 248);
                    dataGrid.BackgroundColor = Color.FromArgb(240, 243, 248);
//                    pictureBox11.BackColor = Color.FromArgb(240, 243, 248);
                    pictureBox5.BackColor = Color.FromArgb(240, 243, 248);
                    panel3.BackColor = Color.FromArgb(78, 169, 239);
                    pictureBox7.BackColor = Color.FromArgb(78, 169, 239);
                    tabPage3.ForeColor = Color.FromArgb(35, 38, 43);
                    tabPage4.ForeColor = Color.FromArgb(35, 38, 43);
                    tabPage2.ForeColor = Color.FromArgb(35, 38, 43);
                    tabPage1.ForeColor = Color.FromArgb(35, 38, 43);
                    tabPage7.ForeColor = Color.FromArgb(35, 38, 43);
                    tabPage3_2.ForeColor = Color.FromArgb(35, 38, 43);
                    Status.ForeColor = Color.FromArgb(35, 38, 43);
                    localserver.ForeColor = Color.FromArgb(35, 38, 43);
                    remoteserver.ForeColor = Color.FromArgb(35, 38, 43);
                    ConfigurarlauncherLabel.ForeColor = Color.FromArgb(35, 38, 43);
                    IPutilizados.ForeColor = Color.FromArgb(35, 38, 43);
                    AddUserlabel.ForeColor = Color.FromArgb(35, 38, 43);
                    Nickname.ForeColor = Color.FromArgb(35, 38, 43);
                    PasswordLabel.ForeColor = Color.FromArgb(35, 38, 43);
                    labelTitle.ForeColor = Color.FromArgb(35, 38, 43);
                    LaunchOption.ForeColor = Color.FromArgb(78, 169, 239);
                    SelectMapLabel.ForeColor = Color.FromArgb(35, 38, 43);
                    LaunchOption.FlatStyle = FlatStyle.Flat;
                    opendotaBox.FlatStyle = FlatStyle.Flat;
                    GCExterno.FlatStyle = FlatStyle.Flat;
                    NewuserAmbos.FlatStyle = FlatStyle.Flat;
                    CerrarBox.Image = Resources.CerrarClaro_1;
                    MiniBox.Image = Resources.MinimizeClaro_1;
                    Aplicmap.BackColor = Color.FromArgb(240, 243, 248);
                    Aplicmap.ForeColor = Color.FromArgb(78, 169, 239);
                    Aplicmap.FlatAppearance.BorderSize = 1;
                    clientlbtn.BackColor = Color.FromArgb(240, 243, 248);
                    clientlbtn.ForeColor = Color.FromArgb(78, 169, 239);
                    clientlbtn.FlatAppearance.BorderSize = 1;
                    serverlBtn.BackColor = Color.FromArgb(240, 243, 248);
                    serverlBtn.ForeColor = Color.FromArgb(78, 169, 239);
                    serverlBtn.FlatAppearance.BorderSize = 1;
                    SearchServerGC.BackColor = Color.FromArgb(240, 243, 248);
                    SearchServerGC.ForeColor = Color.FromArgb(78, 169, 239);
                    SearchServerGC.FlatAppearance.BorderSize = 1;
                    btnGuardar.BackColor = Color.FromArgb(240, 243, 248);
                    btnGuardar.ForeColor = Color.FromArgb(78, 169, 239);
                    btnGuardar.FlatAppearance.BorderSize = 1;
                    AgregarBtn.BackColor = Color.FromArgb(240, 243, 248);
                    AgregarBtn.ForeColor = Color.FromArgb(78, 169, 239);
                    AgregarBtn.FlatAppearance.BorderSize = 1;
                    AdminUsers.BackColor = Color.FromArgb(240, 243, 248);
                    AdminUsers.ForeColor = Color.FromArgb(78, 169, 239);
                    AdminUsers.FlatAppearance.BorderSize = 1;
                    EditServerINI.BackColor = Color.FromArgb(240, 243, 248);
                    EditServerINI.ForeColor = Color.FromArgb(78, 169, 239);
                    EditServerINI.FlatAppearance.BorderSize = 1;
                    TINupdate.BackColor = Color.FromArgb(240, 243, 248);
                    TINupdate.ForeColor = Color.FromArgb(78, 169, 239);
                    TINupdate.FlatAppearance.BorderSize = 1;
                    PICSupdate.BackColor = Color.FromArgb(240, 243, 248);
                    PICSupdate.ForeColor = Color.FromArgb(78, 169, 239);
                    PICSupdate.FlatAppearance.BorderSize = 1;
                    EditClientINI.BackColor = Color.FromArgb(240, 243, 248);
                    EditClientINI.ForeColor = Color.FromArgb(78, 169, 239);
                    EditClientINI.FlatAppearance.BorderSize = 1;
                    ClientUpdate.BackColor = Color.FromArgb(240, 243, 248);
                    ClientUpdate.ForeColor = Color.FromArgb(78, 169, 239);
                    ClientUpdate.FlatAppearance.BorderSize = 1;
                    AboutBtn.BackColor = Color.FromArgb(240, 243, 248);
                    AboutBtn.ForeColor = Color.FromArgb(78, 169, 239);
                    AboutBtn.FlatAppearance.BorderSize = 1;
                    LaunchOption.BackColor = BackColor;
                    Cerrarlauncher.BackColor = BackColor;
                    Minimlauncher.BackColor = BackColor;
                    MinimBlauncher.BackColor = BackColor;
                    mantenerA.BackColor = BackColor;
                    opendotaBox.BackColor = BackColor;
                    GCExterno.BackColor = BackColor;
                    TINserverO.ForeColor = tabPage1.ForeColor;
                    TINserverO.BackColor = BackColor;
                    txtuserINI.BackColor = BackColor;
                    txtuserDB3.BackColor = BackColor;
                    NewuserAmbos.BackColor = BackColor;
                    NewuserAmbos.ForeColor = tabPage2.ForeColor;
                    txtuserINI.ForeColor = tabPage2.ForeColor;
                    txtuserDB3.ForeColor = tabPage2.ForeColor;
                    LaunchOption.ForeColor = tabPage1.ForeColor;
                    Cerrarlauncher.ForeColor = tabPage1.ForeColor;
                    Minimlauncher.ForeColor = tabPage1.ForeColor;
                    MinimBlauncher.ForeColor = tabPage1.ForeColor;
                    mantenerA.ForeColor = tabPage1.ForeColor;
                    opendotaBox.ForeColor = tabPage1.ForeColor;
                    GCExterno.ForeColor = tabPage1.ForeColor;
                    Ayuda.ForeColor = ForeColor;
                    Ayuda.Text = "";
                    SelectMapLabel.ForeColor = ForeColor;
                    SelectMapLabel.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    Aplicmap.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    mapaBox.ForeColor = ForeColor;
                    DotaBtn.Image = Resources.Dota2_claro;
                    Settings1.ForeColor = Color.FromArgb(78, 169, 239);
                    Settings2.ForeColor = Color.FromArgb(78, 169, 239);
                }
                catch (Exception exc) { Crearlog(exc.Message); }
                }
            }
        #endregion
        private void Thema_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarThema();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
        #region  Efecto en los Botones ...

        private void BtnHome_MouseMove(object sender, MouseEventArgs e)
        {
            OpcionesNulas();
            try 
               {
            InicioColor.BackColor = Color.FromArgb(78, 169, 239);
               }
            catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void BtnHome_MouseLeave(object sender, EventArgs e)
        {
            try { AplicarThema();} catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void UserBtn_MouseMove(object sender, MouseEventArgs e)
        {
            OpcionesNulas();
            try 
            {
            UserPicture.BackColor = Color.FromArgb(78, 169, 239);
            } catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void UserBtn_MouseLeave(object sender, EventArgs e)
        {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void button3_MouseMove(object sender, MouseEventArgs e)
        {
            OpcionesNulas();
            try 
            {
            DotaPicture.BackColor = Color.FromArgb(78, 169, 239);
            } catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void button3_MouseLeave(object sender, EventArgs e)
        {
            try { AplicarThema(); }catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void ConfigBtn_MouseMove(object sender, MouseEventArgs e)
        {
            OpcionesNulas();
            try 
            {
            ConfigPicture.BackColor = Color.FromArgb(78, 169, 239);
            } catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void ConfigBtn_MouseLeave(object sender, EventArgs e)
        {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void Aplicmap_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
            Aplicmap.ForeColor = Color.FromArgb(255, 255, 255);
            Aplicmap.BackColor = Color.FromArgb(57, 62, 63);
            }
    } catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void Aplicmap_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }

        private void clientlbtn_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                clientlbtn.ForeColor = Color.FromArgb(255, 255, 255);
                clientlbtn.BackColor = Color.FromArgb(57, 62, 63);
            }
    } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void serverlBtn_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                serverlBtn.ForeColor = Color.FromArgb(255, 255, 255);
                serverlBtn.BackColor = Color.FromArgb(57, 62, 63);
            }
    } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void btnGuardar_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                btnGuardar.ForeColor = Color.FromArgb(255, 255, 255);
                btnGuardar.BackColor = Color.FromArgb(57, 62, 63);
            }  
            } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void AgregarBtn_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                AgregarBtn.ForeColor = Color.FromArgb(255, 255, 255);
                AgregarBtn.BackColor = Color.FromArgb(57, 62, 63);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void AdminUsers_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                AdminUsers.ForeColor = Color.FromArgb(255, 255, 255);
                AdminUsers.BackColor = Color.FromArgb(57, 62, 63);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void AboutBtn_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                AboutBtn.ForeColor = Color.FromArgb(255, 255, 255);
                AboutBtn.BackColor = Color.FromArgb(57, 62, 63);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void EditServerINI_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                EditServerINI.ForeColor = Color.FromArgb(255, 255, 255);
                EditServerINI.BackColor = Color.FromArgb(57, 62, 63);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void EditClientINI_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                EditClientINI.ForeColor = Color.FromArgb(255, 255, 255);
                EditClientINI.BackColor = Color.FromArgb(57, 62, 63);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void TINupdate_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                TINupdate.ForeColor = Color.FromArgb(255, 255, 255);
                TINupdate.BackColor = Color.FromArgb(57, 62, 63);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void PICSupdate_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                PICSupdate.ForeColor = Color.FromArgb(255, 255, 255);
                PICSupdate.BackColor = Color.FromArgb(57, 62, 63);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void ClientUpdate_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                ClientUpdate.ForeColor = Color.FromArgb(255, 255, 255);
                ClientUpdate.BackColor = Color.FromArgb(57, 62, 63);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void clientlbtn_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }

        private void serverlBtn_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void btnGuardar_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }
        private void SearchServerGC_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    SearchServerGC.ForeColor = Color.FromArgb(255, 255, 255);
                    SearchServerGC.BackColor = Color.FromArgb(57, 62, 63);
                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void SearchServerGC_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }

        private void AgregarBtn_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void AdminUsers_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void EditServerINI_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void TINupdate_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void PICSupdate_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void EditClientINI_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void ClientUpdate_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void AboutBtn_MouseLeave(object sender, EventArgs e)
        {
        if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void Cerrarlauncher_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                    Cerrarlauncher.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack3.BackColor = Color.FromArgb(57, 62, 63);
                    Cerrarlauncher.BackColor = FontBack3.BackColor;

                }
            } catch (Exception exc) { Crearlog(exc.Message); }

        }
        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            cursor();
        }

        private void panel5_MouseMove(object sender, MouseEventArgs e)
        {
            cursor();
        }

        private void panel6_MouseMove(object sender, MouseEventArgs e)
        {
            cursor();
        }

        private void Minimlauncher_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                    Minimlauncher.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack2.BackColor = Color.FromArgb(57, 62, 63);
                    Minimlauncher.BackColor = FontBack2.BackColor;
                }
            } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void MinimBlauncher_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                    MinimBlauncher.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack4.BackColor = Color.FromArgb(57, 62, 63);
                    MinimBlauncher.BackColor = FontBack4.BackColor;

                }
            } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void mantenerA_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                    mantenerA.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack5.BackColor = Color.FromArgb(57, 62, 63);
                    mantenerA.BackColor = FontBack5.BackColor;
                }
            } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void localserver_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                localserver.ForeColor = Color.FromArgb(255, 255, 255);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

            //Ayuda.Text = "Seleccione para lanzar el Servidor local y el cliente";

        }

        private void remoteserver_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                remoteserver.ForeColor = Color.FromArgb(255, 255, 255);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void localserver_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }

        private void remoteserver_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }

        private void LaunchOption_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }

        private void Cerrarlauncher_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void Minimlauncher_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void MinimBlauncher_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void mantenerA_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }


        private void LaunchOption_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                    LaunchOption.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack1.BackColor = Color.FromArgb(57, 62, 63);
                    LaunchOption.BackColor = FontBack1.BackColor;

                }
            } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void txtuserINI_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                txtuserINI.ForeColor = Color.FromArgb(255, 255, 255);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void txtuserDB3_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                txtuserDB3.ForeColor = Color.FromArgb(255, 255, 255);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void NewuserAmbos_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                NewuserAmbos.ForeColor = Color.FromArgb(255, 255, 255);
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void txtuserINI_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }

        }

        private void txtuserDB3_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void NewuserAmbos_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }


        private void LaunchOption_CheckedChanged(object sender, EventArgs e)
        {
            if (LaunchOption.Checked == true)
            {
                ActivarOpciones();
                try
                {
                    if (BackColor == Color.FromArgb(28, 29, 33))
                    {
                        FontBack2.Show();
                        FontBack3.Show();
                        FontBack4.Show();
                        FontBack5.Show();
                        if (dotaMenu.Checked == true)
                        {
                            FontBack9.Show();
                        }
                        else
                        {
                            FontBack9.Hide();
                        }
                        FontBack10.Show();
                        FontBack11.Show();
                    }
                }
                catch (Exception exc) { Crearlog(exc.Message); }




            }
            else if (LaunchOption.Checked == false)
            {
                DesactivarOpciones();
                try
                {
                    if (BackColor == Color.FromArgb(28, 29, 33))
                    {
                        FontBack2.Hide();
                        FontBack3.Hide();
                        FontBack4.Hide();
                        FontBack5.Hide();
                        FontBack9.Hide();
                        FontBack10.Hide();
                        FontBack11.Hide();
                    }
                }
                catch (Exception exc) { Crearlog(exc.Message); }
            }
        }

        private void LaunchOption_MouseMove_1(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                LaunchOption.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack1.BackColor = Color.FromArgb(57, 62, 63);
                LaunchOption.BackColor = FontBack1.BackColor;
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void LaunchOption_MouseLeave_1(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void FontBack1_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                LaunchOption.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack1.BackColor = Color.FromArgb(57, 62, 63);
                LaunchOption.BackColor = FontBack1.BackColor;
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void Cerrarlauncher_MouseMove_1(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                Cerrarlauncher.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack3.BackColor = Color.FromArgb(57, 62, 63);
                Cerrarlauncher.BackColor = FontBack3.BackColor;
            }
                } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void FontBack3_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                Cerrarlauncher.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack3.BackColor = Color.FromArgb(57, 62, 63);
                Cerrarlauncher.BackColor = FontBack3.BackColor;
            }
    } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void Minimlauncher_MouseMove_1(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                Minimlauncher.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack2.BackColor = Color.FromArgb(57, 62, 63);
                Minimlauncher.BackColor = FontBack2.BackColor;
            }
    } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void FontBack2_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                Minimlauncher.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack2.BackColor = Color.FromArgb(57, 62, 63);
                Minimlauncher.BackColor = FontBack2.BackColor;
            }
    } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void MinimBlauncher_MouseMove_1(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                MinimBlauncher.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack4.BackColor = Color.FromArgb(57, 62, 63);
                MinimBlauncher.BackColor = FontBack4.BackColor;
            }
    } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void mantenerA_MouseMove_1(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                mantenerA.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack5.BackColor = Color.FromArgb(57, 62, 63);
                mantenerA.BackColor = FontBack5.BackColor;
            }
    } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void FontBack5_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                mantenerA.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack5.BackColor = Color.FromArgb(57, 62, 63);
                mantenerA.BackColor = FontBack5.BackColor;
            }
    } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void FontBack1_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void Cerrarlauncher_MouseLeave_1(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void FontBack3_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }

        private void Minimlauncher_MouseLeave_1(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void FontBack2_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void MinimBlauncher_MouseLeave_1(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void mantenerA_MouseLeave_1(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void FontBack5_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void txtuserINI_MouseMove_1(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                txtuserINI.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack6.BackColor = Color.FromArgb(57, 62, 63);
                txtuserINI.BackColor = FontBack6.BackColor;
            }
     } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void FontBack6_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                txtuserINI.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack6.BackColor = Color.FromArgb(57, 62, 63);
                txtuserINI.BackColor = FontBack6.BackColor;
            }
     } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void txtuserDB3_MouseMove_1(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                txtuserDB3.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack7.BackColor = Color.FromArgb(57, 62, 63);
                txtuserDB3.BackColor = FontBack7.BackColor;
            }
     } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void FontBack7_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                txtuserDB3.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack7.BackColor = Color.FromArgb(57, 62, 63);
                txtuserDB3.BackColor = FontBack7.BackColor;
            }
     } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void NewuserAmbos_MouseMove_1(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                NewuserAmbos.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack8.BackColor = Color.FromArgb(57, 62, 63);
                NewuserAmbos.BackColor = FontBack8.BackColor;
            }
    } catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void FontBack8_MouseMove(object sender, MouseEventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                NewuserAmbos.ForeColor = Color.FromArgb(255, 255, 255);
                FontBack8.BackColor = Color.FromArgb(57, 62, 63);
                NewuserAmbos.BackColor = FontBack8.BackColor;
            }
      } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void txtuserINI_MouseLeave_1(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void FontBack6_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void txtuserDB3_MouseLeave_1(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void FontBack7_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void NewuserAmbos_MouseLeave_1(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
                }
        }

        private void FontBack8_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
            try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }
        private void opendotaBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    opendotaBox.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack9.BackColor = Color.FromArgb(57, 62, 63);
                    opendotaBox.BackColor = FontBack9.BackColor;
                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void FontBack9_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    opendotaBox.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack9.BackColor = Color.FromArgb(57, 62, 63);
                    opendotaBox.BackColor = FontBack9.BackColor;
                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void opendotaBox_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }
        private void GCExterno_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    GCExterno.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack11.BackColor = Color.FromArgb(57, 62, 63);
                    GCExterno.BackColor = FontBack11.BackColor;
                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void GCExterno_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }
        private void FontBack11_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    GCExterno.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack11.BackColor = Color.FromArgb(57, 62, 63);
                    GCExterno.BackColor = FontBack11.BackColor;
                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void FontBack11_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }

        }

        private void FontBack9_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }

        private void FontBack8_Click(object sender, EventArgs e)
        {
            if (NewuserAmbos.Checked == true)
                {
            NewuserAmbos.Checked = false;
                }
            else if (NewuserAmbos.Checked == false)
                {
            NewuserAmbos.Checked = true;
                }
        }

        private void FontBack7_Click(object sender, EventArgs e)
        {
            if (txtuserDB3.Checked == false)
            {
                txtuserDB3.Checked = true;
                txtuserINI.Checked = false;
            }
        }

        private void FontBack6_Click(object sender, EventArgs e)
        {
            if (txtuserINI.Checked == false)
            {
                txtuserINI.Checked = true;
                txtuserDB3.Checked = false;
            }
        }

        private void FontBack3_Click(object sender, EventArgs e)
        {
            if (Cerrarlauncher.Checked == false)
            {
                Cerrarlauncher.Checked = true;
                Minimlauncher.Checked = false;
                MinimBlauncher.Checked = false;
                mantenerA.Checked = false;
            }
        }

        private void FontBack2_Click(object sender, EventArgs e)
        {
            if (Minimlauncher.Checked == false)
            {
                Cerrarlauncher.Checked = false;
                Minimlauncher.Checked = true;
                MinimBlauncher.Checked = false;
                mantenerA.Checked = false;
            }
        }

        private void FontBack5_Click(object sender, EventArgs e)
        {
            if (mantenerA.Checked == false)
            {
                Cerrarlauncher.Checked = false;
                Minimlauncher.Checked = false;
                MinimBlauncher.Checked = false;
                mantenerA.Checked = true;
            }
        }

        private void FontBack1_Click(object sender, EventArgs e)
        {
            if (LaunchOption.Checked == true)
            {
                LaunchOption.Checked = false;
            }
            else if (LaunchOption.Checked == false)
            {
                LaunchOption.Checked = true;
            }
        }
        private void FontBack11_Click(object sender, EventArgs e)
        {
            if (GCExterno.Checked == false)
            {
                GCExterno.Checked = true;
            }
            else if (GCExterno.Checked == true)
            {
                GCExterno.Checked = false;
            }

        }
        #endregion

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void GordoShop_MouseHover(object sender, EventArgs e)
        {
            try { if (BackColor == Color.FromArgb(28, 29, 33))
            {
                //toolTip1.SetToolTip(GordoShop, "El gordo de la chopi... jajaja");
            }
    } 
            catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void SteamGuard_TextChanged(object sender, EventArgs e)
        {
            if (LanguajeBox.Text == "Español")
            {
                if (SteamGuard.Text != "Clave de Steam Guard")
                {
                    SteamGuard.UseSystemPasswordChar = true;
                }
            }
            else if (LanguajeBox.Text == "English")
            {
                if (SteamGuard.Text != "Steam Guard Key")
                {
                    SteamGuard.UseSystemPasswordChar = true;
                }
            }
        }

        private void clientloctxt2_TextChanged(object sender, EventArgs e)
        {
            clientloctxt.Text = clientloctxt2.Text;
        }

        private void serverloctxt2_TextChanged(object sender, EventArgs e)
        {
            serverloctxt.Text = serverloctxt2.Text; 
        }

        private void tabPage3_MouseHover(object sender, EventArgs e)
        {
            cursor();
        }


        private void tabPage3_MouseMove(object sender, MouseEventArgs e)
        {
            cursor();
        }

        private void UserINIEdit_TextChanged(object sender, EventArgs e)
        {
            string SerDir = serverloctxt.Text;
            string INIUser = UserINIEdit.Text;
            File.WriteAllText(SerDir + @"\TINserver.users.ini", INIUser);
        }

        private void INIeditor_TextChanged(object sender, EventArgs e)
        {
            if (INIeditorID.Text == "TINserver.ini")
               {
            _Serverdir = serverloctxt.Text;
            string INIUser = INIeditor.Text;
            File.WriteAllText(_Serverdir + @"\TINserver.ini", INIUser);
               }
            else if (INIeditorID.Text == "TINserverclient.ini")
            {
                _clientdir = clientloctxt.Text;
                string INIUser = INIeditor.Text;
                File.WriteAllText(_clientdir + @"\TINserverclient.ini", INIUser);
            }
        }

        private void dataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNick.Text = dataGrid.CurrentRow.Cells["username"].Value.ToString();
        }

        private void dataGrid_MouseWheel(object sender, MouseEventArgs e)
        {
            dataGrid.EndEdit();
            if (e.Delta.Equals(120) && dataGrid.CurrentRow.Index != 0)
                SendKeys.Send("{Up}");

            else if (!e.Delta.Equals(120) && dataGrid.CurrentRow.Index != dataGrid.Rows.Count - 1)

                SendKeys.Send("{Down}");
        }


        private void dataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
                {
            SQLiteConnection conn = null;
            string dbPath = "Data Source=" + _Serverdir + @"\TINserver.users.db3;";
            conn = new SQLiteConnection(dbPath);//Create database connection
            conn.Open();//open database connection, if the data file not exist, it will create an empty one.

            SQLiteCommand cmdInsert = new SQLiteCommand(conn);
            //cmdInsert.CommandText = "INSERT INTO accounts(username, password, banned) VALUES(" + txtNick.Text + ", " + txtpass.Text + ", '1')"; //insert sample data
            //cmdInsert.ExecuteNonQuery();
            cmdInsert.CommandText = "UPDATE accounts SET username = '" +
            dataGrid.CurrentRow.Cells["username"].Value.ToString() + "', password = '" +
            dataGrid.CurrentRow.Cells["password"].Value.ToString() + "' ,banned = '" +
            dataGrid.CurrentRow.Cells["banned"].Value.ToString() + "' where username = '" +
            dataGrid.CurrentRow.Cells["username"].Value.ToString() + "';";

            cmdInsert.ExecuteNonQuery();
            conn.Close();
            CargarUsers();
                } catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Userdelete.Text = dataGrid.CurrentRow.Cells["username"].Value.ToString();
        }

        private void dataGrid_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source=" + _Serverdir + @"\TINserver.users.db3;";
            conn = new SQLiteConnection(dbPath);//Create database connection
            conn.Open();//open database connection, if the data file not exist, it will create an empty one.

            SQLiteCommand cmdInsert = new SQLiteCommand(conn);
            cmdInsert.CommandText = "DELETE FROM accounts WHERE username = '" + Userdelete.Text + "'";
            cmdInsert.ExecuteNonQuery();
            conn.Close();
            CargarUsers();
        }

        private void MainDark_FormClosing(object sender, FormClosingEventArgs e)
        {
            Borrardll();
            Borrarbin();
            Borrarlog();
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TINserverlauncher");
            key.SetValue("IpBox", "");
            key.Close();
            IpBox.Text = "";
            SaveSettings.SaveAllSettings(Application.ProductName, this);
        }
        private void Borrarbin()
        {
            if (BorrarTempS.Checked == true)
            {
                try
                {
                    _Serverdir = serverloctxt.Text;
                    foreach (string BIN in Directory.GetFiles(_Serverdir, "*.bin"))
                    {
                        File.Delete(BIN);
                    }
                }
                catch (Exception exc) { Crearlog(exc.Message); }
            }
        }
        private void Borrarlog()
        {
        if (BorrarTempC.Checked == true)
            {
                try
                {
                    Clientdir = clientloctxt.Text;
                    foreach (string LOG in Directory.GetFiles(Clientdir, "*.log"))
                    {
                        File.Delete(LOG);
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }



        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            MapsWorker.WorkerReportsProgress = true;
            
                    RegistryKey Patch = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINserverlauncher");
                    Patch.GetValue("Patch", RegistryValueKind.String);
                    String lauchPatch = (String)Patch.GetValue("Patch");
                    //
                    MapaLauncher = lauchPatch + (@"\TINserverlauncher\Maps");
                    if (!Directory.Exists(MapaLauncher))
                    {
                        Directory.CreateDirectory(MapaLauncher);
                        string[] files = Directory.GetFiles(Mapdir);
                        // Copy the files and overwrite destination files if they already exist.
                        foreach (string s in files)
                        {
                            // Método para copiar archivos del directorio.
                            fileName = Path.GetFileName(s);
                            destFile = Path.Combine(MapaLauncher, fileName);
                            File.Copy(s, destFile, true);

                            for (int i = 1; i <= 100; i++)
                            {
                                MapsWorker.ReportProgress(i);
                                if (MapsWorker.CancellationPending)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                    else if (Directory.Exists(MapaLauncher))
                    {
                        string[] files = Directory.GetFiles(Mapdir);
                        // Copy the files and overwrite destination files if they already exist.
                        foreach (string s in files)
                        {
                            // Método para copiar archivos del directorio.
                            fileName = Path.GetFileName(s);
                            destFile = Path.Combine(MapaLauncher, fileName);
                            File.Copy(s, destFile, true);

                            for (int i = 1; i <= 100; i++)
                            {
                                MapsWorker.ReportProgress(i);
                                if (MapsWorker.CancellationPending)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }    
                    }
                }





        private void Ping_On()
        {
            if (!string.IsNullOrEmpty(IpBox.Text))
            {
            PingPicture.Show();
            PingPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            PingPicture.Image = Resources.Ping_On;
            }
        }
        private void Ping_Off()
        {
            PingPicture.Hide();
        }



        private void PingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Si no es el directorio de Pruebas
            if (Environment.CurrentDirectory != @"D:\Instaladores\Programación\Projects\TINserverlauncher\TINserverlauncher\bin\x86\Debug")
            {
            WebRequest webRequest = WebRequest.Create("http://" + PingIP + ":8080");
            webRequest.Timeout = 1000;
            WebResponse webResponse; 
            webResponse = webRequest.GetResponse();
            webResponse.Close();
            }
        }
 

        private void PingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Ping_Off();
            }
            else if (e.Cancelled)
            {
            }
            else
            {
                Ping_On();
            }
        }

        #region Uso de RAM y CPU

        public void CpuUsage(object sender, PerformanceCounterEventArgs e)
        {
            try
            {
                var cpuVal = $"{e.CPUValue:##}";
                if (cpuVal != "")
                CPUp.Text = cpuVal;
                else
                CPUp.Text = "0";
                if (cpuVal != "")
                CPUbox.Width = Convert.ToInt32(cpuVal) / 2;
                else
                CPUbox.Width = 0;

            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }
        public static ulong GetTotalMemoryInBytes()
        {
            return new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
        }
        private readonly float ramInGB = GetTotalMemoryInBytes() / 1073741824F;
        public void RamUsage(float ram)
        {
            try
            {
                float ramUsageGb = ramInGB - ram / 1024;
                float RamUsada = ramUsageGb * 100;
                float PorcientoUsado = RamUsada / ramInGB;
                string PUsage = $"{PorcientoUsado:##}";
                RAMp.Text = PUsage;
                RAMbox.Width = Convert.ToInt32(PorcientoUsado / 2);
            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void PerformanceCounterEventHandler(object sender, PerformanceCounterEventArgs e)
        {
            RamUsage(e.RAMValue);
        }

        #endregion















        private void dotaMenu_CheckedChanged(object sender, EventArgs e)
        {
            DotaMenu();
        }
        private void DotaMenu()
        {
            if (dotaMenu.Checked == false)
            {
                DotaBtn.Hide();
                opendotaBox.Hide();
                FontBack9.Hide();
            }
            else if (dotaMenu.Checked == true)
            {
                DotaBtn.Show();
                if (LaunchOption.Checked == true)
                {
                opendotaBox.Show();
                    if (BackColor == Color.FromArgb(28, 29, 33))
                    {
                        FontBack9.Show();
                    }
                }
            }
            
        }
        /// <summary>
        /// //////////////////////////Sin arreglar
        /// </summary>

        private void IniciarConWindows()
        {
            RegistryKey RunRegistry = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            RunRegistry.GetValue("TINserverlauncher", RegistryValueKind.String);
            string launch = (string)RunRegistry.GetValue("TINserverlauncher");

            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (WStartLauncher.Checked == true)
            {
            try //TINserverlauncher
                {
                    if (string.IsNullOrEmpty(launch))
                    {
                        rk.SetValue("TINserverlauncher", Application.ExecutablePath.ToString());
                    }
                    else if (launch.Contains(" Launcher") == false)
                    {
                        rk.SetValue("TINserverlauncher", launch + " Launcher");
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }
            }
            if (WStartTINserver.Checked == true)
            {
                try //TINserver
                {
                    if (TINserverO.Checked == true)         //TINserver oculto
                    {
                        if (string.IsNullOrEmpty(launch))
                        {
                            rk.SetValue("TINserverlauncher", Application.ExecutablePath.ToString() + " TINserverO");
                        }
                        else if (launch.Contains(" TINserverO") == false)
                        {
                            rk.SetValue("TINserverlauncher", launch + " TINserverO");
                        }
                    }
                    else if (TINserverO.Checked == false)   //TINserver
                    {
                        if (string.IsNullOrEmpty(launch))
                        {
                            rk.SetValue("TINserverlauncher", Application.ExecutablePath.ToString() + " TINserver");
                        }
                        else if (launch.Contains(" TINserver") == false)
                        {
                            rk.SetValue("TINserverlauncher", launch + " TINserver");
                        }
                    }
                } catch (Exception exc) { Crearlog(exc.Message); }
            }

            if (WStartSteam.Checked == true)
            {
                try //Steam
                {
                    if (string.IsNullOrEmpty(launch))
                    {
                        rk.SetValue("TINserverlauncher", Application.ExecutablePath.ToString() + " Steam");
                    }

                    else if (launch.Contains(" Steam") == false)
                    {
                        rk.SetValue("TINserverlauncher", launch + " Steam");
                    }
                }
                catch (Exception exc) { Crearlog(exc.Message); }
            }
        }
        private void NoIniciarConWindows()
        {
            RegistryKey RunRegistry = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            RunRegistry.GetValue("TINserverlauncher", RegistryValueKind.String);
            string launch = (string)RunRegistry.GetValue("TINserverlauncher");

            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (WStartLauncher.Checked == false)
            {
                rk.DeleteValue("TINserverlauncher");

                try //TINserverlauncher
                {
                    if (WStartTINserver.Checked == false && WStartSteam.Checked == false)
                    {
                        rk.DeleteValue("TINserverlauncher");
                    }
                    else if (launch.Contains(" Launcher"))
                    {
                        string SteamReemplazado = launch.Replace(" Launcher", "");
                        rk.SetValue("TINserverlauncher", SteamReemplazado);
                    }

                }
                catch (Exception exc) { Crearlog(exc.Message); }
            }
            if (WStartTINserver.Checked == false)
            {
                try //TINserver
                {
                    if (WStartLauncher.Checked == false && WStartSteam.Checked == false)
                    {
                        rk.DeleteValue("TINserverlauncher");
                    }
                    else if (TINserverO.Checked == true)    //TINserver oculto
                    {
                        if (launch.Contains(" TINserverO"))
                        {
                            string TINserverOReemplazado = launch.Replace(" TINserverO", "");
                            rk.SetValue("TINserverlauncher", TINserverOReemplazado);
                        }
                    }
                    else if (TINserverO.Checked == false)   //TINserver
                    {
                        if (launch.Contains(" TINserver"))
                        {
                            string TINserverReemplazado = launch.Replace(" TINserver", "");
                            rk.SetValue("TINserverlauncher", TINserverReemplazado);
                        }
                    }
                }
                catch (Exception exc) { Crearlog(exc.Message); }
            }

            if (WStartSteam.Checked == false)
            {
                try //Steam
                {
                    if (WStartTINserver.Checked == false && WStartLauncher.Checked == false)
                    {
                        rk.DeleteValue("TINserverlauncher");
                    }
                    else if (launch.Contains(" Steam"))
                    {
                        string SteamReemplazado = launch.Replace(" Steam", "");
                        rk.SetValue("TINserverlauncher", SteamReemplazado);
                    }
                }
                catch (Exception exc) { Crearlog(exc.Message); }
            }
        }



        private void AbrirDesdeTry()
        {
            Show();
            WindowState = FormWindowState.Normal;
            //Ocultamos el icono de la bandeja de sistema
            if (MinBar.Checked == false)
            {
                notifyIcon1.Visible = false;
                ShowInTaskbar = true;
            }
            else if (MinBar.Checked == true)
            {
                notifyIcon1.Visible = true;
                ShowInTaskbar = false;
            }
        }
        private void MinToTryBar()
        {
            string Idioma = LanguajeBox.Text;

                if (Idioma == "Español")
                {
                    Visible = false;
                    notifyIcon1.Text = "TINserverlauncher";
                    notifyIcon1.Visible = true;
                    WindowState = FormWindowState.Minimized;
                    notifyIcon1.BalloonTipText = "El launcher se ha minimizado en la bandeja del sistema";
                    notifyIcon1.BalloonTipTitle = "TINserverLauncher";
                    notifyIcon1.ShowBalloonTip(4000);
                    //notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.ContextMenu = new ContextMenu();
                    notifyIcon1.ContextMenu.MenuItems.Add("Abrir", (s, e) => AbrirDesdeTry());
                    notifyIcon1.ContextMenu.MenuItems.Add("Detener procesos de Steam", (s, e) => TerminarProceso());
                    notifyIcon1.ContextMenu.MenuItems.Add("Cerrar", (s, e) => Application.Exit());
                }
                if (Idioma == "English")
                {
                    Visible = false;
                    notifyIcon1.Text = "TINserverlauncher";
                    notifyIcon1.Visible = true;
                    WindowState = FormWindowState.Minimized;
                    notifyIcon1.BalloonTipText = "The launcher has been minimized in the System tray";
                    notifyIcon1.BalloonTipTitle = "TINserverLauncher";
                    notifyIcon1.ShowBalloonTip(4000);
                    //notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.ContextMenu = new ContextMenu();
                    notifyIcon1.ContextMenu.MenuItems.Add("Open", (s, e) => AbrirDesdeTry());
                    notifyIcon1.ContextMenu.MenuItems.Add("Close Steam process", (s, e) => TerminarProceso());
                    notifyIcon1.ContextMenu.MenuItems.Add("Close", (s, e) => Application.Exit());
                }
        }

        private void MinToTry()
        {
            string Idioma = LanguajeBox.Text;

            if (MinimBlauncher.Checked == true)
            {
                if (Idioma == "Español")
                {
                    Visible = false;
                    notifyIcon1.Text = "TINserverlauncher";
                    notifyIcon1.Visible = true;
                    WindowState = FormWindowState.Minimized;
                    notifyIcon1.BalloonTipText = "El launcher se ha minimizado en la bandeja del sistema";
                    notifyIcon1.BalloonTipTitle = "TINserverLauncher";
                    notifyIcon1.ShowBalloonTip(4000);
                    //notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.ContextMenu = new ContextMenu();
                    notifyIcon1.ContextMenu.MenuItems.Add("Abrir", (s, e) => AbrirDesdeTry());
                    notifyIcon1.ContextMenu.MenuItems.Add("Detener procesos de Steam", (s, e) => TerminarProceso());
                    notifyIcon1.ContextMenu.MenuItems.Add("Cerrar", (s, e) => Application.Exit());


                }
                if (Idioma == "English")
                {
                    Visible = false;
                    notifyIcon1.Text = "TINserverlauncher";
                    notifyIcon1.Visible = true;
                    WindowState = FormWindowState.Minimized;
                    notifyIcon1.BalloonTipText = "The launcher has been minimized in the System tray";
                    notifyIcon1.BalloonTipTitle = "TINserverLauncher";
                    notifyIcon1.ShowBalloonTip(4000);
                    //notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.ContextMenu = new ContextMenu();
                    notifyIcon1.ContextMenu.MenuItems.Add("Open", (s, e) => AbrirDesdeTry());
                    notifyIcon1.ContextMenu.MenuItems.Add("Close Steam process", (s, e) => TerminarProceso());
                    notifyIcon1.ContextMenu.MenuItems.Add("Close", (s, e) => Application.Exit());
                }


            }
        }


        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            //Ocultamos el icono de la bandeja de sistema
            notifyIcon1.Visible = false;
        }




        private void opendotaBox_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
            Clientdir = clientloctxt.Text;
            if (opendotaBox.Checked == true)
            {
                string texto = File.ReadAllText(Clientdir + @"/TINserverClient.ini");
                if (texto.Contains("-applaunch 570") == false)
                {
                    File.WriteAllLines(Clientdir + @"/TINserverClient.ini",
                    File.ReadAllLines(Clientdir + @"/TINserverClient.ini")
                    .Select(x =>
                    {
                        if (x.StartsWith("commandLine")) return x + " -applaunch 570";
                        return x;
                    }));
                }
            }
            else if (opendotaBox.Checked == false)
            {
                string texto = File.ReadAllText(Clientdir + @"/TINserverClient.ini");
                string reemplazado = texto.Replace(" -applaunch 570", "");
                File.WriteAllText(Clientdir + @"/TINserverClient.ini", reemplazado);
            }
        } catch (Exception exc) { Crearlog(exc.Message); }

        }
    private void AllowSkypUpdate()
        {
            try
            {
            Clientdir = clientloctxt.Text;
            string texto = File.ReadAllText(Clientdir + @"/TINserverClient.ini");
            if (texto.Contains("+@AllowSkipGameUpdate 1") == false)
            {
                File.WriteAllLines(Clientdir + @"/TINserverClient.ini",
                File.ReadAllLines(Clientdir + @"/TINserverClient.ini")
                    .Select(x =>
                    {
                    if (x.StartsWith("commandLine")) return x + " +@AllowSkipGameUpdate 1 ";
                    return x;
                    }
                    ));
            }
            } catch (Exception exc) { Crearlog(exc.Message); }

    }


        private void FontBack9_Click(object sender, EventArgs e)
        {
            if (opendotaBox.Checked == false)
            {
                opendotaBox.Checked = true;
            }
            else if (opendotaBox.Checked == true)
            {
                opendotaBox.Checked = false;
            }


        }

        private void WindowsStart_CheckedChanged(object sender, EventArgs e)
        {
            if (WStartLauncher.Checked == true)
            {
                IniciarConWindows();
            }
            else if (WStartLauncher.Checked == false)
            {
                NoIniciarConWindows();
            }
        }





        private void MinBar_CheckedChanged(object sender, EventArgs e)
        {
            if (MinBar.Checked == true)
            {
                string Idioma = LanguajeBox.Text;

                if (Idioma == "Español")
                {
                    notifyIcon1.Text = "TINserverlauncher";
                    notifyIcon1.Visible = true;
                    notifyIcon1.ContextMenu = new ContextMenu();
                    notifyIcon1.ContextMenu.MenuItems.Add("Abrir", (s, f) => AbrirDesdeTry());
                    notifyIcon1.ContextMenu.MenuItems.Add("Detener procesos de Steam", (s, f) => TerminarProceso());
                    notifyIcon1.ContextMenu.MenuItems.Add("Cerrar", (s, f) => Application.Exit());
                }
                if (Idioma == "English")
                {
                    notifyIcon1.Text = "TINserverlauncher";
                    notifyIcon1.Visible = true;
                    notifyIcon1.ContextMenu = new ContextMenu();
                    notifyIcon1.ContextMenu.MenuItems.Add("Open", (s, f) => AbrirDesdeTry());
                    notifyIcon1.ContextMenu.MenuItems.Add("Close Steam process", (s, f) => TerminarProceso());
                    notifyIcon1.ContextMenu.MenuItems.Add("Close", (s,f) => Application.Exit());
                }
            //    ShowInTaskbar = false;
            }
            else if (MinBar.Checked == false)
            {
     //           ShowInTaskbar = true;
                notifyIcon1.Visible = false;
            }
        }

        private void WStartLauncher_CheckedChanged(object sender, EventArgs e)
        {
            if (WStartLauncher.Checked == true)
            {
                IniciarConWindows();
            }
            else if (WStartLauncher.Checked == false)
            {
                NoIniciarConWindows();
            }
        }

        private void WStartTINserver_CheckedChanged(object sender, EventArgs e)
        {
            if (WStartTINserver.Checked == true)
            {
                IniciarConWindows();
            }
            else if (WStartTINserver.Checked == false)
            {
                NoIniciarConWindows();
            }
        }

        private void WStartSteam_CheckedChanged(object sender, EventArgs e)
        {
            if (WStartSteam.Checked == true)
            {
                IniciarConWindows();
            }
            else if (WStartSteam.Checked == false)
            {
                NoIniciarConWindows();
            }

        }

        private void TINserverO_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    TINserverO.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack10.BackColor = Color.FromArgb(57, 62, 63);
                    TINserverO.BackColor = FontBack10.BackColor;
                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void TINserverO_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }

        private void FontBack10_MouseLeave(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }
        }

        private void FontBack10_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    TINserverO.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack10.BackColor = Color.FromArgb(57, 62, 63);
                    TINserverO.BackColor = FontBack9.BackColor;
                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void Settings1_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage3_2);
        }

        private void Settings2_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage3);
        }
        private void GCExterno_CheckedChanged(object sender, EventArgs e)
        {
            if (remoteserver.Checked == true)
            {
                GCExterno.Checked = false;
            }
            else
            {
            if (GCExterno.Checked == true)
            {
                GCfilename.Show();
                SearchServerGC.Show();
            }
            else if (GCExterno.Checked == false)
            {
                GCfilename.Hide();
                SearchServerGC.Hide();
            }
        }
   }
        private void SearchServerGC_Click(object sender, EventArgs e)
        {
            Serverloc = serverloctxt.Text;
            var openDialog = new OpenFileDialog
            {
                InitialDirectory = Serverloc,
                Filter = "Application |*.exe",
                Multiselect = true,
            };
            var userOK = openDialog.ShowDialog();

            if (userOK != DialogResult.OK)
            {
                return;
            }
            string name = openDialog.SafeFileName;
            GCfilename.Text = name;                 //Muestra el nombre del archivo
        }

        public void Stats()
        {
            //ValvePak.dll
            if (!File.Exists(@"MyVideoClass.dll"))
            {
                File.WriteAllBytes(@"MyVideoClass.dll", Resources.MyVideoClass);
                File.SetAttributes(@"MyVideoClass.dll", FileAttributes.Hidden);
            }

            tabPage8_Perfil.BackColor = BackColor;
            tabPage8_Perfil.ForeColor = ForeColor;
            cursor();
            level.Parent = HeroPic;
        }


        private void CargarPerfilW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mainTabs.SelectTab(tabPage8_Perfil);
        }


        private void TINserverO_CheckedChanged(object sender, EventArgs e)
        {
            if (remoteserver.Checked == true)
                TINserverO.Checked = false;
        }

        private void clientlbtn_Click(object sender, EventArgs e)
        {
            clienlocBtn.PerformClick();
        }




        private void serverlBtn_Click(object sender, EventArgs e)
        {
            serverlocBtn.PerformClick();
        }

        private void PICSupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(serverloctxt.Text))
                {
                    Message Mensage = new Message(); //creamos un objeto del formulario 2
                    Mensage._Configurar = "Servidor";
                    string Themes = Thema.Text;
                    Mensage.theme = Themes;
                    string Idioma = LanguajeBox.Text;
                    Mensage.Idioma = Idioma;
                    DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                    if (res == DialogResult.OK)
                    {
                        serverlocBtn.PerformClick();
                        PICSupdate.PerformClick();
                    }
                    if (res == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {

                    //Actualizar datos de Steam
                    if (UserSteam.Text == "Usuario real de Steam" | PassSteam.Text == "Contraseña" | UserSteam.Text == "Real user of Steam" | PassSteam.Text == "Password" | string.IsNullOrEmpty(UserSteam.Text) | string.IsNullOrEmpty(PassSteam.Text))
                    {
                        //MessageBox.Show("Debes configurar los datos de autentificación de Steam");
                        Message Mensage = new Message(); //creamos un objeto del formulario 2
                        Mensage._Configurar = "SteamErr";
                        string Themes = Thema.Text;
                        Mensage.theme = Themes;
                        string Idioma = LanguajeBox.Text;
                        Mensage.Idioma = Idioma;
                        DialogResult res = Mensage.ShowDialog(); //abrimos el formulario 2 como cuadro de dialogo modal

                    }
                    else
                    {
                        _Serverdir = serverloctxt.Text;
                        string PICSloc = _Serverdir + @"\tools\PICSdownloader\";
                        //PICSdownloader
                        File.WriteAllLines(PICSloc + "PICSdownloader.ini",
                             File.ReadAllLines(PICSloc + "PICSdownloader.ini")
                                  .Select(x =>
                                  {
                                      if (x.StartsWith("login")) return "login = " + UserSteam.Text;
                                      else if (x.StartsWith("Login")) return "login = " + UserSteam.Text;
                                      else if (x.StartsWith("password")) return "password = " + PassSteam.Text;
                                      else if (x.StartsWith("Password")) return "password = " + PassSteam.Text;
                                      else if (x.StartsWith("steamGuardKey")) return "steamGuardKey = " + SteamGuard.Text;
                                      else if (x.StartsWith("SteamGuardKey")) return "steamGuardKey = " + SteamGuard.Text;
                                      return x;
                                  }));
                        //TINcft
                        string TINcft = _Serverdir + @"\tools\TINcft\";
                        File.WriteAllLines(TINcft + "TINcft.ini",
                        File.ReadAllLines(TINcft + "TINcft.ini")
                        .Select(x =>
                        {
                            if (x.StartsWith("username")) return "username = " + UserSteam.Text;
                            else if (x.StartsWith("Username")) return "username = " + UserSteam.Text;
                            else if (x.StartsWith("password")) return "password = " + PassSteam.Text;
                            else if (x.StartsWith("Password")) return "password = " + PassSteam.Text;
                            else if (x.StartsWith("steamGuardKey")) return "steamGuardKey = " + SteamGuard.Text;
                            else if (x.StartsWith("SteamGuardKey")) return "steamGuardKey = " + SteamGuard.Text;
                            return x;
                        }));

                        _Serverdir = serverloctxt.Text;
                        //Iniciar actualizacion
                        ProcessStartInfo updateAppInfo = new ProcessStartInfo();
                        updateAppInfo.UseShellExecute = true;
                        updateAppInfo.FileName = "updateAppInfo.cmd";
                        updateAppInfo.WorkingDirectory = _Serverdir;
                        Process.Start(updateAppInfo);

                    }
                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }
        private void button3_Click_2(object sender, EventArgs e)
        {
        }


        private void team_Click(object sender, EventArgs e)
        {
            /*
            MySqlConnection con = new MySqlConnection("Server = " + IPstats + "; User ID = Hackerprod; Password = 123; Database = steam; SslMode = none");
            con.Open();
            string query = "INSERT INTO partida (id_partida, id_steam, equipo, resultado, fecha, k, d, a, opm, epm, duracion, heroe, lasthit, denies, items, level, abandono) VALUES('" + gs.Map.MatchID + "', '" + gs.Player.SteamID + "', '" + gs.Player.Team + "', '" + gs.Map.Win_team + "', '" + DateTime.Now.ToString("dd MMMM yyyy") + "', '" + gs.Player.Kills + "', '" + gs.Player.Deaths + "', '" + gs.Player.Assists + "', '" + gs.Player.GoldPerMinute + "', '" + gs.Player.ExperiencePerMinute + "', '" + gs.Map.GameTime + "', '" + gs.Hero.Name + "', '" + gs.Player.LastHits + "', '" + gs.Player.Denies + "', '" + Inventario.Text + "', '" + gs.Hero.Level + "', '" + Partida.Text + "')";
            //create command and assign the query and connection from the constructor
            MySqlCommand cmd = new MySqlCommand(query.ToString(), con);
            cmd.ExecuteNonQuery(); //Execute command
            con.Close();
            */
        }

        private void HeroPic_Click(object sender, EventArgs e)
        {

        }


        private void txtuserINI_MouseMove_2(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    txtuserINI.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack6.BackColor = Color.FromArgb(57, 62, 63);
                    txtuserINI.BackColor = FontBack6.BackColor;

                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }
        }

        private void FontBack6_MouseMove_1(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    txtuserINI.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack6.BackColor = Color.FromArgb(57, 62, 63);
                    txtuserINI.BackColor = FontBack6.BackColor;

                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void txtuserDB3_MouseMove_2(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    txtuserDB3.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack7.BackColor = Color.FromArgb(57, 62, 63);
                    txtuserDB3.BackColor = FontBack7.BackColor;

                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void FontBack7_MouseMove_1(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    txtuserDB3.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack7.BackColor = Color.FromArgb(57, 62, 63);
                    txtuserDB3.BackColor = FontBack7.BackColor;

                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void NewuserAmbos_MouseMove_2(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    NewuserAmbos.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack8.BackColor = Color.FromArgb(57, 62, 63);
                    NewuserAmbos.BackColor = FontBack8.BackColor;

                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void FontBack8_MouseMove_1(object sender, MouseEventArgs e)
        {
            try
            {
                if (BackColor == Color.FromArgb(28, 29, 33))
                {
                    NewuserAmbos.ForeColor = Color.FromArgb(255, 255, 255);
                    FontBack8.BackColor = Color.FromArgb(57, 62, 63);
                    NewuserAmbos.BackColor = FontBack8.BackColor;

                }
            }
            catch (Exception exc) { Crearlog(exc.Message); }

        }

        private void txtuserINI_MouseLeave_2(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }

        }

        private void FontBack6_MouseLeave_1(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }

        }

        private void txtuserDB3_MouseLeave_2(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }

        }

        private void FontBack7_MouseLeave_1(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }

        }

        private void NewuserAmbos_MouseLeave_2(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }

        }

        private void FontBack8_MouseLeave_1(object sender, EventArgs e)
        {
            if (BackColor == Color.FromArgb(28, 29, 33))
            {
                try { AplicarThema(); } catch (Exception exc) { Crearlog(exc.Message); }
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage8_LastPlay);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage8_Perfil);
        }
        private void AllPlay_Click(object sender, EventArgs e)
        {
            var tab = new TabPage();
            tab.Controls.Add(new LoadingFile(Thema.Text));

            mainTabs.TabPages.Add(tab);
            mainTabs.SelectTab(tab);
        }

        private void MyPlay_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage8_MisPartidas);
        }

        private void HeroFuerza_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage8_HeroeFavorito);
        }

        private void HeroAgilidad_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage8_HeroeFavorito);
        }

        private void HeroInteligencia_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage8_HeroeFavorito);
        }

        private void MMRanking_Click(object sender, EventArgs e)
        {

        }

        private void PerfiL_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage8_Perfil);
        }

        private void SteamPicture_Click(object sender, EventArgs e)
        {
        }

        private void GoMiPerfil_Click(object sender, EventArgs e)
        {
            mainTabs.SelectTab(tabPage8_Perfil);
        }

        private void IPutilizados_Click(object sender, EventArgs e)
        {


        }


    private void ExportBtN_Click(object sender, EventArgs e)
        {
            Thread receiveThread = new Thread(new ThreadStart(ExpLS));
            receiveThread.IsBackground = true;
            receiveThread.Start();

        }
        private void ExpLS()
        {
            
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
                }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Partidapanel.FlowDirection = FlowDirection.TopDown;
            Partidapanel.AutoScroll = true;
            Panel panel = new Panel();

            panel.BackColor = Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            //panel.Controls.Add(this.label13);
            //panel.Controls.Add(this.label15);
            //panel.Location = new System.Drawing.Point(3, 3);
            panel.Name = "panel31";
            panel.Size = new Size(470, 28);
            Partidapanel.Controls.Add(panel);

        }

        private void Played_Click(object sender, EventArgs e)
        {

    }

        private void SteamName_Click(object sender, EventArgs e)
        {

        }
        public void panel_MouseMove(object sender, MouseEventArgs e)
        {
            Control ctrl = sender as Control;
            ctrl.ForeColor = Color.FromArgb(255, 255, 255);
            Color BackColor = Color.FromArgb(57, 62, 63);
            ctrl.BackColor = BackColor;
        }
        public void panel_MouseLeave(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;
            ctrl.ForeColor = Color.FromArgb(78, 169, 239);
            Color BackColor = Color.FromArgb(51, 54, 61);
            ctrl.BackColor = BackColor;

        }


        private void MainDark_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control is Panel)
            {
                e.Control.MouseMove += panel_MouseMove;
                e.Control.MouseLeave += panel_MouseLeave;
            }
        }

        private void Crearlog(string log)
        {
            DateTime dateTimeVariable = DateTime.Now;
            string date = dateTimeVariable.ToString("yyyy-MM-dd H:mm:ss");
            
            if (File.Exists("TINserverlauncher.log"))
            {
                string texto = File.ReadAllText("TINserverlauncher.log");
                File.WriteAllText("TINserverlauncher.log", texto + Environment.NewLine + date + Environment.NewLine + log + Environment.NewLine);
            }
            else
            {
                File.WriteAllText("TINserverlauncher.log", date + Environment.NewLine + log + Environment.NewLine);
            }
        }
        public void HideCPUUtils()
        {
            RAM.Hide();
            CPU.Hide();
            CPUbox.Hide();
            RAMbox.Hide();
            CPUp.Hide();
            RAMp.Hide();
        }

        private void CkeckServer_Tick(object sender, EventArgs e)
        {
            try
            {
                Process process = Process.GetProcessesByName("TINserver")[0];
            }
            catch (Exception)
            {
                restartedserver = restartedserver + 1;
                secondTimer = 0;
                TINserver();
            }
        }

        int secondTimer = 0;
        int restartedserver = 0;
        private void ServerTimer_Tick(object sender, EventArgs e)
        {
            secondTimer = secondTimer + 1;
            string time = ConvertMinutesToTime(secondTimer);

            Status.Text = $"[Server activo hace {time}] [Server caído {restartedserver} veces]";
        }

        private string ConvertMinutesToTime(int secondTimer)
        {
            TimeSpan t = TimeSpan.FromSeconds(secondTimer);

            string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);
            return answer;
        }
    }
}





/*          //CMD
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = "cmd";
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();
            proc.StandardInput.WriteLine(@"net user Skynet 123 /add");
            proc.StandardInput.Flush();
            proc.StandardInput.Close();
            proc.Close();


            //ZipFile.ExtractToDirectory("master.zip", Environment.CurrentDirectory + @"/lol/"); //extracts it

*/
