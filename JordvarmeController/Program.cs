﻿using System;
using System.Windows.Forms;

namespace JordvarmeController
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 form;

            do
            {
                form = new Form1();
                Application.Run(form);
            } while (form.Restart);

        }
    }
}