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
            if (args.GetLength(0) < 2 || !Int32.TryParse(args[0], out n) || !Int32.TryParse(args[1], out k))
            {
                Console.WriteLine("Uso: JuegoDemonios.exe n k");
                return;
            }
			
			Demonio jugador1 = new Demonios.Azaroso(n, k);
			Demonio jugador2 = new Demonios.Pacifico(n, k);
			
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
