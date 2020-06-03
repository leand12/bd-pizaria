using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pizaria
{
	class Interface
	{
		public static void customDataGridView(SqlCommand cmd, DataGridView dgv, string[] unshown_cols)
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

		public static void customDataGridView(List<Item> item_list, DataGridView dgv, string[] unshown_cols)
		{
			dgv.DataSource = item_list;
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
