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
		}

		private void LoadMyOrders()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.showOrdersToDel('" + Program.email + "')", Program.cn);

			customDataGridView(cmd, dataGridView1, new[] {"ID"});

			Program.cn.Close();
		}

		private void LoadHistory()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.showOrderHistory(1 ,'" + Program.email + "')", Program.cn);

			customDataGridView(cmd, dataGridView4, new[] { "ID" });

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

			LoadHistory();
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

			customDataGridView(cmd, dataGridView2, new[] {"preco"});
			
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

			customDataGridView(cmd, dataGridView3, new[] { "preco" });

			Program.cn.Close();
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
