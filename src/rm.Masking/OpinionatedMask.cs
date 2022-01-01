using System;
using rm.Extensions;

namespace rm.Masking
{
	/// <summary>
	/// An opinionated masking impl.
	/// <para></para>
	/// Masks string, preserving length, by masking X+ characters in middle and
	/// showing Y- characters split between start and end preferring start if Y is odd.
	/// </summary>
	/// <remarks>
	/// X = 6,
	/// Y = 5,
	/// mask character = *
	/// <para></para>
	/// example:
	/// 123456789abc => 123*******bc,
	/// 123456 => ******
	/// </remarks>
	public class OpinionatedMask : IMaskValue
	{
		private readonly char maskCharacter;
		private readonly int minCharactersToMask;
		private readonly int maxCharactersToMask;

		internal const char DefaultMaskCharacter = '*';
		internal const int DefaultMinCharactersToMask = 6;
		internal const int DefaultMaxCharactersToShow = 5;

		// reduce GC pressure for first N strings
		internal const int MaskStringPoolSize = 100;
		private readonly IMaskStringPool maskStringPool;

		public OpinionatedMask(
			char maskCharacter = DefaultMaskCharacter,
			int minCharactersToMask = DefaultMinCharactersToMask,
			int maxCharactersToShow = DefaultMaxCharactersToShow)
		{
			if (minCharactersToMask < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(minCharactersToMask), minCharactersToMask, null);
			}
			if (maxCharactersToShow < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(maxCharactersToShow), maxCharactersToShow, null);
			}

			this.maskCharacter = maskCharacter;
			this.minCharactersToMask = minCharactersToMask;
			this.maxCharactersToMask = maxCharactersToShow;

			maskStringPool = new PreserveLengthMaskStringPool(MaskStringPoolSize, this.maskCharacter);
		}

		/// <summary>
		/// Masks <paramref name="s"/>, preserving length, by masking X+ characters in middle and
		/// showing Y- characters split between start and end preferring start if Y is odd.
		/// </summary>
		/// <remarks>
		/// X = 6,
		/// Y = 5,
		/// mask character = *
		/// </remarks>
		public string Mask(string s)
		{
			if (s == null)
			{
				return s;
			}

			// note: preserve length
			var showCharacters = Math.Min(maxCharactersToMask, Math.Max(0, s.Length - minCharactersToMask));
			var showEndCharacters = showCharacters / 2;
			// show 1 extra start character on odd showCharacters
			var showStartCharacters = showCharacters - showEndCharacters;

			var maskedLength = s.Length - showCharacters;

			// see https://www.chinhdo.com/20070929/stringbuilder-part-2/
			var masked =
				string.Concat(
					s.SubstringFromStart(showStartCharacters),
					maskStringPool.GetString(maskedLength) ?? new string(maskCharacter, maskedLength),
					s.SubstringTillEnd(showEndCharacters));
			return masked;
		}
	}
}
