
using PicConvert.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace PicConvert.Core.Contracts.Services
{
	public interface IFileConversionService
	{
		Task ConvertFileAsync(FileItem file, string format, int quality, string size, bool skipMetadata, string newFilePath, CancellationToken cancellationToken);
		Task MergeImagesToPdfAsync(IEnumerable<FileItem> images, string pdfFilePath, int quality, CancellationToken cancellationToken);
	}
}
