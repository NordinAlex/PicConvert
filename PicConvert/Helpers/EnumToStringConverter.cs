using Microsoft.UI.Xaml.Data;
using PicConvert.Models;
using System;

namespace PicConvert.Helpers;
public class EnumToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is Enum enumValue)
		{
			return Enum.GetName(enumValue.GetType(), enumValue);
		}
		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		if (value is string stringValue && Enum.TryParse(targetType, stringValue, out var enumValue))
		{
			return enumValue;
		}
		return null;
	}
}
