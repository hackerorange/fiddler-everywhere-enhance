using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;

namespace FiddlerBackendSDK.AutoResponder.Client;

public interface IRuleSetsClient
{
	Task<RuleSetDTO> GetAsync(Guid ruleSetId);

	Task<ICollection<RuleSetDTO>> GetMineAsync();

	Task<ICollection<RuleSetDTO>> GetAllSharedWithMeAsync();

	Task<ICollection<RuleSetDTO>> GetAvailableAsync();

	Task<ICollection<AutoResponderRuleBlobs>> DownloadRulesAsync(Guid id, string outputPath);

	Task<RuleSetDTO> CreateAsync(Guid accountId, RuleSet ruleSet);

	Task<RuleSetDTO> UpdateSharesAsync(Guid ruleSetId, IEnumerable<string> newEmails, string reason, string concurrencyToken);

	Task DeleteAsync(Guid ruleSetId, string concurrencyToken);
}
