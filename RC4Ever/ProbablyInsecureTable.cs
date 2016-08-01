using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC4Ever
{
	using System.Drawing;
	using RC4Ever.Key;
	/// <summary>
	/// An example of what a more serious attempt at a RC4 variant cipher would look like
	/// </summary>
	public class ProbablyInsecureTable : IDisposable
	{
		public static int TableSize = 256;	// Because we are using bytes		
		
		private static KeyContainer PrivateKey;

		private bool isDisposed = true;
		private byte[] permutationTable;

		private byte k = 0;
		private byte i = 0;
		private byte j = 0;		
		private byte l = 0;

		public ProbablyInsecureTable(string password)
		{
			isDisposed = false;

			PrivateKey = new KeyContainer(
				passwordHash:  Encoding.ASCII.GetBytes(password),
				roundsPerScramble: 61,
				secretStartDate: DateTime.UtcNow.ToFileTimeUtc()
			);

			permutationTable = new byte[TableSize];

			PrivateKey.InitializeSequence(ref permutationTable);

			ShuffleTable();// Finish initializing the table by shuffling it			
		}

		public ProbablyInsecureTable(KeyContainer privateKey)
		{
			isDisposed = false;
			PrivateKey = privateKey;			
			permutationTable = new byte[TableSize];

			privateKey.InitializeSequence(ref permutationTable);						
			ShuffleTable();// Finish initializing the table by shuffling it			
		}

		public ProbablyInsecureTable(byte[] savedTableState, KeyContainer privateKey)
		{
			isDisposed = false;
			PrivateKey = privateKey;
			permutationTable = savedTableState;

			ShuffleTable();						
		}

		public void Reset()
		{
			this.Dispose(); // We do not want to be able to recreate the starting state so trivially
		}

		public void Dispose()
		{
			if (!isDisposed)
			{
				isDisposed = true;
				// Private key first
				PrivateKey.Dispose();
				PrivateKey = null;

				i = 0;
				j = 0;
				k = 0;
				l = 0;
				permutationTable = Enumerable.Repeat<byte>(byte.MaxValue, TableSize + 1).ToArray();
				permutationTable = null;
			}
		}

		private void DisposeCheck()
		{
			if (isDisposed)
			{
				throw new ObjectDisposedException(this.GetType().Name);
			}
		}
		
		private void ShuffleTable()
		{
			DisposeCheck();

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
			DisposeCheck();

			ShuffleTable();

			byte result = 0;
			if (MoveNext())
			{
				result = (byte)(plainTextIn ^ k);
			}
			return result;
		}

		public string Hash(string plainText)
		{
			DisposeCheck();

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
			if (MoveNext())
			{
				return k;
			}
			return default(byte);
		}

		public byte ReverseByte()
		{
			throw new NotImplementedException();
		}
		
		public bool MoveNext()
		{
			if (isDisposed)
			{
				return false;
			}
			// Just roll over on overflow. This is essentially mod 256, since everything is a byte
			unchecked
			{
				i++;
				j = (byte)(j + permutationTable[i]);

				l = permutationTable[i];
				permutationTable[i] = permutationTable[j];
				permutationTable[j] = l;

				l = (byte)(permutationTable[i] + permutationTable[j]);

				k = permutationTable[l];
			}
			return true;
		}

		public Bitmap ToBitmap()
		{
			return Visualizations.ToBitmap(permutationTable);
		}
	}
}


