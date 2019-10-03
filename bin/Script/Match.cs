namespace System.Text.RegularExpressions
{
	/// <summary>Represents the results from a single regular expression match.</summary>
	[Serializable]
	public class Match : Group
	{
		private Regex regex;

		private IMachine machine;

		private int text_length;

		private GroupCollection groups;

		private static Match empty = new Match();

		/// <summary>Gets the empty group. All failed matches return this empty match.</summary>
		/// <returns>An empty <see cref="T:System.Text.RegularExpressions.Match" />.</returns>
		public static Match Empty => empty;

		/// <summary>Gets a collection of groups matched by the regular expression.</summary>
		/// <returns>The character groups matched by the pattern.</returns>
		public virtual GroupCollection Groups => groups;

		internal Regex Regex => regex;

		private Match()
		{
			regex = null;
			machine = null;
			text_length = 0;
			groups = new GroupCollection(1, 1);
			groups.SetValue(this, 0);
		}

		internal Match(Regex regex, IMachine machine, string text, int text_length, int n_groups, int index, int length)
			: base(text, index, length)
		{
			this.regex = regex;
			this.machine = machine;
			this.text_length = text_length;
		}

		internal Match(Regex regex, IMachine machine, string text, int text_length, int n_groups, int index, int length, int n_caps)
			: base(text, index, length, n_caps)
		{
			this.regex = regex;
			this.machine = machine;
			this.text_length = text_length;
			groups = new GroupCollection(n_groups, regex.Gap);
			groups.SetValue(this, 0);
		}

		/// <summary>Returns a <see cref="T:System.Text.RegularExpressions.Match" /> instance equivalent to the one supplied that is suitable to share between multiple threads.</summary>
		/// <returns>A match that is suitable to share between multiple threads.</returns>
		/// <param name="inner">A match equivalent to the one expected.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="inner" /> is null.</exception>
		[MonoTODO("not thread-safe")]
		public static Match Synchronized(Match inner)
		{
			if (inner == null)
			{
				throw new ArgumentNullException("inner");
			}
			return inner;
		}

		/// <summary>Returns a new <see cref="T:System.Text.RegularExpressions.Match" /> object with the results for the next match, starting at the position at which the last match ended (at the character after the last matched character).</summary>
		/// <returns>The next regular expression match.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public Match NextMatch()
		{
			if (this == Empty)
			{
				return Empty;
			}
			int num = (!regex.RightToLeft) ? (base.Index + base.Length) : base.Index;
			if (base.Length == 0)
			{
				num += ((!regex.RightToLeft) ? 1 : (-1));
			}
			return machine.Scan(regex, base.Text, num, text_length);
		}

		/// <summary>Returns the expansion of the specified replacement pattern. </summary>
		/// <returns>The expanded version of the <paramref name="replacement" /> parameter.</returns>
		/// <param name="replacement">The replacement pattern to use. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="replacement" /> is null.</exception>
		/// <exception cref="T:System.NotSupportedException">Expansion is not allowed for this pattern.</exception>
		public virtual string Result(string replacement)
		{
			if (replacement == null)
			{
				throw new ArgumentNullException("replacement");
			}
			if (machine == null)
			{
				throw new NotSupportedException("Result cannot be called on failed Match.");
			}
			return machine.Result(replacement, this);
		}
	}
}
