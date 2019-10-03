namespace System.Net.Cache
{
	/// <summary>Defines an application's caching requirements for resources obtained by using <see cref="T:System.Net.HttpWebRequest" /> objects.</summary>
	public class HttpRequestCachePolicy : RequestCachePolicy
	{
		private DateTime cacheSyncDate;

		private HttpRequestCacheLevel level;

		private TimeSpan maxAge;

		private TimeSpan maxStale;

		private TimeSpan minFresh;

		/// <summary>Gets the cache synchronization date for this instance.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value set to the date specified when this instance was created. If no date was specified, this property's value is <see cref="F:System.DateTime.MinValue" />.</returns>
		public DateTime CacheSyncDate => cacheSyncDate;

		/// <summary>Gets the <see cref="T:System.Net.Cache.HttpRequestCacheLevel" /> value that was specified when this instance was created.</summary>
		/// <returns>A <see cref="T:System.Net.Cache.HttpRequestCacheLevel" /> value that specifies the cache behavior for resources that were obtained using <see cref="T:System.Net.HttpWebRequest" /> objects.</returns>
		public new HttpRequestCacheLevel Level => level;

		/// <summary>Gets the maximum age permitted for a resource returned from the cache.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> value that is set to the maximum age value specified when this instance was created. If no date was specified, this property's value is <see cref="F:System.DateTime.MinValue" />.</returns>
		public TimeSpan MaxAge => maxAge;

		/// <summary>Gets the maximum staleness value that is permitted for a resource returned from the cache.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> value that is set to the maximum staleness value specified when this instance was created. If no date was specified, this property's value is <see cref="F:System.DateTime.MinValue" />.</returns>
		public TimeSpan MaxStale => maxStale;

		/// <summary>Gets the minimum freshness that is permitted for a resource returned from the cache.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> value that specifies the minimum freshness specified when this instance was created. If no date was specified, this property's value is <see cref="F:System.DateTime.MinValue" />.</returns>
		public TimeSpan MinFresh => minFresh;

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Cache.HttpRequestCachePolicy" /> class. </summary>
		public HttpRequestCachePolicy()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Cache.HttpRequestCachePolicy" /> class using the specified cache synchronization date.</summary>
		/// <param name="cacheSyncDate">A <see cref="T:System.DateTime" /> object that specifies the time when resources stored in the cache must be revalidated.</param>
		public HttpRequestCachePolicy(DateTime cacheSyncDate)
		{
			this.cacheSyncDate = cacheSyncDate;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Cache.HttpRequestCachePolicy" /> class using the specified cache policy.</summary>
		/// <param name="level">An <see cref="T:System.Net.Cache.HttpRequestCacheLevel" /> value. </param>
		public HttpRequestCachePolicy(HttpRequestCacheLevel level)
		{
			this.level = level;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Cache.HttpRequestCachePolicy" /> class using the specified age control and time values. </summary>
		/// <param name="cacheAgeControl">One of the following <see cref="T:System.Net.Cache.HttpCacheAgeControl" /> enumeration values: <see cref="F:System.Net.Cache.HttpCacheAgeControl.MaxAge" />, <see cref="F:System.Net.Cache.HttpCacheAgeControl.MaxStale" />, or <see cref="F:System.Net.Cache.HttpCacheAgeControl.MinFresh" />.</param>
		/// <param name="ageOrFreshOrStale">A <see cref="T:System.TimeSpan" /> value that specifies an amount of time. See the Remarks section for more information. </param>
		/// <exception cref="T:System.ArgumentException">The value specified for the <paramref name="cacheAgeControl" /> parameter cannot be used with this constructor.</exception>
		public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan ageOrFreshOrStale)
		{
			switch (cacheAgeControl)
			{
			case HttpCacheAgeControl.MaxAge:
				maxAge = ageOrFreshOrStale;
				break;
			case HttpCacheAgeControl.MaxStale:
				maxStale = ageOrFreshOrStale;
				break;
			case HttpCacheAgeControl.MinFresh:
				minFresh = ageOrFreshOrStale;
				break;
			default:
				throw new ArgumentException("ageOrFreshOrStale");
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Cache.HttpRequestCachePolicy" /> class using the specified maximum age, age control value, and time value.</summary>
		/// <param name="cacheAgeControl">An <see cref="T:System.Net.Cache.HttpCacheAgeControl" /> value. </param>
		/// <param name="maxAge">A <see cref="T:System.TimeSpan" /> value that specifies the maximum age for resources.</param>
		/// <param name="freshOrStale">A <see cref="T:System.TimeSpan" /> value that specifies an amount of time. See the Remarks section for more information.  </param>
		/// <exception cref="T:System.ArgumentException">The value specified for the <paramref name="cacheAgeControl" /> parameter is not valid.</exception>
		public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan maxAge, TimeSpan freshOrStale)
		{
			this.maxAge = maxAge;
			switch (cacheAgeControl)
			{
			case HttpCacheAgeControl.MaxStale:
				maxStale = freshOrStale;
				break;
			case HttpCacheAgeControl.MinFresh:
				minFresh = freshOrStale;
				break;
			default:
				throw new ArgumentException("freshOrStale");
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Cache.HttpRequestCachePolicy" /> class using the specified maximum age, age control value, time value, and cache synchronization date.</summary>
		/// <param name="cacheAgeControl">An <see cref="T:System.Net.Cache.HttpCacheAgeControl" /> value. </param>
		/// <param name="maxAge">A <see cref="T:System.TimeSpan" /> value that specifies the maximum age for resources.</param>
		/// <param name="freshOrStale">A <see cref="T:System.TimeSpan" /> value that specifies an amount of time. See the Remarks section for more information.  </param>
		/// <param name="cacheSyncDate">A <see cref="T:System.DateTime" /> object that specifies the time when resources stored in the cache must be revalidated.</param>
		public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan maxAge, TimeSpan freshOrStale, DateTime cacheSyncDate)
			: this(cacheAgeControl, maxAge, freshOrStale)
		{
			this.cacheSyncDate = cacheSyncDate;
		}

		/// <summary>Returns a string representation of this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> value that contains the property values for this instance.</returns>
		[MonoTODO]
		public override string ToString()
		{
			throw new NotImplementedException();
		}
	}
}
