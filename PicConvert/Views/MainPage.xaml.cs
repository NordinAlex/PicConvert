using Microsoft.UI.Xaml.Controls;
using PicConvert.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PicConvert.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainViewModel _viewModel { get; } = new MainViewModel();
		public MainPage()
		{
			this.InitializeComponent();
			this.DataContext = _viewModel;
			this.Height = 300;
			this.Width = 400;

		}

		public MainViewModel ViewModel => _viewModel;
	}
}
