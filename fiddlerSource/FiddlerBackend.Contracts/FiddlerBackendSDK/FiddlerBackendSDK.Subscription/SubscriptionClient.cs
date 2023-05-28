using System.Net;
using System.Threading.Tasks;
using System.Web;
using FiddlerBackend;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;

namespace FiddlerBackendSDK.Subscription;

public class SubscriptionClient : ISubscriptionClient
{
	private const string SubscriptionRelativePath = "subscription";

	private const string EverywhereTrialRelativePath = "trials/Everywhere";

	private readonly IIdentityHttpClient identityHttpClient;

	private readonly IFiddlerHttpClient fiddlerHttpClient;

	private readonly IValidationExceptionFactory exceptionFactory;

	private readonly ICryptoService cryptoService;

	public SubscriptionClient(IIdentityHttpClient identityHttpClient, IFiddlerHttpClient fiddlerHttpClient, IValidationExceptionFactory exceptionFactory, ICryptoService cryptoService)
	{
		this.identityHttpClient = identityHttpClient;
		this.fiddlerHttpClient = fiddlerHttpClient;
		this.exceptionFactory = exceptionFactory;
		this.cryptoService = cryptoService;
	}

	public async Task StartTrialAsync(string machineId)
	{
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.NoContent).WithErrorCodes(HttpStatusCode.Unauthorized, HttpStatusCode.Conflict, HttpStatusCode.UnavailableForLegalReasons, HttpStatusCode.NotFound, HttpStatusCode.BadRequest).Create();
		string machineId2 = cryptoService.Encrypt(machineId);
		StartTrialDTO resource = new StartTrialDTO
		{
			MachineId = machineId2
		};
		await fiddlerHttpClient.PostAsync("trials/Everywhere", resource, statusCodeValidator);
	}

	public async Task<bool> IsTrialAvailableAsync(string machineId)
	{
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.Unauthorized).Create();
		string text = HttpUtility.UrlEncode(cryptoService.Encrypt(machineId));
		return (await fiddlerHttpClient.GetAsync<ValueDTO<bool>>("trials/Everywhere/availability?machineId=" + text, statusCodeValidator)).Value;
	}

	public async Task<string> CreateToken(string idToken, string refreshToken)
	{
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.Unauthorized, HttpStatusCode.BadRequest).Create();
		CreateTokenDTO resource = new CreateTokenDTO
		{
			IdToken = idToken,
			RefreshToken = refreshToken
		};
		return await identityHttpClient.PostAsync<CreateTokenDTO, string>("tokens/create", resource, statusCodeValidator);
	}
}
