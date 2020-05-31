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
		}

		private void LoadMyOrders()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.showOrdersToDel('" + Program.email + "')", Program.cn);
			SqlDataReader reader = cmd.ExecuteReader();

			dataGridView1.DataSource = null;
			dataGridView1.Refresh();

			List<Encomenda> list_enc = new List<Encomenda>();
			while (reader.Read())
			{
				list_enc.Add(new Encomenda(int.Parse(reader["ID"].ToString()), reader["nome"].ToString(), int.Parse(reader["contato"].ToString()), reader["cliente_email"].ToString(), reader["endereco_fisico"].ToString(), reader["hora"].ToString()));

			}

			dataGridView1.DataSource = list_enc;
			dataGridView1.Columns[0].Visible = false;
			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

			Program.cn.Close();
		}


		//Confirm delivery
		private void button1_Click_1(object sender, EventArgs e)
		{
			if (dataGridView1.CurrentRow.DataBoundItem == null)
				return;

			if (!Program.verifySGBDConnection())
				return;
			Encomenda E = (Encomenda)dataGridView1.CurrentRow.DataBoundItem;
			SqlCommand cmd = new SqlCommand("delete from PIZARIA.Encomenda where ID=" + E.ID, Program.cn);
			cmd.ExecuteNonQuery();

			LoadMyOrders();

			Program.cn.Close();
		}




		//logout
		private void button2_Click(object sender, EventArgs e)
		{
			this.Hide();
			var login = new Login();
			login.ShowDialog();
			this.Close();
		}


		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			Encomenda E = (Encomenda)dataGridView1.CurrentRow.DataBoundItem;

		}


	}
}
