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
			comboBox1.SelectedItem= "Card";
		}

		public void BalancePrice(double price) { this.price += price; }

		private void ClientMain_Load(object sender, EventArgs e)
		{
			LoadMenus();
			LoadPizzas();
			LoadOrders();
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

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.MenuView", Program.cn);

			customDataGridView(cmd, dataGridView2, new[] {"Id"});

			Program.cn.Close();
		}
		private void LoadOrders()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.showAllOrders('"+Program.email+"')", Program.cn);
			SqlDataReader reader = cmd.ExecuteReader();
			listBox4.Items.Clear();

			while (reader.Read())
			{
				Encomenda I = new Encomenda(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), int.Parse(reader["contato"].ToString()), reader["estafeta_email"].ToString(), reader["endereco_fisico"].ToString(), reader["hora"].ToString());
				listBox4.Items.Add(I);
			}

			Program.cn.Close();
		}

		private void LoadPizzas()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.PizaView", Program.cn);

			customDataGridView(cmd, dataGridView3, new[] {"ID", "pic"});

			Program.cn.Close();
		}

		// List Menu
		private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (0 > e.RowIndex || e.RowIndex >= dataGridView2.Rows.Count)
				return;

			string id = dataGridView2.Rows[e.RowIndex].Cells["ID"].Value.ToString();

			SqlCommand cmd = new SqlCommand("select * from Pizaria.showMenu ('" + id + "')", Program.cn);

			if (!Program.verifySGBDConnection())
				return;

			customDataGridView(cmd, dataGridView1, null);

			Program.cn.Close();
		}

		// List Pizzas
		private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (0 > e.RowIndex || e.RowIndex >= dataGridView3.Rows.Count)
				return;

			string id = dataGridView3.Rows[e.RowIndex].Cells["ID"].Value.ToString();

			SqlCommand cmd = new SqlCommand("select * from Pizaria.showPiza ('" + id + "')", Program.cn);

			if (!Program.verifySGBDConnection())
				return;

			customDataGridView(cmd, dataGridView7, null);
			//dataGridView7.Columns[""].Visible = false;

			byte[] image = Convert.FromBase64String(dataGridView3.Rows[e.RowIndex].Cells["pic"].Value.ToString());
			Image ret = null;
			using (MemoryStream ms = new MemoryStream(image))
			{
				ret = Image.FromStream(ms);
			}
			pictureBox2.Image = ret;
			pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;


			Program.cn.Close();
		}

		/*
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
*/

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
			if (textBox2.Text != "" && int.TryParse(textBox2.Text, out int n))
			{
				cmd.Parameters["@des_codigo"].Value = n;
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
				LoadOrders();
				tabControl4.SelectedIndex = 1;

			listBox6.Items.Clear();
			textBox1.Clear();
			textBox2.Clear();
			shop_cart.Clear();
			LoadShopCart();
			listBox8.Items.Clear();
			textBox5.Clear();
			textBox6.Clear();
			textBox7.Clear();
			textBox8.Clear();
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
            if (listBox6.SelectedIndex > 0 || listBox6.SelectedItem != null)
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

				customDataGridView(cmd, dataGridView3, new[] { "ID", "pic" });

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
				customDataGridView(cmd, dataGridView2, new[] { "Id" });

				Program.cn.Close();
			}
		}

		private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBox4.SelectedIndex>0 || listBox4.SelectedItem!= null){
				Encomenda enc = (Encomenda) listBox4.SelectedItem;

				textBox5.Text = enc.nome;
				textBox6.Text = enc.contato.ToString();
				textBox7.Text = enc.email;
				textBox8.Text = enc.endereco_fisico;

				if (!Program.verifySGBDConnection())
					return;

				SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.showOrder('" + enc.ID + "')", Program.cn);
				SqlDataReader reader = cmd.ExecuteReader();
				listBox8.Items.Clear();

				while (reader.Read())
				{
					listBox8.Items.Add(reader["nome"].ToString()+"    "+ int.Parse(reader["quantidade"].ToString()));
				}

				Program.cn.Close();

			}
		}

		private void customDataGridView(SqlCommand cmd, DataGridView dgv, string[] unshown_cols)
		{
			DataTable dt = new DataTable();
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			dgv.DataSource = dt;
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

		
	}
}
