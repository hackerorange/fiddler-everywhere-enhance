using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Core;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using FiddlerBackendSDK.Files.Client;

namespace FiddlerBackendSDK.JAM;

public class JAMSessionsClient : BaseEntityClient, IJAMSessionsClient
{
	private readonly string jamSessionsRelativePath = "jam-sessions";

	private readonly string jamWorkspacesSessionRelativePathFormat = "jam-workspaces/{0}/sessions/{1}";

	private readonly IFiddlerHttpClient fiddlerHttpClient;

	private readonly IFiddlerHttpStatusCodeValidator getStatusCodeValidator;

	public JAMSessionsClient(IFiddlerHttpClient fiddlerHttpClient, IFileClient fileClient, IFileDownloader fileDownloader, IMD5Calculator md5Calculator, IValidationExceptionFactory exceptionFactory)
		: base(fileClient, fileDownloader, md5Calculator, exceptionFactory)
	{
		this.fiddlerHttpClient = fiddlerHttpClient;
		getStatusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(base.ExceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
	}

	public async Task<JAMWorkspaceSessionDTO> GetWorkspaceSessionAsync(Guid workspaceId, Guid sessionId)
	{
		string requestUri = string.Format(jamWorkspacesSessionRelativePathFormat, workspaceId, sessionId);
		return await fiddlerHttpClient.GetAsync<JAMWorkspaceSessionDTO>(requestUri, getStatusCodeValidator);
	}

	public async Task<JAMSessionDTO> GetAsync(Guid sessionId, string sharingToken)
	{
		string requestUri = $"{jamSessionsRelativePath}/{sessionId}";
		(string, string)[] headers = (string.IsNullOrEmpty(sharingToken) ? null : new(string, string)[1] { ("X-Sharing-Token", sharingToken) });
		return await fiddlerHttpClient.GetAsync<JAMSessionDTO>(requestUri, getStatusCodeValidator, headers);
	}

	public async Task DownloadJAMSessionAsync(JAMSessionDTO jamSession, string filePath, string sharingToken = null, string password = null)
	{
		if (!jamSession.HasPassword && !string.IsNullOrEmpty(password))
		{
			throw new ArgumentException("You have provided a password for a JAM session which doesn't require one", "password");
		}
		if (jamSession.HasPassword && string.IsNullOrEmpty(password))
		{
			throw new ArgumentException("You have not provided a password for a JAM session which requires one", "password");
		}
		FileDTO file = jamSession.File;
		if (File.Exists(filePath))
		{
			if (string.IsNullOrEmpty(file.ContentMD5))
			{
				return;
			}
			string text = base.MD5Calculator.Calculate(filePath);
			if (!(file.ContentMD5 != text))
			{
				return;
			}
			File.Delete(filePath);
		}
		EnsureDirectoryExists(filePath);
		string requestUri = $"{jamSessionsRelativePath}/{jamSession.Id}/file";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(base.ExceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		List<(string, string)> list = new List<(string, string)>();
		if (!string.IsNullOrEmpty(sharingToken))
		{
			list.Add(("X-Sharing-Token", sharingToken));
		}
		if (!string.IsNullOrEmpty(password))
		{
			list.Add(("X-Auth-Pass", password));
		}
		string downloadUrl = (await fiddlerHttpClient.GetAsync<GetJAMSessionFileResultDTO>(requestUri, statusCodeValidator, (list.Count > 0) ? list : null)).DownloadUrl;
		IFiddlerHttpStatusCodeValidator statusCodeValidator2 = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(base.ExceptionFactory).WithSuccessCodes(HttpStatusCode.OK, HttpStatusCode.PartialContent).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
		if (password == null)
		{
			await base.FileDownloader.DownloadFileAsync(downloadUrl, filePath, statusCodeValidator2);
			return;
		}
		if (jamSession.FilesEncryptionSalt == null || jamSession.FilesEncryptionSalt.Length < 1 || !jamSession.FilesEncryptionIterations.HasValue)
		{
			throw new InvalidOperationException("The specified JAM session cannot be decrypted with the provided password because it doesn't have encryption salt or number of iterations stored in the metadata");
		}
		string encryptedFilePath = Path.ChangeExtension(filePath, ".cipher");
		await base.FileDownloader.DownloadFileAsync(downloadUrl, encryptedFilePath, statusCodeValidator2);
		using FileStream inputStream = File.OpenRead(encryptedFilePath);
		using FileStream outputStream = File.Create(filePath);
		await base.MD5Calculator.DecryptAsync(inputStream, outputStream, password, jamSession.FilesEncryptionSalt, jamSession.FilesEncryptionIterations.Value);
		if (File.Exists(encryptedFilePath))
		{
			File.Delete(encryptedFilePath);
		}
	}
}
