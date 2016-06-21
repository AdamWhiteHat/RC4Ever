using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace RC4Ever.Key
{
	public class KeyContainer : IDisposable
	{
		private Key _key;
		private int _skipCounter;
		private bool isDisposed = true;
		
		public KeyContainer(int coprimeTo256, byte beginOffsetIndex, string passwordHash, int roundsPerScramble, DateTime secretStartDate)
		{
			isDisposed = false;
			_key = new Key(coprimeTo256, beginOffsetIndex, passwordHash, roundsPerScramble, secretStartDate);
			_skipCounter = roundsPerScramble;
		}

		public void Dispose()
		{
			if (!isDisposed)
			{
				isDisposed = true;
				_key.Dispose();
				_key = null;
			}
		}

		public bool TakeNext()
		{
			if (!isDisposed)
			{
				_skipCounter--;

				if (_skipCounter < 1)
				{
					_skipCounter = _key.RoundsPerScramble;
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
		}

		public IEnumerable<byte> InitializeSequence()
		{
			if (!isDisposed)
			{
				int temp = 0;
				byte index = 0;
				int counter = 256;

				// The large prime will just roll over. This is essentially just modular arithmetic
				// By choosing a co-prime to 256, we ensure we get every value from 0-255 exactly once,
				// and in a semi-uniformly distributed pattern (some co-primes are better than others)
				unchecked
				{
					while (counter-- > 0)
					{
						temp = index + _key.Coprime;
						if (temp > 255)
						{
							temp = temp % 256;
						}
						index = (byte)temp;
						yield return index;
					}
				}
			}
			yield break;
		}
	}
}
