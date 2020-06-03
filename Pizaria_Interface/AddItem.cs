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
		private ClientMain clientMain;
		private List<Item> shop_cart;
		private decimal _shop_cart_price;

		private decimal shop_cart_price
		{
			get { return _shop_cart_price; }
			set
			{
				_shop_cart_price = value;
				textBox1.Text = value.ToString();
			}
		}

		public AddItem(ClientMain clientMain, List<Item> shop_cart)
		{
			this.clientMain = clientMain;
			this.shop_cart = shop_cart;
			InitializeComponent();
			this.shop_cart_price = clientMain.shop_cart_price;
		}

		private void AddItem_Load(object sender, EventArgs e)
		{
			LoadIngredients();
			LoadDrinks();
			LoadMenus();
			LoadPizzas();
			LoadShopCart();
		}

		public void LoadShopCart()
		{
			if (this.shop_cart.Count > 0)
			{
				dataGridView7.DataSource = null;
				customDataGridView(shop_cart, dataGridView7, new[] { "ID", "quantity" });
			}
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
				Item I = new Item(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), decimal.Parse(reader["preco"].ToString()));
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
				Item I = new Item(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), decimal.Parse(reader["preco"].ToString())); ;
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
				Item I = new Item(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), decimal.Parse(reader["preco"].ToString()));
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
				Item I = new Item(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), decimal.Parse(reader["preco"].ToString()));
				listBox4.Items.Add(I);
			}

			Program.cn.Close();
		}



		// Confirm Items
		private void button1_Click(object sender, EventArgs e)
		{
			clientMain.Enabled = true;
			clientMain.LoadShopCart();
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
			Boolean item_in_cart = false;
			clientMain.shop_cart_price += item.price;
			shop_cart_price = clientMain.shop_cart_price;
			foreach (var item_cart in this.shop_cart)
			{
				if (item.ID == item_cart.ID)
				{
					item_cart.toOrder += 1;
					item_in_cart = true;
				}
			}
			if (item_in_cart == false)
			{
				item.toOrder = item.toOrder + 1;
				shop_cart.Add(item);
			}

			LoadShopCart();
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
			clientMain.shop_cart_price += item.price;
			shop_cart_price = clientMain.shop_cart_price;
			Boolean item_in_cart = false;
			foreach (var item_cart in this.shop_cart)
			{
				if (item.ID == item_cart.ID)
				{
					item_cart.toOrder += 1;
					item_in_cart = true;
				}
			}
			if (item_in_cart == false)
			{
				item.toOrder = item.toOrder + 1;
				shop_cart.Add(item);
			}

			LoadShopCart();
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
			clientMain.shop_cart_price += item.price;
			shop_cart_price = clientMain.shop_cart_price;
			Boolean item_in_cart = false;
			foreach (var item_cart in this.shop_cart)
			{
				if (item.ID == item_cart.ID)
				{
					item_cart.toOrder += 1;
					item_in_cart = true;
				}
			}
			if (item_in_cart == false)
			{
				item.toOrder = item.toOrder + 1;
				shop_cart.Add(item);
			}

			LoadShopCart();
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
			clientMain.shop_cart_price += item.price;
			shop_cart_price = clientMain.shop_cart_price;
			Boolean item_in_cart = false;
			foreach (var item_cart in this.shop_cart)
			{
				if (item.ID == item_cart.ID)
				{
					item_cart.toOrder += 1;
					item_in_cart = true;
				}
			}
			if (item_in_cart == false)
			{
				item.toOrder = item.toOrder + 1;
				shop_cart.Add(item);
			}

			LoadShopCart();
		}

		// Remove Item
		private void button5_Click(object sender, EventArgs e)
		{
			if (dataGridView7.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			if (this.shop_cart.Count > 0)
			{
				int index = int.Parse(dataGridView7.SelectedRows[0].Index.ToString());
				Item I = (Item)dataGridView7.Rows[index].DataBoundItem;
				clientMain.shop_cart_price = clientMain.shop_cart_price -( I.price * I.toOrder);
				shop_cart_price = clientMain.shop_cart_price;
				this.shop_cart.Remove(I);
				I.toOrder = 0;
				dataGridView7.DataSource = null;
				customDataGridView(shop_cart, dataGridView7, new[] { "ID", "quantity" });
			}
		}

		public void customDataGridView(List<Item> item_list, DataGridView dgv, string[] unshown_cols)
		{
			dgv.DataSource = item_list;
			dgv.ReadOnly = true;
			dgv.AllowUserToResizeColumns = false;
			dgv.MultiSelect = false;
			dgv.AllowUserToResizeRows = false;
			dgv.AllowUserToOrderColumns = true;
			dgv.AllowUserToAddRows = false;
			dgv.RowHeadersVisible = false;
			if (unshown_cols != null)
				foreach (string col in unshown_cols)
				{
					dgv.Columns[col].Visible = false;
				}
			dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			dgv.Columns.GetLastColumn(
				DataGridViewElementStates.Visible,
				DataGridViewElementStates.None
				).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			if (dataGridView7.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView7.SelectedRows[0].Index.ToString());
			Item I = (Item)dataGridView7.Rows[index].DataBoundItem;

			I.toOrder = Convert.ToInt32(numericUpDown1.Value);

			clientMain.shop_cart_price = clientMain.checkBalance(this.shop_cart);
			shop_cart_price = clientMain.shop_cart_price;
		}

		private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (dataGridView7.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView7.SelectedRows[0].Index.ToString());
			Item I = (Item)dataGridView7.Rows[index].DataBoundItem;
			numericUpDown1.Value = I.toOrder;
		}
	}
}
