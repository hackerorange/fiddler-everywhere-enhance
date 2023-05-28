// Decompiled with JetBrains decompiler
// Type: FiddlerBackendSDK.AutoResponder.Client.RuleSetsClient
// Assembly: FiddlerBackendSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E490FE11-87F6-4AF2-9559-82C30C4C5736
// Assembly location: /Users/zhongchongtao/GitHub/fiddler-everywhere-enhance/v4.0.1-mac/FiddlerBackendSDK.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Core;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using FiddlerBackendSDK.Files.Client;

namespace FiddlerBackendSDK.AutoResponder.Client
{
  public class RuleSetsClient : BaseEntityClient, IRuleSetsClient
  {
    private const string RuleSetsRelativePath = "rulesets";
    private readonly IFiddlerHttpClient fiddlerHttpClient;
    private readonly IBackendConfiguration backendConfiguration;
    private readonly IFiddlerHttpStatusCodeValidator getRulesStatusCodeValidator;

    public RuleSetsClient(
      IFiddlerHttpClient fiddlerHttpClient,
      IFileClient fileClient,
      IFileDownloader fileDownloader,
      IMD5Calculator md5Calculator,
      IValidationExceptionFactory exceptionFactory,
      IBackendConfiguration backendConfiguration)
      : base(fileClient, fileDownloader, md5Calculator, exceptionFactory)
    {
      this.fiddlerHttpClient = fiddlerHttpClient;
      this.backendConfiguration = backendConfiguration;
      this.getRulesStatusCodeValidator = new FiddlerHttpStatusCodeValidator.Builder(this.ExceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
    }

    public async Task<RuleSetDTO> GetAsync(Guid ruleSetId)
    {
      RuleSetsClient ruleSetsClient = this;
      string requestUri = string.Format("{0}/{1}", (object) "rulesets", (object) ruleSetId);
      IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerHttpStatusCodeValidator.Builder(ruleSetsClient.ExceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
      return await ruleSetsClient.fiddlerHttpClient.GetAsync<RuleSetDTO>(requestUri, statusCodeValidator);
    }

    public async Task<ICollection<RuleSetDTO>> GetMineAsync() => await this.fiddlerHttpClient.GetAsync<ICollection<RuleSetDTO>>("rulesets/mine", this.getRulesStatusCodeValidator);

    public async Task<ICollection<RuleSetDTO>> GetAllSharedWithMeAsync() => await this.fiddlerHttpClient.GetAsync<ICollection<RuleSetDTO>>("rulesets/shared-with-me", this.getRulesStatusCodeValidator);

    public async Task<ICollection<RuleSetDTO>> GetAvailableAsync() => await this.fiddlerHttpClient.GetAsync<ICollection<RuleSetDTO>>("rulesets", this.getRulesStatusCodeValidator);

    public async Task<ICollection<AutoResponderRuleBlobs>> DownloadRulesAsync(
      Guid id,
      string outputPath)
    {
      RuleSetsClient ruleSetsClient = this;
      // ISSUE: explicit non-virtual call
      RuleSetDTO async = await __nonvirtual (ruleSetsClient.GetAsync(id));
      outputPath = Path.Join((ReadOnlySpan<char>) outputPath, (ReadOnlySpan<char>) string.Format("{0}", (object) id));
      if (!Directory.Exists(outputPath))
        Directory.CreateDirectory(outputPath);
      List<AutoResponderRuleBlobs> ruleSetBlobs = new List<AutoResponderRuleBlobs>();
      foreach (AutoResponderRuleDTO rule in (IEnumerable<AutoResponderRuleDTO>) async.Rules)
      {
        AutoResponderRuleBlobs blobs = new AutoResponderRuleBlobs();
        blobs.Id = rule.Id;
        if (rule.Headers != null)
        {
          blobs.HeadersFile = Path.Combine(outputPath, string.Format("{0}_headers", (object) rule.Id));
          await ruleSetsClient.DownloadFileAsync(rule.Headers, blobs.HeadersFile);
        }
        if (rule.Body != null)
        {
          blobs.BodyFile = Path.Combine(outputPath, string.Format("{0}_body", (object) rule.Id));
          await ruleSetsClient.DownloadFileAsync(rule.Body, blobs.BodyFile);
        }
        ruleSetBlobs.Add(blobs);
        blobs = (AutoResponderRuleBlobs) null;
      }
      ICollection<AutoResponderRuleBlobs> responderRuleBlobses = (ICollection<AutoResponderRuleBlobs>) ruleSetBlobs;
      ruleSetBlobs = (List<AutoResponderRuleBlobs>) null;
      return responderRuleBlobses;
    }

    public async Task<RuleSetDTO> CreateAsync(Guid accountId, RuleSet ruleSet)
    {
      RuleSetsClient ruleSetsClient = this;
      string resourcePath = "rulesets";
      IFiddlerHttpStatusCodeValidator validator = new FiddlerHttpStatusCodeValidator.Builder(ruleSetsClient.ExceptionFactory).WithSuccessCode(HttpStatusCode.Created).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.PaymentRequired, HttpStatusCode.BadRequest, HttpStatusCode.Conflict, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
      CreateRuleSetDTO createRuleSetDTO = new CreateRuleSetDTO()
      {
        AccountId = accountId,
        Name = ruleSet.Name
      };
      foreach (AutoResponderRule rule in (IEnumerable<AutoResponderRule>) ruleSet.Rules)
      {
        CreateAutoResponderRuleDTO createAutoResponderRuleDTO = new CreateAutoResponderRuleDTO()
        {
          Match = rule.Match,
          Action = rule.Action,
          Comment = rule.Comment
        };
        if (rule.Headers != null)
          createAutoResponderRuleDTO.HeadersFileId = new Guid?(await ruleSetsClient.UploadFileToS3Async(accountId, rule.Headers));
        if (rule.Body != null)
          createAutoResponderRuleDTO.BodyFileId = new Guid?(await ruleSetsClient.UploadFileToS3Async(accountId, rule.Body));
        createRuleSetDTO.Rules.Add(createAutoResponderRuleDTO);
        createAutoResponderRuleDTO = (CreateAutoResponderRuleDTO) null;
      }
      RuleSetDTO async = await ruleSetsClient.fiddlerHttpClient.PutAsync<CreateRuleSetDTO, RuleSetDTO>(resourcePath, createRuleSetDTO, validator);
      resourcePath = (string) null;
      validator = (IFiddlerHttpStatusCodeValidator) null;
      createRuleSetDTO = (CreateRuleSetDTO) null;
      return async;
    }

    public async Task<RuleSetDTO> UpdateSharesAsync(
      Guid ruleSetId,
      IEnumerable<string> newEmails,
      string reason,
      string concurrencyToken)
    {
      RuleSetsClient ruleSetsClient = this;
      string requestUri = string.Format("{0}/{1}/shares", (object) "rulesets", (object) ruleSetId);
      IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerHttpStatusCodeValidator.Builder(ruleSetsClient.ExceptionFactory).WithSuccessCodes(HttpStatusCode.Created, HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized, HttpStatusCode.PaymentRequired, HttpStatusCode.PreconditionFailed, HttpStatusCode.Forbidden).Create();
      List<RuleSetShareDTO> list = newEmails.Select<string, RuleSetShareDTO>((Func<string, RuleSetShareDTO>) (email =>
      {
        return new RuleSetShareDTO()
        {
          Email = email,
          Note = reason,
          Permissions = Roles.All
        };
      })).ToList<RuleSetShareDTO>();
      return await ruleSetsClient.fiddlerHttpClient.PostAsync<IEnumerable<RuleSetShareDTO>, RuleSetDTO>(requestUri, (IEnumerable<RuleSetShareDTO>) list, statusCodeValidator, ruleSetsClient.CreateModificationRequestHeaders(concurrencyToken));
    }

    public async Task DeleteAsync(Guid ruleSetId, string concurrencyToken)
    {
      RuleSetsClient ruleSetsClient = this;
      string requestUri = string.Format("{0}/{1}", (object) "rulesets", (object) ruleSetId);
      IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerHttpStatusCodeValidator.Builder(ruleSetsClient.ExceptionFactory).WithSuccessCodes(HttpStatusCode.NoContent, HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
      await ruleSetsClient.fiddlerHttpClient.DeleteAsync(requestUri, statusCodeValidator, ruleSetsClient.CreateModificationRequestHeaders(concurrencyToken));
    }

    private IEnumerable<(string, string)> CreateModificationRequestHeaders(string concurrencyToken)
    {
      if (this.backendConfiguration.DisableConcurrency)
        return (IEnumerable<(string, string)>) new List<(string, string)>();
      return (IEnumerable<(string, string)>) new List<(string, string)>()
      {
        ("If-Match", concurrencyToken)
      };
    }

    private async Task<Guid> UploadFileToS3Async(Guid accountId, Stream stream)
    {
      RuleSetsClient ruleSetsClient = this;
      stream.Seek(0L, SeekOrigin.Begin);
      // ISSUE: reference to a compiler-generated method
      string chunkChecksums = string.Join<string>(';', ruleSetsClient.GetFileChunks(stream).ToList<byte[]>().Select<byte[], string>(new Func<byte[], string>(ruleSetsClient.\u003CUploadFileToS3Async\u003Eb__14_0)));
      stream.Seek(0L, SeekOrigin.Begin);
      Dictionary<string, string> uploadHeaders = ruleSetsClient.GetUploadHeaders(stream, chunkChecksums);
      InitiateMultipartUploadResponseDTO multipartUploadResponse = await ruleSetsClient.FileClient.InitiateMultipartFileUploadAsync(accountId, uploadHeaders);
      IEnumerable<PartETagDTO> s3Async1 = await ruleSetsClient.FileClient.UploadToS3Async(stream, (IList<string>) multipartUploadResponse.UploadUrls.ToList<string>());
      Guid fileId = multipartUploadResponse.FileId;
      await ruleSetsClient.FileClient.CompleteFileUploadAsync(accountId, fileId, s3Async1);
      Guid s3Async2 = fileId;
      multipartUploadResponse = (InitiateMultipartUploadResponseDTO) null;
      return s3Async2;
    }

    private IList<byte[]> GetFileChunks(Stream stream)
    {
      byte[] numArray = new byte[this.FileClient.ChunkSize];
      List<byte[]> fileChunks = new List<byte[]>();
      int count;
      while ((count = stream.Read(numArray, 0, numArray.Length)) > 0)
      {
        byte[] dst = new byte[count];
        Buffer.BlockCopy((Array) numArray, 0, (Array) dst, 0, count);
        fileChunks.Add(dst);
      }
      return (IList<byte[]>) fileChunks;
    }

    private Dictionary<string, string> GetUploadHeaders(Stream stream, string chunkChecksums)
    {
      long length = stream.Length;
      string str = this.MD5Calculator.Calculate(stream);
      return new Dictionary<string, string>()
      {
        {
          "X-Upload-Content-Type",
          "application/octet-stream"
        },
        {
          "X-Upload-Content-Length",
          length.ToString()
        },
        {
          "X-Upload-Content-MD5",
          str
        },
        {
          "X-Upload-Chunk-Size",
          this.FileClient.ChunkSize.ToString()
        },
        {
          "X-Upload-Chunk-Checksums",
          chunkChecksums
        }
      };
    }
  }
}
