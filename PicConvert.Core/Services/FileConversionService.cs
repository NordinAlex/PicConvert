
using PicConvert.Core.Contracts.Services;
using PicConvert.Core.Models;
using System.Threading.Tasks;

namespace PicConvert.Core.Services
{
	public class FileConversionService : IFileConversionService
	{
		public async Task ConvertFileAsync(FileItem file, string format, int quality, string size, bool skipMetadata)
		{
			// Logik för filkonvertering här
		}
	}
}
