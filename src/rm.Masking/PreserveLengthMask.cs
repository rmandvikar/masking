﻿namespace rm.Masking;

/// <summary>
/// A masking impl preserving length.
/// </summary>
public class PreserveLengthMask : IMaskValue
{
	private readonly char maskCharacter;

	internal const char DefaultMaskCharacter = '*';

	// reduce GC pressure for first N strings
	internal const int MaskStringPoolSize = 100;
	private readonly IMaskStringPool maskStringPool;

	public PreserveLengthMask(
		char maskCharacter = DefaultMaskCharacter)
	{
		this.maskCharacter = maskCharacter;

		maskStringPool = new PreserveLengthMaskStringPool(MaskStringPoolSize, this.maskCharacter);
	}

	/// <inheritdoc/>
	public string Mask(string s)
	{
		if (s == null)
		{
			return s;
		}
		var maskedLength = s.Length;
		return maskStringPool.GetString(maskedLength) ?? new string(maskCharacter, maskedLength);
	}
}
