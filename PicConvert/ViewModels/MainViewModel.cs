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
using PicConvert.Core.Contracts.Services;
using PicConvert.Core.Models;
using PicConvert.Helpers;
using System.IO;

namespace PicConvert.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<FileItemModel> selectedFiles;

		[ObservableProperty]
		private object selectedFormat;

		[ObservableProperty]
		private StorageFolder selectedFolder;


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
        public ICommand ConvertCommand { get; }
        public ICommand SelectFolderCommand { get; }

        public MainViewModel()
        {
            SelectedFiles = new ObservableCollection<FileItemModel>();
            Quality = 75; // Standardvärde för Quality
            Size = 50; // Standardvärde för Size
            //SelectedFormat = ImageFormats.WebP; // Standardvärde för SelectedFormat
            
			OpenFilePickerCommand = new RelayCommand(async () => await OpenFilePicker());
            SelectAllCommand = new RelayCommand(SelectAllFiles);
            RemoveSelectedCommand = new RelayCommand(RemoveSelectedFiles);
            ConvertCommand = new RelayCommand(async () => await ConvertFilesAsync());
            SelectFolderCommand = new RelayCommand(async () => await SelectFolderAsync());
        }

        private async Task OpenFilePicker()
        {
            var openPicker = new FileOpenPicker();

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
					// SelectFolderAsync

                    //var folder = await SelectFolderAsync();
                    
					fileItems.Add(new FileItemModel
                    {
                        Path = file.Path,
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

		private async Task ConvertFilesAsync()
		{
			if (SelectedFolder == null)
			{
				// Hantera fallet där ingen mapp är vald
				return;
			}

			var conversionService = App.GetService<IFileConversionService>();

			foreach (var file in SelectedFiles)
			{
				if (file.IsSelected)
				{
					var newFilePath = Path.Combine(SelectedFolder.Path, file.Name);

					await conversionService.ConvertFileAsync(new FileItem
					{
						Path = file.Path,
						Name = file.Name,
						Format = file.Format.ToString(),
						Size = file.Size
					}, SelectedFormat.ToString(), Quality, Size.ToString(), SkipMetadata, newFilePath);
				}
			}
		}


		private async Task SelectFolderAsync()
        {
            var folderPicker = new FolderPicker();
            var window = App.MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hWnd);

            var folder = await folderPicker.PickSingleFolderAsync();
			if (folder != null)
			{
				SelectedFolder = folder;
			}
		}
    }
}
