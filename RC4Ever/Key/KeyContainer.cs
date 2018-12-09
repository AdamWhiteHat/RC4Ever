using System;
using System.Linq;

namespace RC4Ever.Key
{
    public sealed class KeyContainer : IDisposable
	{
		private Key _key;
		private int _skipCounter;
		private static int _roundsPerScramble;
		private bool isDisposed = true;
		
		public KeyContainer(byte[] passwordHash, Byte roundsPerScramble, Int64 secretStartDate)
		{
			isDisposed = false;			
			_roundsPerScramble = roundsPerScramble;
			_skipCounter = _roundsPerScramble;			
			_key = new Key(passwordHash, roundsPerScramble, secretStartDate);

			passwordHash = Enumerable.Repeat(Byte.MaxValue, passwordHash.Length + 1).ToArray();
			passwordHash = null;
			roundsPerScramble = Byte.MaxValue;
			secretStartDate = Int64.MaxValue;
		}

		public void Dispose()
		{
			if (!isDisposed)
			{
				isDisposed = true;
				_key.Dispose();
				_key = null;
				_skipCounter = 0;
				_roundsPerScramble = 0;
			}
		}

		public bool TakeNext()
		{
			if (!isDisposed)
			{
				_skipCounter--;

				if (_skipCounter < 1)
				{
					_skipCounter = _roundsPerScramble;
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
		}

		public void InitializeSequence(ref byte[] table)
		{
			if (!isDisposed)
			{
				_key.InitializeTable(ref table);	
			}			
		}
	}
}
