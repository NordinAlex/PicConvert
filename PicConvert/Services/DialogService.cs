using Microsoft.UI.Xaml.Controls;
using PicConvert.Contracts.Services;
using PicConvert.Views;
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
}
