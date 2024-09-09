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
        private static char[,,] tablero = new char[3, 3, 3];
        private static bool acabado = false;

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
            int x;
            int y;
            int z = 0;

            RellenarTablero();

            while (!acabado)
            {
                string entrada;
                x = 0;
                y = 0;
                
                while (true)
                {
                    string numeroJugador = jugador ? "Jugador 1 (X)" : "Jugador 2 (O)";

                    ImprimirTablero(z);

                    Console.WriteLine(numeroJugador);
                    Console.WriteLine("Indica las cordenadas donde quiere colocar la ficha (ej: 1,2; 1 2; 1-2)");
                    Console.WriteLine("Dimension Z = " + z + " de " + (tablero.GetLength(0) - 1));
                    Console.WriteLine("Escriba < para retroceder y > para avanzar");
                    entrada = Console.ReadLine();

                    if (entrada == "<")
                    {
                        if (z != 0)
                        {
                            z--;
                            Console.WriteLine("Cambiando de dimension...");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("No puedes reducir mas la Z");
                            break;
                        }
                    }
                    else if (entrada == ">")
                    {
                        if (z != tablero.GetLength(0) - 1)
                        {
                            z++;
                            Console.WriteLine("Cambiando de dimension...");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("No puedes aumentar mas la Z");
                            break;
                        }
                    }

                    while (entrada.Length < 3)
                    {
                        entrada += "o";
                    }

                    y = SacarCordenadas(entrada, 0);
                    x = SacarCordenadas(entrada, 2);

                    if (Comprobaciones(x, y, z, numeroJugador)) 
                        break;
                }
            }
            Console.WriteLine("La partida ha finalizado, presione cualquier tecla para salir");
            Console.ReadKey();
        }

        /// <summary>
        /// Metodo que llama a otros metodos para realizar las comprobaciones del juego
        /// </summary>
        /// <param name="x">Valor del ejeX</param>
        /// <param name="y">Valor del ejeY</param>
        /// <param name="numeroJugador">Cadena de texto que nos devuelve que jugador esta jugando</param>
        /// <returns>True si hay errores y false cuando esta bien</returns>
        private static bool Comprobaciones(int x, int y, int z, string numeroJugador)
        {

            if (x >= tablero.GetLength(1) || y >= tablero.GetLength(0))
            {
                Console.WriteLine("Introduzca un numero del 0 al 2");
                return true;
            }

            if (x == -1 || y == -1)
            {
                Console.WriteLine("Introduzca unas cordenadas correctas");
                return true;
            }

            if (!EstaVacio(x, y, z))
            {
                Console.WriteLine("El espacio no esta disponible");
                return true;
            }

            ColocarFicha(y,x,z);

            if (HayGanador(z))
            {
                ImprimirTablero(z);
                acabado = true;
                Console.WriteLine("El " + numeroJugador + " ha ganado la partida!");
                return true;
            }
                    
            jugador = !jugador;

            if (!QuedaEspacio())
            {
                ImprimirTablero(z);
                acabado = true;
                Console.WriteLine("No queda espacio en el tablero");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Metodo que imprime el estado del tablero
        /// </summary>
        static void ImprimirTablero(int z)
        {
            string line = "";

            Console.Write("    ");
            for (int i = 0; i < tablero.GetLength(1); i++)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                line = "";

                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    line += tablero[i, j, z];
                    if (j < tablero.GetLength(1) - 1)
                        line += "|";

                    if (j == 0)
                        Console.Write(i + "   ");
                }
                Console.WriteLine(line);

                if (i < tablero.GetLength(0) - 1)
                {
                    Console.Write("    ");
                    for (int k = 0; k < tablero.GetLength(1); k++)
                    {
                        Console.Write("-");
                        if (k < tablero.GetLength(1) - 1)
                            Console.Write("+");
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
                    for (int k = 0; k < tablero.GetLength(2); k++)
                    {
                        tablero[i, j, k] = '*';
                    }
                }
            }
        }

        /// <summary>
        /// Metodo que nos permite colocar la ficha en el lugar que el jugador a designado
        /// </summary>
        /// <param name="y">Representacion del eje Y</param>
        /// <param name="x">Representacion del eje X</param>
        static void ColocarFicha(int y, int x, int z)
        {
            if (jugador)
                tablero[y, x, z] = 'X';
            else
                tablero[y, x, z] = 'O';
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
                return resultado;

            return -1;
        }

        /// <summary>
        /// Metodo que nos permite comprobar si la casilla donde el jugador quiere poner una ficha esta ocupada o no
        /// </summary>
        /// <param name="x">Representacion del eje X</param>
        /// <param name="y">Representacion del eje Y</param>
        /// <param name="z"></param>
        /// <returns>Devuelve un boleano donde true es que la casila no esta ocupada y false es que si que esta ocupada</returns>
        static bool EstaVacio(int x, int y, int z)
        {
            if (tablero[y,x,z] == '*')
                return true;
            else
                return false;
        }

        static bool HayGanador(int z)
        {
            if (HayGanador2D(z))
                return true;
            else if (HayGanador3D())
                return true;
            else
                return false;
        }

        /// <summary>
        /// Metodo que comprueba si ha ganado alguien en el area 3D
        /// </summary>
        /// <returns>Devuelve un boleano de true cuando ha ganado alguien y de false cuando no</returns>
        static bool HayGanador3D()
        {
            char simbolo = jugador ? 'X' : 'O';
            int contador = 0;

            
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    contador = 0;
                    for (int k = 0; k < tablero.GetLength(2); k++)
                    {
                        if (tablero[i, j, k] == simbolo)
                        {
                            contador++;
                            if (contador == tablero.GetLength(1)) 
                                return true;
                        }
                    }
                }
            }

            for (int i = 0; i < tablero.GetLength(1); i++)
            {
                contador = 0;
                for (int j = 0; j < tablero.GetLength(0); j++)
                {
                    if (tablero[i, j, j] == simbolo)
                    {
                        contador++;
                        if (contador == tablero.GetLength(0))
                            return true;
                    }
                }
            }

            for (int i = 0; i < tablero.GetLength(1); i++)
            {
                contador = 0;
                for (int j = 0; j < tablero.GetLength(0); j++)
                {
                    if (tablero[j, i, j] == simbolo)
                    {
                        contador++;
                        if (contador == tablero.GetLength(0))
                            return true;
                    }
                }
            }

            contador = 0;
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                if (tablero[i, i, i] == simbolo)
                {
                    contador++;
                    if (contador == tablero.GetLength(0))
                        return true;
                }
            }

            contador = 0;
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = tablero.GetLength(1) - 1; j >= 0; j--)
                {
                    if (tablero[i, j, i] == simbolo)
                    {
                        contador++;
                        if (contador == tablero.GetLength(0))
                            return true;
                    }
                }
            }

            contador = 0;
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = tablero.GetLength(1) - 1; j >= 0; j--)
                {
                    if (tablero[j, j, i] == simbolo)
                    {
                        contador++;
                        if (contador == tablero.GetLength(0))
                            return true;
                    }
                }
            }

            contador = 0;
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = tablero.GetLength(1) - 1; j >= 0; j--)
                {
                    if (tablero[i, j, j] == simbolo)
                    {
                        contador++;
                        if (contador == tablero.GetLength(0))
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Metodo que comprueba si hay ganador en el area 2D
        /// </summary>
        /// <param name="z">Variable del eje Z</param>
        /// <returns>Devuelve True si alguien ha ganado y False si na ha ganado nadie</returns>
        static bool HayGanador2D(int z)
        {
            char simbolo = jugador ? 'X' : 'O';
            int contador;

            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                contador = 0;
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    if (tablero[i, j, z] == simbolo)
                    {
                        contador++;
                        if (contador == tablero.GetLength(1))
                            return true;
                    }

                }
            }

            for (int i = 0; i < tablero.GetLength(1); i++)
            {
                contador = 0;
                for (int j = 0; j < tablero.GetLength(0); j++)
                {
                    if (tablero[j, i, z] == simbolo)
                    {
                        contador++;
                        if (contador == tablero.GetLength(0))
                            return true;
                    }
                }
            }

            contador = 0;
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                if (tablero[i, i, z] == simbolo)
                {
                    contador++;
                    if (contador == tablero.GetLength(0))
                        return true;
                }
            }

            contador = 0;
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                if (tablero[i, tablero.GetLength(0) - 1 - i, z] == simbolo)
                {
                    contador++;
                    if (contador == tablero.GetLength(0))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Metodo que comprueba si quedan espacios disponibles en el tablero
        /// </summary>
        /// <returns>Este metodo devuelve true cuando quedan casillas y false cuando no quedan casilla</returns>
        static bool QuedaEspacio()
        {
            for (int i = 0; i < tablero.GetLength(2); i++)
            {
                for (int j = 0; j < tablero.GetLength(0); j++)
                {
                    for (int k = 0; k < tablero.GetLength(1); k++)
                    {
                        if (tablero[j, k, i] == '*')
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}