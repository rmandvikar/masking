using System;
using System.Text;
using rm.Extensions;

namespace rm.Masking
{
	/// <summary>
	/// A masking impl substituting with given string.
	/// </summary>
	public class SimpleSubstitutionMask : IMaskValue
	{
		private readonly string substitution;

		public SimpleSubstitutionMask(
			string substitution)
		{
			this.substitution = substitution.NullIfWhiteSpace()
				?? throw new ArgumentNullException(nameof(substitution));
		}

		/// <inheritdoc/>
		public string Mask(string s)
		{
			if (s == null)
			{
				return s;
			}
			var quotient = s.Length / substitution.Length;
			var remainder = s.Length - (quotient * substitution.Length);
			var masked =
				new StringBuilder(s.Length)
					.Insert(0, substitution, quotient)
					.Append(substitution.SubstringFromStart(remainder))
					.ToString();
			return masked;
		}
	}
}
