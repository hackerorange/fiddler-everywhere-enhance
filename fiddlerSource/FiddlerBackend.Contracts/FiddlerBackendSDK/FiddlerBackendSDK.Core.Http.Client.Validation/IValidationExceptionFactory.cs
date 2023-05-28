using System.Net.Http;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Core.Http.Client.Validation;

public interface IValidationExceptionFactory
{
	Task<HttpException> CreateExpectedExceptionAsync(HttpResponseMessage responseMessage);
}
