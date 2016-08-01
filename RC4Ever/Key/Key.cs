using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RC4Ever.Key
{
	using RC4Ever.Key.Internal;

	internal sealed class Key : IDisposable
	{
		private bool isDisposed = true;

		private ProtectedBuffer protectedBuffer = null;

		public Key(byte[] passwordHash, Byte roundsPerScramble, Int64 secretStartDate)
		{
			isDisposed = false;

			protectedBuffer = new ProtectedBuffer(passwordHash);

			protectedBuffer.SetSecretDate(secretStartDate);
			protectedBuffer.SetRoundsPerScramble(roundsPerScramble);

			roundsPerScramble = Byte.MinValue;
			secretStartDate = Int64.MinValue;
		}

		public void Dispose()
		{
			if (!isDisposed)
			{
				isDisposed = true;
			}
		}

		internal void InitializeTable(ref byte[] table)
		{
			if (!isDisposed)
			{
				Byte beginOffsetIndex = new Byte();
				using (CryptoRNG rand = new CryptoRNG())
				{
					beginOffsetIndex = rand.Next(Byte.MaxValue);
				}
				protectedBuffer.SetBeginOffset(beginOffsetIndex);
				int temp = beginOffsetIndex;
				
				int increment = 300;
				while (Key.FindGCD(256, increment) != 1)
				{
					increment++;
				}
				protectedBuffer.SetCoprime((uint)increment);

				// The large prime will just roll over. This is essentially just modular arithmetic
				// By choosing a co-prime to 256, we ensure we get every value from 0-255 exactly once,
				// and in a semi-uniformly distributed pattern (some co-primes are better than others)
				int counter = 256;
				List<byte> result = new List<byte>();
				unchecked
				{
					while (counter-- > 0)
					{
						temp += increment;
						if (temp > 255)
						{
							temp = temp % 256;
						}
						result.Add((byte)temp);
					}
				}

				result.CopyTo(table);
				result.Clear();
				result = null;
				beginOffsetIndex = Byte.MinValue;
				increment = 0;
				counter = -1;
				temp = 0;
			}
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
