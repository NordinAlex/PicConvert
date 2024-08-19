using System.Threading.Tasks;

namespace PicConvert.Contracts.Services;

public interface ILocalSettingsService
{
	Task<T> ReadSettingAsync<T>(string key);

	Task SaveSettingAsync<T>(string key, T value);
}
