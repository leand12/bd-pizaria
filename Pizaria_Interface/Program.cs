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

		static public SqlConnection getSGBDConnection() {
			return new SqlConnection("Data Source = tcp:mednat.ieeta.pt\SQLSERVER,8101; Initial Catalog = p6g5 ; uid = p6g5;" + "password = 1l1E2i3c5-pg813");
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
