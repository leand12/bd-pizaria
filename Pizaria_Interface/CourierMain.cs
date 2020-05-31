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

			customDataGridView(cmd, dataGridView1);
			dataGridView1.Columns["ID"].Visible = false;

			Program.cn.Close();
		}


		//Confirm delivery
		private void button1_Click_1(object sender, EventArgs e)
		{
			string id = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();

			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("delete from PIZARIA.Encomenda where ID=" + id, Program.cn);
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

		// show delivery details
		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (0 > e.RowIndex || e.RowIndex >= dataGridView1.Rows.Count)
				return;

			string id = dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString();

			SqlCommand cmd = new SqlCommand("select * from Pizaria.showEncomenda ('" + id + "')", Program.cn);
			
			if (!Program.verifySGBDConnection())
				return;

			customDataGridView(cmd, dataGridView2);
			dataGridView2.Columns["preco"].Visible = false;
			
			Program.cn.Close();
		}

		private void customDataGridView(SqlCommand cmd, DataGridView dgv)
		{
			DataTable dt = new DataTable();
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			dgv.DataSource = dt;
			dgv.ReadOnly = true;
			dgv.AllowUserToResizeColumns = false;
			dgv.MultiSelect = false;
			dgv.AllowUserToOrderColumns = true;
			dgv.RowHeadersVisible = false;
			dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			dgv.Columns[dgv.Columns.Count-1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
		}
	}
}
