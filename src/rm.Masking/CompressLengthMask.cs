namespace rm.Masking;

/// <summary>
/// A masking impl compressing length.
/// </summary>
public class CompressLengthMask : IMaskValue
{
	private readonly char maskCharacter;

	internal const char DefaultMaskCharacter = '*';

	// reduce GC pressure for first N strings
	internal const int MaskStringPoolSize = 100;
	private readonly IMaskStringPool maskStringPool;

	public CompressLengthMask(
		char maskCharacter = DefaultMaskCharacter)
	{
		this.maskCharacter = maskCharacter;

		maskStringPool = new CompressLengthMaskStringPool(MaskStringPoolSize, this.maskCharacter);
	}

	/// <inheritdoc/>
	public string Mask(string s)
	{
		if (s == null)
		{
			return s;
		}
		var maskedLength = s.Length;
		return maskStringPool.GetString(maskedLength) ??
			(maskedLength >= 3 ? $"{maskCharacter}{maskedLength}{maskCharacter}" : new string(maskCharacter, maskedLength));
	}
}
