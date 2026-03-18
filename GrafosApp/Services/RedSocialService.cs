using GrafosApp.Models;

namespace GrafosApp.Services;

public class RedSocialService
{
    private readonly RedSocial _red;

    public RedSocialService(int capacidad)
    {
        _red = new RedSocial(capacidad);
    }

    public RedSocial Red => _red;

    public void CargarDatosDePrueba()
    {
        // Agregar 5 personas de prueba
        _red.AgregarPersona("Ana");
        _red.AgregarPersona("Carlos");
        _red.AgregarPersona("María");
        _red.AgregarPersona("Pedro");
        _red.AgregarPersona("Lucía");

        // Crear conexiones de amistad
        _red.AgregarAmistad(0, 1); // Ana - Carlos
        _red.AgregarAmistad(0, 2); // Ana - María
        _red.AgregarAmistad(1, 2); // Carlos - María
        _red.AgregarAmistad(1, 3); // Carlos - Pedro
        _red.AgregarAmistad(2, 4); // María - Lucía
    }

    public void ImprimirMatrizAdyacencia()
    {
        int n = _red.CantidadPersonas;

        if (n == 0)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("  No hay personas en la red.");
            Console.ResetColor();
            return;
        }

        // Encabezado con nombres
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("          ");
        for (int j = 0; j < n; j++)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string nombre = _red.Personas[j].Nombre;
            if (nombre.Length > 6) nombre = nombre[..6];
            Console.Write($"{nombre,-8}");
        }
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("        " + new string('─', n * 8));
        Console.ResetColor();

        // Filas de la matriz
        for (int i = 0; i < n; i++)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string nombre = _red.Personas[i].Nombre;
            if (nombre.Length > 6) nombre = nombre[..6];
            Console.Write($"  {nombre,-6}│ ");
            Console.ResetColor();

            for (int j = 0; j < n; j++)
            {
                int valor = _red.MatrizAdyacencia[i, j];
                if (valor == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{"1",-8}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"{"0",-8}");
                }
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    public void ImprimirAmigos(int idPersona)
    {
        var persona = _red.BuscarPersonaPorId(idPersona);
        if (persona == null) return;

        var amigos = _red.ObtenerAmigos(idPersona);

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"  Amigos de {persona.Nombre}: {amigos.Count}");
        Console.ResetColor();

        foreach (var amigo in amigos)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"    • {amigo.Nombre}");
        }
        Console.ResetColor();
    }

    public void ImprimirAmigosEnComun(int id1, int id2)
    {
        var p1 = _red.BuscarPersonaPorId(id1);
        var p2 = _red.BuscarPersonaPorId(id2);
        if (p1 == null || p2 == null) return;

        var comunes = _red.EncontrarAmigosEnComun(id1, id2);

        if (comunes.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"  {p1.Nombre} y {p2.Nombre} no tienen amigos en común.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  Amigos en común entre {p1.Nombre} y {p2.Nombre}:");
            Console.ResetColor();

            foreach (var amigo in comunes)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"    • {amigo.Nombre}");
            }
        }
        Console.ResetColor();
    }

    public void ListarPersonas()
    {
        if (_red.CantidadPersonas == 0)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("  No hay personas registradas.");
            Console.ResetColor();
            return;
        }

        foreach (var persona in _red.Personas)
        {
            var amigos = _red.ObtenerAmigos(persona.Id);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"  [{persona.Id}] {persona.Nombre}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  ({amigos.Count} amigos)");
        }
        Console.ResetColor();
    }

    public void ImprimirRecorridoDFS(int idInicio)
    {
        var persona = _red.BuscarPersonaPorId(idInicio);
        if (persona == null) return;

        var recorrido = _red.RecorridoDFS(idInicio);

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"  DFS desde {persona.Nombre}:\n");
        Console.ResetColor();

        Console.Write("  ");
        for (int i = 0; i < recorrido.Count; i++)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"[{recorrido[i].Nombre}]");
            Console.ResetColor();
            if (i < recorrido.Count - 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" → ");
                Console.ResetColor();
            }
        }
        Console.WriteLine();
    }

    public void ImprimirRecorridoBFS(int idInicio)
    {
        var persona = _red.BuscarPersonaPorId(idInicio);
        if (persona == null) return;

        var recorrido = _red.RecorridoBFS(idInicio);

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"  BFS desde {persona.Nombre}:\n");
        Console.ResetColor();

        Console.Write("  ");
        for (int i = 0; i < recorrido.Count; i++)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"[{recorrido[i].Nombre}]");
            Console.ResetColor();
            if (i < recorrido.Count - 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" → ");
                Console.ResetColor();
            }
        }
        Console.WriteLine();
    }

    // TODO [Compañera]: Implementar ImprimirCamino
    // Debe llamar a _red.EncontrarCamino(idOrigen, idDestino) y mostrar el camino
    // con formato: [Persona1] → [Persona2] → [Persona3] ...
    // Si no hay camino, mostrar mensaje de error con ConsoleColor.Red

    // TODO [Compañera]: Implementar ImprimirSugerencias
    // Debe llamar a _red.SugerirAmigos(idPersona) y mostrar las sugerencias
    // con formato: "  • NombrePersona (X amigos en común)"
    // Usar ConsoleColor.Green para el nombre y DarkGray para los amigos en común
}
