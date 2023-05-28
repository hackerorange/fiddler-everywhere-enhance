using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Core.Http.Client.Validation;

internal class FiddlerHttpStatusCodeValidator : IFiddlerHttpStatusCodeValidator
{
	internal class Builder
	{
		private ISet<HttpStatusCode> SuccessCodes { get; } = new HashSet<HttpStatusCode>();


		private ISet<HttpStatusCode> ClientErrorCodes { get; } = new HashSet<HttpStatusCode>();


		private IValidationExceptionFactory ExceptionFactory { get; set; }

		public Builder(IValidationExceptionFactory exceptionFactory)
		{
			ExceptionFactory = exceptionFactory;
		}

		internal Builder WithSuccessCode(HttpStatusCode statusCode)
		{
			SuccessCodes.Add(statusCode);
			return this;
		}

		internal Builder WithSuccessCodes(params HttpStatusCode[] statusCodes)
		{
			SuccessCodes.UnionWith(statusCodes);
			return this;
		}

		internal Builder WithErrorCode(HttpStatusCode statusCode)
		{
			ClientErrorCodes.Add(statusCode);
			return this;
		}

		internal Builder WithErrorCodes(params HttpStatusCode[] statusCodes)
		{
			ClientErrorCodes.UnionWith(statusCodes);
			return this;
		}

		internal IFiddlerHttpStatusCodeValidator Create()
		{
			return new FiddlerHttpStatusCodeValidator(ExceptionFactory, SuccessCodes, ClientErrorCodes);
		}
	}

	private readonly IValidationExceptionFactory exceptionFactory;

	public ISet<HttpStatusCode> SuccessCodes { get; private set; }

	public ISet<HttpStatusCode> ErrorCodes { get; private set; }

	private FiddlerHttpStatusCodeValidator(IValidationExceptionFactory exceptionFactory, ISet<HttpStatusCode> successCodes, ISet<HttpStatusCode> errorCodes)
	{
		this.exceptionFactory = exceptionFactory;
		SuccessCodes = successCodes;
		ErrorCodes = errorCodes;
	}

	public async Task Validate(HttpResponseMessage responseMessage)
	{
		if (SuccessCodes.Contains(responseMessage.StatusCode))
		{
			return;
		}
		if (ErrorCodes.Contains(responseMessage.StatusCode))
		{
			throw await exceptionFactory.CreateExpectedExceptionAsync(responseMessage);
		}
		string details = await responseMessage.Content.ReadAsStringAsync();
		throw new UnexpectedStatusCodeException(responseMessage.StatusCode, details);
	}
}
