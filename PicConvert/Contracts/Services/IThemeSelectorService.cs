﻿using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace PicConvert.Contracts.Services;

public interface IThemeSelectorService
{
	ElementTheme Theme
	{
		get;
	}

	Task InitializeAsync();

	Task SetThemeAsync(ElementTheme theme);

	Task SetRequestedThemeAsync();
}
