namespace System.ComponentModel
{
	internal class LicFileLicense : License
	{
		private string _key;

		public override string LicenseKey => _key;

		public LicFileLicense(string key)
		{
			_key = key;
		}

		public override void Dispose()
		{
		}
	}
}
