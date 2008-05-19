using System;
using System.Collections.Generic;
using System.Text;
using JuegoDemonios;

namespace JuegoDemonios
{
	using Casilla = Int32;
	enum ResultadoDelJuego { Gano1, Gano2, Empataron, NoTermino };

	class Juego
	{
		private Tablero tablero;
		private Demonio jugador1, jugador2;
		private Casilla pos_j1, pos_j2;
		private int k;

		public Juego(int n, int k, Demonio jugador1, Demonio jugador2)
		{
			this.k = k;
			this.tablero = new Tablero(n);
			tablero.Quemar(0);
			tablero.Quemar(n-1);

			this.jugador1 = jugador1;
			this.jugador2 = jugador2;
			this.pos_j1 = 0;
			this.pos_j2 = n-1;
		}

		public ResultadoDelJuego ComenzarJuego()
		{
			Console.WriteLine("n: " + tablero.n);
			Console.WriteLine("k: " + k);
			
			while (EstadoDelJuego() == ResultadoDelJuego.NoTermino)
			{
				Casilla jugada1 = jugador1.Jugar(tablero, pos_j1, pos_j2, k);
				Casilla jugada2 = jugador2.Jugar(tablero, pos_j2, pos_j1, k);

				if (!EsJugadaValida(pos_j1, jugada1))
				{
					Console.WriteLine("ERROR: el jugador 1 queria ir de " + pos_j1 + " a " + jugada1 + " y no puede");
				}

				if (!EsJugadaValida(pos_j2, jugada2))
				{
					Console.WriteLine("ERROR: el jugador 2 queria ir de " + pos_j2 + " a " + jugada2 + " y no puede");
				}

				ActualizarEstadoJugador1(jugada1);
				ActualizarEstadoJugador2(jugada2);

				MostrarTablero();

				Console.WriteLine("--------------------------------------------");
				Console.ReadLine();
			}

			return EstadoDelJuego();
		}

		private void MostrarTablero()
		{
			// Veo quien esta antes y quien despues
			Casilla pos_min = Math.Min(pos_j1, pos_j2);
			Casilla pos_max = Math.Max(pos_j1, pos_j2);
			string nom_min, nom_max;
			if (pos_min == pos_j1)
			{
				nom_min = "1";
				nom_max = "2";
			}
			else
			{
				nom_min = "2";
				nom_max = "1";
			}

			// Dibujo los demonios
			if (pos_j1 == pos_j2)
			{
				Console.Write(" ");
				for (int i = 0; i < pos_j1; ++i)
				{
					Console.Write("  ");
				}
				Console.Write("1");
				Console.WriteLine();
				Console.Write(" ");
				for (int i = 0; i < pos_j2; ++i)
				{
					Console.Write("  ");
				}
				Console.Write("2");
				Console.WriteLine();
			}
			else
			{
				Console.Write(" ");
				for (int i = 0; i < pos_min; ++i)
				{
					Console.Write("  ");
				}
				Console.Write(nom_min);
				Console.Write(" ");
				for (int i = pos_min + 1; i < pos_max; ++i)
				{
					Console.Write("  ");
				}
				Console.Write(nom_max);
				Console.WriteLine();
			}

			// Dibujo el tablero
			Console.Write("|");
			foreach(EstadoCasilla estado in tablero)
			{
				if(estado == EstadoCasilla.Libre)
					Console.Write(" |");
				else if(estado == EstadoCasilla.Quemada)
					Console.Write("x|");
			}
			Console.WriteLine("");
		}

		private void ActualizarEstadoJugador1(Casilla jugada)
		{
			pos_j1 = jugada;
			tablero[jugada] = EstadoCasilla.Quemada;
		}

		private void ActualizarEstadoJugador2(Casilla jugada)
		{
			pos_j2 = jugada;
			tablero[jugada] = EstadoCasilla.Quemada;
		}

		private bool EsJugadaValida(Casilla desde, Casilla hasta)
		{
			return (Math.Abs(hasta - desde) <= k) && (tablero[hasta] == EstadoCasilla.Libre);
		}

		private ResultadoDelJuego EstadoDelJuego()
		{
			// Verifico si alguien perdio o empataron
			int posibles_jug1 = jugador1.JugadasPosibles(tablero, pos_j1, k).Count;
			int posibles_jug2 = jugador2.JugadasPosibles(tablero, pos_j2, k).Count;

			if (posibles_jug1 == 0 && posibles_jug2 == 0)
			{
				return ResultadoDelJuego.Empataron;
			}
			else if (posibles_jug1 == 0)
			{
				return ResultadoDelJuego.Gano2;
			}
			else if (posibles_jug2 == 0)
			{
				return ResultadoDelJuego.Gano1;
			}
			else
			{
				return ResultadoDelJuego.NoTermino;
			}
		}
	}
}
