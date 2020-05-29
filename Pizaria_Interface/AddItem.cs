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
		private List<Item> shop_cart;

		public AddItem(ClientMain clientMain, List<Item> shop_cart)
		{
			this.clientMain = clientMain;
			this.shop_cart = shop_cart;
			InitializeComponent();
		}

		private void AddItem_Load(object sender, EventArgs e)
		{
			LoadIngredients();
			LoadDrinks();
			LoadMenus();
			LoadPizzas();
		}

		private void LoadMenus()
		{
			if (!Program.verifySGBDConnection())
				return;


			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.MenuView", Program.cn);
			SqlDataReader reader = cmd.ExecuteReader();
			listBox1.Items.Clear();

			while (reader.Read())
			{
				Item I = new Item(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), double.Parse(reader["preco"].ToString()));
				listBox1.Items.Add(I);
			}

			Program.cn.Close();
		}

		private void LoadPizzas()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.PizaView", Program.cn);
			SqlDataReader reader = cmd.ExecuteReader();
			listBox2.Items.Clear();

			while (reader.Read())
			{
				Item I = new Item(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), double.Parse(reader["preco"].ToString()));
				listBox2.Items.Add(I);
			}

			Program.cn.Close();
		}

		private void LoadDrinks()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.BebidaView", Program.cn);
			SqlDataReader reader = cmd.ExecuteReader();
			listBox3.Items.Clear();

			while (reader.Read())
			{
				Item I = new Item(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), double.Parse(reader["preco"].ToString()));
				listBox3.Items.Add(I);
			}

			Program.cn.Close();
		}

		private void LoadIngredients()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.IngredienteView", Program.cn);
			SqlDataReader reader = cmd.ExecuteReader();
			listBox4.Items.Clear();

			while (reader.Read())
			{
				Item I = new Item(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), double.Parse(reader["preco"].ToString()));
				listBox4.Items.Add(I);
			}

			Program.cn.Close();
		}



		// Confirm Items
		private void button1_Click(object sender, EventArgs e)
		{
			clientMain.BalancePrice(item_price);
			clientMain.Enabled = true;
			clientMain.LoadShopCart();
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
		
		// List Menus (list 1)
		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int curr_menu = listBox1.SelectedIndex;

			Item item = (Item)listBox1.Items[curr_menu];

			SqlCommand cmd;
			cmd = new SqlCommand("select * from Pizaria.showMenu ('" + item.ID + "')", Program.cn);
			if (!Program.verifySGBDConnection())
				return;

			listBox5.Items.Clear();
			using (SqlDataReader reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					string name = reader["nome"].ToString();
					string price = reader["preco"].ToString();
					string quantity = reader["quantidade"].ToString();

					listBox5.Items.Add(name + " " + quantity);
				}
			}

			Program.cn.Close();
		}
		
		// List Pizzas (list 2)
		private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			int curr_pizza = listBox2.SelectedIndex;

			Item item = (Item)listBox2.Items[curr_pizza];

			SqlCommand cmd;
			cmd = new SqlCommand("select * from Pizaria.showPiza ('" + item.ID + "')", Program.cn);
			if (!Program.verifySGBDConnection())
				return;

			listBox6.Items.Clear();
			using (SqlDataReader reader = cmd.ExecuteReader())
			{

				while (reader.Read())
				{
					string name = reader["nome"].ToString();
					string quantity = reader["quantidade"].ToString();

					listBox6.Items.Add(name + " " + quantity);
				}
			}

			Program.cn.Close();
		}
		
		// Add Menu to Shop Cart
		private void listBox1_DoubleClick(object sender, EventArgs e)
		{
			int curr_item = listBox1.SelectedIndex;
			if (curr_item < 0)
			{
				return;
			}
			Item item = (Item)listBox1.Items[curr_item];
			item.toOrder = item.toOrder + 1;
			if (!shop_cart.Contains(item))
			{
				shop_cart.Add(item);
			}
			listBox7.Items.Add(item);

			
		}

		// Add Pizza to Shop Cart
		private void listBox2_DoubleClick(object sender, EventArgs e)
		{
			int	curr_item = listBox2.SelectedIndex;
			if (curr_item < 0)
			{
				return;
			}
			Item item = (Item)listBox2.Items[curr_item];

			item.toOrder = item.toOrder + 1;
			if (!shop_cart.Contains(item))
			{
				shop_cart.Add(item);
			}
			listBox7.Items.Add(item);

			
		}

		// Add Drink to Shop Cart
		private void listBox3_DoubleClick(object sender, EventArgs e)
		{
			int curr_item = listBox3.SelectedIndex;
			if (curr_item < 0)
			{
				return;
			}
			Item item = (Item)listBox3.Items[curr_item];
			item.toOrder = item.toOrder+1;
			if (!shop_cart.Contains(item))
			{
				shop_cart.Add(item);
			}
			
			listBox7.Items.Add(item);

			
		}
		
		// Add Ingredients to Shop Cart
		private void listBox4_DoubleClick(object sender, EventArgs e)
		{
			int curr_item = listBox4.SelectedIndex;
			if (curr_item < 0)
			{
				return;
			}
			Item item = (Item)listBox4.Items[curr_item];
			item.toOrder = item.toOrder + 1;
			if (!shop_cart.Contains(item))
			{
				shop_cart.Add(item);
			}
			listBox7.Items.Add(item);
		}

		private void insClienteItem(Item item) {
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand
			{
				CommandType = CommandType.StoredProcedure,
				CommandText = "Pizaria.insClienteItem"
			};
			cmd.Parameters.Add(new SqlParameter("@cli_email", SqlDbType.NVarChar, 255));
			cmd.Parameters.Add(new SqlParameter("@quantity", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@item_ID", SqlDbType.Int));
			cmd.Parameters["@cli_email"].Value = Program.email;
			cmd.Parameters["@quantity"].Value = item.quantity;
			cmd.Parameters["@item_ID"].Value = item.ID;

			cmd.Connection = Program.cn;

			cmd.ExecuteNonQuery();

			Program.cn.Close();
		}
	}
}
