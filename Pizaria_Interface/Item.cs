using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizaria
{
	public class Item
	{
		private int _ID;
		private string _name;
		private decimal _price;
		private int _quantity;
		private int _toOrder;

		public int ID {
			get { return _ID; }
			set { _ID = value;  }
		}

		public string name
		{
			get { return _name; }
			set { _name = value; }
		}

		public int toOrder
		{
			get { return _toOrder; }
			set { _toOrder = value; }
		}

		public decimal price
		{
			get { return _price; }
			set { _price = value; }
		}

		public int quantity
		{
			get { return _quantity; }
			set { _quantity = value; }
		}

		public Item(int ID, string name, decimal price) {
			this.ID = ID;
			this.name = name;
			this.price = price;
			this.quantity = 1;
			this.toOrder = 0;
		}

		public Item()
		{

		}

		public override string ToString() {
			//return String.Format("%20s %f€", name, price);
			return _name + "  " + _price;
		}
	}
}
