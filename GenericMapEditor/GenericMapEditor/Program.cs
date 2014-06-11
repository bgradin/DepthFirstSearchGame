using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GenericMapEditor
{
	public static class EditorConst
	{
		public const int DEFAULT_TILE_SIZE = 16;
		public const int WINDOW_DEFAULT_WIDTH = 765;
		public const int WINDOW_DEFAULT_HEIGHT = 399;
		public const int WINDOW_DEFAULT_X = 300;
		public const int WINDOW_DEFAULT_Y = 301;
		public const bool WINDOW_DEFAULT_MAXIMIZATION = false;
		public const int MAXIMUM_RECENT_FILES = 5;
		public const string CONTENT_ROOT_DIR = "GFX";
	}

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
