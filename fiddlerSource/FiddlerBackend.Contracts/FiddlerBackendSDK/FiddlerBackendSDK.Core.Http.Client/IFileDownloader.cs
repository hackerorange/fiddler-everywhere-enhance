using System.Threading.Tasks;
using FiddlerBackendSDK.Core.Http.Client.Validation;

namespace FiddlerBackendSDK.Core.Http.Client;

public interface IFileDownloader
{
	Task DownloadFileAsync(string fileUrl, string targetPath, IFiddlerHttpStatusCodeValidator statusCodeValidator);
}
