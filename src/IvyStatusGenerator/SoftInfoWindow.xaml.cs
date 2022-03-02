using Invary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Invary.IvyStatusGenerator
{
	/// <summary>
	/// SoftInfoWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class SoftInfoWindow : Window
	{
		public SoftInfo Info { get; set; }

		public SoftInfoWindow(SoftInfo info)
		{
			InitializeComponent();

			Info = info;
			DataContext = Info;
		}


		private void OnDownload(object sender, RoutedEventArgs e)
		{
			//ex. "https://github.com/Invary/IvyMediaDownloader/releases"
			if (string.IsNullOrEmpty(Info.strDownloadURL) || Info.strDownloadURL.IndexOf("https://github.com/Invary/") != 0)
			{
				MessageBox.Show("Download URL must be set github URL", "error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			_ = OnDownloadAsync();
		}


		private async Task OnDownloadAsync()
		{
			var html = await Uty.DownloadTextAsync(Info.strDownloadURL, 30, null);
			if (html == null)
				return;

			string ver = "";
			{
				var matchs = Regex.Matches(html, "/releases/tag/Ver([\\d]+?)\"");

				foreach (Match match in matchs)
				{
					if (match.Groups.Count < 2)
						continue;

					ver = match.Groups[1].Value;
					break;
				}
			}

			try
			{
				if (string.IsNullOrEmpty(ver) == false)
				{
					int nVer = int.Parse(ver);

					//succeeded
					Info.nVer = nVer;
					Info.strVer = $"Ver{nVer}";
					Info.OnPropertyChanged("strVer");
					return;
				}
			}
			catch (Exception)
			{
			}
			MessageBox.Show("Cannot obtain version number from download URL.", "error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void OnNameChanged(object sender, TextChangedEventArgs e)
		{
			var source = e.Source as TextBox;
			if (source == null)
				return;

			Info.strName = source.Text;
			Info.OnPropertyChanged("strName");
		}


		private void OnVersionNoChanged(object sender, TextChangedEventArgs e)
		{
			var source = e.Source as TextBox;
			if (source == null)
				return;

			Info.strVer = $"Ver{source.Text}";
			Info.OnPropertyChanged("strVer");
		}
		private void OnDownloadURLChanged(object sender, TextChangedEventArgs e)
		{
			var source = e.Source as TextBox;
			if (source == null)
				return;

			Info.strDownloadURL = source.Text;
			Info.OnPropertyChanged("strDownloadURL");
		}

		private void OnGuidChanged(object sender, TextChangedEventArgs e)
		{
			var source = e.Source as TextBox;
			if (source == null)
				return;

			Info.strGuid = source.Text;
			Info.OnPropertyChanged("strGuid");
		}

		private void OnDateChanged(object sender, TextChangedEventArgs e)
		{
			var source = e.Source as TextBox;
			if (source == null)
				return;

			//Info.dtDate = source.Text;
			Info.OnPropertyChanged("dtDate");
		}


		private void OnSetCurrentDateTime(object sender, RoutedEventArgs e)
		{
			Info.dtDate = DateTime.Now;
			Info.OnPropertyChanged("dtDate");
			Info.OnPropertyChanged("dtUTC");
		}



		private void OnClose(object sender, RoutedEventArgs e)
		{
			Close();
		}

	}
}
