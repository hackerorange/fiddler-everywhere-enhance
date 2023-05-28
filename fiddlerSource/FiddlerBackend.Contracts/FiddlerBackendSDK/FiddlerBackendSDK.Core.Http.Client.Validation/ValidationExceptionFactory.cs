using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;
using Newtonsoft.Json;

namespace FiddlerBackendSDK.Core.Http.Client.Validation;

public class ValidationExceptionFactory : IValidationExceptionFactory
{
	public async Task<HttpException> CreateExpectedExceptionAsync(HttpResponseMessage responseMessage)
	{
		string content = await responseMessage.Content.ReadAsStringAsync();
		switch (responseMessage.StatusCode)
		{
		case HttpStatusCode.Unauthorized:
			return new UnauthorizedClientErrorException(responseMessage.ReasonPhrase);
		case HttpStatusCode.BadRequest:
		case HttpStatusCode.Forbidden:
		case HttpStatusCode.NotFound:
		case HttpStatusCode.Conflict:
			return CreateErrorException(responseMessage.StatusCode, content);
		case HttpStatusCode.PaymentRequired:
			return CreateQuotaErrorException(responseMessage.StatusCode, content);
		case HttpStatusCode.PreconditionFailed:
			return new ConcurrentModificationException(responseMessage.StatusCode);
		default:
			return new HttpException(responseMessage.StatusCode);
		}
	}

	private ClientErrorException CreateErrorException(HttpStatusCode statusCode, string content)
	{
		ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(content);
		return new ClientErrorException(statusCode, error);
	}

	private ClientErrorException CreateQuotaErrorException(HttpStatusCode statusCode, string content)
	{
		QuotaErrorDTO quotaErrorDTO = JsonConvert.DeserializeObject<QuotaErrorDTO>(content);
		if (string.IsNullOrWhiteSpace(quotaErrorDTO.ExceededQuota))
		{
			InactiveAccountErrorDTO error = JsonConvert.DeserializeObject<InactiveAccountErrorDTO>(content);
			return new ClientErrorException(statusCode, error);
		}
		return new ClientErrorException(statusCode, quotaErrorDTO);
	}
}
