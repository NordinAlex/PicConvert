using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using PicConvert.Contracts.Services;
using PicConvert.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Input;
using Windows.ApplicationModel;

using Windows.Globalization;

namespace PicConvert.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
	private readonly IThemeSelectorService _themeSelectorService;
	private readonly ResourceLoader _settingsResourceLoader;

	[ObservableProperty]
	private ElementTheme _elementTheme;

	[ObservableProperty]
	private string _versionDescription;

	[ObservableProperty]
	private CultureInfo _selectedLanguage;

	public ObservableCollection<CultureInfo> AvailableLanguages
	{
		get;
	} = new ObservableCollection<CultureInfo>
	{
		new CultureInfo("en-US"),
		new CultureInfo("sv-SE")
	};

	public ICommand SwitchThemeCommand
	{
		get;
	}

	public SettingsViewModel(IThemeSelectorService themeSelectorService)
	{
		_themeSelectorService = themeSelectorService;
		_settingsResourceLoader = new ResourceLoader(); // Specifikt resursnamn
		_elementTheme = _themeSelectorService.Theme;
		_versionDescription = GetVersionDescription();
		_selectedLanguage = CultureInfo.CurrentCulture;

		SwitchThemeCommand = new RelayCommand<ElementTheme>(
			async (param) =>
			{
				if (ElementTheme != param)
				{
					ElementTheme = param;
					await _themeSelectorService.SetThemeAsync(param);
				}
			});

		_selectedLanguage = new CultureInfo(ApplicationLanguages.PrimaryLanguageOverride);
	}

	partial void OnSelectedLanguageChanged(CultureInfo value)
	{
		ApplicationLanguages.PrimaryLanguageOverride = value.Name;
		// Reload the app to load the new language		
		// Ask the user if they want to restart the app now
		ShowRestartDialog();



	}
	private async void ShowRestartDialog()
	{
		var dialog = new ContentDialog
		{
			Title = _settingsResourceLoader.GetString("RestartDialogTitle"),
			Content = _settingsResourceLoader.GetString("RestartDialogContent"),
			PrimaryButtonText = _settingsResourceLoader.GetString("RestartDialogPrimaryButtonText"),
			CloseButtonText = _settingsResourceLoader.GetString("RestartDialogCloseButtonText"),
			DefaultButton = ContentDialogButton.Primary,
			XamlRoot = App.MainWindow.Content.XamlRoot
		};

		var result = await dialog.ShowAsync();

		if (result == ContentDialogResult.Primary)
		{
			// Restart the application
			RestartApplication();
		}
	}

	private void RestartApplication()
	{
		var process = new ProcessStartInfo
		{
			FileName = Process.GetCurrentProcess().MainModule.FileName,
			UseShellExecute = true
		};
		Process.Start(process);
		Application.Current.Exit();
	}



	private static string GetVersionDescription()
	{
		Version version;

		if (RuntimeHelper.IsMSIX)
		{
			var packageVersion = Package.Current.Id.Version;

			version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
		}
		else
		{
			version = Assembly.GetExecutingAssembly().GetName().Version!;
		}

		return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
	}
}

