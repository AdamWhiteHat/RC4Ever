using System;
using System.Security.Cryptography;

namespace RC4Ever.Key.Internal
{
    public sealed class CryptoRNG : IDisposable
	{
		private bool IsDisposed = false;
		private byte[] rngBytes1 = new byte[1];
		private RNGCryptoServiceProvider rngCsp;

		public CryptoRNG()
		{
			disposeCheck();
			rngCsp = new RNGCryptoServiceProvider();
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (rngCsp != null)
				{
					rngCsp.Dispose();
					rngCsp = null;
				}
				
				if (rngBytes1 != null)
				{
					ZeroBuffer(ref rngBytes1);
					rngBytes1 = null;
				}
			}
		}

		private void disposeCheck()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException("CryptoRNG");
			}
		}

		public static void ZeroBuffer(ref byte[] buffer)
		{
			if (buffer == null)
			{
				return;
			}
			if (buffer.Length > 0)
			{
				int size = buffer.Length;
				int counter = 0;
				while (counter < size)
				{
					buffer[counter] = byte.MinValue;
				}
				size = 0;
				counter = 0;
			}
			buffer = null;
		}

		public void NextBytes(byte[] buffer)
		{
			disposeCheck();
			rngCsp.GetBytes(buffer);
		}
		
		public byte Next()
		{
			disposeCheck();
			rngCsp.GetBytes(rngBytes1);
			return rngBytes1[0];
		}

		public byte Next(byte maxValue)
		{
			disposeCheck();
			return (byte)(Next() % maxValue);
		}

		
	}
}
