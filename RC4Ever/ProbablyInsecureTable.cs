using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

namespace RC4Ever
{
	using Key;
	using Key.Internal;

	/// <summary>
	/// An example of what a more serious attempt at a RC4 variant cipher would look like
	/// </summary>
	public class ProbablyInsecureTable : IDisposable
	{
		public bool IsDisposed { get; private set; } = true;
		public static int TableSize = byte.MaxValue;  // Because we are using bytes		

		private byte k = 0;
		private byte i = 0;
		private byte j = 0;
		private byte l = 0;

		private byte[] _table = null;
		private static KeyContainer PrivateKey = null;

		public ProbablyInsecureTable(string password)
		{
			IsDisposed = false;

			i = 0; j = 0; k = 0; l = 0;

			PrivateKey = new KeyContainer(
				passwordHash: Encoding.ASCII.GetBytes(password),
				roundsPerScramble: 61,
				secretStartDate: DateTime.UtcNow.ToFileTimeUtc()
			);

			_table = new byte[TableSize + 1];
			PrivateKey.InitializeSequence(ref _table);

			ShuffleTable();// Finish initializing the table by shuffling it			
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				IsDisposed = true;

				i = 0;
				j = 0;
				k = 0;
				l = 0;

				CryptoRNG.ZeroBuffer(_table);
				_table = null;

				PrivateKey.Dispose();
				PrivateKey = null;
			}
		}

		private void ThrowIfDisposed()
		{
			if (IsDisposed) { throw new ObjectDisposedException(nameof(ProbablyInsecureTable)); }
		}

		private void ShuffleTable()
		{
			ThrowIfDisposed();

			while (!PrivateKey.TakeNext())
			{
				if (!MoveNext())
				{
					break;
				}
			}
		}

		public byte Hash(byte plainTextIn)
		{
			ThrowIfDisposed();

			ShuffleTable();

			if (MoveNext())
			{
				return (byte)(plainTextIn ^ k);
			}

			return default(byte);
		}

		public string Hash(string plainText)
		{
			ThrowIfDisposed();

			List<byte> cypherBytes = new List<byte>();
			foreach (byte byteIn in Encoding.ASCII.GetBytes(plainText))
			{
				cypherBytes.Add(Hash(byteIn));
			}

			if (cypherBytes.Count % 2 != 0)
			{
				throw new ArrayTypeMismatchException("cypherBytes.Count % 2 != 0");
			}

			return Encoding.ASCII.GetString(cypherBytes.ToArray());
		}

		public byte NextByte()
		{
			ThrowIfDisposed();

			if (MoveNext())
			{
				return k;
			}

			return default(byte);
		}

		public byte ReverseByte()
		{
			ThrowIfDisposed();
			throw new NotImplementedException();
		}

		public bool MoveNext()
		{
			ThrowIfDisposed();

			// Just roll over on overflow. This is essentially mod 256, since everything is a byte
			unchecked
			{
				i++;
				j = (byte)(j + _table[i]);

				l = _table[i];
				_table[i] = _table[j];
				_table[j] = l;

				l = (byte)(_table[i] + _table[j]);

				k = _table[l];
			}

			return true;
		}

		public override string ToString()
		{
			ThrowIfDisposed();
			return Visualizations.ToString(_table);
		}

		public Bitmap ToBitmap()
		{
			ThrowIfDisposed();
			return Visualizations.ToBitmap(_table);
		}
	}
}


