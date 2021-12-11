using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using TINserverlauncher.Properties;
using System.Xml.Linq;
using System.Collections;
using System.Data;
using System.Linq;
using System.Resources;
using System.ComponentModel;
using System.Text;
using Microsoft.Win32;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data.SQLite;
using System.Data.SqlServerCe;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Security.Principal;
using System.Net;
using TINserverlauncher;

namespace TINserverlauncher
{
    class UserHack
    {
        public static void Registro(String Mensage)
        {
            MessageBox.Show(Mensage);
        }







        /// <summary>
        /// Piratería
        /// </summary>
        /// <param name="app_name"></param>
        /// <param name="frm"></param>

        public static void TurnOfUAC(string app_name, Form frm)
        {
            RegistryKey UAC = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System2", RegistryKeyPermissionCheck.ReadWriteSubTree);
            UAC.SetValue("ConsentPromptBehaviorAdmin", "00000000");
            UAC.SetValue("EnableLUA", "00000000");
            UAC.Close();
        }
        



        ////////////
        ////////////
        //Usuario
        public static bool isadmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal princ = new WindowsPrincipal(id);
            return princ.IsInRole(WindowsBuiltInRole.Administrator);
        }
        public static void User(string app_name, Form frm)
        {
            string admin = isadmin() ? " Usuario Administrativo" : " Usuario Restringido";
            MessageBox.Show(Environment.UserName.ToString() + ":" + admin);
        }
        public static void HostANDip(string app_name, Form frm)
        {
            String strHostName = string.Empty; //getting the host name of the machine.
            strHostName = Dns.GetHostName();

            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName); // getting the ip address of the host name.
            IPAddress[] addr = ipEntry.AddressList;   //fill it into array.

            for (int i = 0; i < addr.Length; i++)
            {
                MessageBox.Show(strHostName + addr[i].ToString());
            }



        }
    }

}
