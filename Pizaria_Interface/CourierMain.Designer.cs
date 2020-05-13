namespace Pizaria
{
	partial class CourierMain
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
			this.label5 = new System.Windows.Forms.Label();
			this.listBox5 = new System.Windows.Forms.ListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(296, 106);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(136, 20);
			this.label5.TabIndex = 20;
			this.label5.Text = "Orders to Deliever";
			// 
			// listBox5
			// 
			this.listBox5.FormattingEnabled = true;
			this.listBox5.ItemHeight = 20;
			this.listBox5.Location = new System.Drawing.Point(300, 149);
			this.listBox5.Name = "listBox5";
			this.listBox5.Size = new System.Drawing.Size(120, 84);
			this.listBox5.TabIndex = 19;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(313, 343);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(98, 53);
			this.button1.TabIndex = 21;
			this.button1.Text = "Order Delivered";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.Color.Yellow;
			this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.button2.Location = new System.Drawing.Point(647, 27);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(111, 41);
			this.button2.TabIndex = 22;
			this.button2.Text = "Log out";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// CourierMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.listBox5);
			this.Name = "CourierMain";
			this.Text = "EstafetaMain";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ListBox listBox5;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}