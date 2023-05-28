using System;

namespace FiddlerBackendSDK.Core;

public class FileCacheException : Exception
{
	public string CacheFolder { get; private set; }

	public string Path { get; private set; }

	public FileCacheException(string cacheFolder, string path, string message, Exception exception = null)
		: base(message, exception)
	{
		CacheFolder = cacheFolder;
		Path = path;
	}
}
