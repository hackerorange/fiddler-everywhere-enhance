using System.Collections.Generic;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;

namespace FiddlerBackendSDK.License;

public interface ILicenseClient
{
	Task<Dictionary<string, List<QuotaDTO>>> GetPlansAsync();
}
