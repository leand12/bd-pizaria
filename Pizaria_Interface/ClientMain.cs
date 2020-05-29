using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace Pizaria
{
	public partial class ClientMain : Form
	{
		private double price;
		private List<Item> shop_cart;

		public ClientMain()
		{
			this.shop_cart = new List<Item>();
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

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.MenuView ", Program.cn);
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

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.PizaView", Program.cn);
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

			if (listBox2.Items.Count == 0 | curr_menu < 0)
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

			if (listBox3.Items.Count == 0 | curr_pizza < 0)
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


					string photo = reader["pic"].ToString();
					byte[] image = Convert.FromBase64String(photo);
					Image ret = null;
					using (MemoryStream ms = new MemoryStream(image))
					{
						ret = Image.FromStream(ms);
					}
					pictureBox2.Image = ret;
					pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
	

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
            listBox6.Items.Clear();
			this.shop_cart.Clear();
		}

		// Finish Order
		private void button4_Click(object sender, EventArgs e)
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand
			{
				CommandType = CommandType.StoredProcedure,
				CommandText = "Pizaria.TranShopCart"
			};
			
			cmd.Parameters.Add(new SqlParameter("@cliente_email", SqlDbType.NVarChar, 255));
			cmd.Parameters.Add(new SqlParameter("@endereco_fisico", SqlDbType.VarChar, 50));
			cmd.Parameters.Add(new SqlParameter("@hora", SqlDbType.Date));
			cmd.Parameters.Add(new SqlParameter("@metodo_pagamento", SqlDbType.VarChar, 30));
			cmd.Parameters.Add(new SqlParameter("@des_codigo", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@response", SqlDbType.VarChar, 50));
			cmd.Parameters["@cliente_email"].Value = Program.email;
			cmd.Parameters["@endereco_fisico"].Value = textBox1.Text;
			cmd.Parameters["@hora"].Value = dateTimePicker1.Value;
			cmd.Parameters["@metodo_pagamento"].Value = "Card";
			cmd.Parameters["@des_codigo"].Value = (textBox2.Text != "") ? int.Parse(textBox2.Text) : 0;
			cmd.Parameters["@response"].Direction = ParameterDirection.Output;

			cmd.Connection = Program.cn;

			cmd.ExecuteNonQuery();

			Program.cn.Close();

			string response = "" + cmd.Parameters["@response"].Value;
			MessageBox.Show(response);

			if (response == "Success")
				tabControl4.SelectedIndex = 1;

			listBox6.Items.Clear();
			LoadShopCart();
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
            if (listBox6.SelectedIndex > 0 && listBox6.SelectedItem != null)
			{
				this.shop_cart.Remove((Item) listBox6.SelectedItem);
                LoadShopCart();
			}
        }

		private void textBox3and4_TextChanged(object sender, EventArgs e)
		{
			String name = textBox4.Text;
			String priceStr = textBox3.Text.Trim();
			decimal price;
			try {
               price =(priceStr != "") ? decimal.Parse(priceStr, System.Globalization.CultureInfo.GetCultureInfo("en-US")) : 9999;
			}
			catch
			{
				return;
			}

			using (SqlDataAdapter da = new SqlDataAdapter())
			{
				if (!Program.verifySGBDConnection())
					return;


				SqlCommand cmd = new SqlCommand
				{
					CommandType = CommandType.StoredProcedure,
					CommandText = "Pizaria.filterItem"
				};
				cmd.Parameters.Add(new SqlParameter("@price", SqlDbType.Decimal));
				cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar,30));
				cmd.Parameters.Add(new SqlParameter("@item_type", SqlDbType.VarChar, 30));
				cmd.Parameters["@price"].Value = price;
				cmd.Parameters["@name"].Value = name;
				cmd.Parameters["@item_type"].Value = "Piza";

				listBox3.Items.Clear();
				listBox7.Items.Clear();
				listBox2.Items.Clear();
				listBox1.Items.Clear();

				pictureBox2.Image = null;

				cmd.Connection = Program.cn;
				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						Item I = new Item(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), double.Parse(reader["preco"].ToString()));
						listBox3.Items.Add(I);
					}
				}
				

				cmd = new SqlCommand
				{
					CommandType = CommandType.StoredProcedure,
					CommandText = "Pizaria.filterItem"
				};
				cmd.Parameters.Add(new SqlParameter("@price", SqlDbType.Decimal));
				cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 30));
				cmd.Parameters.Add(new SqlParameter("@item_type", SqlDbType.VarChar, 30));
				cmd.Parameters["@price"].Value = price;
				cmd.Parameters["@name"].Value = name;
				cmd.Parameters["@item_type"].Value = "Menu";

				cmd.Connection = Program.cn;

				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						Item I = new Item(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), double.Parse(reader["preco"].ToString()));
						listBox2.Items.Add(I);
					}
				}

				Program.cn.Close();
			}
		}
	}
}
