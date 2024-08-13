using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PicConvert.Contracts.Services;
using PicConvert.Helpers;
using PicConvert.ViewModels;
using PicConvert.Views.CustomDialogViews;
using System;

namespace PicConvert.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
	public SettingsViewModel ViewModel
	{
		get;
	}
	private readonly IDialogService _dialogService;
	public SettingsPage()
	{
		ViewModel = App.GetService<SettingsViewModel>();
		_dialogService = App.GetService<IDialogService>();
		InitializeComponent();
		
	}

	private async void OnThirdPartyLicensesClick(object sender, RoutedEventArgs e)
	{

		await _dialogService.ShowThirdPartyLicensesDialogAsync();


	}
	// Visa ContentDialog
	private async void ShowLicenseDialog()
	{
		var licenseDialog = new LicenseDialog(); // Skapa en instans av ContentDialog
		licenseDialog.XamlRoot = App.MainWindow.Content.XamlRoot;
		await licenseDialog.ShowAsync(); // Visa dialogen asynkront
	}
	private void ShowLicenseButton_Click(object sender, RoutedEventArgs e)
	{
		ShowLicenseDialog(); // Anropa metoden som visar dialogen
	}

}
