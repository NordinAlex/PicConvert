using CommunityToolkit.Mvvm.ComponentModel;
using PicConvert.ViewModels;

namespace PicConvert.Models;


public partial class FileModel : ObservableObject
{
	[ObservableProperty]
	private string _name;

	[ObservableProperty]
	private string _format;

	[ObservableProperty]
	private long _size;
}