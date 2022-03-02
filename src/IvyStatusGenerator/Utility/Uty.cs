using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Invary.Utility
{
	internal class Uty
	{
		public static void OpenURL(string url)
		{
			ProcessStartInfo pi = new ProcessStartInfo()
			{
				FileName = url,
				UseShellExecute = true,
			};
			using (Process.Start(pi))
			{
			}
		}


		/// <summary>
		/// Get executable path
		/// 
		/// ex. @"c:\program files\test\test.exe"
		/// </summary>
		public static string GetExecutablePath()
		{
			var process = Process.GetCurrentProcess();
			if (process.MainModule == null || process.MainModule.FileName == null)
				throw new Exception();

			return process.MainModule.FileName;
		}



		/// <summary>
		/// Get executable folder
		/// 
		/// ex. @"c:\program files\test"
		/// </summary>
		public static string GetExecutableFolder()
		{
			var exe = GetExecutablePath();
			var folder = Path.GetDirectoryName(exe);
			if (folder == null)
				return "";
			return folder;
		}



		public static async Task<string> DownloadTextAsync(string url, double dTimeoutSec, CancellationToken? ct)
		{
			try
			{
				using (var wc = new HttpClient())
				{
					wc.DefaultRequestHeaders.Add(
						"User-Agent",
						"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.45 Safari/537.36");

					//wc.DefaultRequestHeaders.Add("Accept-Language", "ja-JP");

					if (dTimeoutSec > 0)
						wc.Timeout = TimeSpan.FromSeconds(dTimeoutSec);

					if (ct == null)
						return await wc.GetStringAsync(url);
					else
						return await wc.GetStringAsync(url, (CancellationToken)ct);
				}
			}
			catch (Exception)
			{
			}
			return "";
		}



	}
}
