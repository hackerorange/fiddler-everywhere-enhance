using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace FiddlerBackendSDK.Files.Client;

internal class CounterModeCryptoTransform : ICryptoTransform, IDisposable
{
	private readonly byte[] nonceAndCounter;

	private readonly ICryptoTransform counterEncryptor;

	private readonly Queue<byte> xorMask = new Queue<byte>();

	private readonly SymmetricAlgorithm symmetricAlgorithm;

	private byte[] counterModeBlock;

	private ulong counter;

	public int InputBlockSize => symmetricAlgorithm.BlockSize / 8;

	public int OutputBlockSize => symmetricAlgorithm.BlockSize / 8;

	public bool CanTransformMultipleBlocks => true;

	public bool CanReuseTransform => false;

	public CounterModeCryptoTransform(SymmetricAlgorithm symmetricAlgorithm, byte[] key, ulong nonce, ulong counter)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		this.symmetricAlgorithm = symmetricAlgorithm ?? throw new ArgumentNullException("symmetricAlgorithm");
		this.counter = counter;
		nonceAndCounter = new byte[16];
		BitConverter.TryWriteBytes(nonceAndCounter, nonce);
		BitConverter.TryWriteBytes(new Span<byte>(nonceAndCounter, 8, 8), counter);
		byte[] rgbIV = new byte[this.symmetricAlgorithm.BlockSize / 8];
		counterEncryptor = symmetricAlgorithm.CreateEncryptor(key, rgbIV);
	}

	public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		byte[] array = new byte[inputCount];
		TransformBlock(inputBuffer, inputOffset, inputCount, array, 0);
		return array;
	}

	public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
	{
		for (int i = 0; i < inputCount; i++)
		{
			if (NeedMoreXorMaskBytes())
			{
				EncryptCounterThenIncrement();
			}
			byte b = xorMask.Dequeue();
			outputBuffer[outputOffset + i] = (byte)(inputBuffer[inputOffset + i] ^ b);
		}
		return inputCount;
	}

	public void Dispose()
	{
		counterEncryptor.Dispose();
	}

	private bool NeedMoreXorMaskBytes()
	{
		return xorMask.Count == 0;
	}

	private void EncryptCounterThenIncrement()
	{
		if (counterModeBlock == null)
		{
			counterModeBlock = new byte[symmetricAlgorithm.BlockSize / 8];
		}
		counterEncryptor.TransformBlock(nonceAndCounter, 0, nonceAndCounter.Length, counterModeBlock, 0);
		IncrementCounter();
		byte[] array = counterModeBlock;
		foreach (byte item in array)
		{
			xorMask.Enqueue(item);
		}
	}

	private void IncrementCounter()
	{
		counter++;
		BinaryPrimitives.TryWriteUInt64BigEndian(new Span<byte>(nonceAndCounter, 8, 8), counter);
	}
}
