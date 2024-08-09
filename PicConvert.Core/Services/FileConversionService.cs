
using ImageMagick;
using PicConvert.Core.Contracts.Services;
using PicConvert.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PicConvert.Core.Services
{
	public class FileConversionService : IFileConversionService
	{
		public async Task ConvertFileAsync(FileItem file, string format, int quality, string size, bool skipMetadata, string newFilePath, CancellationToken cancellationToken)
		{
			using (var image = new MagickImage(file.Path))
			{
				image.Quality = quality;

				if (!string.IsNullOrEmpty(size))
				{
					var dimensions = size.Split('x');
					if (dimensions.Length == 2 && int.TryParse(dimensions[0], out int width) && int.TryParse(dimensions[1], out int height))
					{
						image.Resize(width, height);
					}
				}

				if (skipMetadata)
				{
					image.Strip();
				}

				var newFormat = format.ToLower();
				var newFilePaths = System.IO.Path.ChangeExtension(newFilePath, newFormat);

				await Task.Run(() => image.Write(newFilePaths), cancellationToken);
			}
		}
		public async Task MergeImagesToPdfAsync(IEnumerable<FileItem> images, string pdfFilePath, int quality, CancellationToken cancellationToken)
		{
			// Skapa en lista för att hålla bilderna i minnet tills PDF-filen är skriven
			var imageList = new List<MagickImage>();

			try
			{
				foreach (var image in images)
				{
					cancellationToken.ThrowIfCancellationRequested();

					// Skapa en ny instans av MagickImage för varje bild
					var magickImage = new MagickImage(image.Path);
					magickImage.Quality = quality;

					// Lägg till bilden i listan
					imageList.Add(magickImage);
				}

				// Skapa en MagickImageCollection och lägg till alla bilder från listan
				using (var pdf = new MagickImageCollection())
				{
					foreach (var magickImage in imageList)
					{
						pdf.Add(magickImage);
					}

					// Skriv PDF-filen
					await Task.Run(() => pdf.Write(pdfFilePath), cancellationToken);
				}
			}
			finally
			{
				// Dispose varje bild i listan
				foreach (var magickImage in imageList)
				{
					magickImage.Dispose();
				}
			}
		}


	}
}
