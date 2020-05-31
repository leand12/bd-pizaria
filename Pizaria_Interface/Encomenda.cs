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
		private string _email;
		private string _endereco_fisico;
		private string _hora;
		private string _nome;
		private int _contato;

		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		public string email
		{
			get { return _email; }
			set { _email = value; }
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

		public Encomenda(int ID, string nome, int contato, string email, string endereco_fisico, string hora)
		{
			this.ID = ID;
			this.nome = nome;
			this.contato = contato;
			this.email = email;
			this.endereco_fisico = endereco_fisico;
			this.hora = hora;
		}

		public Encomenda()
		{

		}

		public override string ToString()
		{
			return hora;
		}
	}
}
