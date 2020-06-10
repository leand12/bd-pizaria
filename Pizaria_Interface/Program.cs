using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Pizaria
{
	static class Program
	{
		static public SqlConnection cn;
		static public string email;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			cn = getSGBDConnection();

			Application.Run(new Login());
		}

		static public SqlConnection getSGBDConnection()
		{
			return new SqlConnection("data source=LAPTOP-Q472I841;integrated security=true;initial catalog=master");
			//DESKTOP-4LBRH8P
		}


		static public bool verifySGBDConnection()
		{
			if (cn == null)
				cn = getSGBDConnection();

			if (cn.State != ConnectionState.Open)
				cn.Open();

			return cn.State == ConnectionState.Open;
		}


	}
}
