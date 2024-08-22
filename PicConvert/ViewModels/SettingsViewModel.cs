using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using PicConvert.Contracts.Services;
using PicConvert.Helpers;
using PicConvert.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Globalization;

namespace PicConvert.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
	private readonly IThemeSelectorService _themeSelectorService;
	private readonly ResourceLoader _settingsResourceLoader;
	private readonly IDialogService _dialogService;

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

	public SettingsViewModel(IThemeSelectorService themeSelectorService,IDialogService dialogService)
	{
		_themeSelectorService = themeSelectorService;
		_settingsResourceLoader = new ResourceLoader(); // Specifikt resursnamn
		_elementTheme = _themeSelectorService.Theme;
		_versionDescription = GetVersionDescription();
		_selectedLanguage = CultureInfo.CurrentCulture;
		_dialogService = dialogService;
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
		ShowRestartDialog();
	}
	
	private async void ShowRestartDialog()
	{
		var result = await _dialogService.ShowCustomDialogAsync(
			_settingsResourceLoader.GetString("ConDialog_RestartDialogTitle"),
			_settingsResourceLoader.GetString("ConDialog_RestartDialogContent"),
			_settingsResourceLoader.GetString("ConDialog_RestartDialogPrimaryButtonText"),
			_settingsResourceLoader.GetString("ConDialog_RestartDialogCloseButtonText"),
			ContentDialogButton.Primary);

		if (result == ContentDialogResult.Primary)
		{
			// Restart the application
			RestartApplication();
		}
	}
	private static void RestartApplication()
	{
		// The restart will be executed immediately.
		AppRestartFailureReason failureReason = Microsoft.Windows.AppLifecycle.AppInstance.Restart(string.Empty);

		// If the restart fails, handle it here.
		switch (failureReason)
		{
			case AppRestartFailureReason.RestartPending:
				break;
			case AppRestartFailureReason.NotInForeground:
				break;
			case AppRestartFailureReason.InvalidUser:
				break;
			default:
				break;
		}
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

