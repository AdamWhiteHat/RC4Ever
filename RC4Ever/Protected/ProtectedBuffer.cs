using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RC4Ever
{
	using RC4Ever.Key.Internal;

	public class ProtectedBuffer : IDisposable
	{
		public bool IsSecretDateSet { get; private set; }
		public bool IsCoprimeSet { get; private set; }
		public bool IsBeginOffsetSet { get; private set; }
		public bool IsRoundsPerScramblSet { get; private set; }

		private Byte[] Salt = null;
		private Byte[] ProtectedMemory16 = null;
		private bool IsDisposed = false;

		private void CheckDisposed()
		{
			if (IsDisposed) { throw new ObjectDisposedException("ProtectedBuffer"); }
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				ZeroBuffer(Salt);
				ZeroBuffer(ProtectedMemory16);
			}
		}

		public ProtectedBuffer(Byte[] password)
		{
			Salt = new Byte[16];
			ProtectedMemory16 = new Byte[16];
			try
			{
				using (CryptoRNG rand = new CryptoRNG())
				{
					rand.NextBytes(Salt);
				}

				using (Rfc2898DeriveBytes derivedBytes = new Rfc2898DeriveBytes(password, Salt, 1000))
				{
					ProtectedMemory16 = derivedBytes.GetBytes(ProtectedMemory16.Length);
					ZeroBuffer(password);
				}
			}
			finally
			{
				ProtectedMemory.Protect(ProtectedMemory16, MemoryProtectionScope.SameProcess);
				ProtectedMemory.Protect(Salt, MemoryProtectionScope.SameProcess);
			}

			IsDisposed = false;
		}

		private void SetBuffer(Byte[] value, int offset)
		{
			try
			{
				UnprotectMem();

				int index = offset;
				while (index < value.Length)
				{
					ProtectedMemory16[index] ^= value[index - offset];
					index++;
				}
				index = 0;
				offset = 0;
			}
			finally
			{
				ProtectMem();
			}
		}

		public void SetSecretDate(Int64 startDate)
		{
			CheckDisposed();
			Byte[] temp = null;
			try
			{
				temp = BitConverter.GetBytes(startDate);
				SetBuffer(temp, 1);
			}
			finally
			{
				startDate = Int64.MinValue;
				ZeroBuffer(temp);
				temp = null;
				IsSecretDateSet = true;
			}
		}

		public void SetCoprime(UInt32 coprime)
		{
			CheckDisposed();
			Byte[] temp = null;
			try
			{
				temp = BitConverter.GetBytes(coprime);
				SetBuffer(temp, 9);
			}
			finally
			{
				coprime = UInt32.MinValue;
				ZeroBuffer(temp);
				temp = null;
				IsCoprimeSet = true;
			}
		}

		public void SetBeginOffset(Byte beginOffset)
		{
			CheckDisposed();
			Byte[] temp = null;
			try
			{
				temp = new Byte[] { beginOffset };
				SetBuffer(temp, 13);
			}
			finally
			{
				beginOffset = Byte.MinValue;
				ZeroBuffer(temp);
				temp = null;
				IsBeginOffsetSet = true;				
			}
		}

		public void SetRoundsPerScramble(Byte roundsPerScrample)
		{
			CheckDisposed();
			Byte[] temp = null;
			try
			{
				temp = new Byte[] { roundsPerScrample };
				SetBuffer(temp, 14);
			}
			finally
			{
				roundsPerScrample = Byte.MinValue;
				ZeroBuffer(temp);
				temp = null;
				IsRoundsPerScramblSet = true;
			}
		}

		public static void ZeroBuffer(Byte[] buffer)
		{
			if (buffer == null) { throw new ArgumentNullException("buffer"/*nameof(buffer)*/); }

			int index = 0;
			while (index < buffer.Length)
			{
				buffer[index++] = Byte.MinValue;
			}
			index = 0;
			buffer = null;
		}

		private void ProtectMem()
		{
			CheckDisposed();
			ProtectedMemory.Protect(ProtectedMemory16, MemoryProtectionScope.SameProcess);
		}

		private void UnprotectMem()
		{
			CheckDisposed();
			ProtectedMemory.Unprotect(ProtectedMemory16, MemoryProtectionScope.SameProcess);
		}
	}
}
