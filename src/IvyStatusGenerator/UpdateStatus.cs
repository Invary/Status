using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invary.IvyStatusGenerator
{

	public class UpdateStatus : INotifyPropertyChanged
	{
		public List<SoftInfo> listSoftware { set; get; } = new List<SoftInfo>();
		public DonationInfo Donation { set; get; } = new DonationInfo();


		public event PropertyChangedEventHandler? PropertyChanged;

		public void OnPropertyChanged(string PropertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
		}

		public static UpdateStatus? FromJson(string json)
		{
			try
			{
				return JsonSerializer.Deserialize<UpdateStatus>(json);
			}
			catch (Exception)
			{
			}
			return null;
		}

		public string ToJson()
		{
			try
			{
				return JsonSerializer.Serialize(this);
			}
			catch (Exception)
			{
			}
			return "";
		}



		public bool SaveXml(string strFilePath)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(UpdateStatus));
				using (FileStream fs = new FileStream(strFilePath, FileMode.Create))
				{
					serializer.Serialize(fs, this);
					fs.Close();
				}
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}



		public static UpdateStatus? LoadXml(string strFilePath)
		{
			if (string.IsNullOrEmpty(strFilePath))
				return null;
			if (File.Exists(strFilePath) == false)
				return null;

			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(UpdateStatus));

				using (FileStream fs = new FileStream(strFilePath, FileMode.Open))
				{
					var load = (UpdateStatus?)serializer.Deserialize(fs);
					fs.Close();
					return load;
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

	}






	public class SoftInfo : INotifyPropertyChanged
	{
		public string strGuid { set; get; } = "";
		public string strName { set; get; } = "";
		public string strVer { set; get; } = "";
		public int nVer { set; get; } = 0;

		public DateTime dtUTC { set; get; } = DateTime.MinValue;


		[JsonIgnore]
		public string strDownloadURL { set; get; } = "";



		[XmlIgnore]
		[JsonIgnore]
		public DateTime dtDate
		{
			set
			{
				dtUTC = value.ToUniversalTime();
			}
			get
			{
				return dtUTC.ToLocalTime();
			}
		}



		public event PropertyChangedEventHandler? PropertyChanged;

		public void OnPropertyChanged(string PropertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
		}
	}


	public class DonationInfo
	{
		public decimal dUsd { set; get; } = 0;

		public int nCount { set; get; } = 0;

		public DateTime dtUTC { set; get; } = DateTime.MinValue;


		[XmlIgnore]
		[JsonIgnore]
		public DateTime dtDate
		{
			set
			{
				dtUTC = value.ToUniversalTime();
			}
			get
			{
				return dtUTC.ToLocalTime();
			}
		}
	}
}
