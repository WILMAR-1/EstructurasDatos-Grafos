namespace GrafosApp.UI;

public static class ConsolaUI
{
    public static void MostrarTitulo(string titulo)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine();
        Console.WriteLine("╔" + new string('═', titulo.Length + 2) + "╗");
        Console.WriteLine($"║ {titulo} ║");
        Console.WriteLine("╚" + new string('═', titulo.Length + 2) + "╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    public static void MostrarSubtitulo(string subtitulo)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n── {subtitulo} ──\n");
        Console.ResetColor();
    }

    public static void MostrarExito(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✓ {mensaje}");
        Console.ResetColor();
    }

    public static void MostrarError(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✗ {mensaje}");
        Console.ResetColor();
    }

    public static void MostrarInfo(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($"  {mensaje}");
        Console.ResetColor();
    }

    public static void MostrarLinea()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(new string('─', 50));
        Console.ResetColor();
    }

    public static string? LeerCampo(string etiqueta)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"  {etiqueta}: ");
        Console.ResetColor();

        string input = "";
        while (true)
        {
            var tecla = Console.ReadKey(true);

            if (tecla.Key == ConsoleKey.Escape)
            {
                Console.WriteLine();
                return null;
            }
            else if (tecla.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return input;
            }
            else if (tecla.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input = input[..^1];
                    Console.Write("\b \b");
                }
            }
            else if (!char.IsControl(tecla.KeyChar))
            {
                input += tecla.KeyChar;
                Console.Write(tecla.KeyChar);
            }
        }
    }

    public static void MostrarCancelado()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n  Operación cancelada.");
        Console.ResetColor();
    }

    public static void Pausar()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n  Presiona cualquier tecla para continuar...");
        Console.ResetColor();
        Console.ReadKey(true);
    }

    public static void LimpiarPantalla()
    {
        Console.Clear();
    }
}
