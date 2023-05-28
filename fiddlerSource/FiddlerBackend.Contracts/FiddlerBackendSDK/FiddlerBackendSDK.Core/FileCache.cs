using System.IO;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Core;

public class FileCache : IFileCache
{
	private readonly string cacheFolder;

	public FileCache(IBackendConfiguration backendConfiguration)
	{
		cacheFolder = backendConfiguration.CacheFolder;
	}

	public string GetFileLocation(string cachePath)
	{
		string text = Path.Combine(cacheFolder, cachePath);
		if (!File.Exists(text))
		{
			return null;
		}
		return text;
	}

	public string GetTargetPath(string cachePath)
	{
		string text = Path.Combine(cacheFolder, cachePath);
		if (File.Exists(text))
		{
			File.Delete(text);
		}
		EnsureDirectoryExists(text);
		return text;
	}

	public void DeletePath(string cachePath)
	{
		string path = Path.Combine(cacheFolder, cachePath);
		if (Directory.Exists(path))
		{
			Directory.Delete(path, recursive: true);
		}
		else if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	public string SaveFile(string cachePath, string filePath, bool deleteOriginal = false)
	{
		string text = Path.Combine(cacheFolder, cachePath);
		if (File.Exists(text))
		{
			throw new FileCacheException(cacheFolder, cachePath, "The file you are trying to save already exists!");
		}
		EnsureDirectoryExists(text);
		if (deleteOriginal)
		{
			File.Move(filePath, text);
		}
		else
		{
			File.Copy(filePath, text);
		}
		return text;
	}

	public async Task<string> SaveFileAsync(string cachePath, Stream contentStream)
	{
		string targetPath = Path.Combine(cacheFolder, cachePath);
		if (File.Exists(targetPath))
		{
			throw new FileCacheException(cacheFolder, cachePath, "The file you are trying to save already exists!");
		}
		EnsureDirectoryExists(targetPath);
		string result;
		await using (FileStream fs = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			await contentStream.CopyToAsync(fs);
			result = targetPath;
		}
		return result;
	}

	private void EnsureDirectoryExists(string path)
	{
		string directoryName = Path.GetDirectoryName(path);
		if (!Directory.Exists(directoryName))
		{
			EnsureDirectoryExists(directoryName);
			Directory.CreateDirectory(directoryName);
		}
	}
}
