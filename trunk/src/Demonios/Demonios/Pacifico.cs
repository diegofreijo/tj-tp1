using System;
using System.Collections.Generic;
using System.Text;
using JuegoDemonios;

namespace JuegoDemonios.Demonios
{
	using Casilla = Int32;

	class Pacifico : Demonio
	{
		private double importancia_medio, importancia_lejos, importancia_restantes;

		public Pacifico(int n, int k) : base(n, k)
		{
			importancia_medio = n / 8;
			importancia_lejos = (double)k / (4 * (double)n);
			importancia_restantes = n / 3;
		}

		/// <summary>
		/// Elige la posicion que le da mas chances de sobrevivir
		/// </summary>
		protected override int ElegirJugada(Tablero tablero, Casilla posicion, Casilla oponente, int k)
		{
			List<Casilla> posibles = JugadasPosibles(tablero, posicion, k);

			// Eligo la casilla con mayor valor
			Casilla max = posibles[0];
			double valor_max = Valor(max, tablero, posicion, oponente, k);
			for (int i = 1; i < posibles.Count; ++i)
			{
				if (Valor(posibles[i], tablero, posicion, oponente, k) > valor_max)
				{
					max = posibles[i];
					valor_max = Valor(posibles[i], tablero, posicion, oponente, k);
				}
			}

			return max;
		}

		/// <summary>
		/// Calcula el valor de ir a destino. A mas valor, mejor para la estrategia.
		/// </summary>
		private double Valor(Casilla destino, Tablero tablero, Casilla posicion, Casilla oponente, int k)
		{
			// Valor por estar cerca del medio del tablero
			double valorxmedio = 4 * importancia_medio / tablero.n * (-(destino * destino) / tablero.n + destino);

			// Valor por estar lejos del oponente
			double valorxlejos = Math.Abs(destino - oponente) * importancia_lejos;

			// Valor por dejar jugadas restantes para el proxmo turno
			double valorxrestantes = JugadasPosibles(tablero, destino, k).Count * importancia_restantes;

			return valorxmedio + valorxlejos + valorxrestantes;
		}
	}
}
