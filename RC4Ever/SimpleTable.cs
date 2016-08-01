using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RC4Ever
{
	/// <summary>
	/// Creates a simple RC4 table which does not permutate initial state and provides methods to help visualize the state of the table.
	/// </summary>
	public class SimpleTable : IDisposable
	{
		private byte i;
		private byte j;
		private byte k;
		private byte l;

		private byte[] _table;
		public bool IsDisposed { get; private set; }
		public static int TableSize = 256;	// Because we are using bytes
		
		public SimpleTable()
		{			
			i = 0;
			j = 0;
			k = 0;
			l = 0;

			_table = Enumerable.Range(0, TableSize).Select(b => (byte)b).ToArray();
			
			IsDisposed = false;
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
			}
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
				byte i_previous_value = (byte)(i-1);
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

		private void ThrowIfDisposed()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(this.GetType().Name);
			}
		}

		public override string ToString()
		{
			ThrowIfDisposed();

			StringBuilder result = new StringBuilder();
			int q = 0;
			while (q < TableSize)
			{				
				int r = 0;
				while (r < 16)
				{
					if (r != 0)
					{
						result.Append('|');
					}
					result.Append(
						string.Format("{0,3}", _table[q + r])
						.PadLeft(4)
						.PadRight(5)
						);

					r++;
				}
				result.AppendLine();
				result.AppendLine();
				q += 16;
			}
			
			return result.ToString().TrimEnd();
		}
		
		public Bitmap ToBitmap()
		{
			return Visualizations.ToBitmap(_table);
		}
	}
}
