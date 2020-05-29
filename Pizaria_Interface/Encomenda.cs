using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizaria
{
	public class Encomenda
	{
		private int _ID;
		private string _estafeta_email;
		private string _endereco_fisico;
		private string _hora;
		private string _nome;
		private int _contato;

		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		public string estafeta_email
		{
			get { return _estafeta_email; }
			set { _estafeta_email = value; }
		}

		public string nome
		{
			get { return _nome; }
			set { _nome = value; }
		}

		public int contato
		{
			get { return _contato; }
			set { _contato = value; }
		}

		public string hora
		{
			get { return _hora; }
			set { _hora = value; }
		}

		public string endereco_fisico
		{
			get { return _endereco_fisico; }
			set { _endereco_fisico = value; }
		}

		public Encomenda(int ID, string nome, int contato, string estafeta_email, string endereco_fisico, string hora)
		{
			this.ID = ID;
			this.nome = nome;
			this.contato = contato;
			this.estafeta_email = estafeta_email;
			this.endereco_fisico = endereco_fisico;
			this.hora = hora;
		}

		public Encomenda()
		{

		}

		public override string ToString()
		{
			return "Courier's name:  " + nome + Environment.NewLine + "Courier's contact: " + contato.ToString() + Environment.NewLine + "Courier's email: " + estafeta_email + Environment.NewLine + "Order being delivered at: " + endereco_fisico + Environment.NewLine + "Deliver arrives at:  " + hora;
		}
	}
}
