using System;

namespace FiddlerBackendSDK.Snapshot.Client;

public class WrongPasswordException : Exception
{
	public WrongPasswordException(string message)
		: base(message)
	{
	}
}
