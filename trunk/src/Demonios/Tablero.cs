using System;
using System.Collections.Generic;
using System.Text;

namespace JuegoDemonios
{
	using Casilla = Int32;
	public enum EstadoCasilla { Libre, Quemada };

	public class Tablero : List<EstadoCasilla>
	{
		public Tablero(int n) : base(n)
		{
			for (int i = 0; i < n; ++i)
			{
				this.Add(EstadoCasilla.Libre);
			}
		}

		public Tablero(Tablero t) : base(t)
		{
		}

		public void Quemar(Casilla c)
		{
			this[c] = EstadoCasilla.Quemada;
		}

		public int n
		{
			get { return this.Capacity; }
		}

		public EstadoCasilla VerEstado(int i)
		{
			return this[i];
		}
	}
}
