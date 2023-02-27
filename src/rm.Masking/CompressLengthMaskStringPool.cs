using System;

namespace rm.Masking;

/// <inheritdoc cref="IMaskStringPool"/>
public class CompressLengthMaskStringPool : IMaskStringPool
{
	private readonly string[] pool;

	public CompressLengthMaskStringPool(
		byte size,
		char maskCharacter)
	{
		if (size < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(size), size, null);
		}

		pool = new string[size + 1];
		BuildPool(maskCharacter);
	}

	private void BuildPool(char maskCharacter)
	{
		for (int i = 0; i < pool.Length; i++)
		{
			pool[i] = i >= 3 ? $"{maskCharacter}{i}{maskCharacter}" : new string(maskCharacter, i);
		}
	}

	/// <inheritdoc/>
	public string GetString(int length)
	{
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(length), length, null);
		}
		return length < pool.Length ? pool[length] : null;
	}
}
