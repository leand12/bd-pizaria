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
		private List<Item> shop_cart;
		public decimal _shop_cart_price;
	
		public decimal shop_cart_price
		{
			get { return _shop_cart_price; }
			set {
				textBox9.Text = value.ToString();
				_shop_cart_price = value;  }
		}

		public ClientMain()
		{
			this.shop_cart = new List<Item>();
			InitializeComponent();
			comboBox1.SelectedItem= "Card";
			this.shop_cart_price= 0.00m;
		}


		private void ClientMain_Load(object sender, EventArgs e)
		{
			LoadMenus();
			LoadPizzas();
			LoadOrders();
			LoadDiscounts();
		}

		public void LoadShopCart()
		{
			dataGridView6.DataSource = null;
			Interface.customDataGridView(shop_cart, dataGridView6, new[] { "ID", "quantity" });
			dataGridView6.Columns["name"].HeaderCell.Value = "Name";
			dataGridView6.Columns["toOrder"].HeaderCell.Value = "Quantity";
			dataGridView6.Columns["price"].HeaderCell.Value = "Price";
		}

		private void LoadMenus()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.MenuView", Program.cn);

			Interface.customDataGridView(cmd, dataGridView2, new[] {"Id"});
			dataGridView2.Columns["nome"].HeaderCell.Value = "Name";
			dataGridView2.Columns["preco"].HeaderCell.Value = "Price";

			Program.cn.Close();
		}

		private void LoadOrders()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.showAllOrders('"+Program.email+"')", Program.cn);

			dataGridView5.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView5, new[] { "ID", "contato", "nome", "endereco_fisico", "estafeta_email" });
			dataGridView5.Columns["hora"].HeaderCell.Value = "Delivery Time";

			Program.cn.Close();
		}

		private void LoadDiscounts()
		{
			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.showAllDiscounts('" + Program.email + "')", Program.cn);

			dataGridView4.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView4, null);
			dataGridView4.Columns["codigo"].HeaderCell.Value = "Code";
			dataGridView4.Columns["percentagem"].HeaderCell.Value = "Percentage";
			dataGridView4.Columns["inicio"].HeaderCell.Value = "Start Date";
			dataGridView4.Columns["fim"].HeaderCell.Value = "End Date";

			Program.cn.Close();
		}

		private void LoadPizzas()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.PizaView", Program.cn);

			Interface.customDataGridView(cmd, dataGridView3, new[] {"ID", "pic"});
			dataGridView3.Columns["nome"].HeaderCell.Value = "Name";
			dataGridView3.Columns["preco"].HeaderCell.Value = "Price";

			Program.cn.Close();
		}

		// List Menu
		private void dataGridView2_SelectionChanged(object sender, EventArgs e)
		{
			if (dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView2.SelectedRows[0].Index.ToString());
			string id = dataGridView2.Rows[index].Cells["ID"].Value.ToString();

			SqlCommand cmd = new SqlCommand("select * from Pizaria.showMenu ('" + id + "')", Program.cn);

			if (!Program.verifySGBDConnection())
				return;

			Interface.customDataGridView(cmd, dataGridView1, null);
			dataGridView1.Columns["nome"].HeaderCell.Value = "Name";
			dataGridView1.Columns["preco"].HeaderCell.Value = "Price";
			dataGridView1.Columns["quantidade"].HeaderCell.Value = "Quantity";

			Program.cn.Close();
		}

		// List Pizzas
		private void dataGridView3_SelectionChanged(object sender, EventArgs e)
		{

			if (dataGridView3.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView3.SelectedRows[0].Index.ToString());
			string id = dataGridView3.Rows[index].Cells["ID"].Value.ToString();

			SqlCommand cmd = new SqlCommand("select * from Pizaria.showPiza ('" + id + "')", Program.cn);

			if (!Program.verifySGBDConnection())
				return;

			Interface.customDataGridView(cmd, dataGridView7, null);
			dataGridView7.Columns["nome"].HeaderCell.Value = "Name";
			dataGridView7.Columns["preco"].HeaderCell.Value = "Price";
			dataGridView7.Columns["quantidade"].HeaderCell.Value = "Quantity";

			byte[] image = Convert.FromBase64String(dataGridView3.Rows[index].Cells["pic"].Value.ToString());
			Image ret = null;
			using (MemoryStream ms = new MemoryStream(image))
			{
				ret = Image.FromStream(ms);
			}
			pictureBox2.Image = ret;
			pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

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
			dataGridView6.DataSource = null;
			this.shop_cart_price = 0.00m;
			textBox1.Clear();
			textBox2.Clear();
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
			cmd.Parameters.Add(new SqlParameter("@hora", SqlDbType.DateTime));
			cmd.Parameters.Add(new SqlParameter("@metodo_pagamento", SqlDbType.VarChar, 30));
			cmd.Parameters.Add(new SqlParameter("@lista", SqlDbType.VarChar));
			cmd.Parameters.Add(new SqlParameter("@des_codigo", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@response", SqlDbType.VarChar, 50));
			cmd.Parameters["@cliente_email"].Value = Program.email;
			cmd.Parameters["@endereco_fisico"].Value = textBox1.Text;
			String myDate =dateTimePicker2.Text + " "+dateTimePicker1.Text;
			cmd.Parameters["@hora"].Value = DateTime.Parse(myDate);
			cmd.Parameters["@metodo_pagamento"].Value = comboBox1.SelectedItem.ToString();
			string list="";
			foreach (var item in this.shop_cart )
			{
				list+=item.ID + "," + item.toOrder.ToString()+",";
			}
			cmd.Parameters["@lista"].Value = list;
			if (textBox2.Text != "")
			{
				cmd.Parameters["@des_codigo"].Value = textBox2.Text;
			}
			else if (textBox2.Text== "") {
				cmd.Parameters["@des_codigo"].Value = DBNull.Value;
			}
			else
			{
				MessageBox.Show("Invalid Number for Discount Code");
				Program.cn.Close();
				return;
			}

			cmd.Parameters["@response"].Direction = ParameterDirection.Output;
			cmd.Connection = Program.cn;
			cmd.ExecuteNonQuery();

			Program.cn.Close();

			string response = "" + cmd.Parameters["@response"].Value;

			if (response == "Success")
			{
				LoadOrders();
				tabControl4.SelectedIndex = 1;
			}
			else
			{
				MessageBox.Show(response);
			}

			dataGridView6.DataSource = null;
			textBox1.Clear();
			textBox2.Clear();
			shop_cart.Clear();
			LoadShopCart();
			LoadDiscounts();
			this.shop_cart_price = 0.00m;
			dataGridView8.DataSource = null;
			textBox5.Clear();
			textBox6.Clear();
			textBox7.Clear();
			textBox8.Clear();
		}

		// Add Items
		private void button6_Click(object sender, EventArgs e)
		{
			this.Enabled = false;
			var addItem = new AddItem(this, this.shop_cart);
			addItem.ShowDialog();
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
				cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 30));
				cmd.Parameters.Add(new SqlParameter("@item_type", SqlDbType.VarChar, 30));
				cmd.Parameters["@price"].Value = price;
				cmd.Parameters["@name"].Value = name;
				cmd.Parameters["@item_type"].Value = "Piza";

				dataGridView3.DataSource = null;
				dataGridView7.DataSource = null;
				dataGridView2.DataSource = null;
				dataGridView1.DataSource = null;

				pictureBox2.Image = null;

				cmd.Connection = Program.cn;

				Interface.customDataGridView(cmd, dataGridView3, new[] { "ID", "pic" });
				dataGridView3.Columns["nome"].HeaderCell.Value = "Name";
				dataGridView3.Columns["preco"].HeaderCell.Value = "Price";

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
				Interface.customDataGridView(cmd, dataGridView2, new[] { "Id" });
				dataGridView2.Columns["nome"].HeaderCell.Value = "Name";
				dataGridView2.Columns["preco"].HeaderCell.Value = "Price";

				Program.cn.Close();
			}
		}

		private void dataGridView5_SelectionChanged(object sender, EventArgs e)
		{
			if (dataGridView5.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView5.SelectedRows[0].Index.ToString());

			textBox5.Text = dataGridView5.Rows[index].Cells["nome"].Value.ToString();
			textBox6.Text = dataGridView5.Rows[index].Cells["contato"].Value.ToString();
			textBox7.Text = dataGridView5.Rows[index].Cells["estafeta_email"].Value.ToString();
			textBox8.Text = dataGridView5.Rows[index].Cells["endereco_fisico"].Value.ToString();

			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.showOrder('" + dataGridView5.Rows[index].Cells["ID"].Value.ToString() + "')", Program.cn);

			dataGridView8.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView8, new[] { "item_ID" });
			dataGridView8.Columns["nome"].HeaderCell.Value = "Item";
			dataGridView8.Columns["quantidade"].HeaderCell.Value = "Quantity";

			Program.cn.Close();
		}

		private void button7_Click(object sender, EventArgs e)
		{
			if (textBox2.Text != "")
			{
				MessageBox.Show("Already chosen a Discount for this Order.");
				return;
			}

			if (dataGridView4.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView4.SelectedRows[0].Index.ToString());

			string id = dataGridView4.Rows[index].Cells["codigo"].Value.ToString();

			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("select Pizaria.isValidDiscount("+id+",'" + Program.email + "')", Program.cn);
			int valid = (int)cmd.ExecuteScalar();

			if (valid != 0)
			{
				this.shop_cart_price = this.shop_cart_price - this.shop_cart_price * valid / 100;
				textBox2.Text = id;
				return;
			}
			else
			{
				MessageBox.Show("Invalid Discount.");
			}

			Program.cn.Close();

		}

		public decimal checkBalance(List<Item> shop_cart)
		{
			decimal price = 0.00m;
			foreach (var item in this.shop_cart)
			{
				price += item.price*item.toOrder;
			}
			return price;
		}
	}
}
