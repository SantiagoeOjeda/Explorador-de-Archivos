using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploradorDeArchivos
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // Variable que va a guardar la ruta a explorar
            string directorio;

            do
            {
                // Pide al usuario que ingrese la ruta que vamos a explorar
                Console.Write("Por favor, ingrese la ruta del directorio: ");
                directorio = Console.ReadLine();

                // Verifica si el directorio existe
                if (!Directory.Exists(directorio))
                {
                    Console.WriteLine("La ruta especificada no existe. Por favor, ingrese una ruta válida.");
                }

            } while (!Directory.Exists(directorio)); // Mientras el directorio ingresado no exista, seguiremos pidiendo uno válido

            // Si el directorio ingresado si existe, salimos del do-while y ejecutamos el método con ese directorio como argumento
            ExplorarDirectorio(directorio);
        }

        static void ExplorarDirectorio(string directorioPa)
        {
            // Variable para controlar el ciclo encargado de seguir ejecutando el programa
            bool continuar = true;

            // Mientras la variable "continuar" sea true, el programa se seguirá ejecutando
            while (continuar)
            {
                Console.Clear();
                // Mostramos un mensaje con el nombre del directorio que estamos explorando
                Console.WriteLine($"Contenido de: {directorioPa}\n");

                // Obtenemos una lista de todos los archivos y subdirectorios
                string[] archivosSubdirectorios = Directory.GetFileSystemEntries(directorioPa);

                // Mandamos la matriz con los archivos y subdirectorios al método para que se muestren ordenadamente
                MostrarTabla(archivosSubdirectorios);

                // Le pedimos al usuario que ingrese una opción de las mostradas en la tabla, según su índice. O le damos la opción de salir (s) del programa, o de navegar hacía atrás en la ruta (a)
                Console.Write("Ingresa el número de la opción que deseas explorar (o 'a' para ir hacia atrás en la ruta, 'n' para ingresar una nueva ruta, o 's' para salir): ");
                string opcion = Console.ReadLine();

                // Analizamos las opciones usando una estructura else-if
                if (opcion.ToLower() == "s") // Convertimos a minúscula la "s"
                {
                    // Asignamos el valor de "false" a la variable continuar, lo que hace que el programa termine
                    continuar = false;
                }
                else if (opcion.ToLower() == "a") // Convertimos a minúscula la "a"
                {
                    // Para regresar un nivel en la ruta, usamos a GetDirectoryName para extraer la última parte de la ruta, de esta forma logramos volver hacía atrás
                    directorioPa = Path.GetDirectoryName(directorioPa);
                }
                else if (opcion.ToLower() == "n") // Convertimos a minúscula la "n"
                {
                    Console.Clear();

                    // Pedimos al usuario que vuelva a ingresar una ruta para poder explorarla
                    Console.Write("Ingresa la nueva ruta: ");
                    string nuevaRuta = Console.ReadLine();

                    // Verificamos que sea una ruta valida
                    if (Directory.Exists(nuevaRuta))
                    {
                        directorioPa = nuevaRuta;
                    }
                    else
                    {
                        // Le hacemos saber al usuario que ha ingresado una ruta inválida
                        Console.WriteLine("Ingresa una ruta válida");
                        // Dejamos que el usuario vea que se equivocó, y no escogió un directorio
                        Console.WriteLine("Presiona cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                }
                //// Lógica para renombrar un archivo
                //else if (opcion.ToLower() == "r")
                //{
                //    Console.Write("Ingresa el número del archivo que deseas renombrar: ");
                //    string indiceArchivo = Console.ReadLine();

                //    if (int.TryParse(indiceArchivo, out int indice) && indice >= 0 && indice < archivosSubdirectorios.Length)
                //    {
                //        string archivoSeleccionado = archivosSubdirectorios[indice];

                //        Console.Write("Ingresa el nuevo nombre para el archivo: ");
                //        string nuevoNombre = Console.ReadLine();

                //        try
                //        {
                //            string nuevoPath = Path.Combine(Path.GetDirectoryName(archivoSeleccionado), nuevoNombre);
                //            File.Move(archivoSeleccionado, nuevoPath);
                //            Console.WriteLine("El archivo se ha renombrado correctamente.");
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine($"Error al renombrar el archivo: {ex.Message}");
                //        }
                //    }
                //    else
                //    {
                //        Console.WriteLine("Índice de archivo inválido.");
                //    }
                //}
                else if (Convert.ToInt32(opcion) >= 0 && Convert.ToInt32(opcion) < archivosSubdirectorios.Length) // Verificamos que la opción numérica (índice) sea válida, que se encuentre dentro del rango existente
                {
                    // Si es válida, entonces convertimos la opción a un valor entero que podamos manipular
                    int opcionEscogida = Convert.ToInt32(opcion);

                    // Verificamos si la opción es un directorio válido
                    if (Directory.Exists(archivosSubdirectorios[opcionEscogida]))
                    {
                        // Si lo es, entonces le asignamos esa ruta a la variable "directorioPa", para navegar dentro de él
                        directorioPa = archivosSubdirectorios[opcionEscogida];
                    }
                    else
                    {

                        OperacionesArchivos(archivosSubdirectorios[opcionEscogida]);

                        Console.WriteLine("Presiona cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    // Si no se ingresa una opción válida, entonces tendremos que indicárselo al usuario
                    Console.WriteLine("¡Ingresa un número válido o 'a' para regresar, 's' para salir!");
                }
            }
        }
        // Método usado para mostrar a los archivos y subdirectorios de forma ordenada
        static void MostrarTabla(string[] archivosSubdirectoriosPa)
        {
            // Imprimimos los títulos de la tabla, dejando espacios y colocándolos a la izquierda
            Console.WriteLine($"{"Índice",-8}{"Nombre",-50}{"Tipo",-13}");

            // Instanciamos a string para crear una cadena con guiones que separen a los títulos del contenido de la tabla, y lo hacemos del total del espacio de la suma de los títulos (8 + 50 + 13 = 71)
            String guiones = new string('-', 71);
            Console.WriteLine(guiones);

            // Declaramos variables para guardar el nombre del archivo/subdirectorio y su tipo (extensión o subdirectorio)
            string nombre, tipo;

            // Recorremos a la matriz que contiene a los archivos y subdirectorios
            for (int i = 0; i < archivosSubdirectoriosPa.Length; i++)
            {
                // Extraemos sólo el nombre del archivo o subdirectorio de la posición en que nos encontremos y se lo asignamos a una variable "nombre"
                nombre = Path.GetFileName(archivosSubdirectoriosPa[i]);

                // Si existe un subdirectorio en nuestra posición actual (for) 
                if (Directory.Exists(archivosSubdirectoriosPa[i]))
                {
                    // Entonces el tipo será un subdirectorio
                    tipo = "Subdirectorio";
                }
                else
                {
                    // Si no, entonces extraemos la extensión del archivo en el que estemos y se la asignamos a "tipo"
                    tipo = "Archivo " + Path.GetExtension(archivosSubdirectoriosPa[i]);
                }

                // Mostramos un índice para el elemento en el que estemos, su nombre (sin ruta completa) y su tipo.
                // Usamos la interpolación de cadenas para alinear los elementos "i", "nombre" y "tipo".
                Console.WriteLine($"{i,-8}{nombre,-50}{tipo,-13}");
            }
            //Dejamos un espacio después de mostrar la tabla.
            Console.WriteLine();
        }

        static void OperacionesArchivos(string rutaArchivo)
        {
            string destinoArchivo, rutaCopiarArchivo, rutaMoverArchivo, nuevoNombreArchivo, rutaArchivoRenombrado;

            string nombreDeArchivo = Path.GetFileName(rutaArchivo); 

            Console.WriteLine("1. Copiar");
            Console.WriteLine("2. Mover");
            Console.WriteLine("3. Eliminar");
            Console.WriteLine("4. Renombrar");

            int opcion = Convert.ToInt32(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    Console.WriteLine("Ingrese la ruta a donde quiere copiar el archivo");
                    rutaCopiarArchivo = Console.ReadLine();


                    if (Directory.Exists(rutaCopiarArchivo))
                    {
                        destinoArchivo = Path.Combine(rutaCopiarArchivo, nombreDeArchivo);

                        if(!Directory.Exists(destinoArchivo))
                        {
                            File.Copy(rutaArchivo, destinoArchivo);
                            MensajeRealizadoConExito("Copiar");
                        }
                        else
                        {
                            Console.WriteLine($"El archivo {nombreDeArchivo} ya existe, ¿Quiere sobrescribir el archivo? SI O NO");
                            string respuesta = Console.ReadLine();  

                            if(respuesta.ToLower() == "si")
                            {
                                File.Copy(rutaArchivo, destinoArchivo, true);
                            }
                            else
                            {
                                MensajeOperacionCancelada();
                            }
                        }
                    }
                    else
                    {
                        MensajeRutaNoValida();
                    }
                    break;
                case 2:
                    Console.WriteLine("Ingrese la ruta a donde quiere Mover el archivo");
                    rutaMoverArchivo = Console.ReadLine();


                    if (Directory.Exists(rutaMoverArchivo))
                    {
                        destinoArchivo = Path.Combine(rutaMoverArchivo, nombreDeArchivo);

                        if (!File.Exists(destinoArchivo))
                        {
                            File.Move(rutaArchivo, destinoArchivo);
                            MensajeRealizadoConExito("Mover");
                        }
                        else
                        {
                            Console.WriteLine($"El archivo {nombreDeArchivo} ya existe, ¿Quiere sobrescribir el archivo? SI O NO");
                            string respuesta = Console.ReadLine();

                            if (respuesta.ToLower() == "si")
                            {
                                File.Delete(rutaArchivo);  
                                File.Move(rutaArchivo, destinoArchivo);
                            }
                            else
                            {
                                MensajeOperacionCancelada();
                            }
                        }
                    }
                    else
                    {
                        MensajeRutaNoValida();
                    }
                    break;
                case 3:
                    Console.WriteLine($"Usted quiere eliminar el archivo {nombreDeArchivo}? S/N para confirmar...");
                    string respuestaConfirmar = Console.ReadLine();

                    if( respuestaConfirmar.ToLower() == "s")
                    {
                        File.Delete(rutaArchivo);
                        MensajeRealizadoConExito("Eliminar");
                    }
                    else
                    {
                        MensajeOperacionCancelada();
                    }

                    break;
                case 4:
                    Console.WriteLine("Ingrese el nuevo nombre del archivo: ");
                    nuevoNombreArchivo = Console.ReadLine();    

                    Console.WriteLine($"El nombre {nuevoNombreArchivo} sera el nuevo nombre de {nombreDeArchivo}. Esta de acuerdo? S/N");
                    string resp = Console.ReadLine();   

                    if(resp.ToLower() == "s")
                    {
                        rutaArchivoRenombrado = Path.Combine(Path.GetDirectoryName(rutaArchivo), nuevoNombreArchivo);

                        File.Move(rutaArchivo,rutaArchivoRenombrado);
                        MensajeRealizadoConExito("Renombrar");
                    }
                    else
                    {
                        MensajeOperacionCancelada();    
                    }
                    break;
                default:
                    Console.WriteLine("Ingrese un OPCION valida");
                    break;
            }
        }

        static void MensajeRealizadoConExito(string tipoMovimientoPa)
        {
            Console.WriteLine($"El archivo que ha querido {tipoMovimientoPa} se ha realizado con exito!");
            Console.ReadKey();

        }

        static void MensajeOperacionCancelada()
        {
            Console.WriteLine("La operacion ha sido cancelada! Presione cualquier tecla para continuar");
            Console.ReadKey();
            
        }

        static void MensajeRutaNoValida()
        {
            Console.WriteLine("Ruta NO valida! Ingrese otra");
            Console.ReadKey();
        }
    }
}
