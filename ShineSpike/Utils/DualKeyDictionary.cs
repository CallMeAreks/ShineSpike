using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ShineSpike.Utils
{
	/// <summary>
	/// Multi-Key Dictionary Class
	/// </summary>	
	/// <typeparam name="TPrimaryKey">Primary Key Type</typeparam>
	/// <typeparam name="TSecondaryKey">Sub Key Type</typeparam>
	/// <typeparam name="TValue">Value Type</typeparam>
	public class DualKeyDictionary<TPrimaryKey, TSecondaryKey, TValue>
	{
		internal readonly Dictionary<TPrimaryKey, TValue> baseDictionary = new Dictionary<TPrimaryKey, TValue>();
		internal readonly Dictionary<TSecondaryKey, TPrimaryKey> subDictionary = new Dictionary<TSecondaryKey, TPrimaryKey>();
		internal readonly Dictionary<TPrimaryKey, TSecondaryKey> primaryToSubkeyMapping = new Dictionary<TPrimaryKey, TSecondaryKey>();

		ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

		public TValue this[TSecondaryKey subKey]
		{
			get
			{
				if (TryGetValue(subKey, out TValue item))
					return item;

				throw new KeyNotFoundException("sub key not found: " + subKey.ToString());
			}
		}

		public TValue this[TPrimaryKey primaryKey]
		{
			get
			{
				if (TryGetValue(primaryKey, out TValue item))
					return item;

				throw new KeyNotFoundException("primary key not found: " + primaryKey.ToString());
			}
		}

		public void Associate(TSecondaryKey subKey, TPrimaryKey primaryKey)
		{
			readerWriterLock.EnterUpgradeableReadLock();

			try
			{
				if (!baseDictionary.ContainsKey(primaryKey))
					throw new KeyNotFoundException(string.Format("The base dictionary does not contain the key '{0}'", primaryKey));

				if (primaryToSubkeyMapping.ContainsKey(primaryKey)) // Remove the old mapping first
				{
					readerWriterLock.EnterWriteLock();

					try
					{
						subDictionary.Remove(primaryToSubkeyMapping[primaryKey]);
						primaryToSubkeyMapping.Remove(primaryKey);
					}
					finally
					{
						readerWriterLock.ExitWriteLock();
					}
				}

				subDictionary[subKey] = primaryKey;
				primaryToSubkeyMapping[primaryKey] = subKey;
			}
			finally
			{
				readerWriterLock.ExitUpgradeableReadLock();
			}
		}

		public bool TryGetValue(TSecondaryKey subKey, out TValue val)
		{
			val = default;

			readerWriterLock.EnterReadLock();

			try
			{
				if (subDictionary.TryGetValue(subKey, out TPrimaryKey primaryKey))
				{
					return baseDictionary.TryGetValue(primaryKey, out val);
				}
			}
			finally
			{
				readerWriterLock.ExitReadLock();
			}

			return false;
		}

		public bool TryGetValue(TPrimaryKey primaryKey, out TValue val)
		{
			readerWriterLock.EnterReadLock();

			try
			{
				return baseDictionary.TryGetValue(primaryKey, out val);
			}
			finally
			{
				readerWriterLock.ExitReadLock();
			}
		}

		public bool ContainsKey(TSecondaryKey subKey) => TryGetValue(subKey, out TValue _);

		public bool ContainsKey(TPrimaryKey primaryKey) => TryGetValue(primaryKey, out TValue _);

		public void Remove(TPrimaryKey primaryKey)
		{
			readerWriterLock.EnterWriteLock();

			try
			{
				subDictionary.Remove(primaryToSubkeyMapping[primaryKey]);
				primaryToSubkeyMapping.Remove(primaryKey);

				baseDictionary.Remove(primaryKey);
			}
			finally
			{
				readerWriterLock.ExitWriteLock();
			}
		}

		public void Remove(TSecondaryKey subKey)
		{
			readerWriterLock.EnterWriteLock();

			try
			{
				baseDictionary.Remove(subDictionary[subKey]);
				primaryToSubkeyMapping.Remove(subDictionary[subKey]);
				subDictionary.Remove(subKey);
			}
			finally
			{
				readerWriterLock.ExitWriteLock();
			}
		}

		public void Add(TPrimaryKey primaryKey, TValue val)
		{
			readerWriterLock.EnterWriteLock();

			try
			{
				baseDictionary.Add(primaryKey, val);
			}
			finally
			{
				readerWriterLock.ExitWriteLock();
			}
		}

		public void Add(TPrimaryKey primaryKey, TSecondaryKey subKey, TValue val)
		{
			Add(primaryKey, val);
			Associate(subKey, primaryKey);
		}

		public TValue[] CloneValues()
		{
			readerWriterLock.EnterReadLock();

			try
			{
				TValue[] values = new TValue[baseDictionary.Values.Count];

				baseDictionary.Values.CopyTo(values, 0);

				return values;
			}
			finally
			{
				readerWriterLock.ExitReadLock();
			}
		}

		public List<TValue> Values
		{
			get
			{
				readerWriterLock.EnterReadLock();

				try
				{
					return baseDictionary.Values.ToList();
				}
				finally
				{
					readerWriterLock.ExitReadLock();
				}
			}
		}

		public TPrimaryKey[] ClonePrimaryKeys()
		{
			readerWriterLock.EnterReadLock();

			try
			{
				TPrimaryKey[] values = new TPrimaryKey[baseDictionary.Keys.Count];

				baseDictionary.Keys.CopyTo(values, 0);

				return values;
			}
			finally
			{
				readerWriterLock.ExitReadLock();
			}
		}

		public TSecondaryKey[] CloneSubKeys()
		{
			readerWriterLock.EnterReadLock();

			try
			{
				TSecondaryKey[] values = new TSecondaryKey[subDictionary.Keys.Count];

				subDictionary.Keys.CopyTo(values, 0);

				return values;
			}
			finally
			{
				readerWriterLock.ExitReadLock();
			}
		}

		public void Clear()
		{
			readerWriterLock.EnterWriteLock();

			try
			{
				baseDictionary.Clear();
				subDictionary.Clear();
				primaryToSubkeyMapping.Clear();
			}
			finally
			{
				readerWriterLock.ExitWriteLock();
			}
		}

		public int Count
		{
			get
			{
				readerWriterLock.EnterReadLock();

				try
				{
					return baseDictionary.Count;
				}
				finally
				{
					readerWriterLock.ExitReadLock();
				}
			}
		}

		public IEnumerator<KeyValuePair<TPrimaryKey, TValue>> GetEnumerator()
		{
			readerWriterLock.EnterReadLock();

			try
			{
				return baseDictionary.GetEnumerator();
			}
			finally
			{
				readerWriterLock.ExitReadLock();
			}
		}
	}
}
