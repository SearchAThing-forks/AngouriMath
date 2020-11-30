using System;

namespace GraphicExample
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {           
            GuiToolkit.CreateGui<MainWindow>();
        }
    }
}
