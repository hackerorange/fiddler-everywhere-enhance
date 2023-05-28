using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ionic.Zip;
using Newtonsoft.Json;

namespace FiddlerBackendSDK.Snapshot;

public class SnapshotTransformer : ISnapshotTransformer
{
	private const string DeletedFilesFileName = "__deletedFiles.json";

	public async Task<Stream> CreateDeltaAsync(string mainFilePath, string versionFilePath, string password)
	{
		Validate(mainFilePath, versionFilePath);
		Stream result;
		await using (FileStream mainStream = new FileStream(mainFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
		{
			Stream stream;
			await using (FileStream fileStream = new FileStream(versionFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				MemoryStream memoryStream = new MemoryStream();
				ZipFile mainArchive = OpenMainFileZipArchiveForReading(mainStream);
				try
				{
					ZipFile val = OpenVersionFileZipArchiveForReading(fileStream);
					try
					{
						ZipFile val2 = new ZipFile();
						try
						{
							if (!string.IsNullOrEmpty(password))
							{
								mainArchive.Password = password;
								mainArchive.Encryption = (EncryptionAlgorithm)3;
								val.Password = password;
								val.Encryption = (EncryptionAlgorithm)3;
								val2.Password = password;
								val2.Encryption = (EncryptionAlgorithm)3;
							}
							List<string> list = new List<string>();
							foreach (ZipEntry mainEntry in mainArchive.Entries)
							{
								ZipEntry val3 = ((IEnumerable<ZipEntry>)val.Entries).FirstOrDefault((Func<ZipEntry, bool>)((ZipEntry x) => x.FileName == mainEntry.FileName));
								if (val3 != null)
								{
									if (val3.CompressedSize != mainEntry.CompressedSize || val3.Crc != mainEntry.Crc)
									{
										CreateZipEntry(val2, mainEntry.FileName, val3, password);
									}
								}
								else
								{
									list.Add(mainEntry.FileName);
								}
							}
							foreach (ZipEntry item in val.Entries.Where((ZipEntry x) => mainArchive.Entries.All((ZipEntry e) => e.FileName != x.FileName)))
							{
								CreateZipEntry(val2, item.FileName, item, password);
							}
							if (list.Count > 0)
							{
								CreateDeletedFilesEntry(val2, list);
							}
							val2.Save((Stream)memoryStream);
							memoryStream.Seek(0L, SeekOrigin.Begin);
							stream = memoryStream;
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				finally
				{
					if (mainArchive != null)
					{
						((IDisposable)mainArchive).Dispose();
					}
				}
			}
			result = stream;
		}
		return result;
	}

	public async Task<Stream> ApplyDeltaAsync(string mainFilePath, string versionFilePath, string password)
	{
		Validate(mainFilePath, versionFilePath);
		Stream result;
		await using (FileStream mainStream = new FileStream(mainFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
		{
			Stream stream;
			await using (FileStream fileStream = new FileStream(versionFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				MemoryStream memoryStream = new MemoryStream();
				ZipFile val = OpenMainFileZipArchiveForReading(mainStream);
				try
				{
					ZipFile val2 = OpenVersionFileZipArchiveForReading(fileStream);
					try
					{
						ZipFile val3 = new ZipFile();
						try
						{
							if (!string.IsNullOrEmpty(password))
							{
								val.Password = password;
								val.Encryption = (EncryptionAlgorithm)3;
								val2.Password = password;
								val2.Encryption = (EncryptionAlgorithm)3;
								val3.Password = password;
								val3.Encryption = (EncryptionAlgorithm)3;
							}
							List<string> deletedFilesEntry = GetDeletedFilesEntry(val2, password);
							foreach (ZipEntry mainEntry in val.Entries)
							{
								if (!deletedFilesEntry.Contains(mainEntry.FileName))
								{
									ZipEntry val4 = ((IEnumerable<ZipEntry>)val2.Entries).FirstOrDefault((Func<ZipEntry, bool>)((ZipEntry x) => x.FileName == mainEntry.FileName));
									CreateZipEntry(val3, mainEntry.FileName, val4 ?? mainEntry, password);
								}
							}
							foreach (ZipEntry versionEntry in val2.Entries.Where((ZipEntry x) => x.FileName != "__deletedFiles.json"))
							{
								if (!val.Entries.Any((ZipEntry x) => x.FileName == versionEntry.FileName))
								{
									CreateZipEntry(val3, versionEntry.FileName, versionEntry, password);
								}
							}
							val3.Save((Stream)memoryStream);
							memoryStream.Seek(0L, SeekOrigin.Begin);
							stream = memoryStream;
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			result = stream;
		}
		return result;
	}

	private static ZipFile OpenMainFileZipArchiveForReading(Stream fileStream)
	{
		try
		{
			return OpenZipArchiveForReading(fileStream);
		}
		catch (InvalidDataException innerException)
		{
			throw new ArgumentException("Error reading main file!", innerException);
		}
	}

	private static ZipFile OpenVersionFileZipArchiveForReading(Stream fileStream)
	{
		try
		{
			return OpenZipArchiveForReading(fileStream);
		}
		catch (InvalidDataException innerException)
		{
			throw new ArgumentException("Error reading version file!", innerException);
		}
	}

	private static ZipFile OpenZipArchiveForReading(Stream fileStream)
	{
		return ZipFile.Read(fileStream);
	}

	private void CreateZipEntry(ZipFile archive, string entryName, ZipEntry entry, string password)
	{
		using MemoryStream memoryStream = new MemoryStream();
		if (string.IsNullOrEmpty(password))
		{
			entry.Extract((Stream)memoryStream);
		}
		else
		{
			entry.ExtractWithPassword((Stream)memoryStream, password);
		}
		byte[] array = memoryStream.ToArray();
		archive.AddEntry(entryName, array);
	}

	private void CreateDeletedFilesEntry(ZipFile archive, List<string> deletedFiles)
	{
		archive.AddEntry("__deletedFiles.json", JsonConvert.SerializeObject((object)deletedFiles));
	}

	private List<string> GetDeletedFilesEntry(ZipFile archive, string password)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JsonSerializer val = new JsonSerializer();
		if (!archive.ContainsEntry("__deletedFiles.json"))
		{
			return new List<string>();
		}
		foreach (ZipEntry item in archive)
		{
			if (!(item.FileName == "__deletedFiles.json"))
			{
				continue;
			}
			using MemoryStream memoryStream = new MemoryStream();
			if (string.IsNullOrEmpty(password))
			{
				item.Extract((Stream)memoryStream);
			}
			else
			{
				item.ExtractWithPassword((Stream)memoryStream, password);
			}
			memoryStream.Seek(0L, SeekOrigin.Begin);
			using StreamReader streamReader = new StreamReader(memoryStream);
			return val.Deserialize((TextReader)streamReader, typeof(List<string>)) as List<string>;
		}
		return new List<string>();
	}

	private void Validate(string mainFilePath, string versionFilePath)
	{
		if (string.IsNullOrWhiteSpace(mainFilePath))
		{
			throw new ArgumentException("Main file path is required for creating delta!");
		}
		if (string.IsNullOrWhiteSpace(versionFilePath))
		{
			throw new ArgumentException("Version file path is required for creating delta!");
		}
		if (!File.Exists(mainFilePath))
		{
			throw new FileNotFoundException("The provided main file \"" + mainFilePath + "\" does not exist!");
		}
		if (!File.Exists(versionFilePath))
		{
			throw new FileNotFoundException("The provided version file \"" + versionFilePath + "\" does not exist!");
		}
	}
}
