using System;
using System.Collections.Generic;
using System.Text;
using JuegoDemonios;

namespace JuegoDemonios.Demonios
{
	using Casilla = Int32;

	public class Azaroso : Demonio
	{
		private Random random = new Random(System.DateTime.Now.Millisecond);
		
		public Azaroso(int n, int k) : base(n, k)
		{	
		}

		/// <summary>
		/// Elige un casillero al azar
		/// </summary>
		protected override Casilla ElegirJugada(Tablero tablero, Casilla posicion, Casilla oponente, int k)
		{
			List<Casilla> posibles = JugadasPosibles(tablero, posicion, k);
			
			return posibles[random.Next(0, posibles.Count-1)];
		}
	}
}
