using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKYNET
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]


        static void Main(string[] args)
        {
            var app = new frmMain();
            if (args.Length < 1)
            {
                Application.EnableVisualStyles();
                Application.Run(new frmMain());  //Testing
            }
            else if (args.Length > 0)
            {
                try
                {
                switch (args[0])
                {
                    case "Launcher":
                    Application.EnableVisualStyles();
                    Application.Run(new frmMain());
                    break;
                    case "TINserver":
                    app.TINserver();
                    break;
                    case "TINserverO":
                    app.TINserverOculto();
                    break;
                    case "Steam":
                    app.TINserverclient();
                    break;
                }
                switch (args[1])
                {
                    case "Launcher":
                        Application.EnableVisualStyles();
                        Application.Run(new frmMain());
                        break;
                    case "TINserver":
                        app.TINserver();
                        break;
                    case "TINserverO":
                        app.TINserverOculto();
                        break;
                    case "Steam":
                        app.TINserverclient();
                        break;
                }
                switch (args[2])
                {
                    case "Launcher":
                        Application.EnableVisualStyles();
                        Application.Run(new frmMain());
                        break;
                    case "TINserver":
                        app.TINserver();
                        break;
                    case "TINserverO":
                        app.TINserverOculto();
                        break;
                    case "Steam":
                        app.TINserverclient();
                        break;
                }
            } catch { }


            }
        }
    }
}
