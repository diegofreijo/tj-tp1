using System;
using System.Collections.Generic;
using System.Text;

namespace JuegoDemonios
{
	class Program
	{
		static void Main(string[] args)
		{
			int n = 50;
			int k = 7;
			Demonio jugador1 = new Demonios.PasoAPaso(n, k);
			Demonio jugador2 = new Demonios.Pacifico(n, k);
			
			Juego juego = new Juego(n, k, jugador1, jugador2);
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
