using System;
using rm.Extensions;

namespace rm.Masking;

/// <summary>
/// A masking impl showing last N chars, and preserving length.
/// </summary>
public class ShowLastNCharsPreserveLengthMask : IMaskValue
{
	private readonly char maskCharacter;
	private readonly int nCharactersToShow;

	internal const char DefaultMaskCharacter = '*';
	internal const int DefaultNCharactersToShow = 4;

	// reduce GC pressure for first N strings
	internal const int MaskStringPoolSize = 100;
	private readonly IMaskStringPool maskStringPool;

	public ShowLastNCharsPreserveLengthMask(
		char maskCharacter = DefaultMaskCharacter,
		int nCharactersToShow = DefaultNCharactersToShow)
	{
		if (nCharactersToShow < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(nCharactersToShow), nCharactersToShow, null);
		}

		this.maskCharacter = maskCharacter;
		this.nCharactersToShow = nCharactersToShow;

		maskStringPool = new PreserveLengthMaskStringPool(MaskStringPoolSize, this.maskCharacter);
	}

	/// <inheritdoc/>
	public string Mask(string s)
	{
		if (s == null)
		{
			return s;
		}

		// note: preserve length
		var maskedLength = Math.Max(0, s.Length - nCharactersToShow);

		// see https://www.chinhdo.com/20070929/stringbuilder-part-2/
		var masked =
			string.Concat(
				maskStringPool.GetString(maskedLength) ?? new string(maskCharacter, maskedLength),
				s.SubstringTillEnd(nCharactersToShow));
		return masked;
	}
}
