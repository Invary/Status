using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Invary.IvyStatusGenerator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public UpdateStatus Status { get; set; }


		const string JsonPath = @"../../../../../status.json";
		const string XmlPath = @"../../../../data.xml";


		public MainWindow()
		{
			InitializeComponent();

			var load = UpdateStatus.LoadXml(XmlPath);
			if (load == null)
				load = new UpdateStatus();


			Status = load;

			foreach (var item in Status.listSoftware)
			{
				mainDataGrid.Items.Add(item);
			}

			Closing += delegate
			{
				var ret = MessageBox.Show("Save current status to xml?", "save", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (ret == MessageBoxResult.Yes)
				{
					Status.SaveXml(XmlPath);
				}
			};


		}

		private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton != MouseButtonState.Pressed)
				return;

			var item = mainDataGrid.SelectedItem as SoftInfo;
			if (item == null)
				return;

			var wnd = new SoftInfoWindow(item);
			wnd.Owner = this;
			wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			wnd.ShowDialog();
		}

		private void Menu_OnAdd(object sender, RoutedEventArgs e)
		{
			var item = new SoftInfo();
			Status.listSoftware.Add(item);
			mainDataGrid.Items.Add(item);
		}

		private void Menu_OnRemove(object sender, RoutedEventArgs e)
		{
			var item = mainDataGrid.SelectedItem as SoftInfo;
			if (item == null)
				return;

			var ret = MessageBox.Show("Remove selected item?", "remove", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
			if (ret != MessageBoxResult.OK)
				return;

			mainDataGrid.Items.Remove(item);
			Status.listSoftware.Remove(item);
		}

		private void OnExport(object sender, RoutedEventArgs e)
		{
			var json = Status.ToJson();

			using(FileStream fs = new FileStream(JsonPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
			using(StreamWriter sw = new StreamWriter(fs))
			{
				sw.WriteLine(json);
			}
		}
	}
}
