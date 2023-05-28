using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;

namespace FiddlerBackendSDK.License;

public class LicenseClient : ILicenseClient
{
	private readonly string licensesRelativePath = "licenses";

	private readonly IFiddlerHttpClient fiddlerHttpClient;

	private readonly IValidationExceptionFactory exceptionFactory;

	public LicenseClient(IFiddlerHttpClient fiddlerHttpClient, IValidationExceptionFactory exceptionFactory)
	{
		this.fiddlerHttpClient = fiddlerHttpClient;
		this.exceptionFactory = exceptionFactory;
	}

	public async Task<Dictionary<string, List<QuotaDTO>>> GetPlansAsync()
	{
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.Unauthorized).Create();
		return await fiddlerHttpClient.GetAsync<Dictionary<string, List<QuotaDTO>>>(licensesRelativePath + "/plans", statusCodeValidator);
	}
}
