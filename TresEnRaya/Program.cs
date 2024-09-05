using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TresEnRaya
{
    internal class Program
    {
        private static bool jugador = true;
        private static char[,] tablero = new char[3, 3];

        /// <summary>
        /// Metodo de ejecucion principal del programa
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Jugar();
        }

        /// <summary>
        /// Metodo que gestiona la logica del tres en raya
        /// </summary>
        static void Jugar()
        {
            bool acabado = false;
            RellenarTablero();
            while (!acabado)
            {
                string entrada;
                int x = 0;
                int y = 0;
                while (true)
                {
                    string numeroJugador = jugador ? "Jugador 1 (X)" : "Jugador 2 (O)";
                    ImprimirTablero();
                    Console.WriteLine(numeroJugador);
                    Console.WriteLine("Indica las cordenadas donde quiere colocar la ficha (ej: 1,2; 1 2; 1-2)");
                    entrada = Console.ReadLine();

                    while (entrada.Length < 3)
                    {
                        entrada += "o";
                    }

                    y = SacarCordenadas(entrada, 0);
                    x = SacarCordenadas(entrada, 2);

                    if (x >= 3 || y >= 3)
                    {
                        Console.WriteLine("Introduzca un numero del 0 al 2");
                        break;
                    }

                    if (x == -1 && y == -1)
                    {
                        Console.WriteLine("Introduzca unas cordenadas correctas");
                        break;
                    }

                    if (!ComprobarCasilla(x,y))
                    {
                        Console.WriteLine("El espacio no esta disponible");
                        break;
                    }

                    ColocarFicha(y,x);

                    if (ComprobarGanador())
                    {
                        ImprimirTablero();
                        acabado = true;
                        Console.WriteLine("El " + numeroJugador + " ha ganado la partida!");
                        break;
                    }
                    
                    jugador = !jugador;

                    if (!ComprobacionEspacio())
                    {
                        ImprimirTablero();
                        acabado = true;
                        Console.WriteLine("No queda espacio en el tablero");
                    }
                }
            }
            Console.WriteLine("La partida ha finalizado, presione cualquier tecla para salir");
            Console.ReadKey();
        }

        /// <summary>
        /// Metodo que imprime el estado del tablero
        /// </summary>
        static void ImprimirTablero()
        {
            Console.WriteLine("  0 1 2");
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                string line = "";
                
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    line += tablero[i,j];
                    if (j < 3 - 1)
                        line += "|";
                    if (j == 0)
                        Console.Write(i + " ");
                }
                Console.WriteLine(line);
                if (i < 3 - 1)
                {
                    Console.Write("  ");
                    for (int k = 0; k < 3; k++)
                    {
                        Console.Write("-");
                        if (k < 3 - 1)
                            Console.Write("-");
                    }
                    Console.WriteLine();
                }
            }

        }

        /// <summary>
        /// Metodo que rellena el tablero al empezar una partida
        /// </summary>
        static void RellenarTablero()
        {
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    tablero[i, j] = '*';
                }
            }
        }

        /// <summary>
        /// Metodo que nos permite colocar la ficha en el lugar que el jugador a designado
        /// </summary>
        /// <param name="y">Representacion del eje Y</param>
        /// <param name="x">Representacion del eje X</param>
        static void ColocarFicha(int y, int x)
        {
            if (jugador)
                tablero[y, x] = 'X';
            else
                tablero[y, x] = 'O';
        }

        /// <summary>
        /// Metodo que nos permite la extraccion de las cordenadas de un string
        /// </summary>
        /// <param name="cordenadas">Cadena de texto donde sacaremos un caracter especifico</param>
        /// <param name="posicion">Parametro que nos da la posicion que debemos de sacar</param>
        /// <returns>Este metodo devuelve un int que puede que puede ser cualquier numero positivo si ha funcionado bien y un -1 si ha fallado</returns>
        static int SacarCordenadas(string cordenadas, int posicion)
        {
            string cordenada = "";
            cordenada += cordenadas[posicion];
            bool funciona = int.TryParse(cordenada , out int resultado);

            if (funciona)
            {
                return resultado;
            }

            return -1;
        }

        /// <summary>
        /// Metodo que nos permite comprobar si la casilla donde el jugador quiere poner una ficha esta ocupada o no
        /// </summary>
        /// <param name="y">Representacion del eje Y</param>
        /// <param name="x">Representacion del eje X</param>
        /// <returns>Devuelve un boleano donde true es que la casila no esta ocupada y false es que si que esta ocupada</returns>
        static bool ComprobarCasilla(int x, int y)
        {
            if (tablero[y,x] == '*')
                return true;
            else
                return false;
        }

        /// <summary>
        /// Metodo que comprueba si ha ganado alguien
        /// </summary>
        /// <returns>Devuelve un boleano de true cuando ha ganado alguien y de false cuando no</returns>
        static bool ComprobarGanador()
        {
            char simbolo = jugador ? 'X' : 'O';

            for (int i = 0; i < 3; i++)
            {
                if (tablero[i, 0] == simbolo && tablero[i, 1] == simbolo && tablero[i, 2] == simbolo)
                    return true;
            }

            for (int i = 0; i < 3; i++)
            {
                if (tablero[0, i] == simbolo && tablero[1, i] == simbolo && tablero[2, i] == simbolo)
                    return true;
            }

            if ((tablero[0, 0] == simbolo && tablero[1, 1] == simbolo && tablero[2, 2] == simbolo) ||
                (tablero[0, 2] == simbolo && tablero[1, 1] == simbolo && tablero[2, 0] == simbolo))
                return true;

            return false;
        }

        /// <summary>
        /// Metodo que comprueba si quedan espacios disponibles en el tablero
        /// </summary>
        /// <returns>Este metodo devuelve true cuando quedan casillas y false cuando no quedan casilla</returns>
        static bool ComprobacionEspacio()
        {
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    if (tablero[i,j] == '*')
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
