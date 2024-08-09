using Microsoft.UI.Xaml.Controls;
using System.Threading;

namespace PicConvert.Views
{
	public sealed partial class ProgressDialog : ContentDialog
	{
		private CancellationTokenSource cancellationTokenSource;

		public ProgressDialog(CancellationTokenSource cts)
		{
			this.InitializeComponent();
			this.cancellationTokenSource = cts;
		}

		public void SetProgress(int value)
		{
			ConversionProgressBar.Value = value;
		}

		public void SetStatus(string status)
		{
			ConversionStatusText.Text = status;
		}

		private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			cancellationTokenSource.Cancel();
			sender.Hide();
		}
	}
}
