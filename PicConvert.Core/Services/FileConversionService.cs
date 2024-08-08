
using ImageMagick;
using PicConvert.Core.Contracts.Services;
using PicConvert.Core.Models;
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

	}
}
