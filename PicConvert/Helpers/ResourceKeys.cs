using Microsoft.Windows.ApplicationModel.Resources;
using System.Text;

namespace PicConvert.Helpers;

public static class MainResourceKeys
{
	private static readonly ResourceLoader _resourceLoader = new ResourceLoader();

	public static string InputImages => _resourceLoader.GetString("Main_ResourceKeys_InputImages");
	public static string SelectAll => _resourceLoader.GetString("Main_ResourceKeys_SelectAll");
	public static string RemoveSelected => _resourceLoader.GetString("Main_ResourceKeys_RemoveSelected");
	public static string Settings => _resourceLoader.GetString("Main_ResourceKeys_Settings");
	public static string Format => _resourceLoader.GetString("Main_ResourceKeys_Format");
	public static string Quality => _resourceLoader.GetString("Main_ResourceKeys_Quality");
	public static string Size => _resourceLoader.GetString("Main_ResourceKeys_Size");
	public static string SkipMetadata => _resourceLoader.GetString("Main_ResourceKeys_SkipMetadata");
	public static string NullSetting => _resourceLoader.GetString("Main_ResourceKeys_NullSetting");
	public static string SelectFolder => _resourceLoader.GetString("Main_ResourceKeys_SelectFolder");
	public static string Convert => _resourceLoader.GetString("Main_ResourceKeys_Convert");
	
}