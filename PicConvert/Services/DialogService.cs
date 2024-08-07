using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace PicConvert.Services
{
	public interface IDialogService
	{
		Task ShowAsync(string message);
	}

	public class DialogService : IDialogService
	{
		public async Task ShowAsync(string message)
		{
			// Kontrollera att vi har en referens till ett fönster
			var window = App.MainWindow;
			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

			var dialog = new ContentDialog
			{
				Title = "Status",
				Content = new TextBlock { Text = message },
				CloseButtonText = "OK"
			};

			// Försök att initialisera dialogen med fönsterhanteraren
			try
			{
				WinRT.Interop.InitializeWithWindow.Initialize(dialog, hWnd);
				await dialog.ShowAsync();
			}
			catch (Exception ex)
			{
				// Logga eller hantera undantaget
				System.Diagnostics.Debug.WriteLine($"Fel vid visning av dialog: {ex.Message}");
			}
		}
	}

}
