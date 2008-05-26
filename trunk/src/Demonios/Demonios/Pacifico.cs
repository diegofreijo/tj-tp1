using System;
using System.Collections.Generic;
using System.Text;
using JuegoDemonios;

namespace JuegoDemonios.Demonios
{
	using Casilla = Int32;

	public class Pacifico : Demonio
	{
		// Variables de las heuristicas
		private double importancia_medio, importancia_restantes;
		int turnos_explorados;

		public Pacifico(int n, int k) : base(n, k)
		{
			importancia_medio = n / 6;
			importancia_restantes = n / 2;
			turnos_explorados = 4;
		}

		/// <summary>
		/// Elige la posicion que le da mas chances de sobrevivir
		/// </summary>
		protected override Casilla ElegirJugada(Tablero tablero, Casilla posicion, Casilla oponente, int k)
		{
			Casilla ret = -1;
			List<Casilla> posibles = JugadasPosibles(tablero, posicion, k);

			// Calculo los valores de cada posicion posible
			Dictionary<Casilla, double> valores = new Dictionary<int, double>();
			foreach (Casilla c in posibles)
			{
                Consola.Write("(" + c + ", ");
				valores.Add(c, Valor(c, tablero, posicion, oponente, k));
                Consola.WriteLine(" = " + valores[c] + ")");
			}


			// Eligo la casilla con mayor valor
			Casilla max = -1;
			foreach (Casilla c in valores.Keys)
			{
				if (max == -1 || valores[c] > valores[max])
					max = c;
			}

			// Averiguo todas las casillas de valor maximo
			List<Casilla> maximos = new List<Casilla>();
			foreach (Casilla c in valores.Keys)
			{
				if (valores[max] == valores[c]) maximos.Add(c);
			}

			// Desempato eligiendo el de mayor distancia al oponente
			ret = -1;
			foreach (Casilla c in maximos)
			{
				if (ret == -1 || Math.Abs(c - oponente) > Math.Abs(ret - oponente))
					ret = c;
			}
			
			return ret;
		}

		/// <summary>
		/// Calcula el valor de ir a destino. A mas valor, mejor para la estrategia.
		/// Para el pacifico, valora estar cerca del medio y que queden muchos casilleros para los proximos turnos
		/// </summary>
		private double Valor(Casilla destino, Tablero tablero, Casilla posicion, Casilla oponente, int k)
		{
			// Valor por estar cerca del medio del tablero
            double valorxmedio = 4 * importancia_medio / (double)tablero.n * (-((double)destino * (double)destino) / (double)tablero.n + (double)destino);

			// Valor por dejar jugadas restantes para los siguientes turnos
            int cant_jugadas_futuras = CantidadDeJugadasFuturas(tablero, destino, k, turnos_explorados);
			double valorxrestantes = cant_jugadas_futuras * importancia_restantes;

            Consola.Write(valorxmedio + "   + " + valorxrestantes + "   [" + cant_jugadas_futuras + "]");

			return valorxmedio + valorxrestantes;
		}

		/// <summary>
		/// Calcula cuantas casillas van a quedar al final de la exploracion para cada posible turno
		/// </summary>
		private int CantidadDeJugadasFuturas(Tablero tablero, Casilla destino, int k, int turnos_a_explorar)
		{
			int ret = 0;

			// Verifico en que estado de la recursion estoy
			if(turnos_a_explorar == 1)
			{
				// Devuelvo las casillas que tengo para el siguiente turno
				ret = JugadasPosibles(tablero, destino, k).Count;
			}
			else
			{
				// Casillas posibles para el siguiente turno
				List<Casilla> posibles_futuro = JugadasPosibles(tablero, destino, k);

				// Verifico que queden casillas por explorar (capaz miro muy en el futuro, donde ya esta todo quemado)
				if (posibles_futuro.Count > 0)
				{
					// Genero un nuevo tablero, que tenga quemada donde hubiese saltado
					Tablero tablero_futuro = new Tablero(tablero);
					tablero_futuro.Quemar(destino);

					// Calculo cuantas casillas van a quedar al final de la exploracion para cada posible jugada en el siguiente turno
					Dictionary<Casilla, int> casillas_final = new Dictionary<Casilla, int>();
					foreach (Casilla futuro_destino in posibles_futuro)
					{
						casillas_final.Add(futuro_destino, CantidadDeJugadasFuturas(tablero_futuro, futuro_destino, k, turnos_a_explorar - 1));
					}

					// Veo cual futuro_destino me dejo mayor cantidad de casillas al final de la exploracion
					Casilla max = -1;
					foreach (Casilla futuro_destino in casillas_final.Keys)
					{
						if (max == -1 || casillas_final[max] < casillas_final[futuro_destino])
							max = futuro_destino;
					}

					ret = casillas_final[max];
				}
				else
				{
					// Mire demasiado lejos
					ret = 0;
				}
			}

			return ret;
		}

		/// <summary>
		/// Muestra de forma bonita el estado del jugador
		/// </summary>
		public override string ToString()
		{
			return "Pacifico(ImportanciaMedio: " + importancia_medio + ", ImportanciaRestantes: " + importancia_restantes + ", TurnosExplorados: " + turnos_explorados + ")";
		}
	}
}
