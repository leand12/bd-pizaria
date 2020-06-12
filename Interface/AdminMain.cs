using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
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
		}
		private void AdminMain_Load(object sender, EventArgs e)
		{
			LoadStats();
			LoadDiscounts();
			LoadCouriers();
			LoadRestaurants();
			LoadStock();
		}


		// Show Stock
		private void LoadStock()
		{
			if (!Program.verifySGBDConnection())
				return;

			SqlCommand cmd = new SqlCommand("select * from Pizaria.showLowStock() order by 4 asc", Program.cn);
			dataGridView10.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView10, new[] { "ID" } );
			dataGridView10.Columns[1].HeaderCell.Value = "Item";
			dataGridView10.Columns[2].HeaderCell.Value = "Price";
			dataGridView10.Columns[3].HeaderCell.Value = "Quantity in Stock";

			Program.cn.Close();
		}

		// Show Restaurants
		private void LoadRestaurants()
		{
			if (!Program.verifySGBDConnection())
				return;
			SqlCommand cmd = new SqlCommand("select * from Pizaria.showRestaurante()", Program.cn);
			dataGridView3.DataSource = null;

			cmd.Connection = Program.cn;

			Interface.customDataGridView(cmd, dataGridView3, new[] { "contato", "morada", "lotacao", "hora_abertura", "hora_fecho" });
			dataGridView3.Columns[0].HeaderCell.Value = "Name";

			Program.cn.Close();
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
			dataGridView6.Columns[1].HeaderCell.Value = "Courier's Email With the Most Delivered Order";
			dataGridView6.Columns[2].HeaderCell.Value = "Courier's Name";
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
			try
			{
				cmd.Connection = Program.cn;
				cmd.ExecuteNonQuery();
			}
			catch (SqlException er)
			{
				MessageBox.Show(er.Message);
				textBox5.Clear();
				numericUpDown1.Value = 1;
				dateTimePicker1.Value = DateTime.Now;
				dateTimePicker2.Value = DateTime.Now;
				LoadDiscounts();
				return;
			}
			MessageBox.Show(cmd.Parameters["@response"].Value.ToString());
			textBox5.Clear();
			numericUpDown1.Value = 1;
			dateTimePicker1.Value = DateTime.Now;
			dateTimePicker2.Value = DateTime.Now;
			LoadDiscounts();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			textBox5.Clear();
			numericUpDown1.Value = 1;
			dateTimePicker1.Value = DateTime.Now;
			dateTimePicker2.Value = DateTime.Now;
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
			try
			{
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

			SqlCommand cmd = new SqlCommand("delete from PIZARIA.Estafeta where email='" + id + "'", Program.cn);
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (System.Data.SqlClient.SqlException err)
			{
				MessageBox.Show("Error firing Courier" + err.Message);

			}

			LoadStats();
			LoadCouriers();

			Program.cn.Close();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			int contato = 0;
			if (dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
			{
				MessageBox.Show("Please select a Restaurant for the Courier.");
				return;
			}

			if (textBox2.Text == "")
			{
				MessageBox.Show("Please insert an email for the Courier.");
				return;
			}
			else if (textBox3.Text == "")
			{
				MessageBox.Show("Please insert the name of the Courier.");
				return;
			}
			else if (textBox4.Text == "")
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
			catch (OverflowException)
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
			LoadStats();
		}


		private void dataGridView3_SelectionChanged(object sender, EventArgs e)
		{
			if (dataGridView3.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;
			
			int index = int.Parse(dataGridView3.SelectedRows[0].Index.ToString());
			textBox1.Text = dataGridView3.Rows[index].Cells["contato"].Value.ToString();
			textBox6.Text = dataGridView3.Rows[index].Cells["nome"].Value.ToString();
			textBox11.Text = dataGridView3.Rows[index].Cells["morada"].Value.ToString();
			numericUpDown3.Value = decimal.Parse(dataGridView3.Rows[index].Cells["lotacao"].Value.ToString());
			dateTimePicker8.Value = DateTime.Parse(dataGridView3.Rows[index].Cells["hora_abertura"].Value.ToString());
			dateTimePicker7.Value = DateTime.Parse(dataGridView3.Rows[index].Cells["hora_fecho"].Value.ToString());
		}

		private void button8_Click(object sender, EventArgs e)
		{
			SqlCommand cmd = new SqlCommand
			{
				CommandType = CommandType.StoredProcedure,
				CommandText = "Pizaria.updRestaurante"
			};
			cmd.Parameters.Add(new SqlParameter("@morada", SqlDbType.VarChar, 50));
			cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar, 50));
			cmd.Parameters.Add(new SqlParameter("@contato", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@lotacao", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@dono", SqlDbType.NVarChar, 255));
			cmd.Parameters["@morada"].Value = textBox11.Text;
			cmd.Parameters["@nome"].Value = textBox6.Text;
			cmd.Parameters["@contato"].Value = textBox1.Text;
			cmd.Parameters["@lotacao"].Value = numericUpDown3.Value;
			cmd.Parameters.AddWithValue("@hora_abertura", dateTimePicker8.Value);
			cmd.Parameters.AddWithValue("@hora_fecho", dateTimePicker7.Value);
			cmd.Parameters["@dono"].Value = Program.email;

			if (!Program.verifySGBDConnection())
				return;

			cmd.Connection = Program.cn;
			cmd.ExecuteNonQuery();

			LoadCouriers();
			LoadRestaurants();
			LoadStats();
		}

		private void button9_Click(object sender, EventArgs e)
		{
			if (textBox9.Text == "" || textBox12.Text == "" || textBox8.Text == "" || textBox8.Text.Length!=9){
				MessageBox.Show("Please fill all parameters to add a Restaurant.");
			}
			int contato=0;
			try
			{
				contato = int.Parse(textBox8.Text);
			}
			catch
			{
				MessageBox.Show("Please type a valid contact number.");
			}

			SqlCommand cmd = new SqlCommand
			{
				CommandType = CommandType.StoredProcedure,
				CommandText = "Pizaria.insRestaurante"
			};

			cmd.Parameters.Add(new SqlParameter("@morada", SqlDbType.VarChar, 50));
			cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar, 50));
			cmd.Parameters.Add(new SqlParameter("@contato", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@lotacao", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@dono", SqlDbType.NVarChar, 255));
			cmd.Parameters.Add(new SqlParameter("@response", SqlDbType.VarChar, 50));
			cmd.Parameters["@morada"].Value = textBox12.Text;
			cmd.Parameters["@nome"].Value = textBox9.Text;
			cmd.Parameters["@contato"].Value = textBox8.Text;
			cmd.Parameters["@lotacao"].Value = numericUpDown2.Value;
			cmd.Parameters.AddWithValue("@hora_abertura", dateTimePicker5.Value);
			cmd.Parameters.AddWithValue("@hora_fecho", dateTimePicker6.Value);
			cmd.Parameters["@dono"].Value = Program.email;
			cmd.Parameters["@response"].Direction = ParameterDirection.Output;

			if (!Program.verifySGBDConnection())
				return;

			cmd.Connection = Program.cn;
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch
			{
				MessageBox.Show("Error adding a Restaurant");
			}

			string response = "" + cmd.Parameters["@response"].Value;
			MessageBox.Show(response);
			if (response == "Sucess!")
			{
				LoadCouriers();
				LoadRestaurants();
				LoadStats();
				textBox9.Text = "";
				textBox9.Text = "";
				textBox9.Text = "";
				numericUpDown2.Value = 10;
			}
			Program.cn.Close();
		}

		private void button10_Click(object sender, EventArgs e)
		{
			if (dataGridView10.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
				return;

			if (!Program.verifySGBDConnection())
				return;
			
			int index = int.Parse(dataGridView10.SelectedRows[0].Index.ToString());

			SqlCommand cmd = new SqlCommand
			{
				CommandType = CommandType.StoredProcedure,
				CommandText = "Pizaria.updStock"
			};
			cmd.Parameters.AddWithValue("@ID", int.Parse(dataGridView10.Rows[index].Cells["ID"].Value.ToString()));
			cmd.Parameters.AddWithValue("@amount", numericUpDown4.Value);

			cmd.Connection = Program.cn;
			cmd.ExecuteNonQuery();

			LoadStock();
			Program.cn.Close();
			numericUpDown4.Value = 1;
		}

		private void button11_Click(object sender, EventArgs e)
		{
			LoadCouriers();
		}
	}
}
