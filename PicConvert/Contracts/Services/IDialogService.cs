using Microsoft.UI.Xaml.Controls;
using PicConvert.Views.CustomDialogViews;
using System.Threading;
using System.Threading.Tasks;

namespace PicConvert.Contracts.Services
{
	public interface IDialogService
	{
		Task ShowMessageDialogAsync(string title, string content);
		Task<ContentDialogResult> ShowCustomDialogAsync(string title, string content, string primaryButtonText, string closeButtonText, ContentDialogButton defaultButton);
		Task<ProgressDialog> ShowProgressDialogAsync(CancellationTokenSource cts);
		Task ShowThirdPartyLicensesDialogAsync();
	}

}
