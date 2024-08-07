using System.ComponentModel;

namespace PicConvert.Models
{
	public class FileItemModel : INotifyPropertyChanged
	{
		private bool _isSelected;

		public string Name { get; set; }
		public ImageFormats ImgConFormat { get; set; }
		public string Format { get; set; }
		public int Size { get; set; }
		public string Path { get; set; }
		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					OnPropertyChanged(nameof(IsSelected));
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
