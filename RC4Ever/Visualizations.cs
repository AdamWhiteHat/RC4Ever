using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC4Ever
{
	public static class Visualizations
	{
		public static Bitmap ToBitmap(byte[] table)
		{
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
					result.SetPixel(x, y, CalculateColorFromByte(table[counter + x]));
					x++;
				}
				y++;
				counter += 16;
			}

			return result;
		}

		private static int r = 0;
		private static int g = 0;
		private static int b = 0;
		private static Color CalculateColorFromByte(byte value)
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

		private static int Map42ValueTo255(int value)
		{
			return (value * 6) % byte.MaxValue;
		}
	}
}
