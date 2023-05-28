using System.IO;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Core;

public interface IFileCache
{
	string GetFileLocation(string cachePath);

	string GetTargetPath(string cachePath);

	Task<string> SaveFileAsync(string cachePath, Stream contentStream);

	string SaveFile(string cachePath, string filePath, bool deleteOriginal = false);

	void DeletePath(string cachePath);
}
