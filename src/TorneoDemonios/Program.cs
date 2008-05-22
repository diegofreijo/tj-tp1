using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using JuegoDemonios;

namespace TorneoDemonios
{
	class Program
	{
		static void Main(string[] args)
		{
            int min_n = 0;
            int max_n = 0;

            // Levanto los parametros
            if (args.GetLength(0) < 4 || !Int32.TryParse(args[0], out min_n) || !Int32.TryParse(args[1], out max_n))
            {
                Console.WriteLine("Uso: TorneoDemonios.exe minimo_n maximo_n estrategia_j1 estrategia_j2");
                return;
            }

            // Guardo las estrategias de cada jugador y las muestro
            //Assembly juego_demonios = Assembly.Load("JuegoDemonios.dll");
            Type estrategia_j1 = Type.GetType("JuegoDemonios.Demonios.Azaroso, JuegoDemonios");
            Type estrategia_j2 = Type.GetType("JuegoDemonios.Demonios.Pacifico, JuegoDemonios");
            Console.WriteLine("Estrategia del jugador 1: " + estrategia_j1);
            Console.WriteLine("Estrategia del jugador 2: " + estrategia_j2);

			Demonio jugador1 = null;
			Demonio jugador2 = null;
			Juego juego = null;

			List<ResultadoDelJuego> resultados = new List<ResultadoDelJuego>();

			// Guardo la hora de inicio
			DateTime inicio = DateTime.Now;
            Console.WriteLine();
			Console.WriteLine("Inicio:		" + inicio.ToString());
			Console.WriteLine("------------------------------------------------");
			
			// Comienzo a jugar
			for (int n = min_n; n < max_n; ++n)
			{
				Console.Write(n + "  ");
				for (int k = 2; k < n; ++k)
				{
					jugador1 = (Demonio)Activator.CreateInstance(estrategia_j1, new object[]{ n, k });
                    jugador2 = (Demonio)Activator.CreateInstance(estrategia_j2, new object[]{ n, k });
					juego = new Juego(n, k, jugador1, jugador2, false);

					resultados.Add(juego.ComenzarJuego());
				}
			}
			Console.WriteLine();

			// Cuento resultados
			int gano1 = 0;
			int gano2 = 0;
			int empataron = 0;
			foreach (ResultadoDelJuego r in resultados)
			{
				if (r == ResultadoDelJuego.Gano1) ++gano1;
				else if (r == ResultadoDelJuego.Gano2) ++gano2;
				else if (r == ResultadoDelJuego.Empataron) ++empataron;
			}

			// Guardo la hora de fin
			DateTime fin = DateTime.Now;

			// Imprimo resultados
			Console.WriteLine("------------------------------------------------");
			Console.WriteLine("Hora de fin:		" + fin.ToString());
			Console.WriteLine("Tiempo de procesamiento: " + fin.Subtract(inicio).ToString());
			Console.WriteLine();

			Console.WriteLine("Gano 1: " + gano1);
			Console.WriteLine("Gano 2: " + gano2);
			Console.WriteLine("Empataron: " + empataron);

			Console.ReadLine();
		}
	}
}
