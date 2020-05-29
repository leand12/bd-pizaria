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
	public partial class Login : Form
	{
		public Login()
		{
			InitializeComponent();
		}
        
        // Log In
        private void button1_Click(object sender, EventArgs e)
		{
			string email = textBox1.Text;
			string pass = textBox2.Text;

            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "Pizaria.UsersLogin"
            };
            cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 255));
            cmd.Parameters.Add(new SqlParameter("@pass", SqlDbType.VarChar, 30));
            cmd.Parameters.Add(new SqlParameter("@response", SqlDbType.NVarChar, 255));
            cmd.Parameters["@email"].Value = email;
            cmd.Parameters["@pass"].Value = pass;
            cmd.Parameters["@response"].Direction = ParameterDirection.Output;

            if (!Program.verifySGBDConnection())
                return;
            cmd.Connection = Program.cn;
            cmd.ExecuteNonQuery(); 

            if ("" + cmd.Parameters["@response"].Value == "User successfully logged in")
            {
                MessageBox.Show("Login sucess!");
                Program.email = email;

                SqlCommand comand = new SqlCommand("select Pizaria.isAdmin ('" + email + "')", Program.cn);
                int valor = (int)comand.ExecuteScalar();
                if (valor == 1)
                {
                    this.Hide();
                    var admin = new AdminMain();
                    admin.ShowDialog();
                    this.Close();
                    
                }
               
                comand = new SqlCommand("select Pizaria.isEstafeta ('" + email + "')", Program.cn);
                valor = (int)comand.ExecuteScalar();
                if (valor == 1)
                {
                    this.Hide();
                    var courier = new CourierMain();
                    courier.ShowDialog();
                    this.Close();
                }

                this.Hide();
                var clientMain = new ClientMain();
                clientMain.ShowDialog();
                this.Close();
            }
            else if ("" + cmd.Parameters["@response"].Value == "Incorrect password")
            {
                MessageBox.Show("Wrong password! Please try again");
            }
            else
            {
                MessageBox.Show("Invalid login!");
            }

            Program.cn.Close();

           
        }
        
        // Create Account
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var register = new Register();
            register.ShowDialog();
            this.Close();
        }
    }
}
