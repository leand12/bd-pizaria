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

		private void textBox3_TextChanged(object sender, EventArgs e)
		{
			String name = textBox4.Text;
			String priceStr = textBox3.Text.Trim();
			decimal price = 99999999;
			if (priceStr != "")
			{
				price = decimal.Parse(priceStr, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
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
				cmd.Connection = Program.cn;

				da.SelectCommand = cmd;

				da.SelectCommand.CommandType = CommandType.StoredProcedure;
					

				DataSet ds = new DataSet();
				da.Fill(ds, "result_name");
				DataTable dt = ds.Tables["result_name"];
				listBox3.Items.Clear();
					

				if (dt != null && dt.Rows.Count > 0)
				{
					foreach (DataRow row in dt.Rows)
					{
						Item I = new Item(int.Parse(row["ID"].ToString()), row["nome"].ToString(), double.Parse(row["preco"].ToString()));
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

				da.SelectCommand = cmd;

				da.SelectCommand.CommandType = CommandType.StoredProcedure;


				ds = new DataSet();
				da.Fill(ds, "result_name");
				dt = ds.Tables["result_name"];
				listBox2.Items.Clear();


				if (dt != null && dt.Rows.Count > 0)
				{
					foreach (DataRow row in dt.Rows)
					{
						Item I = new Item(int.Parse(row["ID"].ToString()), row["nome"].ToString(), double.Parse(row["preco"].ToString()));
						listBox2.Items.Add(I);
					}
				}

				Program.cn.Close();
			}

		}

		private void textBox4_TextChanged(object sender, EventArgs e)
		{
			String name = textBox4.Text;
			String priceStr = textBox3.Text.Trim();
			decimal price = 99999999;
			if (priceStr != "")
			{
				price = decimal.Parse(priceStr);
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
				cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 30));
				cmd.Parameters.Add(new SqlParameter("@item_type", SqlDbType.VarChar, 30));

				cmd.Parameters["@price"].Value = price;
				cmd.Parameters["@name"].Value = name;
				cmd.Parameters["@item_type"].Value = "Piza";
				cmd.Connection = Program.cn;

				da.SelectCommand = cmd;

				da.SelectCommand.CommandType = CommandType.StoredProcedure;


				DataSet ds = new DataSet();
				da.Fill(ds, "result_name");
				DataTable dt = ds.Tables["result_name"];
				listBox3.Items.Clear();


				if (dt != null && dt.Rows.Count > 0)
				{
					foreach (DataRow row in dt.Rows)
					{
						Item I = new Item(int.Parse(row["ID"].ToString()), row["nome"].ToString(), double.Parse(row["preco"].ToString()));
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

				da.SelectCommand = cmd;

				da.SelectCommand.CommandType = CommandType.StoredProcedure;


				ds = new DataSet();
				da.Fill(ds, "result_name");
				dt = ds.Tables["result_name"];
				listBox2.Items.Clear();


				if (dt != null && dt.Rows.Count > 0)
				{
					foreach (DataRow row in dt.Rows)
					{
						Item I = new Item(int.Parse(row["ID"].ToString()), row["nome"].ToString(), double.Parse(row["preco"].ToString()));
						listBox2.Items.Add(I);
					}
				}

				Program.cn.Close();
			}
		}
	}
}
