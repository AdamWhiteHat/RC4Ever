using System;
using System.Security.Cryptography;

namespace RC4Ever.Key.Internal
{
	public sealed class CryptoRNG : IDisposable
	{
		public bool IsDisposed { get; private set; } = true;

		private byte[] _rngBytes1 = new byte[1];
		private RNGCryptoServiceProvider _rngCsp;

		public CryptoRNG()
		{
			IsDisposed = false;
			_rngCsp = new RNGCryptoServiceProvider();
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				IsDisposed = true;

				if (_rngBytes1 != null)
				{
					ZeroBuffer(_rngBytes1);
					_rngBytes1 = null;
				}

				if (_rngCsp != null)
				{
					_rngCsp.Dispose();
					_rngCsp = null;
				}
			}
		}

		private void ThrowIfDisposed()
		{
			if (IsDisposed) { throw new ObjectDisposedException(nameof(CryptoRNG)); }
		}

		public static void ZeroBuffer(byte[] buffer)
		{
			if (buffer == null || buffer.Length <= 0) { return; }

			int max = buffer.Length;
			int index = -1;
			while (++index < max)
			{
				buffer[index] = byte.MinValue;
			}
			max = 0;
			index = 0;
			buffer = null;
		}

		public void NextBytes(byte[] buffer)
		{
			ThrowIfDisposed();
			_rngCsp.GetBytes(buffer);
		}

		public byte Next(byte maxValue)
		{
			ThrowIfDisposed();

			byte[] result = new byte[1];
			NextBytes(result);

			return (byte)(result[0] % maxValue);
		}
	}
}
