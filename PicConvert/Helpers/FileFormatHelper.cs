using PicConvert.Models;
using System.Collections.Generic;

namespace PicConvert.Helpers;

public static class FileFormatHelper
{
	private static readonly Dictionary<InputImageFormats, string> _fileExtensionMap = new()
		{
			{ InputImageFormats.JPEG, ".jpg" },
			{ InputImageFormats.PNG, ".png" },			
			{ InputImageFormats.SVG, ".svg" },
			{ InputImageFormats.WebP, ".webp" }
		};

	public static string GetFileExtension(InputImageFormats format)
	{
		return _fileExtensionMap[format];
	}
}
