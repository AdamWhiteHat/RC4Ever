using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using RC4Ever;
using System.Numerics;

namespace RC4EverGUI
{
	public partial class MainForm : Form
	{
		private BigInteger rounds;
		private ProbablyInsecureTable table;
		//private SimpleTable table;

		public MainForm()
		{
			InitializeComponent();
			rounds = 0;
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			table = new ProbablyInsecureTable("mYpaSSwoRd");
			//table = new SimpleTable();
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
			Step(GetStepAmount());
		}

		private void btnUndoX_Click(object sender, EventArgs e)
		{
			Undo(GetStepAmount());
		}

		private int GetStepAmount()
		{
			int amount = 0;
			if (int.TryParse(tbStepAmount.Text, out amount))
			{
				return amount;
			}
			return 0;
		}

		private void SetStepAmount(int amount)
		{
			int value = Math.Max(1, amount);
			tbStepAmount.Text = value.ToString();
		}

		private void Step(int amount)
		{
			if (amount < 1) return;

			List<byte> bytes = new List<byte>(amount + 1);

			int counter = 0;
			while (counter < amount)
			{
				bytes.Add(table.NextByte());
				rounds++;
				counter++;
			}

			bytes.Reverse();
			SetOutputBytes(tbOutBytes, bytes);

			ShowTable();
		}

		private void Undo(int amount)
		{
			if (amount < 1) return;

			List<byte> bytes = new List<byte>(amount + 1);

			int counter = 0;
			while (counter < amount)
			{
				bytes.Add(table.ReverseByte());
				rounds--;
				counter++;
			}

			bytes.Reverse();
			SetOutputBytes(tbOutUntoBytes, bytes);

			ShowTable();
		}

		private void SetOutputBytes(TextBox textBox, List<byte> bytes)
		{
			// Encountered performance problems when the text string grew too large (500KB+)
			// You should write to a file if you need to retain this data			
			textBox.Text = new string(textBox.Text.Insert(0, string.Concat(string.Join(", ", bytes.Select(b => b.ToString())), ", ")).Take(1000).ToArray());
		}

		private void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox source = sender as TextBox;
			if (source != null)
			{
				if (e.Control && e.KeyCode == Keys.A) // CTRL+A
				{
					source.SelectAll();
				}
			}
		}

		private void stepQuantity_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox source = sender as TextBox;
			if (source != null)
			{

				Action<int> stepAction = null;
				if (e.Control)
				{
					stepAction = Undo;
				}
				else
				{
					stepAction = Step;
				}


				int? amount = null;
				if (e.KeyCode == Keys.Enter)
				{
					amount = GetStepAmount();
				}
				else if (e.KeyCode == Keys.Add)
				{
					amount = (GetStepAmount() + 1);
					SetStepAmount(amount.Value);
				}
				else if (e.KeyCode == Keys.Subtract)
				{
					amount = (GetStepAmount() - 1);
					SetStepAmount(amount.Value);
				}

				if (amount.HasValue)
				{
					e.SuppressKeyPress = true;
					stepAction(Math.Max(1, amount.Value));
				}
			}
		}
	}
}
