using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Files.Client;

public class MD5Calculator : IMD5Calculator
{
	public string Calculate(Stream stream)
	{
		using MD5 mD = MD5.Create();
		return Convert.ToBase64String(mD.ComputeHash(stream));
	}

	public string Calculate(string filePath)
	{
		using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		return Calculate(stream);
	}

	public string Calculate(byte[] bytes)
	{
		using MD5 mD = MD5.Create();
		return Convert.ToBase64String(mD.ComputeHash(bytes));
	}

	public async Task DecryptAsync(Stream input, Stream output, string password, byte[] encryptionSalt, uint iterations)
	{
		using Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), encryptionSalt, (int)iterations, HashAlgorithmName.SHA256);
		byte[] bytes = pbkdf2.GetBytes(32);
		AesManaged symmetricAlgorithm = new AesManaged
		{
			Mode = CipherMode.ECB,
			Padding = PaddingMode.None
		};
		ulong counter = 0uL;
		ulong nonce = BitConverter.ToUInt64(encryptionSalt);
		using CounterModeCryptoTransform cryptoTransform = new CounterModeCryptoTransform(symmetricAlgorithm, bytes, nonce, counter);
		using CryptoStream cryptoStream = new CryptoStream(input, cryptoTransform, CryptoStreamMode.Read);
		await cryptoStream.CopyToAsync(output);
	}
}
