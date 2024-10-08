﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using PicConvert.Activation;
using PicConvert.Contracts.Services;
using PicConvert.Core.Contracts.Services;
using PicConvert.Core.Services;
using PicConvert.Helpers;
using PicConvert.Models;
using PicConvert.Services;
using PicConvert.ViewModels;
using PicConvert.Views;
using PicConvert.Views.CustomDialogViews;
using System;

namespace PicConvert
{
	public partial class App : Application
	{
		public IHost Host { get; }

		public static T GetService<T>()
			where T : class
		{
			if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
			{
				throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
			}

			return service;
		}

		public static WindowEx MainWindow { get; } = new MainWindow();

		public static UIElement AppTitlebar { get; set; }

		public App()
		{
			InitializeComponent();
			Host = Microsoft.Extensions.Hosting.Host.
			CreateDefaultBuilder().
			UseContentRoot(AppContext.BaseDirectory).
			ConfigureServices((context, services) =>
			{
				services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();
				
				services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
				services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
				services.AddTransient<INavigationViewService, NavigationViewService>();
		
				services.AddSingleton<IActivationService, ActivationService>();
				services.AddSingleton<IPageService, PageService>();
				services.AddSingleton<INavigationService, NavigationService>();
				services.AddSingleton<IDialogService, DialogService>();

				services.AddSingleton<IFileService, FileService>();
				services.AddSingleton<IFileConversionService, FileConversionService>();

				services.AddTransient<SettingsViewModel>();
				services.AddTransient<SettingsPage>();

				services.AddTransient<MainViewModel>();
				services.AddTransient<MainPage>();
				services.AddTransient<ProgressDialog>();

				services.AddTransient<ShellViewModel>();
				services.AddTransient<ShellPage>();
				

				services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
			}).
			Build();

			UnhandledException += App_UnhandledException;
		}

		private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
		{
			// TODO: Log and handle exceptions as appropriate.
		}

		protected async override void OnLaunched(LaunchActivatedEventArgs args)
		{
			base.OnLaunched(args);

			await App.GetService<IActivationService>().ActivateAsync(args);
		}
	}
}
