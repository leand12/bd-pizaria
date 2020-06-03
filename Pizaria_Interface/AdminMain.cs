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
	public partial class AdminMain : Form
	{

		public AdminMain()
		{
			InitializeComponent();
			LoadStats();
			LoadDiscounts();
		}


		// Show Discounts
		private void LoadDiscounts()
		{
			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.Desconto", Program.cn);

			dataGridView2.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView2, null);
			dataGridView2.Columns[0].HeaderCell.Value = "Code";
			dataGridView2.Columns[1].HeaderCell.Value = "Percentage";
			dataGridView2.Columns[2].HeaderCell.Value = "Start Date";
			dataGridView2.Columns[3].HeaderCell.Value = "End Date";

			Program.cn.Close();

		}

		// Show Stats
		private void LoadStats()
		{
			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.statsEncomenda ('Piza')", Program.cn);

			dataGridView4.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView4, new[] { "ID" });
			dataGridView4.Columns[0].HeaderCell.Value = "Name";
			dataGridView4.Columns[1].HeaderCell.Value = "Price";
			dataGridView4.Columns[3].HeaderCell.Value = "Number of Sales";

			Program.cn.Close();

			cmd = new SqlCommand("SELECT * FROM Pizaria.statsEncomenda ('Menu')", Program.cn);

			dataGridView5.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView5, new[] { "ID" });

			dataGridView5.Columns[0].HeaderCell.Value = "Name";
			dataGridView5.Columns[1].HeaderCell.Value = "Price";
			dataGridView5.Columns[3].HeaderCell.Value = "Number of Sales";
			Program.cn.Close();
		}

		// Log Out
		private void button3_Click(object sender, EventArgs e)
		{
			this.Hide();
			var login = new Login();
			login.ShowDialog();
			this.Close();
		}

		private void button6_Click(object sender, EventArgs e)
		{
			if (textBox5.Text == "")
			{
				MessageBox.Show("Please insert a code for the Discount.");
				return;
			}
			SqlCommand cmd = new SqlCommand
			{
				CommandType = CommandType.StoredProcedure,
				CommandText = "Pizaria.insDesconto"
			};
			cmd.Parameters.Add(new SqlParameter("@codigo", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@percentagem", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@inicio", SqlDbType.DateTime));
			cmd.Parameters.Add(new SqlParameter("@fim", SqlDbType.DateTime));
			cmd.Parameters.Add(new SqlParameter("@response", SqlDbType.NVarChar, 255));	
			cmd.Parameters["@codigo"].Value = textBox5.Text;
			cmd.Parameters["@percentagem"].Value = numericUpDown1.Text;
			String startDate = dateTimePicker1.Text + " " + dateTimePicker3.Text;
			cmd.Parameters["@inicio"].Value = DateTime.Parse(startDate);
			String endDate = dateTimePicker2.Text + " " + dateTimePicker4.Text;
			cmd.Parameters["@fim"].Value = DateTime.Parse(endDate);
			cmd.Parameters["@response"].Direction = ParameterDirection.Output;

			if (!Program.verifySGBDConnection())
				return;

			cmd.Connection = Program.cn;
			cmd.ExecuteNonQuery();

			MessageBox.Show(cmd.Parameters["@response"].Value.ToString());
			textBox5.Clear();
			numericUpDown1.Value=1;
			dateTimePicker1.Value = DateTimePicker.MinimumDateTime;
			dateTimePicker2.Value = DateTimePicker.MinimumDateTime;
			LoadDiscounts();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			textBox5.Clear();
			numericUpDown1.Value = 1;
			dateTimePicker1.Value = DateTimePicker.MinimumDateTime;
			dateTimePicker2.Value = DateTimePicker.MinimumDateTime;
		}

		private void button7_Click(object sender, EventArgs e)
		{
			if (dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView2.SelectedRows[0].Index.ToString());

			string id = dataGridView2.Rows[index].Cells["codigo"].Value.ToString();

			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("delete from PIZARIA.Desconto where codigo=" + id, Program.cn);
			try {
				Console.WriteLine(cmd.ExecuteNonQuery());
			}
			catch (System.Data.SqlClient.SqlException)
			{
				MessageBox.Show("Discount is in Use in an Order.");
			}

			LoadDiscounts();

			Program.cn.Close();

		}
	}
}
