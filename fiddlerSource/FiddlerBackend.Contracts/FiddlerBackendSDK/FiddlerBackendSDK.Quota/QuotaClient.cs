using System;
using System.Net;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;

namespace FiddlerBackendSDK.Quota;

public class QuotaClient : IQuotaClient
{
	private readonly IFiddlerHttpClient fiddlerHttpClient;

	private readonly IValidationExceptionFactory exceptionFactory;

	public QuotaClient(IFiddlerHttpClient fiddlerHttpClient, IValidationExceptionFactory exceptionFactory)
	{
		this.fiddlerHttpClient = fiddlerHttpClient;
		this.exceptionFactory = exceptionFactory;
	}

	public async Task<QuotasUsageDTO> GetQuotaUsageAsync(Guid accountId)
	{
		string requestUri = $"quotas-usage/{accountId}";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
		return await fiddlerHttpClient.GetAsync<QuotasUsageDTO>(requestUri, statusCodeValidator);
	}
}
