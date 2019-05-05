using System;
using System.IO;
using System.Text;
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
			Application.ThreadException += Application_ThreadException;
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
			Application.Run(new MainForm());
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			LogException(e.Exception);
		}

		private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
		{
			LogException(e.Exception);
		}

		private static void LogException(Exception ex)
		{
			try
			{
				StringBuilder output = new StringBuilder();
				output.AppendLine(DateTime.Now.ToString("MM'/'dd'/'yyyy @ HH':'mm':'ss.fff"));
				output.AppendLine(ex.ToString());
				output.AppendLine();
				output.AppendLine("-----------------------------------------------------------------");
				output.AppendLine();
				output.AppendLine();
				File.WriteAllText("Exceptions.log.txt", output.ToString());
			}
			catch
			{
			}
		}
	}
}
