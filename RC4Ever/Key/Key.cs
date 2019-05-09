using System;
using System.Collections.Generic;

namespace RC4Ever.Key
{
	using Internal;

	internal sealed class Key : IDisposable
	{
		public bool IsDisposed { get; private set; } = true;
		public static int TableSize = byte.MaxValue+1;

		private ProtectedBuffer protectedBuffer = null;

		public Key(byte[] passwordHash, byte roundsPerScramble, Int64 secretStartDate)
		{
			IsDisposed = false;

			protectedBuffer = new ProtectedBuffer(passwordHash);

			protectedBuffer.SetSecretDate(secretStartDate);
			protectedBuffer.SetRoundsPerScramble(roundsPerScramble);

			roundsPerScramble = 0;
			secretStartDate = 0;
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				IsDisposed = true;

				protectedBuffer.Dispose();
				protectedBuffer = null;
			}
		}
		private void ThrowIfDisposed()
		{
			if (IsDisposed) { throw new ObjectDisposedException(nameof(Key)); }
		}

		internal void InitializeTable(ref byte[] table)
		{
			ThrowIfDisposed();

			byte beginOffsetIndex = new byte();
			using (CryptoRNG rand = new CryptoRNG())
			{
				beginOffsetIndex = rand.Next(byte.MaxValue);
			}
			protectedBuffer.SetBeginOffset(beginOffsetIndex);
			int temp = beginOffsetIndex;

			int increment = 300;
			while (FindGCD(TableSize, (++increment)) != 1);

			protectedBuffer.SetCoprime((uint)increment);

			// The large prime will just roll over. This is essentially just modular arithmetic
			// By choosing a co-prime to 256, we ensure we get every value from 0-255 exactly once,
			// and in a semi-uniformly distributed pattern (some co-primes are better than others)			
			int counter = 0;
			unchecked
			{
				while (counter < TableSize)
				{
					temp += increment;
					if (temp > TableSize)
					{
						temp = temp % TableSize;
					}
					table[counter] = ((byte)temp);
					counter++;
				}
			}

			beginOffsetIndex = 0;
			increment = -1;
			counter = -1;
			temp = -1;
		}

		private static int FindGCD(int value1, int value2)
		{
			while (value1 != 0 && value2 != 0)
			{
				if (value1 > value2)
				{
					value1 %= value2;
				}
				else
				{
					value2 %= value1;
				}
			}
			return Math.Max(value1, value2);
		}
	}
}
