using System;

namespace PicConvert.Contracts.Services;


public interface IPageService
{
	Type GetPageType(string key);
}

