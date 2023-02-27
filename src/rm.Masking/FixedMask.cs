using System;

namespace rm.Masking;

/// <summary>
/// A masking impl with a fixed mask.
/// </summary>
public class FixedMask : IMaskValue
{
	private readonly string fixedMask;

	internal const string DefaultFixedMask = "****";

	public FixedMask(
		string fixedMask = DefaultFixedMask)
	{
		this.fixedMask = fixedMask
			?? throw new ArgumentNullException(nameof(fixedMask));
	}

	/// <inheritdoc/>
	public string Mask(string s)
	{
		if (s == null)
		{
			return s;
		}
		return fixedMask;
	}
}
