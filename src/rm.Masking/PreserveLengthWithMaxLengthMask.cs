using System;

namespace rm.Masking;

/// <summary>
/// A masking impl preserving length with max length.
/// </summary>
public class PreserveLengthWithMaxLengthMask : IMaskValue
{
	private readonly char maskCharacter;
	private readonly int maxMaskLength;

	internal const char DefaultMaskCharacter = '*';
	internal const int DefaultMaxMaskLength = int.MaxValue;

	// reduce GC pressure for first N strings
	internal const int MaskStringPoolSize = 100;
	private readonly IMaskStringPool maskStringPool;

	public PreserveLengthWithMaxLengthMask(
		char maskCharacter = DefaultMaskCharacter,
		int maxMaskLength = DefaultMaxMaskLength)
	{
		this.maskCharacter = maskCharacter;
		this.maxMaskLength = maxMaskLength;

		maskStringPool = new PreserveLengthMaskStringPool(MaskStringPoolSize, this.maskCharacter);
	}

	/// <inheritdoc/>
	public string Mask(string s)
	{
		if (s == null)
		{
			return s;
		}
		var maskedLength = Math.Min(s.Length, maxMaskLength);
		return maskStringPool.GetString(maskedLength) ?? new string(maskCharacter, maskedLength);
	}
}
