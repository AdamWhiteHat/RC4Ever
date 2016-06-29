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
			ThrowIfDisposed();
			
			int x = 0;
			int y = 0;
			int counter = 0;
			Bitmap result = new Bitmap(17, 17);

			while (counter < 256)
			{
				x = 0;
				while (x < 16)
				{
					// For black & white image, use: Color.FromArgb(b, b, b)
					result.SetPixel(x, y, CalculateColorFromByte(_table[counter + x]));	
					x++;
				}
				y++;
				counter += 16;
			}

			return result;
		}

		private int r = 0;
		private int g = 0;
		private int b = 0;
		private Color CalculateColorFromByte(byte value)
		{ 			
			if (value < 43)	// Black to red
			{	// Red ascending from 0-255 red
				r = Map42ValueTo255(value); 
			}
			else if (value >= 43 && value < 86) // Red to yellow
			{
				r = 255; // Full Red
				g = Map42ValueTo255(value - 43); // Green ascending from 0-255
			}
			else if (value >= 86 && value < 128) // Yellow to green
			{
				g = 255; // Full Green
				r = Map42ValueTo255(byte.MaxValue - (value - 85)); // Red descending from 255-0
			}
			else if (value >= 128 && value < 171) // Green to light blue
			{
				g = 255; // Full Green
				b = Map42ValueTo255(value - 128); // Blue ascending from 0-255 
			}
			else if (value >= 171 && value < 213)  // Light blue to deep blue
			{
				b = 255; // Full Blue
				g = Map42ValueTo255(byte.MaxValue - (value - 170)); // Green descending from 255-0
			}			
			else if (value >= 213 && value <= 255) // Blue to white
			{
				b = 255; // Full Blue
				r = Map42ValueTo255(value - 213); // Red ascending from 0-255
				g = Map42ValueTo255(value - 213); // Green ascending from 0-255
			}

			return Color.FromArgb(r, g, b);
		}

		private int Map42ValueTo255(int value)
		{
			return (value * 6) % byte.MaxValue;
		}
	}
}
