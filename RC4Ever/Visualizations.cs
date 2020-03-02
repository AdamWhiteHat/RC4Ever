using System;
using System.Drawing;
using System.Text;

namespace RC4Ever
{
	public static partial class Visualizations
	{
		public static Bitmap ToBitmap(byte[] table)
		{
			int x = 0;
			int y = 0;
			int counter = 0;
			Bitmap result = new Bitmap(17, 17);

			while (counter < 240)
			{
				x = 0;
				while (x <= 16)
				{
					// For black & white image, use: Color.FromArgb(b, b, b)
					result.SetPixel(x, y, CalculateColorFromByte(table[counter + x]));
					x++;
				}
				y++;
				counter += 16;
			}

			return result;
		}

		private static Color CalculateColorFromByte(byte value)
		{
			return Visualizations.ColorPalette[value];
		}

		public static string ToString(byte[] _table)
		{			
			int q = 0;
			int tableSize = _table.Length;

			StringBuilder result = new StringBuilder();
			result.Append(Environment.NewLine);

			while (q <= 240)
			{
				result.Append("|");

				int r = 0;
				while (r < 16)
				{
					if (r != 0)
					{
						result.Append('|');
					}
					result.Append(
						string.Format("{0,3}", _table[q + r])
						.PadLeft(3)
						.PadRight(6)
						);

					r++;
				}
				result.AppendLine();
				result.AppendLine();
				q += 16;
			}

			return result.ToString().TrimEnd();
		}
	}
}
