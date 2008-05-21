using System;
using System.Collections.Generic;
using System.Text;
using JuegoDemonios;

namespace JuegoDemonios.Demonios
{
	using Casilla = Int32;

	public class Hibrido : Demonio
	{
		// Variables de las heuristicas
		private double importancia_medio, importancia_restantes, nivel_de_violencia;

		// Lo utiliza el ValorViolento
		private Hibrido pacifico = null;


		public Hibrido(int n, int k, double nivel_de_violencia) : base(n, k)
		{
			if (nivel_de_violencia > 1 || nivel_de_violencia < 0)
				throw new ArgumentException("El nivel de violencia debe estar en [0,1]");
			
			importancia_medio = n / 6;
			importancia_restantes = n / 4;
			this.nivel_de_violencia = nivel_de_violencia;
			if(nivel_de_violencia > 0.0) pacifico = new Hibrido(n, k, 0.0);
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
				valores.Add(c, Valor(c, tablero, posicion, oponente, k));
			}

			// Eligo la casilla con mayor valor
			Casilla max = -1;
			foreach(Casilla c in valores.Keys)
			{
				if (max == -1 || valores[c] > valores[max])
					max = c;
			}
			
			// Averiguo todas las casillas de valor maximo
			List<Casilla> maximos = new List<Casilla>();
			foreach(Casilla c in valores.Keys)
			{
				if(valores[max] == valores[c]) maximos.Add(c);
			}
			
			// Elgio de los maximos el que mas representa al nivel de violencia
			if(nivel_de_violencia == 0.0)
			{
				// Eligo el de mayor distancia al oponente
				ret = -1;				
				foreach(Casilla c in maximos)
				{
					if(ret == -1 || Math.Abs(c - oponente) > Math.Abs(ret - oponente))
						ret = c;
				}
			}
			else if(nivel_de_violencia == 1.0)
			{
				// Eligo el de menor distancia al oponente
				ret = -1;				
				foreach(Casilla c in maximos)
				{
					if(ret == -1 || Math.Abs(c - oponente) < Math.Abs(ret - oponente))
						ret = c;
				}
			}
			else
			{
				ret = maximos[0];
			}

			return ret;
		}

		/// <summary>
		/// Calcula el valor de ir a destino. A mas valor, mejor para la estrategia.
		/// Para el hibrido, pondera entre el pacifico y el violento.
		/// </summary>
		private double Valor(Casilla destino, Tablero tablero, Casilla posicion, Casilla oponente, int k)
		{
			return nivel_de_violencia * (nivel_de_violencia == 0 ? 0 : ValorViolento(destino, tablero, posicion, oponente, k)) + 
				(1-nivel_de_violencia) * (1-nivel_de_violencia == 0 ? 0 : ValorPacifico(destino, tablero, posicion, oponente, k));
		}

		/// <summary>
		/// Calcula el valor de ir a destino. A mas valor, mejor para la estrategia.
		/// Para el pacifico, valora estar cerca del medio y que queden muchos casilleros para el proximo turno
		/// </summary>
		private double ValorPacifico(Casilla destino, Tablero tablero, Casilla posicion, Casilla oponente, int k)
		{
			// Valor por estar cerca del medio del tablero
			double valorxmedio = 4 * importancia_medio / tablero.n * (-(destino * destino) / tablero.n + destino);

			// Valor por dejar jugadas restantes para el proxmo turno
			double valorxrestantes = JugadasPosibles(tablero, destino, k).Count * importancia_restantes;

			return valorxmedio + valorxrestantes;
		}

		/// <summary>
		/// Calcula el valor de ir a destino. A mas valor, mejor para la estrategia.
		/// Para el violento, valora disminuir los futuros valores del oponente, suponiendo que juega como el pacifico.
		/// </summary>
		private double ValorViolento(Casilla destino, Tablero tablero, Casilla posicion, Casilla oponente, int k)
		{
			// Averiguo el movimiento del oponente
			Casilla oponente_futuro = pacifico.ElegirJugada(tablero, oponente, destino, k);

			// Valor por dejarlo lejos del medio
			double valorxmedio = -(4 * importancia_medio / tablero.n * (-(oponente_futuro * oponente_futuro) / tablero.n + oponente_futuro)) + importancia_medio;

			// Valor por dejarle pocas jugadas
			Tablero tablero_futuro = new Tablero(tablero);
			tablero_futuro.Quemar(destino);
			double valorxrestantes = -JugadasPosibles(tablero_futuro, oponente_futuro, k).Count * importancia_restantes + tablero.n * importancia_restantes;

			return valorxmedio + valorxrestantes;
		}


		public override string ToString()
		{
			return "Hibrido(NivelDeViolencia: " + nivel_de_violencia + ")";
		}
	}
}
