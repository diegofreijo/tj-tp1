using System;
using System.Collections.Generic;
using System.Text;

namespace JuegoDemonios
{
    static class Consola
    {
        private static bool interactivo = true;

        public static bool Interactivo
        {
            get { return interactivo; }
            set { interactivo = value; }
        }

        public static string ReadLine()
        {
            if (interactivo) return Console.ReadLine();
            else return "";
        }

        public static void WriteLine()
        {
            if (interactivo) Console.WriteLine();
        }

        public static void WriteLine(string s)
        {
            if (interactivo) Console.WriteLine(s);
        }

        public static void Write(string s)
        {
            if (interactivo) Console.Write(s);
        }
    }
}
