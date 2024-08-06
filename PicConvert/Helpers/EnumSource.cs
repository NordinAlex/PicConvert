using System;
using System.Collections.Generic;
using System.Linq;

namespace PicConvert.Helpers;

public static class EnumSource
{
	public static List<EnumValue> GetValues<T>() where T : Enum
	{
		return Enum.GetValues(typeof(T)).Cast<T>().Select(e => new EnumValue(e)).ToList();
	}
}

public class EnumValue
{
	public EnumValue(Enum value)
	{
		Value = value;
		DisplayValue = value.ToString();
	}

	public Enum Value { get; }
	public string DisplayValue { get; }
}