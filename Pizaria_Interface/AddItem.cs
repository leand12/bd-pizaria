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
	public partial class AddItem : Form
	{
		private double item_price = 0;
		private ClientMain clientMain;

		public AddItem(ClientMain clientMain)
		{
			this.clientMain = clientMain;
			InitializeComponent();
		}
		
		// Add Items
		private void button1_Click(object sender, EventArgs e)
		{
			// ...

			clientMain.balancePrice(item_price);
			clientMain.Enabled = true;
			this.Close();
		}
		
		// Cancel
		private void button2_Click(object sender, EventArgs e)
		{
			clientMain.Enabled = true;
			this.Close();
		}
		
		private void AddItem_FormClosed(object sender, FormClosedEventArgs e)
		{
			clientMain.Enabled = true;
		}
	}
}
