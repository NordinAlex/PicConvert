using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PicConvert.Contracts.Services;
using PicConvert.Helpers;
using PicConvert.Models;
using PicConvert.Services;
using PicConvert.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PicConvert.Views
{
	public sealed partial class MainPage : Page
	{
		public MainViewModel ViewModel { get; }
		
		public List<ImageFormats> FileFormats { get; }

		public MainPage()
		{
			var dialogService = App.GetService<IDialogService>(); // Hämta tjänsten från DI
			ViewModel = new MainViewModel(dialogService);

			FileFormats = Enum.GetValues(typeof(ImageFormats)).Cast<ImageFormats>().ToList();
			this.InitializeComponent();		
			DataContext = ViewModel;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
		}
	}
}
