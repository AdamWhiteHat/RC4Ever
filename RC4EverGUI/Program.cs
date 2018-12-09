using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace RC4EverGUI
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
			Application.Run(new MainForm());
		}

		private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
		{
			try
			{
				List<string> lines = new List<string>();

				lines.Add(DateTime.Now.ToString("MM'/'dd'/'yyyy @ HH':'mm':'ss.fff"));
				lines.Add(e.Exception.ToString());
				lines.Add("");
				lines.Add("-----------------------------------------------------------------");
				lines.Add(Environment.NewLine);

				File.WriteAllLines("Exceptions.log.txt", lines);
			}
			catch
			{
			}
		}
	}
}
