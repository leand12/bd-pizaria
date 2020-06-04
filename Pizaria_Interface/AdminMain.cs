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
			LoadCouriers();
		}

		// Show Couriers
		private void LoadCouriers()
		{
			if (!Program.verifySGBDConnection())
				return;
			SqlCommand cmd = new SqlCommand("select * from Pizaria.showAllEstafetas()", Program.cn);

			dataGridView1.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView1, null);
			dataGridView1.Columns[0].HeaderCell.Value = "Restaurant";
			dataGridView1.Columns[1].HeaderCell.Value = "Courier's Name";
			dataGridView1.Columns[2].HeaderCell.Value = "Courier's Email";
			dataGridView1.Columns[3].HeaderCell.Value = "Orders Delivered";
			dataGridView1.Columns[4].HeaderCell.Value = "Orders to Deliver";

			Program.cn.Close();

			if (!Program.verifySGBDConnection())
				return;

			cmd = new SqlCommand("select nome, contato from Pizaria.Restaurante", Program.cn);

			dataGridView7.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView7, new[] { "contato" });
			dataGridView7.Columns[0].HeaderCell.Value = "Restaurant";

			Program.cn.Close();

		}

		// Show Discounts
		private void LoadDiscounts()
		{
			if (!Program.verifySGBDConnection())
				return;
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
			if (!Program.verifySGBDConnection())
				return;
			SqlCommand cmd = new SqlCommand("SELECT * FROM Pizaria.statsEncomenda ('Menu',0)", Program.cn);

			dataGridView4.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView4, new[] { "ID" });
			dataGridView4.Columns[0].HeaderCell.Value = "Name";
			dataGridView4.Columns[1].HeaderCell.Value = "Price";
			dataGridView4.Columns[3].HeaderCell.Value = "Number of Sales";

			Program.cn.Close();

			if (!Program.verifySGBDConnection())
				return;

			cmd = new SqlCommand("SELECT * FROM Pizaria.statsEncomenda ('Menu',1)", Program.cn);

			dataGridView5.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView5, new[] { "ID" });

			dataGridView5.Columns[0].HeaderCell.Value = "Name";
			dataGridView5.Columns[1].HeaderCell.Value = "Price";
			dataGridView5.Columns[3].HeaderCell.Value = "Number of Sales";
			Program.cn.Close();

			if (!Program.verifySGBDConnection())
				return;

			cmd = new SqlCommand("SELECT * FROM Pizaria.statsEncomenda ('Piza',1)", Program.cn);

			dataGridView8.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView8, new[] { "ID" });

			dataGridView8.Columns[0].HeaderCell.Value = "Name";
			dataGridView8.Columns[1].HeaderCell.Value = "Price";
			dataGridView8.Columns[3].HeaderCell.Value = "Number of Sales";
			Program.cn.Close();

			if (!Program.verifySGBDConnection())
				return;

			cmd = new SqlCommand("SELECT * FROM Pizaria.statsEncomenda ('Piza',0)", Program.cn);

			dataGridView9.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView9, new[] { "ID" });

			dataGridView9.Columns[0].HeaderCell.Value = "Name";
			dataGridView9.Columns[1].HeaderCell.Value = "Price";
			dataGridView9.Columns[3].HeaderCell.Value = "Number of Sales";
			Program.cn.Close();

			if (!Program.verifySGBDConnection())
				return;

			cmd = new SqlCommand("Exec Pizaria.statsRestaurante", Program.cn);

			dataGridView6.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView6, null);
			dataGridView6.Columns[0].HeaderCell.Value = "Restaurant";
			dataGridView6.Columns[1].HeaderCell.Value = "Courier's Name With the Most Delivered Order";
			dataGridView6.Columns[2].HeaderCell.Value = "Courier's Email";
			dataGridView6.Columns[3].HeaderCell.Value = "Numbers of Delivered Order by Courier";
			dataGridView6.Columns[4].HeaderCell.Value = "Total Orders";
			dataGridView6.Columns[5].HeaderCell.Value = "Total Money Made";

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

		private void button2_Click(object sender, EventArgs e)
		{
			textBox4.Clear();
			textBox3.Clear();
			textBox2.Clear();
		}

		private void button1_Click(object sender, EventArgs e)
		{

			if (dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			int index = int.Parse(dataGridView1.SelectedRows[0].Index.ToString());

			string id = dataGridView1.Rows[index].Cells["email"].Value.ToString();

			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("delete from PIZARIA.Estafeta where email='" + id+"'", Program.cn);
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (System.Data.SqlClient.SqlException err)
			{
				MessageBox.Show("Error firing Courier"+ err.Message);
			}

			LoadCouriers();

			Program.cn.Close();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			int contato=0;
			if (dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
			{
				MessageBox.Show("Please select a Restaurant for the Courier.");
				return;
			}

			if (textBox2.Text == "")
			{
				MessageBox.Show("Please insert an email for the Courier.");
				return;
			} else if (textBox3.Text == "")
			{
				MessageBox.Show("Please insert the name of the Courier.");
				return;
			} else if (textBox4.Text == "")
			{
				MessageBox.Show("Please insert a contact for the Courier.");
				return;
			}
			try
			{
				contato = int.Parse(textBox4.Text);
			}
			catch (System.FormatException)
			{
				MessageBox.Show("Please insert a valid contact for the Courier.");
				return;
			}
			catch(OverflowException)
			{
				MessageBox.Show("The contact number for the Courier is too large.");
				return;
			}

			int index = int.Parse(dataGridView7.SelectedRows[0].Index.ToString());
			int res_contato = int.Parse(dataGridView7.Rows[index].Cells["contato"].Value.ToString());

			SqlCommand cmd = new SqlCommand
			{
				CommandType = CommandType.StoredProcedure,
				CommandText = "Pizaria.insEstafeta"
			};
			cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 255));
			cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar, 50));
			cmd.Parameters.Add(new SqlParameter("@contato", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@res_contato", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@response", SqlDbType.NVarChar, 255));
			cmd.Parameters["@email"].Value = textBox2.Text;
			cmd.Parameters["@nome"].Value = textBox3.Text;
			cmd.Parameters["@contato"].Value = contato;
			cmd.Parameters["@res_contato"].Value = res_contato;
			cmd.Parameters["@response"].Direction = ParameterDirection.Output;

			if (!Program.verifySGBDConnection())
				return;

			cmd.Connection = Program.cn;
			cmd.ExecuteNonQuery();

			MessageBox.Show(cmd.Parameters["@response"].Value.ToString());
			textBox4.Clear();
			textBox3.Clear();
			textBox2.Clear();
			LoadCouriers();
		}

		private void AdminMain_Load(object sender, EventArgs e)
		{

		}
	}
}
