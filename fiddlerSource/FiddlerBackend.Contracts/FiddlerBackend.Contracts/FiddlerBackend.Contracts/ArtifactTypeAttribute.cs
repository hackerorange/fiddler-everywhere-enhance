using System;

namespace FiddlerBackend.Contracts;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ArtifactTypeAttribute : Attribute
{
	public string Type { get; private set; }

	public ArtifactTypeAttribute(string type)
	{
		Type = type;
	}
}
