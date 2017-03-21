using Garson.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
		System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();

		ObservableCollection<ResultInfo> records = new ObservableCollection<ResultInfo>();
		private static FileSystemWatcher fileSystemWatcher;
		public MainWindow()
		{
			InitializeComponent();
			listView.ItemsSource = records;
			CommandBinding cb = new CommandBinding(ApplicationCommands.Copy, CopyCmdExecuted, CopyCmdCanExecute);
			listView.CommandBindings.Add(cb);
			notifyIcon.Icon = Garson.Properties.Resources.DefaultState;
			notifyIcon.Visible = true;
			notifyIcon.DoubleClick +=
				delegate (object sender, EventArgs args)
				{
					if (this.WindowState == WindowState.Normal)
					{
						this.Hide();
						this.WindowState = WindowState.Minimized;
					}
					else if(this.WindowState == WindowState.Minimized)
					{
						this.Show();
						this.WindowState = WindowState.Normal;
					}
				};
			notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
			notifyIcon.ContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				new System.Windows.Forms.MenuItem("&Hide", Item_Click),
				new System.Windows.Forms.MenuItem("&Exit", Item_Click)
			});


			RotateTransform rotateTransform = new RotateTransform(45);
			//notifyIcon.Icon.
				// img.RenderTransform = rotateTransform;

		}

		private void Item_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.MenuItem item = sender as System.Windows.Forms.MenuItem;
			if (item == null) return;
			if(item.Text == "&Show")
			{
				item.Text = "&Hide";
				this.Show();
				this.WindowState = WindowState.Normal;
			}
			else if(item.Text == "&Hide")
			{
				item.Text = "&Show";
				this.Hide();
				this.WindowState = WindowState.Minimized;
			}
			else if(item.Text == "&Exit")
			{
				this.Close();
			}
		}

		void CopyCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			try
			{
				ListView lw = e.OriginalSource as ListView;
				string copyContent = String.Empty;
				// The SelectedItems could be ListBoxItem instances or data bound objects depending on how you populate the ListBox.  
				foreach (ResultInfo info in lw.SelectedItems)
				{
					copyContent += info.Link;

					// Add a NewLine for carriage return  
					copyContent += Environment.NewLine;
				}
				Clipboard.SetText(copyContent);
			}
			catch (Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}
		void CopyCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			try
			{
				ListBox lb = e.OriginalSource as ListBox;
				// CanExecute only if there is one or more selected Item.  
				if (lb.SelectedItems.Count > 0)
					e.CanExecute = true;
				else
					e.CanExecute = false;
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				Settings.Default.Save();
				if (notifyIcon != null)
				{
					notifyIcon.Visible = false;
					if (notifyIcon.Icon != null)
					{
						notifyIcon.Icon.Dispose();
						notifyIcon.Icon = null;
					}
						notifyIcon.Dispose();
						notifyIcon = null;
				}
				StopFileSystemWatcher();
			}
			catch (Exception err) { MessageBox.Show(err.Message); }
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
				notifyIcon.Icon = Garson.Properties.Resources.SyncState;
			}));
			

			if (info.EventType == WatcherChangeTypes.Created)
			{
				info.UploadStatus = Upload.UploadStatus.WAITING;
				string filePath = System.IO.Path.Combine(Settings.Default.Folder, info.FileName);
				if (GetIdleFile(filePath) == false)
				{
					return;
				}

				Upload upload = new Upload(Settings.Default.ApiKey, Settings.Default.UserName, Settings.Default.SiteUrl, Settings.Default.Folder);
				Upload.UploadResult result = upload.UploadPicture(filePath);
				info.UploadStatus = result.status;
				info.Link = result.url;
				
			}

			Dispatcher.BeginInvoke(new Action(() =>
			{
				notifyIcon.Icon = Garson.Properties.Resources.DefaultState;
			}));
		}


		void DisplayFileSystemWatcherInfo(System.IO.WatcherChangeTypes watcherChangeTypes, string name, string oldName = null)
		{
			if (watcherChangeTypes == System.IO.WatcherChangeTypes.Created)
			{
				
				Thread th = new Thread(ProcessItem);
				th.IsBackground = true;
				th.Start(new ResultInfo(name, watcherChangeTypes));
			}

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
