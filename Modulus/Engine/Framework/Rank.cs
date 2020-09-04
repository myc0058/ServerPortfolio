using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Framework
{
	public class Rank
	{
		//score, key list
		private SortedDictionary<long, HashSet<long>> sortedScore = new SortedDictionary<long, HashSet<long>>();

		//key, score
		private Dictionary<long, long> scoreFinder = new Dictionary<long, long>();

		public void AddOrUpdate(long score, long key)
		{
			var oldScore = Contains(key);

			if (oldScore >= 0)
			{
				if (oldScore != score)
				{
					Remove(key);
					Add(score, key);
				}
				else
				{
					return;
				}
			}
			else
			{
				Add(score, key);
			}
			
			
		}

		private void Add(long score, long key)
		{
			if (sortedScore.TryGetValue(score, out var list) == true)
			{
				list.Add(key);
				scoreFinder.Add(key, score);
			}
			else
			{
				list = new HashSet<long>();
				list.Add(key);
				sortedScore.Add(score, list);
				scoreFinder.Add(key, score);
			}
		}

		private long Contains(long key)
		{
			if (scoreFinder.TryGetValue(key, out var score) == true)
			{
				return score;
			}
			else
			{
				return -1;
			}
		}

		public void Remove(long key)
		{
			if (scoreFinder.TryGetValue(key, out var score) == true)
			{
				scoreFinder.Remove(key);
				if (sortedScore.TryGetValue(score, out var keyList) == true)
				{
					keyList.Remove(key);
					if (keyList.Count < 1)
					{
						sortedScore.Remove(score);
					}
				}
			}
		}

		public long PopFirstKey()
		{
			long key = FirstKey();
			Remove(key);
			return key;
		}

		public long PopLastKey()
		{
			long key = LastKey();
			Remove(key);
			return key;
		}

		public long FirstKey()
		{
			if (sortedScore.Count < 1)
			{
				return -1;
			}

			var keyList = sortedScore.Values.First();
			if (keyList.Count < 1)
			{
				return -1;
			}

			return keyList.First();
		}

		public long LastKey()
		{
			if (sortedScore.Count < 1)
			{
				return -1;
			}

			var keyList = sortedScore.Values.Last();
			if (keyList.Count < 1)
			{
				return -1;
			}

			return keyList.Last();
		}
	}
}
