namespace RC4EverGUI
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnStep1 = new System.Windows.Forms.Button();
			this.tbOutput = new System.Windows.Forms.TextBox();
			this.tbOutBytes = new System.Windows.Forms.TextBox();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.btnStep256 = new System.Windows.Forms.Button();
			this.tbRounds = new System.Windows.Forms.TextBox();
			this.btnStepX = new System.Windows.Forms.Button();
			this.tbStepAmount = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnUndo1 = new System.Windows.Forms.Button();
			this.btnUndo256 = new System.Windows.Forms.Button();
			this.tbOutUntoBytes = new System.Windows.Forms.TextBox();
			this.btnUndoX = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// btnStep1
			// 
			this.btnStep1.Location = new System.Drawing.Point(93, 7);
			this.btnStep1.Name = "btnStep1";
			this.btnStep1.Size = new System.Drawing.Size(75, 23);
			this.btnStep1.TabIndex = 1;
			this.btnStep1.Text = "Step 1";
			this.btnStep1.UseVisualStyleBackColor = true;
			this.btnStep1.Click += new System.EventHandler(this.btnStep1_Click);
			// 
			// tbOutput
			// 
			this.tbOutput.Location = new System.Drawing.Point(13, 57);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.Size = new System.Drawing.Size(785, 416);
			this.tbOutput.TabIndex = 2;
			// 
			// tbOutBytes
			// 
			this.tbOutBytes.Location = new System.Drawing.Point(804, 318);
			this.tbOutBytes.Multiline = true;
			this.tbOutBytes.Name = "tbOutBytes";
			this.tbOutBytes.Size = new System.Drawing.Size(255, 75);
			this.tbOutBytes.TabIndex = 3;
			this.tbOutBytes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
			// 
			// pictureBox
			// 
			this.pictureBox.Location = new System.Drawing.Point(803, 57);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(256, 256);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox.TabIndex = 4;
			this.pictureBox.TabStop = false;
			// 
			// btnStep256
			// 
			this.btnStep256.Location = new System.Drawing.Point(168, 7);
			this.btnStep256.Name = "btnStep256";
			this.btnStep256.Size = new System.Drawing.Size(75, 23);
			this.btnStep256.TabIndex = 5;
			this.btnStep256.Text = "Step 256";
			this.btnStep256.UseVisualStyleBackColor = true;
			this.btnStep256.Click += new System.EventHandler(this.btnStep256_Click);
			// 
			// tbRounds
			// 
			this.tbRounds.Enabled = false;
			this.tbRounds.Location = new System.Drawing.Point(13, 8);
			this.tbRounds.Name = "tbRounds";
			this.tbRounds.ReadOnly = true;
			this.tbRounds.Size = new System.Drawing.Size(74, 20);
			this.tbRounds.TabIndex = 6;
			// 
			// btnStepX
			// 
			this.btnStepX.Location = new System.Drawing.Point(261, 7);
			this.btnStepX.Name = "btnStepX";
			this.btnStepX.Size = new System.Drawing.Size(75, 23);
			this.btnStepX.TabIndex = 7;
			this.btnStepX.Text = "Step X";
			this.btnStepX.UseVisualStyleBackColor = true;
			this.btnStepX.Click += new System.EventHandler(this.btnStepX_Click);
			// 
			// tbStepAmount
			// 
			this.tbStepAmount.Location = new System.Drawing.Point(357, 20);
			this.tbStepAmount.Name = "tbStepAmount";
			this.tbStepAmount.Size = new System.Drawing.Size(73, 20);
			this.tbStepAmount.TabIndex = 8;
			this.tbStepAmount.Text = "3";
			this.tbStepAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(74, 17);
			this.label1.TabIndex = 9;
			this.label1.Text = "(Rounds)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// btnUndo1
			// 
			this.btnUndo1.Location = new System.Drawing.Point(93, 30);
			this.btnUndo1.Name = "btnUndo1";
			this.btnUndo1.Size = new System.Drawing.Size(75, 23);
			this.btnUndo1.TabIndex = 10;
			this.btnUndo1.Text = "Undo 1";
			this.btnUndo1.UseVisualStyleBackColor = true;
			this.btnUndo1.Click += new System.EventHandler(this.btnUndo1_Click);
			// 
			// btnUndo256
			// 
			this.btnUndo256.Location = new System.Drawing.Point(168, 30);
			this.btnUndo256.Name = "btnUndo256";
			this.btnUndo256.Size = new System.Drawing.Size(75, 23);
			this.btnUndo256.TabIndex = 11;
			this.btnUndo256.Text = "Undo 256";
			this.btnUndo256.UseVisualStyleBackColor = true;
			this.btnUndo256.Click += new System.EventHandler(this.btnUndo256_Click);
			// 
			// tbOutUntoBytes
			// 
			this.tbOutUntoBytes.Location = new System.Drawing.Point(804, 398);
			this.tbOutUntoBytes.Multiline = true;
			this.tbOutUntoBytes.Name = "tbOutUntoBytes";
			this.tbOutUntoBytes.Size = new System.Drawing.Size(255, 75);
			this.tbOutUntoBytes.TabIndex = 12;
			this.tbOutUntoBytes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
			// 
			// btnUndoX
			// 
			this.btnUndoX.Location = new System.Drawing.Point(261, 30);
			this.btnUndoX.Name = "btnUndoX";
			this.btnUndoX.Size = new System.Drawing.Size(75, 23);
			this.btnUndoX.TabIndex = 13;
			this.btnUndoX.Text = "Undo X";
			this.btnUndoX.UseVisualStyleBackColor = true;
			this.btnUndoX.Click += new System.EventHandler(this.btnUndoX_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(338, 23);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(17, 13);
			this.label2.TabIndex = 14;
			this.label2.Text = "X:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1065, 485);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnUndoX);
			this.Controls.Add(this.tbOutUntoBytes);
			this.Controls.Add(this.btnUndo256);
			this.Controls.Add(this.btnUndo1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbStepAmount);
			this.Controls.Add(this.btnStepX);
			this.Controls.Add(this.tbRounds);
			this.Controls.Add(this.btnStep256);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.tbOutBytes);
			this.Controls.Add(this.tbOutput);
			this.Controls.Add(this.btnStep1);
			this.DoubleBuffered = true;
			this.Name = "MainForm";
			this.Text = "View RC4 Table";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStep1;
		private System.Windows.Forms.TextBox tbOutput;
		private System.Windows.Forms.TextBox tbOutBytes;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Button btnStep256;
		private System.Windows.Forms.TextBox tbRounds;
		private System.Windows.Forms.Button btnStepX;
		private System.Windows.Forms.TextBox tbStepAmount;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnUndo1;
		private System.Windows.Forms.Button btnUndo256;
		private System.Windows.Forms.TextBox tbOutUntoBytes;
		private System.Windows.Forms.Button btnUndoX;
		private System.Windows.Forms.Label label2;
	}
}

