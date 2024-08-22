using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using PicConvert.Contracts.Services;
using PicConvert.Core.Contracts.Services;
using PicConvert.Core.Models;
using PicConvert.Helpers;
using PicConvert.Models;
using PicConvert.Services;
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
		private readonly IDialogService _dialogService;
		private readonly ResourceLoader _loader;

		// Observable properties automatically notify the UI when they change
		[ObservableProperty]
		private ObservableCollection<FileItemModel> inputImages = new();

		[ObservableProperty]
		private object selectedFormat = ImageFormats.WebP; // Default to WebP format

		[ObservableProperty]
		private StorageFolder selectedFolder;

		[ObservableProperty]
		private int quality = 75; // Default quality

		[ObservableProperty]
		private int size = 100; // Default size

		[ObservableProperty]
		private bool skipMetadata;

		[ObservableProperty]
		private bool nullSetting;

		// Property för att aktivera sammanslagning av bilder till en PDF
		[ObservableProperty]
		private bool mergeToPdf;

		// Property för att kontrollera om PDF är valt
		[ObservableProperty]
		private bool isPdfSelected;

		// ICommand properties for UI bindings
		public ICommand OpenFilePickerCommand { get; }
		public ICommand SelectAllCommand { get; }
		public ICommand RemoveSelectedCommand { get; }
		public ICommand ConvertCommand { get; }
		public ICommand SelectFolderCommand { get; }
		public ICommand NullSettingCommand { get; }

		public MainViewModel(IDialogService dialogService)
		{
			// Initialize commands with their respective methods
			_dialogService = dialogService;
			_loader = new ResourceLoader();
			OpenFilePickerCommand = new RelayCommand(async () => await OpenFilePickerAsync());
			SelectAllCommand = new RelayCommand(SelectAllFiles);
			RemoveSelectedCommand = new RelayCommand(async () => await RemoveSelectedFilesAsync());
			ConvertCommand = new RelayCommand(async () => await ConvertFilesAsync());
			SelectFolderCommand = new RelayCommand(async () => await SelectFolderAsync());
			NullSettingCommand = new RelayCommand(DefaultSetting);
		}

		// Method to set the default values
		private void DefaultSetting()
		{
			Quality = 75;
			Size = 100;
			SkipMetadata = false;
		}
		// when the selected format changes, check if it is PDF
		partial void OnSelectedFormatChanged(object value)
		{
			IsPdfSelected = value != null && value.Equals(ImageFormats.PDF);
		}

		// Method to open a file picker and allow the user to select multiple images
		private async Task OpenFilePickerAsync()
		{
			var openPicker = new FileOpenPicker();

			// Initialize the file picker with the app window handle
			var window = App.MainWindow;
			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
			WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

			openPicker.ViewMode = PickerViewMode.Thumbnail;
			openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
			foreach (var format in Enum.GetValues(typeof(InputImageFormats)).Cast<InputImageFormats>())
			{
				openPicker.FileTypeFilter.Add(FileFormatHelper.GetFileExtension(format));
			}

			var files = await openPicker.PickMultipleFilesAsync();

			if (files != null && files.Any())
			{
				var fileItems = files.Select(async file =>
				{
					var properties = await file.GetBasicPropertiesAsync();
					return new FileItemModel
					{
						Path = file.Path,
						Name = file.Name,
						Format = file.FileType,
						Size = (int)(properties.Size / 1024), // size in KB		
						DisplaySize = properties.Size < 1024 * 1024 ? $"{properties.Size / 1024} KB" : $"{properties.Size / (1024 * 1024):F2} MB",
						IsSelected = false
					};
				});

				SelectFiles(await Task.WhenAll(fileItems));
			}
		}

		// Method to add selected files to the collection, ensuring no duplicates
		private void SelectFiles(IEnumerable<FileItemModel> files)
		{
			if (files == null) return;

			foreach (var file in files)
			{
				if (!InputImages.Any(f => f.Path == file.Path))
				{
					InputImages.Add(file);
				}
			}
		}

		// Method to select all files in the collection
		private void SelectAllFiles()
		{
			foreach (var file in InputImages)
			{
				file.IsSelected = true;
			}
		}

		// Method to remove selected files from the collection
		private async Task RemoveSelectedFilesAsync()
		{
			var filesToRemove = InputImages.Where(file => file.IsSelected).ToList();
			if (!filesToRemove.Any())
			{
				await _dialogService.ShowMessageDialogAsync(
					_loader.GetString("Main_RemoveSelectedFiles_CD_Title"),
					_loader.GetString("Main_RemoveSelectedFiles_CD_Content"));
				return;
			}

			foreach (var file in filesToRemove)
			{
				InputImages.Remove(file);
			}
		}


		// Method to convert selected files to the selected format
		private async Task ConvertFilesAsync()
		{
			if (!await ValidateInputsAsync()) return;

			// Create a cancellation token source to allow cancellation of the conversion process
			cancellationTokenSource = new CancellationTokenSource();

			// Show progress dialog
			var progressDialog = await _dialogService.ShowProgressDialogAsync(cancellationTokenSource);
			Action<int> reportProgress = value =>
			{
				progressDialog.SetProgress(value);
				progressDialog.SetStatus($"{_loader.GetString("Main_ConvertFiles_CD_Converting")}, {value}% {_loader.GetString("Main_ConvertFiles_CD_complete")}");


			};

			try
			{
				var conversionService = App.GetService<IFileConversionService>();
				int totalFiles = InputImages.Count;
				int processedFiles = 0;

				if (SelectedFormat.ToString() == "PDF" && MergeToPdf == true)
				{
					var pdfFilePath = Path.Combine(SelectedFolder.Path, "MergedImages.pdf");
					await conversionService.MergeImagesToPdfAsync(InputImages.Select(file => new FileItem
					{
						Path = file.Path,
						Name = file.Name,
						Format = file.Format,
						Size = file.Size
					}), pdfFilePath, Quality, cancellationTokenSource.Token);
					progressDialog.Hide();
					await _dialogService.ShowMessageDialogAsync(
						_loader.GetString("Main_ConvertFiles_CD_OK_PDF_Title"),
						_loader.GetString("Main_ConvertFiles_CD_OK_PDF_Content"));
				}
				else
				{
					foreach (var file in InputImages)
					{
						cancellationTokenSource.Token.ThrowIfCancellationRequested();

						var newFilePath = Path.Combine(SelectedFolder.Path, file.Name);
						await conversionService.ConvertFileAsync(new FileItem
						{
							Path = file.Path,
							Name = file.Name,
							Format = file.Format,
							Size = file.Size
						}, SelectedFormat.ToString(), Quality, Size.ToString(), SkipMetadata, newFilePath, cancellationTokenSource.Token);

						processedFiles++;
						reportProgress((int)((processedFiles / (double)totalFiles) * 100));
					}
					progressDialog.Hide();					
					await _dialogService.ShowMessageDialogAsync(
						_loader.GetString("Main_ConvertFiles_CD_OK_Title"),
						_loader.GetString("Main_ConvertFiles_CD_OK_Content"));
				}

				progressDialog.Hide();
			}
			catch (OperationCanceledException)
			{				
				await _dialogService.ShowMessageDialogAsync(
						_loader.GetString("Main_ConvertFiles_CD_Cancelled_Title"),
						_loader.GetString("Main_ConvertFiles_CD_Cancelled_Content"));
			}
			finally
			{
				progressDialog.Hide(); // Säkerställ att dialogen stängs vid avbrytande
			}
		}


		// Method to select a folder for saving converted files
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

		// Method to validate user inputs before starting the conversion process
		private async Task<bool> ValidateInputsAsync()
		{
			if (InputImages == null || !InputImages.Any())
			{				
				await _dialogService.ShowMessageDialogAsync(
						_loader.GetString("Main_ValidateInputs_CD_InputImages_Title"),
						_loader.GetString("Main_ValidateInputs_CD_InputImages_Content"));
				return false;
			}
			if (SelectedFolder == null)
			{				
				await _dialogService.ShowMessageDialogAsync(
						_loader.GetString("Main_ValidateInputs_CD_SelectedFolder_Title"),
						_loader.GetString("Main_ValidateInputs_CD_SelectedFolder_Content"));
				return false;
			}

			return true;
		}

	}
}
