namespace rm.Masking
{
	/// <summary>
	/// A masking impl with a noop mask.
	/// </summary>
	public class NoopMask : IMaskValue
	{
		/// <inheritdoc/>
		public string Mask(string s)
		{
			return s;
		}
	}
}
