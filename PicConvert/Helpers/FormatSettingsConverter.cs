using Microsoft.UI.Xaml.Data;
using PicConvert.Models;
using System;

namespace PicConvert.Helpers
{
	public class FormatSettingsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			// Här kan du implementera logik för att konvertera mellan formatinställningar och användargränssnittselement
			if (value is FileFormats format)
			{
				// Implementera din konverteringslogik beroende på formatet
				// Till exempel, returnera olika inställningsalternativ beroende på formatet
				switch (format)
				{
					case FileFormats.JPEG:
						// Return specific settings for JPEG
						break;
					case FileFormats.PNG:
						// Return specific settings for PNG
						break;
					case FileFormats.PDF:
						// Return specific settings for PDF
						break;
					case FileFormats.SVG:
						// Return specific settings for SVG
						break;
					case FileFormats.WebP:
						// Return specific settings for WEBP
						break;
				}
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			// Implementera omvänd konverteringslogik om det behövs
			return null;
		}
	}

}
