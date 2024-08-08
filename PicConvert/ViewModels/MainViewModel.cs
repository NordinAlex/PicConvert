using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;
using PicConvert.Core.Contracts.Services;
using PicConvert.Core.Models;
using PicConvert.Models;
using PicConvert.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT;

namespace PicConvert.ViewModels
{
	public partial class MainViewModel : ObservableObject
	{
		private CancellationTokenSource cancellationTokenSource;

		[ObservableProperty]
		private ObservableCollection<FileItemModel> inputImages;

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
			InputImages = new ObservableCollection<FileItemModel>();
			Quality = 75; // Standardvärde för Quality
			Size = 50; // Standardvärde för Size
			SelectedFormat = ImageFormats.WebP; // Standardvärde för SelectedFormat

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

				if (!InputImages.Contains(file))
				{
					InputImages.Add(file);
				}
			}
		}

		private void SelectAllFiles()
		{
			foreach (var file in InputImages)
			{
				file.IsSelected = true;
			}
		}

		private void RemoveSelectedFiles()
		{
			var filesToRemove = InputImages.Where(file => file.IsSelected).ToList();

			foreach (var file in filesToRemove)
			{
				InputImages.Remove(file);
			}
		}

		private async Task ConvertFilesAsync()
		{
			if (SelectedFolder == null)
			{
				var dialog = new ContentDialog
				{
					Title = "Fel",
					Content = "Välj filer och en mapp att spara till.",
					CloseButtonText = "OK",
					XamlRoot = App.MainWindow.Content.XamlRoot
				};
				await dialog.ShowAsync();
				return;
			}

			// Skapa en ny CancellationTokenSource för denna operation
			cancellationTokenSource = new CancellationTokenSource();

			// Skapa och visa popup-fönster för konverteringsstatus
			var progressDialog = new ProgressDialog(cancellationTokenSource);
			progressDialog.XamlRoot = App.MainWindow.Content.XamlRoot;
			var progressDialogTask = progressDialog.ShowAsync();
			
			
			//var progressvs = new Progress<int>(value =>
			//{
			//	progressDialog.SetProgress(value);
			//	progressDialog.SetStatus($"Konverterar... {value}% klart");
			//});
			Action<int> reportProgress = (value) =>
			{
				progressDialog.SetProgress(value);
				progressDialog.SetStatus($"Konverterar... {value}% klart");
			};
				

			try
			{
				var conversionService = App.GetService<IFileConversionService>();
				int totalFiles = InputImages.Count;
				int processedFiles = 0;

				foreach (var file in InputImages)
				{
					cancellationTokenSource.Token.ThrowIfCancellationRequested();

					var newFilePath = Path.Combine(SelectedFolder.Path, file.Name);
					await conversionService.ConvertFileAsync(new FileItem
					{
						Path = file.Path,
						Name = file.Name,
						Format = file.Format.ToString(),
						Size = file.Size
					}, SelectedFormat.ToString(), Quality, Size.ToString(), SkipMetadata, newFilePath, cancellationTokenSource.Token);

					processedFiles++;
					//int progressValue = (int)((double)processedFiles / totalFiles * 100);
					int progressValue = (int)((processedFiles / (double)totalFiles) * 100);					
					reportProgress(progressValue);
				}

				 //await progressDialogTask; // Vänta tills progress-dialogen är klar att stängas
				progressDialog.Hide();
				// Stäng dialogen efter att konverteringen är klar
				var successDialog = new ContentDialog
				{
					Title = "Klar",
					Content = "Filerna har konverterats.",
					CloseButtonText = "OK",
					XamlRoot = App.MainWindow.Content.XamlRoot
				};
				await successDialog.ShowAsync();



			}
			catch (OperationCanceledException)
			{
				var cancelledDialog = new ContentDialog
				{
					Title = "Avbruten",
					Content = "Konverteringsprocessen har avbrutits.",
					CloseButtonText = "OK",
					XamlRoot = App.MainWindow.Content.XamlRoot
				};
				await cancelledDialog.ShowAsync();
			}
			finally
			{
				progressDialog.Hide(); // Se till att dialogen stängs även vid avbrott
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
