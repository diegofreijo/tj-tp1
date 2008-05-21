using System;
using System.Collections.Generic;
using System.Text;

namespace JuegoDemonios
{
	using Casilla = Int32;

	abstract public class Demonio
	{
		public Demonio(int n, int k)
		{
		}

		public Casilla Jugar(Tablero tablero, Casilla posicion, Casilla oponente, int k)
		{
			return ElegirJugada(tablero, posicion, oponente, k);
		}


		protected abstract Casilla ElegirJugada(Tablero tablero, Casilla posicion, Casilla oponente, int k);


		/// <summary>
		/// Averigua las casillas a las que puedo ir
		/// </summary>
		/// <returns>Lista de casillas</returns>
		public List<Casilla> JugadasPosibles(Tablero tablero, Casilla posicion, int k)
		{
			List<Casilla> ret = new List<Casilla>();

			// Busco y agrego las casillas disponibles que esten en rango
			for (Casilla c = Math.Max(posicion - k, 0); c <= Math.Min(posicion + k, tablero.n - 1); ++c)
			{
				if (tablero[c] == EstadoCasilla.Libre) ret.Add(c);
			}

			return ret;
		}

	}
}
