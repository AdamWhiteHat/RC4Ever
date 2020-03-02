using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

namespace RC4Ever
{
	using Key.Internal;

	/// <summary>
	/// Creates a simple RC4 table which does not permutate initial state and provides methods to help visualize the state of the table.
	/// </summary>
	public class SimpleTable : IDisposable
	{
		public bool IsDisposed { get; private set; }
		public static int TableSize = byte.MaxValue+1;  // Because we are using bytes

		private byte i = 0;
		private byte j = 0;
		private byte k = 0;
		private byte l = 0;

		private byte[] _table;

		public SimpleTable()
		{
			i = 0; j = 0; k = 0; l = 0;

			_table = Enumerable.Range(0, TableSize).Select(b => (byte)b).ToArray();
			IsDisposed = false;
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
			}
		}

		private void ThrowIfDisposed()
		{
			if (IsDisposed) { throw new ObjectDisposedException(nameof(SimpleTable)); }
		}

		public byte NextByte()
		{
			ThrowIfDisposed();

			unchecked // Just roll over on overflow. This is essentially mod 256, since everything is a byte
			{
				i++;
				j = (byte)(j + _table[i]);

				SwapIandJ();

				//byte[] shuffledTable = BitShuffle.Interleave(_table);
				//_table = shuffledTable;

				l = (byte)(_table[i] + _table[j]);
				k = (byte)_table[l]; //K = S[ S[i] + S[j] ]

				return k;
			}
		}

		public byte ReverseByte()
		{
			ThrowIfDisposed();

			unchecked // Just roll over on overflow. This is essentially mod 256, since everything is a byte
			{
				List<byte> tableList = _table.ToList();

				SwapIandJ();

				byte i_value = _table[i];

				byte j_previous_value = (byte)(j - i_value);
				byte i_previous_value = (byte)(i - 1);
				l = (byte)(_table[i_previous_value] + _table[j_previous_value]);
				byte k_previous_value = (byte)_table[l];

				i = i_previous_value;
				j = j_previous_value;
				k = k_previous_value;

				return k;
			}
		}

		private void SwapIandJ()
		{
			l = _table[i];
			_table[i] = _table[j];
			_table[j] = l;
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
