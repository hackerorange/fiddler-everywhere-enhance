using System.IO;
using System.Net;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using FiddlerBackendSDK.Files.Client;

namespace FiddlerBackendSDK.Core;

public class BaseEntityClient
{
	protected IFileClient FileClient { get; private set; }

	protected IFileDownloader FileDownloader { get; private set; }

	protected IMD5Calculator MD5Calculator { get; private set; }

	protected IValidationExceptionFactory ExceptionFactory { get; private set; }

	public BaseEntityClient(IFileClient fileClient, IFileDownloader fileDownloader, IMD5Calculator md5Calculator, IValidationExceptionFactory exceptionFactory)
	{
		FileClient = fileClient;
		FileDownloader = fileDownloader;
		MD5Calculator = md5Calculator;
		ExceptionFactory = exceptionFactory;
	}

	protected async Task DownloadFileAsync(FileDTO file, string outputFilename)
	{
		if (File.Exists(outputFilename))
		{
			if (string.IsNullOrEmpty(file.ContentMD5))
			{
				return;
			}
			string text = MD5Calculator.Calculate(outputFilename);
			if (!(file.ContentMD5 != text))
			{
				return;
			}
			File.Delete(outputFilename);
		}
		string directoryName = Path.GetDirectoryName(outputFilename);
		EnsureDirectoryExists(directoryName);
		string fileUrl = await FileClient.GetFileUrlAsync(file.Id);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(ExceptionFactory).WithSuccessCodes(HttpStatusCode.OK, HttpStatusCode.PartialContent).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
		await FileDownloader.DownloadFileAsync(fileUrl, outputFilename, statusCodeValidator);
	}

	protected void EnsureDirectoryExists(string path)
	{
		string directoryName = Path.GetDirectoryName(path);
		if (!Directory.Exists(directoryName))
		{
			EnsureDirectoryExists(directoryName);
			Directory.CreateDirectory(directoryName);
		}
	}
}
