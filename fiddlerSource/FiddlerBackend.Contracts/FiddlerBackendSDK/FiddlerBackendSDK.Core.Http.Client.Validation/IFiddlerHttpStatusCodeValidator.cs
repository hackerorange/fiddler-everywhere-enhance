using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Core.Http.Client.Validation;

public interface IFiddlerHttpStatusCodeValidator
{
	ISet<HttpStatusCode> SuccessCodes { get; }

	ISet<HttpStatusCode> ErrorCodes { get; }

	Task Validate(HttpResponseMessage responseMessage);
}
