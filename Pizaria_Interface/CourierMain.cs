using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pizaria
{
	public partial class CourierMain : Form
	{

		public CourierMain()
		{
			InitializeComponent();
			LoadMyOrders();
			LoadHistory();
			LoadCouriers();
			LoadRestaurant();
		}

		private void LoadRestaurant() 
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("select Pizaria.isEmployed('" + Program.email + "')", Program.cn);
			int employed = (int)cmd.ExecuteScalar();

			if (employed == 1) {
				return;
			}

			Program.cn.Close();

			if (!Program.verifySGBDConnection())
				return;

			cmd = new SqlCommand("SELECT * FROM Pizaria.getEstRestaurante ('" + Program.email + "')", Program.cn);
			SqlDataReader reader = cmd.ExecuteReader();
			reader.Read();

			textBox1.Text = reader["nome"].ToString();
			textBox3.Text = reader["contato"].ToString();
			textBox2.Text = reader["morada"].ToString();
			textBox7.Text = reader["lotacao"].ToString();
			textBox4.Text = reader["hora_abertura"].ToString();
			textBox5.Text = reader["hora_fecho"].ToString();
			textBox6.Text = reader["dono"].ToString();

			Program.cn.Close();
		}

		private void LoadCouriers()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.showEstafeta ('" + Program.email + "')", Program.cn);

			Interface.customDataGridView(cmd, dataGridView5, null);
			dataGridView5.Columns["email"].HeaderCell.Value = "Email";
			dataGridView5.Columns["nome"].HeaderCell.Value = "Name";
			dataGridView5.Columns["delivered"].HeaderCell.Value = "# Delivered";
			dataGridView5.Columns["to_deliver"].HeaderCell.Value = "# To Deliver";

			Program.cn.Close();
		}

		private void LoadMyOrders()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.showOrdersToDel('" + Program.email + "')", Program.cn);

			Interface.customDataGridView(cmd, dataGridView1, new[] {"ID"});
			dataGridView1.Columns["cliente_email"].HeaderCell.Value = "Client Email";
			dataGridView1.Columns["endereco_fisico"].HeaderCell.Value = "Address";
			dataGridView1.Columns["hora"].HeaderCell.Value = "Deliver Time";
			dataGridView1.Columns["contato"].HeaderCell.Value = "Client Contact";
			dataGridView1.Columns["nome"].HeaderCell.Value = "Client Name";

			Program.cn.Close();
		}

		private void LoadHistory()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.showOrderHistory('" + Program.email + "')", Program.cn);

			Interface.customDataGridView(cmd, dataGridView4, new[] { "ID" });
			dataGridView4.Columns["cli_email"].HeaderCell.Value = "Client Email";
			dataGridView4.Columns["endereco_fisico"].HeaderCell.Value = "Address";
			dataGridView4.Columns["hora"].HeaderCell.Value = "Deliver Time";
			dataGridView4.Columns["metodo_pagamento"].HeaderCell.Value = "Payment Method";

			Program.cn.Close();
		}



		//Confirm delivery
		private void button1_Click_1(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			string id = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();

			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("delete from PIZARIA.Encomenda where ID=" + id, Program.cn);
			cmd.ExecuteNonQuery();

			Program.cn.Close();

			LoadMyOrders();
			LoadHistory();
			LoadCouriers();
		}


		//logout
		private void button2_Click(object sender, EventArgs e)
		{
			this.Hide();
			var login = new Login();
			login.ShowDialog();
			this.Close();
		}

		// show delivery details
		private void dataGridView1_SelectionChanged(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView1.SelectedRows[0].Index.ToString());
			string id = dataGridView1.Rows[index].Cells["ID"].Value.ToString();

			SqlCommand cmd = new SqlCommand("select * from Pizaria.showEncomenda ('" + id + "')", Program.cn);
			
			if (!Program.verifySGBDConnection())
				return;

			Interface.customDataGridView(cmd, dataGridView2, new[] {"preco"});
			dataGridView2.Columns["quantidade"].HeaderCell.Value = "Quantity";
			dataGridView2.Columns["nome"].HeaderCell.Value = "Item Name";

			Program.cn.Close();
		}

		private void dataGridView4_SelectionChanged(object sender, EventArgs e)
		{
			if (dataGridView4.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;
			int index = int.Parse(dataGridView4.SelectedRows[0].Index.ToString());
			string id = dataGridView4.Rows[index].Cells["ID"].Value.ToString();

			SqlCommand cmd = new SqlCommand("select * from Pizaria.showEncEntregue ('" + id + "')", Program.cn);

			if (!Program.verifySGBDConnection())
				return;

			Interface.customDataGridView(cmd, dataGridView3, new[] { "preco" });
			dataGridView3.Columns["quantidade"].HeaderCell.Value = "Quantity";
			dataGridView3.Columns["nome"].HeaderCell.Value = "Item Name";

			Program.cn.Close();
		}
	}
}
