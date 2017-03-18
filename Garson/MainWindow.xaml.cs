﻿using Garson.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Garson
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ObservableCollection<ResultInfo> records = new ObservableCollection<ResultInfo>();
		private static FileSystemWatcher fileSystemWatcher;
		public MainWindow()
		{
			InitializeComponent();
			listView.ItemsSource = records;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Settings.Default.Save();
		}
		private void buttonEditApiKey_Click(object sender, RoutedEventArgs e)
		{
			if (buttonEditApiKey.Content.Equals("Save"))
			{
				textBoxApiKey.IsEnabled = false;
				buttonEditApiKey.Content = "Edit";
				this.Focus();
			}
			else
			{
				textBoxApiKey.IsEnabled = true;
				textBoxApiKey.Focus();
				buttonEditApiKey.Content = "Save";
			}
		}

		private void buttonEditUserName_Click(object sender, RoutedEventArgs e)
		{
			if (buttonEditUserName.Content.Equals("Save"))
			{
				textBoxUserName.IsEnabled = false;
				buttonEditUserName.Content = "Edit";
				this.Focus();
			}
			else
			{
				textBoxUserName.IsEnabled = true;
				textBoxUserName.Focus();
				buttonEditUserName.Content = "Save";
			}
		}

		private void buttonEditSiteUrl_Click(object sender, RoutedEventArgs e)
		{
			if (buttonEditSiteUrl.Content.Equals("Save"))
			{
				textBoxSiteUrl.IsEnabled = false;
				buttonEditSiteUrl.Content = "Edit";
				this.Focus();
			}
			else
			{
				textBoxSiteUrl.IsEnabled = true;
				textBoxSiteUrl.Focus();
				buttonEditSiteUrl.Content = "Save";
			}
		}

		private void buttonEditFolder_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

			dialog.SelectedPath = Settings.Default.Folder;

			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
			
			if (!Directory.Exists(dialog.SelectedPath))
			{
				return;
			}
			textBoxFolder.Text = dialog.SelectedPath;			
		}

		private void buttonEnableSync_Click(object sender, RoutedEventArgs e)
		{
			StartFileSystemWatcher();
			buttonEnableSync.IsEnabled = false;
			buttonDisableSync.IsEnabled = true;

			buttonEditApiKey.IsEnabled = false;
			buttonEditUserName.IsEnabled = false;
			buttonEditSiteUrl.IsEnabled = false;
			buttonEditFolder.IsEnabled = false;
		}

		private void buttonDisableSync_Click(object sender, RoutedEventArgs e)
		{
			StopFileSystemWatcher();
			buttonEnableSync.IsEnabled = true;
			buttonDisableSync.IsEnabled = false;

			buttonEditApiKey.IsEnabled = true;
			buttonEditUserName.IsEnabled = true;
			buttonEditSiteUrl.IsEnabled = true;
			buttonEditFolder.IsEnabled = true;
		}

		private void StartFileSystemWatcher()
		{
			string folderPath = Settings.Default.Folder;

			// If there is no folder selected, to nothing
			if (string.IsNullOrWhiteSpace(folderPath))
				return;

			StopFileSystemWatcher();

			// Recreate
			fileSystemWatcher = new FileSystemWatcher();
			// Set folder path to watch
			fileSystemWatcher.Path = folderPath;
			fileSystemWatcher.Filter = "*.jpg";

			// Gets or sets the type of changes to watch for.
			// In this case we will watch change of filename, last modified time, size and directory name
			fileSystemWatcher.NotifyFilter = NotifyFilters.FileName |
				NotifyFilters.LastWrite |
				NotifyFilters.Size |
				NotifyFilters.DirectoryName;


			// Event handlers that are watching for specific event
			fileSystemWatcher.Created += new FileSystemEventHandler(fileSystemWatcher_Created);
			fileSystemWatcher.Changed += new FileSystemEventHandler(fileSystemWatcher_Changed);
			fileSystemWatcher.Deleted += new FileSystemEventHandler(fileSystemWatcher_Deleted);
			fileSystemWatcher.Renamed += new RenamedEventHandler(fileSystemWatcher_Renamed);

			// NOTE: If you want to monitor specified files in folder, you can use this filter
			// fileSystemWatcher.Filter

			// START watching
			fileSystemWatcher.EnableRaisingEvents = true;
		}
		private void StopFileSystemWatcher()
		{
			if (fileSystemWatcher == null) return;
			fileSystemWatcher.EnableRaisingEvents = false;
			fileSystemWatcher.Created -= fileSystemWatcher_Created;
			fileSystemWatcher.Changed -= fileSystemWatcher_Changed;
			fileSystemWatcher.Deleted -= fileSystemWatcher_Deleted;
			fileSystemWatcher.Renamed -= fileSystemWatcher_Renamed;
			fileSystemWatcher.Dispose();
			fileSystemWatcher = null;
		}

		// ----------------------------------------------------------------------------------
		// Events that do all the monitoring
		// ----------------------------------------------------------------------------------

		void fileSystemWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
		{
			DisplayFileSystemWatcherInfo(e.ChangeType, e.Name);
		}

		void fileSystemWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
		{
			DisplayFileSystemWatcherInfo(e.ChangeType, e.Name);
		}

		void fileSystemWatcher_Deleted(object sender, System.IO.FileSystemEventArgs e)
		{
			DisplayFileSystemWatcherInfo(e.ChangeType, e.Name);
		}

		void fileSystemWatcher_Renamed(object sender, System.IO.RenamedEventArgs e)
		{
			DisplayFileSystemWatcherInfo(e.ChangeType, e.Name, e.OldName);
		}

		// ----------------------------------------------------------------------------------

		private bool IsFileLocked(FileInfo file)
		{
			FileStream stream = null;

			try
			{
				stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			}
			catch (IOException)
			{
				//the file is unavailable because it is:
				//still being written to
				//or being processed by another thread
				//or does not exist (has already been processed)
				return true;
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}

			//file is not locked
			return false;
		}


		private static bool GetIdleFile(string path)
		{
			var fileIdle = false;
			const int MaximumAttemptsAllowed = 30;
			var attemptsMade = 0;

			while (!fileIdle && attemptsMade <= MaximumAttemptsAllowed)
			{
				try
				{
					//Console.WriteLine(attemptsMade + " Try: " + path);
					using (File.Open(path, FileMode.Open, FileAccess.ReadWrite))
					{
						fileIdle = true;
					}
				}
				catch
				{
					attemptsMade++;
					Thread.Sleep(100);
				}
			}

			return fileIdle;
		}

		private void ProcessItem(object obj)
		{
			ResultInfo info = obj as ResultInfo;
			if (info == null) return;

			Dispatcher.BeginInvoke(new Action(() => {
				records.Add(info);
			}));

			if (info.EventType == WatcherChangeTypes.Created)
			{
				string filePath = System.IO.Path.Combine(Settings.Default.Folder, info.FileName);
				if (GetIdleFile(filePath) == false)
				{
					return;
				}

				Upload upload = new Upload(Settings.Default.ApiKey, Settings.Default.UserName, Settings.Default.SiteUrl, Settings.Default.Folder);
				info.UploadStatus = upload.UploadPicture(filePath);
			}
		}


		void DisplayFileSystemWatcherInfo(System.IO.WatcherChangeTypes watcherChangeTypes, string name, string oldName = null)
		{
			Thread th = new Thread(ProcessItem);
			th.IsBackground = true;
			th.Start(new ResultInfo(name, watcherChangeTypes));

			//if (watcherChangeTypes == System.IO.WatcherChangeTypes.Renamed)
			//{
			//	// When using FileSystemWatcher event's be aware that these events will be called on a separate thread automatically!!!
			//	// If you call method AddListLine() in a normal way, it will throw following exception: 
			//	// "The calling thread cannot access this object because a different thread owns it"
			//	// To fix this, you must call this method using Dispatcher.BeginInvoke(...)!
			//	Dispatcher.BeginInvoke(new Action(() => { AddListLine(string.Format("{0} -> {1} to {2} - {3}", watcherChangeTypes.ToString(), oldName, name, DateTime.Now)); }));
			//}
			//else
			//{
			//	Dispatcher.BeginInvoke(new Action(() => { AddListLine(string.Format("{0} -> {1} - {2}", watcherChangeTypes.ToString(), name, DateTime.Now)); }));
			//}
		}

	}
}
