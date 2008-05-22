using System;
using System.Collections.Generic;
using System.Text;
using JuegoDemonios;

namespace JuegoDemonios
{
	using Casilla = Int32;
	public enum ResultadoDelJuego { Gano1, Gano2, Empataron, NoTermino };
	
	public class Juego
	{
		private Tablero tablero;
		private Demonio jugador1, jugador2;
		private Casilla pos_j1, pos_j2;
		private int k;
		
		public Juego(int n, int k, Demonio jugador1, Demonio jugador2, bool interactivo)
		{
			this.k = k;
			this.tablero = new Tablero(n);
			this.tablero.Quemar(0);
			this.tablero.Quemar(n-1);

			this.jugador1 = jugador1;
			this.jugador2 = jugador2;
			this.pos_j1 = 0;
			this.pos_j2 = n-1;

            Consola.Interactivo = interactivo;
		}

		public ResultadoDelJuego ComenzarJuego()
		{
			while (EstadoDelJuego() == ResultadoDelJuego.NoTermino)
			{
				Casilla jugada1 = jugador1.Jugar(tablero, pos_j1, pos_j2, k);
				Casilla jugada2 = jugador2.Jugar(tablero, pos_j2, pos_j1, k);

				if (!EsJugadaValida(pos_j1, jugada1))
				{
					Consola.WriteLine("ERROR: el jugador 1 queria ir de " + pos_j1 + " a " + jugada1 + " y no puede");
				}

				if (!EsJugadaValida(pos_j2, jugada2))
				{
					Consola.WriteLine("ERROR: el jugador 2 queria ir de " + pos_j2 + " a " + jugada2 + " y no puede");
				}

				ActualizarEstadoJugador1(jugada1);
				ActualizarEstadoJugador2(jugada2);

                Consola.WriteLine();
                MostrarTablero();
                Consola.WriteLine(); 
                Consola.WriteLine("=================================================");
                Consola.ReadLine();
			}

			return EstadoDelJuego();
		}

		private void MostrarTablero()
		{
			// Veo el rango de saltos para la proxima de cada jugador
			int minr_jug1 = Math.Max(pos_j1 - k, 0);
			int maxr_jug1 = Math.Min(pos_j1 + k, tablero.n-1);
			int minr_jug2 = Math.Max(pos_j2 - k, 0);
			int maxr_jug2 = Math.Min(pos_j2 + k, tablero.n-1);

			// Dibujo al jugador 1
			int i = 0;
			for (i = 0; i < minr_jug1; ++i)
				Consola.Write("  ");
			Consola.Write("|-");
			for (++i; i < pos_j1; ++i)
				Consola.Write("--");
			Consola.Write("-1");
			for (++i; i <= maxr_jug1; ++i)
				Consola.Write("--");
			Consola.Write("| ");
			Consola.WriteLine();
			
			
			// Dibujo el tablero
			Consola.Write("|");
			foreach (EstadoCasilla estado in tablero)
			{
				if (estado == EstadoCasilla.Libre)
					Consola.Write(" |");
				else if (estado == EstadoCasilla.Quemada)
					Consola.Write("x|");
			}
			Consola.WriteLine();


			// Dibujo al jugador 2
			i = 0;
			for (i = 0; i < minr_jug2; ++i)
				Consola.Write("  ");
			Consola.Write("|-");
			for (++i; i < pos_j2; ++i)
				Consola.Write("--");
			Consola.Write("-2");
			for (++i; i <= maxr_jug2; ++i)
				Consola.Write("--");
			Consola.Write("| ");
			Consola.WriteLine();
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
