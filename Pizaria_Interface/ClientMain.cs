using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Pizaria
{
	public partial class ClientMain : Form
	{
		private double price;

		public ClientMain()
		{
			InitializeComponent();
		}

		public void balancePrice(double price) { this.price += price; }

		private void ClientMain_Load(object sender, EventArgs e)
		{
			//this.Enabled = false;
		}
		
		// History
		private void button1_Click(object sender, EventArgs e)
		{

		}

		// Log Out
		private void button2_Click(object sender, EventArgs e)
		{
			this.Hide();
			var login = new Login();
			login.ShowDialog();
			this.Close();
		}
			
		// Clear All
		private void button3_Click(object sender, EventArgs e)
		{

		}
		
		// Finish Order
		private void button4_Click(object sender, EventArgs e)
		{

		}
			
		// Add Items
		private void button5_Click(object sender, EventArgs e)
		{
			this.Enabled = false;
			var addItem = new AddItem(this);
			addItem.ShowDialog();
		}
	}
}
