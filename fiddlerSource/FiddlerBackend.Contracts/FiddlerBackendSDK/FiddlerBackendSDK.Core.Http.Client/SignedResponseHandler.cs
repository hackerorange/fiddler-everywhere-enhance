using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;

namespace FiddlerBackendSDK.Core.Http.Client;

internal class SignedResponseHandler : DelegatingHandler
{
	private static readonly Regex SignatureRegex = new Regex("^SignedHeaders=(.*), Signature=(.+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
		try
		{
			string header = GetHeader(response, "Signature");
			if (string.IsNullOrEmpty(header))
			{
				throw new ValidationException("The response is not signed");
			}
			Match match = SignatureRegex.Match(header);
			if (!match.Success)
			{
				throw new ValidationException("The response contains an invalid signature");
			}
			byte[] expectedSignature = Convert.FromBase64String(match.Groups[2].Value);
			if (expectedSignature.Length < 4)
			{
				throw new ValidationException("The response contains an invalid signature");
			}
			int publicKeyLength = BinaryPrimitives.ReadInt32BigEndian(new ReadOnlySpan<byte>(expectedSignature).Slice(0, 4));
			if (expectedSignature.Length < 4 + publicKeyLength)
			{
				throw new ValidationException("The response contains an invalid signature");
			}
			string[] source = match.Groups[1].Value.Split(";", StringSplitOptions.RemoveEmptyEntries);
			List<string> list = source.Where((string name) => GetHeader(response, name) == null).ToList();
			if (list.Any())
			{
				throw new ValidationException("The response is missing some signed headers: " + string.Join(", ", list));
			}
			string s = string.Join("\n", source.Select((string name) => string.Format(name + ":" + string.Join(",", GetHeader(response, name)))));
			byte[] headersBuffer = Encoding.UTF8.GetBytes(s);
			byte[] array = ((response.Content == null) ? new byte[0] : (await response.Content.ReadAsByteArrayAsync()));
			byte[] second = array;
			byte[] actualData = headersBuffer.Concat(second).ToArray();
			if (!VerifyData(expectedSignature, publicKeyLength, actualData))
			{
				throw new ValidationException("The response has been tampered!");
			}
			return response;
		}
		catch (Exception)
		{
			throw new ValidationException("Unable to verify response signature");
		}
	}

	private bool VerifyData(byte[] expectedSignature, int publicKeyLength, byte[] actualData)
	{
		ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(expectedSignature);
		using ECDsa eCDsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
		eCDsa.ImportSubjectPublicKeyInfo(readOnlySpan.Slice(4, publicKeyLength), out var _);
		return eCDsa.VerifyData(actualData, readOnlySpan.Slice(4 + publicKeyLength, readOnlySpan.Length - 4 - publicKeyLength), HashAlgorithmName.SHA256);
	}

	private string GetHeader(HttpResponseMessage response, string headerName)
	{
		if (!response.Headers.TryGetValues(headerName, out var values) && !response.Content.Headers.TryGetValues(headerName, out values))
		{
			return null;
		}
		return string.Join(",", values);
	}
}
