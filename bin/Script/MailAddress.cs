using System.Text;

namespace System.Net.Mail
{
	/// <summary>Represents the address of an electronic mail sender or recipient.</summary>
	public class MailAddress
	{
		private string address;

		private string displayName;

		/// <summary>Gets the e-mail address specified when this instance was created.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the e-mail address.</returns>
		public string Address => address;

		/// <summary>Gets the display name composed from the display name and address information specified when this instance was created.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the display name; otherwise, <see cref="F:System.String.Empty" /> ("") if no display name information was specified when this instance was created.</returns>
		public string DisplayName
		{
			get
			{
				if (displayName == null)
				{
					return string.Empty;
				}
				return displayName;
			}
		}

		/// <summary>Gets the host portion of the address specified when this instance was created.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the name of the host computer that accepts e-mail for the <see cref="P:System.Net.Mail.MailAddress.User" /> property.</returns>
		public string Host => Address.Substring(address.IndexOf("@") + 1);

		/// <summary>Gets the user information from the address specified when this instance was created.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the user name portion of the <see cref="P:System.Net.Mail.MailAddress.Address" />.</returns>
		public string User => Address.Substring(0, address.IndexOf("@"));

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.MailAddress" /> class using the specified address. </summary>
		/// <param name="address">A <see cref="T:System.String" /> that contains an e-mail address.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="address" /> is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="address" /> is not in a recognized format.</exception>
		public MailAddress(string address)
			: this(address, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.MailAddress" /> class using the specified address and display name.</summary>
		/// <param name="address">A <see cref="T:System.String" /> that contains an e-mail address.</param>
		/// <param name="displayName">A <see cref="T:System.String" /> that contains the display name associated with <paramref name="address" />. This parameter can be null.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="address" /> is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="address" /> is not in a recognized format.-or-<paramref name="address" /> contains non-ASCII characters.</exception>
		public MailAddress(string address, string displayName)
			: this(address, displayName, Encoding.Default)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.MailAddress" /> class using the specified address, display name, and encoding.</summary>
		/// <param name="address">A <see cref="T:System.String" /> that contains an e-mail address.</param>
		/// <param name="displayName">A <see cref="T:System.String" /> that contains the display name associated with <paramref name="address" />.</param>
		/// <param name="displayNameEncoding">The <see cref="T:System.Text.Encoding" /> that defines the character set used for <paramref name="displayName" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null.-or-<paramref name="displayName" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="address" /> is <see cref="F:System.String.Empty" /> ("").-or-<paramref name="displayName" /> is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="address" /> is not in a recognized format.-or-<paramref name="address" /> contains non-ASCII characters.</exception>
		public MailAddress(string address, string displayName, Encoding displayNameEncoding)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			int num = address.IndexOf('"');
			if (num == 0)
			{
				int num2 = address.IndexOf('"', num + 1);
				if (num2 == -1)
				{
					throw CreateFormatException();
				}
				this.displayName = address.Substring(num + 1, num2 - 1).Trim();
				address = address.Substring(num2 + 1);
			}
			int num3 = address.IndexOf('<');
			if (num3 != -1)
			{
				if (num3 + 1 >= address.Length)
				{
					throw CreateFormatException();
				}
				int num4 = address.IndexOf('>', num3 + 1);
				if (num4 == -1)
				{
					throw CreateFormatException();
				}
				if (this.displayName == null)
				{
					this.displayName = address.Substring(0, num3).Trim();
				}
				address = address.Substring(++num3, num4 - num3);
			}
			if (displayName != null)
			{
				this.displayName = displayName.Trim();
			}
			this.address = address.Trim();
		}

		/// <summary>Compares two mail addresses.</summary>
		/// <returns>true if the two mail addresses are equal; otherwise, false.</returns>
		/// <param name="value">A <see cref="T:System.Net.Mail.MailAddress" /> instance to compare to the current instance.</param>
		public override bool Equals(object obj)
		{
			return Equals(obj as MailAddress);
		}

		private bool Equals(MailAddress other)
		{
			return other != null && Address == other.Address;
		}

		/// <summary>Returns a hash value for a mail address.</summary>
		/// <returns>An integer hash value.</returns>
		public override int GetHashCode()
		{
			return address.GetHashCode();
		}

		/// <summary>Returns a string representation of this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the contents of this <see cref="T:System.Net.Mail.MailAddress" />.</returns>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (DisplayName != null && DisplayName.Length > 0)
			{
				stringBuilder.Append("\"");
				stringBuilder.Append(DisplayName);
				stringBuilder.Append("\"");
				stringBuilder.Append(" ");
				stringBuilder.Append("<");
				stringBuilder.Append(Address);
				stringBuilder.Append(">");
			}
			else
			{
				stringBuilder.Append(Address);
			}
			return stringBuilder.ToString();
		}

		private static FormatException CreateFormatException()
		{
			return new FormatException("The specified string is not in the form required for an e-mail address.");
		}
	}
}
