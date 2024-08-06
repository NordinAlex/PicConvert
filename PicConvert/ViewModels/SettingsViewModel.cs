using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using PicConvert.Contracts.Services;
using PicConvert.Helpers;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;

namespace PicConvert.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
	private readonly IThemeSelectorService _themeSelectorService;

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

