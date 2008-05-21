using System;
using System.Collections.Generic;
using System.Text;

namespace JuegoDemonios
{
	class Program
	{
		static void Main(string[] args)
		{
			int n = 0;
			int k = 0;
			
			// Levanto los parametros
			System.Int32.TryParse(args[0], out n);
			System.Int32.TryParse(args[1], out k);
			
			Demonio jugador1 = new Demonios.Hibrido(n, k, 1.0);
			Demonio jugador2 = new Demonios.Hibrido(n, k, 0.0);
			
			Console.WriteLine("n: " + n);
			Console.WriteLine("k: " + k);
			Console.WriteLine("jugador1: " + jugador1.ToString());
			Console.WriteLine("jugador2: " + jugador2.ToString());
			
			
			Juego juego = new Juego(n, k, jugador1, jugador2, true);
			ResultadoDelJuego resultado = juego.ComenzarJuego();

			switch(resultado)
			{
				case ResultadoDelJuego.Empataron:
					Console.WriteLine("Empataron");
					break;
				case ResultadoDelJuego.Gano1:
					Console.WriteLine("Gano 1");
					break;
				case ResultadoDelJuego.Gano2:
					Console.WriteLine("Gano 2");
					break;
			}

			Console.ReadLine();
		}
	}
}
