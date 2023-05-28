using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Core;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using FiddlerBackendSDK.Files.Client;

namespace FiddlerBackendSDK.AutoResponder.Client
{
    public class RuleSetsClient : BaseEntityClient, IRuleSetsClient
    {
        public RuleSetsClient(IFileClient fileClient, IFileDownloader fileDownloader, IMD5Calculator md5Calculator,
            IValidationExceptionFactory exceptionFactory) : base(fileClient, fileDownloader, md5Calculator,
            exceptionFactory)
        {
        }

        public Task<RuleSetDTO> GetAsync(Guid ruleSetId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<RuleSetDTO>> GetMineAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<RuleSetDTO>> GetAllSharedWithMeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<RuleSetDTO>> GetAvailableAsync()
        {
            return Task.FromResult<ICollection<RuleSetDTO>>(new List<RuleSetDTO>());
        }

        public Task<ICollection<AutoResponderRuleBlobs>> DownloadRulesAsync(Guid id, string outputPath)
        {
            throw new NotImplementedException();
        }

        public Task<RuleSetDTO> CreateAsync(Guid accountId, RuleSet ruleSet)
        {
            throw new NotImplementedException();
        }

        public Task<RuleSetDTO> UpdateSharesAsync(Guid ruleSetId, IEnumerable<string> newEmails, string reason,
            string concurrencyToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid ruleSetId, string concurrencyToken)
        {
            throw new NotImplementedException();
        }
    }
}