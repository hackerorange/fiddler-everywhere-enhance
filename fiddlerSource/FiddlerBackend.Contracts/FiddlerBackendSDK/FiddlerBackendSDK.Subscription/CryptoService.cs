using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using FiddlerBackend.Contracts;

namespace FiddlerBackendSDK.Subscription;

public class CryptoService : ICryptoService
{
	private const string SerializedPublicKey = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTE2Ij8+CjxSU0FQYXJhbWV0ZXJzIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIHhtbG5zOnhzZD0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiPgogIDxFeHBvbmVudD5BUUFCPC9FeHBvbmVudD4KICA8TW9kdWx1cz55TXNBSzlQRFVCOFJUc2Rjc1JHY3FkQ3VYWkFaaERzYWovSVJvdkR0RktmMDROU2FlRk00bm1jTFpxY21sSDNncjI5WWx1cmZsTlQ4WXhCdFNVSVk3b1U0ZWZ1NDk4TWxFdEpWYzBqK3YrTGdYS1NjZnNZd2V5bllldEk1b0ZPUGExRE5RU1ZDTGtLUWd3blh5NGhvUGNoaUsxRkxsUkpwT0l2Z2huMFVhaWdIdXRBa3hjZFZYVlN5cEZTbVVjS3pYMEdpUEczZVVldFh5ZnZVTllEOEd3U2tRb0V6L2dIbzUxeit3R2dYdXVVanlSU1N1VW9jOWFRZm5vbFl4M2t6cnpFRXFVSmVpV21YRHFNSi80bHFrTkhsMGhRT3lzUm9scHNobU10YVVCNFM4NldPRk1FV210S241dmZuRTFVVUN3UFJMWEVmbXJHYTZ2cXlia3dZUnc9PTwvTW9kdWx1cz4KPC9SU0FQYXJhbWV0ZXJzPg==";

	private readonly RSAParameters publicKey;

	public CryptoService()
	{
		publicKey = DeserializeKey("PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTE2Ij8+CjxSU0FQYXJhbWV0ZXJzIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIHhtbG5zOnhzZD0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiPgogIDxFeHBvbmVudD5BUUFCPC9FeHBvbmVudD4KICA8TW9kdWx1cz55TXNBSzlQRFVCOFJUc2Rjc1JHY3FkQ3VYWkFaaERzYWovSVJvdkR0RktmMDROU2FlRk00bm1jTFpxY21sSDNncjI5WWx1cmZsTlQ4WXhCdFNVSVk3b1U0ZWZ1NDk4TWxFdEpWYzBqK3YrTGdYS1NjZnNZd2V5bllldEk1b0ZPUGExRE5RU1ZDTGtLUWd3blh5NGhvUGNoaUsxRkxsUkpwT0l2Z2huMFVhaWdIdXRBa3hjZFZYVlN5cEZTbVVjS3pYMEdpUEczZVVldFh5ZnZVTllEOEd3U2tRb0V6L2dIbzUxeit3R2dYdXVVanlSU1N1VW9jOWFRZm5vbFl4M2t6cnpFRXFVSmVpV21YRHFNSi80bHFrTkhsMGhRT3lzUm9scHNobU10YVVCNFM4NldPRk1FV210S241dmZuRTFVVUN3UFJMWEVmbXJHYTZ2cXlia3dZUnc9PTwvTW9kdWx1cz4KPC9SU0FQYXJhbWV0ZXJzPg==");
	}

	public string Encrypt(string machineId)
	{
		if (machineId != null && machineId.Length > 245)
		{
			throw new ValidationException("The message to encrypt is too long");
		}
		byte[] inArray;
		using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
		{
			byte[] bytes = Encoding.UTF8.GetBytes(machineId);
			try
			{
				rSACryptoServiceProvider.ImportParameters(publicKey);
				inArray = rSACryptoServiceProvider.Encrypt(bytes, fOAEP: false);
			}
			finally
			{
				rSACryptoServiceProvider.PersistKeyInCsp = false;
			}
		}
		return Convert.ToBase64String(inArray);
	}

	private RSAParameters DeserializeKey(string serializedKey)
	{
		if (serializedKey == null)
		{
			throw new ValidationException("Provided key to deserialize is empty");
		}
		byte[] bytes = Convert.FromBase64String(serializedKey);
		StringReader textReader = new StringReader(Encoding.UTF8.GetString(bytes));
		return (RSAParameters)new XmlSerializer(typeof(RSAParameters)).Deserialize(textReader);
	}
}
