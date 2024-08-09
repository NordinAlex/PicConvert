using PicConvert.Models;
using System.Collections.Generic;

namespace PicConvert.Helpers;

public static class FileFormatHelper
{
	private static readonly Dictionary<ImageFormats, string> _fileExtensionMap = new()
		{
			{ ImageFormats.JPEG, ".jpg" },
			{ ImageFormats.PNG, ".png" },
			{ ImageFormats.PDF, ".pdf" },
			{ ImageFormats.SVG, ".svg" },
			{ ImageFormats.WebP, ".webp" }
		};

	public static string GetFileExtension(ImageFormats format)
	{
		return _fileExtensionMap[format];
	}
}
