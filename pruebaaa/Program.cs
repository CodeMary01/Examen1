using System;
using System.Diagnostics;
using System.Globalization;

class Program
{
    // Variables globales
    static int cantidadVentas = 0, cantidadCajero = 0, cantidadPBodega = 0;
    static float acumuladoVentas = 0, acumuladoCajero = 0, acumuladoBodega = 0, diasTrabajo;
    static double horaEntrada, horaSalida;
    static double deduccionTarde, salarioBase = 0;
    static double deduccionTemprano = 0;

    static void Main()
    {
        Menu();
    }

    static void Menu()
    {
        byte opcion;
        do
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("*********-----Empresa M&J--------*********");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1- Registro de Empleados");
            Console.WriteLine("2- Pagos Empleados");
            Console.WriteLine("3- Generar un reporte");
            Console.WriteLine("4- Salir");
            Console.WriteLine("------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Digite una opción: ");
            Console.ForegroundColor = ConsoleColor.White;

            if (!byte.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("Opción inválida. Intente de nuevo.");
                Console.ReadKey();
                continue;
            }
            switch (opcion)
            {
                case 1:
                    RegistrarEmpleado();
                    break;
                case 2:
                    Pagos();
                    break;
                case 3:
                    //no me dio tiempo de hacerla 
                    break;
                case 4:
                    Console.WriteLine("Saliendo del programa...");
                    break;
                default:
                    Console.WriteLine("Opción incorrecta");
                    Console.ReadKey();
                    break;
            }
        } while (opcion != 4);
    }

    static void RegistrarEmpleado()
    {
        Console.Clear();
        Console.WriteLine("Digite su Código de Empleado: ");
        string codigo = Console.ReadLine();

        Console.WriteLine("Digite el nombre del empleado: ");
        string nombre = Console.ReadLine();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Estado de Registro ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("-----------------------");
        Console.WriteLine($"Su código es: {codigo}");
        Console.WriteLine($"Nombre de usuario: {nombre}");
        Console.ReadKey();
    }

    static void Pagos()
    {
        Console.Clear();
        Console.WriteLine("Digite su Código de Empleado: ");
        string codigo = Console.ReadLine();

        Console.WriteLine("Digite el tipo de empleado (1-Ventas, 2-Cajero, 3-Bodega): ");//validaciones el tipo de dato que pido
        if (!int.TryParse(Console.ReadLine(), out int tipoEmpleado) || tipoEmpleado < 1 || tipoEmpleado > 3)
        {
            Console.WriteLine("Tipo de empleado inválido.");
            Console.ReadKey();
            return;
        }

        //problemas con la hora 
        //-----------------
        Console.WriteLine("Digite su hora de Entrada (formato 12 horas, ejemplo: 7:00): ");
        string entrada = Console.ReadLine();
        DateTime horaEntrada = DateTime.ParseExact(entrada, "h:mm", null);

        Console.WriteLine("Digite su hora de Salida (formato 12 horas, ejemplo: 4:00 ): ");
        string salida = Console.ReadLine();
        DateTime horaSalida = DateTime.ParseExact(salida, "h:mm", null);

        //-----------------

        Console.WriteLine("Digite la cantidad de horas laboradas: ");
        if (!float.TryParse(Console.ReadLine(), out float horas) || horas < 0)
        {
            Console.WriteLine("Cantidad de horas inválida.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Digite la cantidad de días trabajados en la semana (debe ser entre 0 y 5): ");
        if (!float.TryParse(Console.ReadLine(), out float diasTrabajo) || diasTrabajo < 0 || diasTrabajo > 5)
        {
            Console.WriteLine("La cantidad de días debe estar entre 0 y 5.");
            Console.ReadKey();
            return;
        }

        // Variables para mis validaciones con if y el cases
        float precioHora = 0, horaExtra = 0, salarioTotal, precioDia;

        //  salario por tipo de empleado
        switch (tipoEmpleado)
        {
            case 1: // Ventas
                precioHora = 1200;
                horaExtra = 1440;
                precioDia = 9600; //basado en 8horas 
                break;
            case 2: // Cajero
                precioHora = 1500;
                horaExtra = 1800;
                precioDia = 12000;
                break;
            case 3: // Bodega
                precioHora = 1350;
                horaExtra = 1620;
                precioDia =10800;
                break;
        }

        // Cálculo del salario base u ordinario y bono adicional 
        if (horas >= 8 && horas <= 40)
        {
            // 8 horas es el horario normal al día
            float horasExtra = horas - 8;
            salarioBase = (8 * precioHora) + (horasExtra * horaExtra);
        }

        //Variables para deducciones de horas 
        double horaEntrada24 = horaEntrada.TimeOfDay.TotalHours;
        double horaSalida24 = horaSalida.TimeOfDay.TotalHours;

        // Calcular deducción por llegar tarde
        if (horaEntrada24 > 7.00) // Si llega tarde (después de las 7:00 AM)
        {
            // Calcula los minutos de retraso
            int minutosTarde = (int)((horaEntrada24 - 7.00) * 60);
            // Deducción proporcional por minutos tarde
            deduccionTarde = minutosTarde * (precioHora / 60);
            salarioBase -= deduccionTarde;  // Aplica la deducción
        }

        // Calcular deducción por salida temprana
        if (horaSalida24 < 4.30) // Si sale antes de las 4:30 PM
        {
            // Calcula los minutos de salida temprana
            int minutosTemprano = (int)((16.00 - horaSalida24) * 60);
            // Deducción proporcional por salida temprana
            deduccionTemprano = minutosTemprano * (precioHora / 60);
            salarioBase -= deduccionTemprano;  // Aplica la deducción
        }//-----------------------

        //salario por dias bono adicional Bonificaciones 
        // Cálculo del salario semanal

        double salarioSemanal = salarioBase * diasTrabajo;  // Salario por día * cantidad de días trabajados
        if (diasTrabajo >= 5)
        {
             

            salarioBase += salarioBase * 0.025f;  // 2.5% de aumento por trabajar 5 días
            Console.WriteLine("¡Bonificación por trabajar 5 días esta semana! (+2.5%)");
        }

       

        // Mostrar resultados
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Registro de Pagos");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("-----------------------");
        Console.WriteLine($"Código de empleado: {codigo}");
        Console.WriteLine($"Tipo de empleado: {(tipoEmpleado == 1 ? "Ventas" : tipoEmpleado == 2 ? "Cajero" : "Bodega")}");
        Console.WriteLine($"Registro de Entrada: {horaEntrada}");
        Console.WriteLine($"Registro de Salida: {horaSalida}");
        Console.WriteLine($"Total horas trabajadas: {horas}");

        Console.WriteLine("Deducción por llegada tarde: " + deduccionTarde);
        Console.WriteLine("Deducción por salida temprana: " + deduccionTemprano);
        Console.WriteLine($"Has trabajado {diasTrabajo} días esta semana.");

        Console.WriteLine("Salario base final: " + salarioBase.ToString("F2"));
        Console.WriteLine($"Salario semanal: {salarioSemanal.ToString("F2")} colones");
        Console.WriteLine();
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }
}
//tengo los decimales mal, la coma no esta correcta, el orden con el salario bruto debe sumar ya todo junto con el semanl 
//Apesar de todo el tiempo no pude llegar a los onbjetivos :( 