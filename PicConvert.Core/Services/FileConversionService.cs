
using ImageMagick;
using PicConvert.Core.Contracts.Services;
using PicConvert.Core.Models;
using System.Threading.Tasks;

namespace PicConvert.Core.Services
{
	public class FileConversionService : IFileConversionService
	{
		public async Task ConvertFileAsync(FileItem file, string format, int quality, string size, bool skipMetadata, string newFilePath)
		{
			using (var image = new MagickImage(file.Path))
			{
				// Ställ in kvalitet
				image.Quality = quality;

				// Om storlek är angiven, ändra storlek
				if (!string.IsNullOrEmpty(size))
				{
					// Exempel på att ändra storlek till en kvadratisk bild
					var dimensions = size.Split('x');
					if (dimensions.Length == 2 && int.TryParse(dimensions[0], out int width) && int.TryParse(dimensions[1], out int height))
					{
						image.Resize(width, height);
					}
				}

				// Om metadata ska hoppas över, ta bort metadata
				if (skipMetadata)
				{
					image.Strip();
				}

				// Bestäm filformat
				var newFormat = format.ToLower();
				var newFilePaths = System.IO.Path.ChangeExtension(newFilePath, newFormat);

				// Spara konverterad bild
				await Task.Run(() => image.Write(newFilePaths));
			}
		}
	}
}
