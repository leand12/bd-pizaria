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
		private List<Item> shop_cart;

		public ClientMain()
		{
			this.shop_cart=new List<Item>();
			InitializeComponent();
		}

		public void BalancePrice(double price) { this.price += price; }

		private void ClientMain_Load(object sender, EventArgs e)
		{
			LoadMenus();
			LoadPizzas();
			LoadShopCart();
		}

		public void LoadShopCart()
		{
			listBox6.Items.Clear();

			foreach (Item item in shop_cart)
			{
				listBox6.Items.Add(item);
			}
		}

		private void LoadMenus()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.Item JOIN Pizaria.Menu ON Item.ID=Menu.ID ", Program.cn);
			SqlDataReader reader = cmd.ExecuteReader();
			listBox2.Items.Clear();

			while (reader.Read())
			{
				Item I = new Item(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), double.Parse(reader["preco"].ToString()));
				listBox2.Items.Add(I);
			}

			Program.cn.Close();

		}

		private void LoadPizzas()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.Item JOIN Pizaria.Piza ON Item.ID=Piza.ID ", Program.cn);
			SqlDataReader reader = cmd.ExecuteReader();
			listBox3.Items.Clear();

			while (reader.Read())
			{
				Item I = new Item(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), double.Parse(reader["preco"].ToString()));
				listBox3.Items.Add(I);
			}

			Program.cn.Close();

		}

		// List Menu
		private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			int curr_menu = listBox2.SelectedIndex;

			if (listBox2.Items.Count == 0 | curr_menu <= 0)
			{
				return;
			}

			Item item = (Item)listBox2.Items[curr_menu];


			SqlCommand cmd;
			cmd = new SqlCommand("select * from Pizaria.showMenu ('" + item.ID + "')", Program.cn);
			if (!Program.verifySGBDConnection())
				return;

			listBox1.Items.Clear();
			using (SqlDataReader reader = cmd.ExecuteReader())
			{

				while (reader.Read())
				{
					string name = reader["nome"].ToString();
					string price = reader["preco"].ToString();
					string quantity = reader["quantidade"].ToString();

					listBox1.Items.Add(name + " " + price + "€ " + quantity);
				}
			}


			Program.cn.Close();
		}

		// List Pizzas
		private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			int curr_pizza = listBox3.SelectedIndex;

			if (listBox3.Items.Count == 0 | curr_pizza <= 0)
			{
				return;
			}


			Item item = (Item)listBox3.Items[curr_pizza];

			SqlCommand cmd;
			cmd = new SqlCommand("select * from Pizaria.showPiza ('" + item.ID + "')", Program.cn);
			if (!Program.verifySGBDConnection())
				return;

			listBox7.Items.Clear();
			using (SqlDataReader reader = cmd.ExecuteReader())
			{

				while (reader.Read())
				{
					string name = reader["nome"].ToString();
					string price = reader["preco"].ToString();
					string quantity = reader["quantidade"].ToString();

					listBox7.Items.Add(name + " " + price + "€ " + quantity);
				}
			}


			Program.cn.Close();
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
		private void button6_Click(object sender, EventArgs e)
		{
			this.Enabled = false;
			var addItem = new AddItem(this, shop_cart);
			addItem.ShowDialog();
		}

		// Remove Item
		private void button5_Click(object sender, EventArgs e)
		{

		}

	}
}
