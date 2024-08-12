using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PicConvert.Models;
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
			var ViewModel = App.GetService<MainViewModel>();

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
