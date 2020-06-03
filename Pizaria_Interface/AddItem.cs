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
				Interface.customDataGridView(shop_cart, dataGridView7, new[] { "ID", "quantity" });
			}
		}

		private void LoadMenus()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.MenuView", Program.cn);

			Interface.customDataGridView(cmd, dataGridView1, new[] {"Id"});
			
			Program.cn.Close();
		}

		private void LoadPizzas()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.PizaView", Program.cn);

			Interface.customDataGridView(cmd, dataGridView2, new[] { "ID", "pic" });

			Program.cn.Close();
		}

		private void LoadDrinks()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.BebidaView", Program.cn);

			Interface.customDataGridView(cmd, dataGridView3, new[] { "ID" });

			Program.cn.Close();
		}

		private void LoadIngredients()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.IngredienteView", Program.cn);

			Interface.customDataGridView(cmd, dataGridView4, new[] { "ID", "quantidade_disponivel" });

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

		// Shop cart
		private void dataGridView7_SelectionChanged(object sender, EventArgs e)
		{
			if (dataGridView7.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView7.SelectedRows[0].Index.ToString());
			Item I = (Item)dataGridView7.Rows[index].DataBoundItem;
			numericUpDown1.Value = I.toOrder;
		}

		// List Menus (list 1)
		private void dataGridView1_SelectionChanged(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView1.SelectedRows[0].Index.ToString());
			string id = dataGridView1.Rows[index].Cells["Id"].Value.ToString();

			SqlCommand cmd;
			cmd = new SqlCommand("select * from Pizaria.showMenu ('" + id + "')", Program.cn);
			
			if (!Program.verifySGBDConnection())
				return;

			dataGridView5.DataSource = null;

			Interface.customDataGridView(cmd, dataGridView5, null);

			Program.cn.Close();
		}

		// List Pizzas (list 2)
		private void dataGridView2_SelectionChanged(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView1.SelectedRows[0].Index.ToString());
			string id = dataGridView1.Rows[index].Cells["Id"].Value.ToString();

			SqlCommand cmd;
			cmd = new SqlCommand("select * from Pizaria.showPiza ('" + id + "')", Program.cn);
			
			if (!Program.verifySGBDConnection())
				return;

			dataGridView6.DataSource = null;

			Interface.customDataGridView(cmd, dataGridView6, null);

			Program.cn.Close();
		}

		// Add Menu to Shop Cart
		private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView1.SelectedRows[0].Index.ToString());
			string id = dataGridView1.Rows[index].Cells["Id"].Value.ToString();
			string name = dataGridView1.Rows[index].Cells["nome"].Value.ToString();
			string price = dataGridView1.Rows[index].Cells["preco"].Value.ToString();

			Item I = new Item(int.Parse(id), name, decimal.Parse(price));

			Boolean item_in_cart = false;
			clientMain.shop_cart_price += I.price;
			shop_cart_price = clientMain.shop_cart_price;
			foreach (var item_cart in this.shop_cart)
			{
				if (I.ID == item_cart.ID)
				{
					item_cart.toOrder += 1;
					item_in_cart = true;
				}
			}
			if (item_in_cart == false)
			{
				I.toOrder = I.toOrder + 1;
				shop_cart.Add(I);
			}

			LoadShopCart();
		}

		// Add Pizza to Shop Cart
		private void dataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView2.SelectedRows[0].Index.ToString());
			string id = dataGridView2.Rows[index].Cells["Id"].Value.ToString();
			string name = dataGridView2.Rows[index].Cells["nome"].Value.ToString();
			string price = dataGridView2.Rows[index].Cells["preco"].Value.ToString();

			Item I = new Item(int.Parse(id), name, decimal.Parse(price));

			clientMain.shop_cart_price += I.price;
			shop_cart_price = clientMain.shop_cart_price;
			Boolean item_in_cart = false;
			foreach (var item_cart in this.shop_cart)
			{
				if (I.ID == item_cart.ID)
				{
					item_cart.toOrder += 1;
					item_in_cart = true;
				}
			}
			if (item_in_cart == false)
			{
				I.toOrder = I.toOrder + 1;
				shop_cart.Add(I);
			}

			LoadShopCart();
		}

		// Add Drink to Shop Cart
		private void dataGridView3_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (dataGridView3.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView3.SelectedRows[0].Index.ToString());
			string id = dataGridView3.Rows[index].Cells["Id"].Value.ToString();
			string name = dataGridView3.Rows[index].Cells["nome"].Value.ToString();
			string price = dataGridView3.Rows[index].Cells["preco"].Value.ToString();

			Item I = new Item(int.Parse(id), name, decimal.Parse(price));

			clientMain.shop_cart_price += I.price;
			shop_cart_price = clientMain.shop_cart_price;
			Boolean item_in_cart = false;
			foreach (var item_cart in this.shop_cart)
			{
				if (I.ID == item_cart.ID)
				{
					item_cart.toOrder += 1;
					item_in_cart = true;
				}
			}
			if (item_in_cart == false)
			{
				I.toOrder = I.toOrder + 1;
				shop_cart.Add(I);
			}

			LoadShopCart();
		}

		// Add Ingredients to Shop Cart
		private void dataGridView4_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (dataGridView4.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView4.SelectedRows[0].Index.ToString());
			string id = dataGridView4.Rows[index].Cells["Id"].Value.ToString();
			string name = dataGridView4.Rows[index].Cells["nome"].Value.ToString();
			string price = dataGridView4.Rows[index].Cells["preco"].Value.ToString();

			Item I = new Item(int.Parse(id), name, decimal.Parse(price));

			clientMain.shop_cart_price += I.price;
			shop_cart_price = clientMain.shop_cart_price;
			Boolean item_in_cart = false;
			foreach (var item_cart in this.shop_cart)
			{
				if (I.ID == item_cart.ID)
				{
					item_cart.toOrder += 1;
					item_in_cart = true;
				}
			}
			if (item_in_cart == false)
			{
				I.toOrder = I.toOrder + 1;
				shop_cart.Add(I);
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
				Interface.customDataGridView(shop_cart, dataGridView7, new[] { "ID", "quantity" });
			}
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
	}
}
