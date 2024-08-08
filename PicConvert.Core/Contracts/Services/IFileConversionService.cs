
using PicConvert.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace PicConvert.Core.Contracts.Services
{
	public interface IFileConversionService
	{
		Task ConvertFileAsync(FileItem file, string format, int quality, string size, bool skipMetadata, string newFilePath, CancellationToken cancellationToken);
	}
}
