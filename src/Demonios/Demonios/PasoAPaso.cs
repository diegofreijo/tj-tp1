using System;
using System.Collections.Generic;
using System.Text;
using JuegoDemonios;

namespace JuegoDemonios.Demonios
{
	using Casilla = Int32;

	class PasoAPaso : Demonio
	{
		public PasoAPaso(int n, int k) : base(n, k)
		{	
		}

		/// <summary>
		/// Elige el casillero mas cercano
		/// </summary>
		protected override Casilla ElegirJugada(Tablero tablero, Casilla posicion, Casilla oponente, int k)
		{
			List<Casilla> posibles = JugadasPosibles(tablero, posicion, k);
			Casilla min = posibles[0];
			for (int i = 1; i < posibles.Count; ++i)
			{
				if (Math.Abs(posibles[i] - posicion) < Math.Abs(min - posicion))
					min = posibles[i];
			}

			return min;
		}
	}
}
