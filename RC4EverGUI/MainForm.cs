using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RC4Ever;

namespace RC4EverGUI
{
	public partial class MainForm : Form
	{
		private long rounds;
		private ProbablyInsecureTable table;

		public MainForm()
		{
			InitializeComponent();
			rounds = 0;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			table = new ProbablyInsecureTable("AdAmrAkaskA");
			ShowTable();
		}

		private void ShowTable()
		{
			// Consider passing the Graphics object, or re-using the Bitmap object if performance becomes an issue.
			pictureBox.Image = table.ToBitmap();

			tbOutput.Text = table.ToString();
			tbRounds.Text = rounds.ToString();
		}

		private void btnStep1_Click(object sender, EventArgs e)
		{
			Step(1);
		}

		private void btnUndo1_Click(object sender, EventArgs e)
		{
			Undo(1);
		}

		private void btnStep256_Click(object sender, EventArgs e)
		{
			Step(256);
		}

		private void btnUndo256_Click(object sender, EventArgs e)
		{
			Undo(256);
		}

		private void btnStepX_Click(object sender, EventArgs e)
		{
			int amount = 0;
			if (int.TryParse(tbStepAmount.Text, out amount))
			{
				Step(amount);
			}			
		}

		private void btnUndoX_Click(object sender, EventArgs e)
		{
			int amount = 0;
			if (int.TryParse(tbStepAmount.Text, out amount))
			{
				Undo(amount);
			}
		}

		private void Step(int amount)
		{
			List<byte> bytes = new List<byte>(amount+1);

			int counter = 0;
			while (counter++ < amount)
			{				
				bytes.Add(table.NextByte());
				rounds++;
			}

			bytes.Reverse();			
			SetOutputBytes(tbOutBytes, bytes);

			ShowTable();
		}

		private void Undo(int amount)
		{
			List<byte> bytes = new List<byte>(amount+1);

			int counter = 0;
			while (counter++ < amount)
			{
				bytes.Add(table.ReverseByte());
				rounds--;
			}

			bytes.Reverse();
			SetOutputBytes(tbOutUntoBytes, bytes);
			
			ShowTable();
		}

		private void SetOutputBytes(TextBox textBox, List<byte> bytes)
		{
			// Encountered performance problems when the text string grew too large (500KB+)
			// You should write to a file if you need to retain this data
			if (textBox.TextLength > 2560)
			{
				textBox.Clear();
			}

			textBox.Text = textBox.Text.Insert(0, string.Concat(string.Join(", ", bytes.Select(b => b.ToString())), ", "));
		}

		private void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox source = sender as TextBox;
			if (source != null)
			{
				if(e.Control && e.KeyCode == Keys.A) // CTRL+A
				{
					source.SelectAll();
				}
			}
		}

		private void tbOutput_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.A)
			{
				tbOutput.SelectAll();
			}
		}
			}
}
