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
	public partial class Register : Form
	{

        public Register()
		{
			InitializeComponent();
		}

		private void Register_Load(object sender, EventArgs e)
		{

		}

        private bool isValidEmail(string email) {
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string pass = textBox2.Text;
            string pass2 = textBox3.Text;
            string nome = textBox4.Text;
            string contato = textBox5.Text;
            string idade = textBox7.Text;
            string genero = textBox6.Text;
            string morada = textBox8.Text;

            if (pass != pass2) {
                MessageBox.Show("Passwords don't match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(email)) {
                MessageBox.Show("Email is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(pass)) {
                MessageBox.Show("Password is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(pass2)) {
                MessageBox.Show("Confirm password is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(nome)) {
                MessageBox.Show("Name is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(contato))
            {
                MessageBox.Show("Contact is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!(isValidEmail(email))) {
                MessageBox.Show("Email inserted is invalid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Pizaria.register"
                };
                cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 255));
                cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar, 50));
                cmd.Parameters.Add(new SqlParameter("@contato", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@pass", SqlDbType.VarChar, 30));
                cmd.Parameters.Add(new SqlParameter("@res_contato", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@idade", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@genero", SqlDbType.Char));
                cmd.Parameters.Add(new SqlParameter("@morada", SqlDbType.NVarChar, 255));
                cmd.Parameters.Add(new SqlParameter("@response", SqlDbType.NVarChar, 255));
                cmd.Parameters["@type"].Value = 2;
                cmd.Parameters["@email"].Value = email;
                cmd.Parameters["@nome"].Value = nome;
                cmd.Parameters["@contato"].Value = int.Parse(contato);
                cmd.Parameters["@pass"].Value = pass;
                cmd.Parameters["@res_contato"].Value = 0;
                cmd.Parameters["@idade"].Value = int.Parse(idade);
                cmd.Parameters["@genero"].Value = char.Parse(genero); // fazer try
                cmd.Parameters["@morada"].Value = (morada != "") ? morada : null;
                cmd.Parameters["@response"].Direction = ParameterDirection.Output;
                if (!Program.verifySGBDConnection())
                    return;
                cmd.Connection = Program.cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@response"].Value == "User successfully registered")
                {
                    MessageBox.Show("Register succedeed!");
                    this.Hide();
                    //ClientMain admin = new ClientMain(email);
                    //admin.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("User email already in use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Program.cn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
		{
			var frm = new Login();
			frm.ShowDialog();
			this.Close();
		}
	}
}
