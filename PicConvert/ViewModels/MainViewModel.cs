using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using PicConvert.Models;
using System;

namespace PicConvert.ViewModels
{
	public partial class MainViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<FileItemModel> selectedFiles;

		[ObservableProperty]
		private FileFormats selectedFormat;

		[ObservableProperty]
		private int quality;

		[ObservableProperty]
		private int size;

		[ObservableProperty]
		private bool skipMetadata;

		[ObservableProperty]
		private bool nullSetting;

		public ICommand OpenFilePickerCommand { get; }
		public ICommand SelectAllCommand { get; }
		public ICommand RemoveSelectedCommand { get; }

		public MainViewModel()
		{
			SelectedFiles = new ObservableCollection<FileItemModel>();
			OpenFilePickerCommand = new RelayCommand(async () => await OpenFilePicker());
			SelectAllCommand = new RelayCommand(SelectAllFiles);
			RemoveSelectedCommand = new RelayCommand(RemoveSelectedFiles);
		}

		private async Task OpenFilePicker()
		{
			var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

			var window = App.MainWindow;
			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
			WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

			openPicker.ViewMode = PickerViewMode.Thumbnail;
			openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
			openPicker.FileTypeFilter.Add(".jpg");
			openPicker.FileTypeFilter.Add(".jpeg");
			openPicker.FileTypeFilter.Add(".png");

			var files = await openPicker.PickMultipleFilesAsync();

			if (files != null)
			{
				var fileItems = new List<FileItemModel>();

				foreach (var file in files)
				{
					var properties = await file.GetBasicPropertiesAsync();
					fileItems.Add(new FileItemModel
					{
						Name = file.Name,
						Format = file.FileType,
						Size = (int)(properties.Size / 1024), // storlek i KB
						IsSelected = false
					});
				}

				SelectFiles(fileItems);
			}
		}

		private void SelectFiles(IEnumerable<FileItemModel> files)
		{
			if (files == null)
			{
				return;
			}

			foreach (var file in files)
			{
				if (file == null)
				{
					continue;
				}

				if (!SelectedFiles.Contains(file))
				{
					SelectedFiles.Add(file);
				}
			}
		}

		private void SelectAllFiles()
		{
			foreach (var file in SelectedFiles)
			{
				file.IsSelected = true;
			}
		}

		private void RemoveSelectedFiles()
		{
			var filesToRemove = SelectedFiles.Where(file => file.IsSelected).ToList();

			foreach (var file in filesToRemove)
			{
				SelectedFiles.Remove(file);
			}
		}
	}
}
