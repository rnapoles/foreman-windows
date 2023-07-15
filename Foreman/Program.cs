﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Foreman
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

#if DEBUG
            Console.WriteLine("Mode=Debug");
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
