using System;
using System.Linq;

namespace RC4Ever.Key
{
	using Internal;

	public sealed class KeyContainer : IDisposable
	{
		public bool IsDisposed { get; private set; } = true;

		private Key _key = null;
		private int _skipCounter = -1;
		private static int _roundsPerScramble = -1;

		public KeyContainer(byte[] passwordHash, byte roundsPerScramble, Int64 secretStartDate)
		{
			IsDisposed = false;
			_roundsPerScramble = roundsPerScramble;
			_skipCounter = _roundsPerScramble;
			_key = new Key(passwordHash, roundsPerScramble, secretStartDate);

			CryptoRNG.ZeroBuffer(passwordHash);
			passwordHash = null;
			roundsPerScramble = byte.MaxValue;
			secretStartDate = Int64.MaxValue;
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				IsDisposed = true;

				_skipCounter = 0;
				_roundsPerScramble = 0;

				_key.Dispose();
				_key = null;
			}
		}

		private void ThrowIfDisposed()
		{
			if (IsDisposed) { throw new ObjectDisposedException(nameof(KeyContainer)); }
		}

		public bool TakeNext()
		{
			ThrowIfDisposed();

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

		public void InitializeSequence(ref byte[] table)
		{
			ThrowIfDisposed();
			_key.InitializeTable(ref table);
		}
	}
}
