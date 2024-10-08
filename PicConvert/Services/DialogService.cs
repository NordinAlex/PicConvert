﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PicConvert.Contracts.Services;
using PicConvert.Views;
using PicConvert.Views.CustomDialogViews;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PicConvert.Services;


public class DialogService : IDialogService
{

	public async Task ShowMessageDialogAsync(string title, string content)
	{
		var dialog = new ContentDialog
		{
			Title = title,// properties
			Content = content,
			CloseButtonText = "OK",
			XamlRoot = App.MainWindow.Content.XamlRoot
		};
		await dialog.ShowAsync();
	}
	public async Task<ContentDialogResult> ShowCustomDialogAsync(string title, string content, string primaryButtonText, string closeButtonText, ContentDialogButton defaultButton)
	{
		var dialog = new ContentDialog
		{
			Title = title,
			Content = content,
			PrimaryButtonText = primaryButtonText,
			CloseButtonText = closeButtonText,
			DefaultButton = defaultButton,
			XamlRoot = App.MainWindow.Content.XamlRoot
		};

		return await dialog.ShowAsync();
	}
	public async Task<ProgressDialog> ShowProgressDialogAsync(CancellationTokenSource cts)
	{
		var progressDialog = new ProgressDialog(cts)
		{
			XamlRoot = App.MainWindow.Content.XamlRoot
		};

		var progressD = progressDialog.ShowAsync(); 

		return await Task.FromResult(progressDialog); 
	}

	public async Task ShowThirdPartyLicensesDialogAsync()
	{
		var dialog = new ThirdPartyLicensesDialog();
		dialog.XamlRoot = App.MainWindow.Content.XamlRoot;
		await dialog.ShowAsync();
	}

	private async void ShowLicenseDialog()
	{
		var licenseDialog = new LicenseDialog(); // Skapa en instans av ContentDialog
		licenseDialog.XamlRoot = App.MainWindow.Content.XamlRoot;
		await licenseDialog.ShowAsync(); // Visa dialogen asynkront
	}

}
