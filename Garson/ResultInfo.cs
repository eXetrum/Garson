using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garson
{
	public class ResultInfo : INotifyPropertyChanged
	{
		DateTime dateTime;
		string fileName;
		WatcherChangeTypes eventType;
		Upload.UploadStatus uploadStatus;
		string link;
		
		public ResultInfo(string fileName, WatcherChangeTypes eventType)
		{
			this.DateTime = DateTime.Now;
			this.FileName = fileName;
			this.EventType = eventType;
			this.UploadStatus = Upload.UploadStatus.NONE;
			this.link = "";
		}

		public override bool Equals(object obj)
		{
			// If parameter is null return false.
			if (obj == null)
			{
				return false;
			}

			// If parameter cannot be cast to Point return false.
			ResultInfo p = obj as ResultInfo;
			if (p == null)
			{
				return false;
			}

			// Return true if the fields match:
			return DateTime.Equals(p.DateTime)
				&& FileName.Equals(p.FileName)
				&& EventType.Equals(p.EventType)
				&& UploadStatus.Equals(p.UploadStatus)
				&& Link.Equals(p.Link);
		}

		public bool Equals(ResultInfo p)
		{
			// If parameter is null return false:
			if ((object)p == null)
			{
				return false;
			}

			// Return true if the fields match:
			return DateTime.Equals(p.DateTime)
				&& FileName.Equals(p.FileName)
				&& EventType.Equals(p.EventType)
				&& UploadStatus.Equals(p.UploadStatus)
				&& Link.Equals(p.Link);
		}

		public override int GetHashCode()
		{
			return DateTime.GetHashCode()
				^ FileName.GetHashCode()
				^ EventType.GetHashCode()
				^ UploadStatus.GetHashCode()
				^ Link.GetHashCode();
		}

		public DateTime DateTime
		{
			get
			{
				return dateTime;
			}
			set
			{
				if(value != dateTime)
				{
					dateTime = value;
					OnPropertyChanged("DateTime");
				}
			}
		}
		public WatcherChangeTypes EventType
		{
			get
			{
				return eventType;
			}
			set
			{
				if (value != eventType)
				{
					eventType = value;
					OnPropertyChanged("EventType");
				}
			}
		}
		public string FileName
		{
			get
			{
				return fileName;
			}
			set
			{
				if (value != fileName)
				{
					fileName = value;
					OnPropertyChanged("FileName");
				}
			}
		}
		public Upload.UploadStatus UploadStatus
		{
			get
			{
				return uploadStatus;
			}
			set
			{
				if (value != uploadStatus)
				{
					uploadStatus = value;
					OnPropertyChanged("UploadStatus");
				}
			}
		}
		public string Link
		{
			get
			{
				return link;
			}
			set
			{
				if(value != link)
				{
					link = value;
					OnPropertyChanged("Link");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		// Create the OnPropertyChanged method to raise the event
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}
	}
}
