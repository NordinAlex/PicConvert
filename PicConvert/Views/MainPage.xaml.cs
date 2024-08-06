using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PicConvert.Helpers;
using PicConvert.Models;
using PicConvert.ViewModels;
using System.Collections.Generic;

namespace PicConvert.Views
{
	public sealed partial class MainPage : Page
	{
		public MainViewModel ViewModel { get; }

		public List<EnumValue> FileFormats { get; }

		public MainPage()
		{
			ViewModel = new MainViewModel();
			FileFormats = EnumSource.GetValues<FileFormats>();
			this.InitializeComponent();
			DataContext = ViewModel;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
		}
	}
}
