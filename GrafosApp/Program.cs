using GrafosApp.Models;
using GrafosApp.Services;
using GrafosApp.UI;

namespace GrafosApp;

class Program
{
    private static readonly RedSocialService _servicio = new(10);

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        _servicio.CargarDatosDePrueba();

        bool ejecutando = true;

        while (ejecutando)
        {
            MostrarMenuPrincipal();
            var opcion = Console.ReadLine()?.Trim() ?? "";

            switch (opcion)
            {
                case "1":
                    MenuPersonas();
                    break;
                case "2":
                    MenuAmistades();
                    break;
                case "3":
                    MenuConsultas();
                    break;
                case "4":
                    MenuRecorridos();
                    break;
                case "5":
                    VerMatriz();
                    break;
                case "0":
                    ejecutando = false;
                    Despedida();
                    break;
                default:
                    ConsolaUI.MostrarError("Opción no válida.");
                    ConsolaUI.Pausar();
                    break;
            }
        }
    }

    static void MostrarMenuPrincipal()
    {
        ConsolaUI.LimpiarPantalla();
        ConsolaUI.MostrarTitulo("RED SOCIAL - GRAFOS CON MATRIZ DE ADYACENCIA");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  Personas: {_servicio.Red.CantidadPersonas} | Capacidad: {_servicio.Red.Capacidad}\n");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("  [1] Gestionar Personas");
        Console.WriteLine("  [2] Gestionar Amistades");
        Console.WriteLine("  [3] Consultas (Amigos en común)");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  [4] Recorridos (DFS / BFS)");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("  [5] Ver Matriz de Adyacencia");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  [0] Salir");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\n  ➤ Selecciona una opción: ");
        Console.ResetColor();
    }

    #region PERSONAS

    static void MenuPersonas()
    {
        bool enMenu = true;

        while (enMenu)
        {
            ConsolaUI.LimpiarPantalla();
            ConsolaUI.MostrarTitulo("GESTIONAR PERSONAS");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  Total: {_servicio.Red.CantidadPersonas} persona(s)\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  [1] Agregar persona");
            Console.WriteLine("  [2] Ver todas las personas");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  [0] Volver al menú principal");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n  ➤ Selecciona una opción: ");
            Console.ResetColor();

            var opcion = Console.ReadLine()?.Trim() ?? "";

            switch (opcion)
            {
                case "1":
                    AgregarPersona();
                    break;
                case "2":
                    VerPersonas();
                    break;
                case "0":
                    enMenu = false;
                    break;
                default:
                    ConsolaUI.MostrarError("Opción no válida.");
                    ConsolaUI.Pausar();
                    break;
            }
        }
    }

    static void AgregarPersona()
    {
        ConsolaUI.LimpiarPantalla();
        ConsolaUI.MostrarTitulo("AGREGAR PERSONA");
        ConsolaUI.MostrarInfo("(Presiona ESC para cancelar)\n");

        var nombre = ConsolaUI.LeerCampo("Nombre de la persona");
        if (nombre == null) { ConsolaUI.MostrarCancelado(); return; }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            ConsolaUI.MostrarError("Debes ingresar un nombre válido.");
            ConsolaUI.Pausar();
            return;
        }

        if (_servicio.Red.AgregarPersona(nombre))
        {
            Console.WriteLine();
            ConsolaUI.MostrarExito($"Persona '{nombre}' agregada a la red.");
        }
        else
        {
            Console.WriteLine();
            ConsolaUI.MostrarError("No se pudo agregar. Ya existe o la red está llena.");
        }

        ConsolaUI.Pausar();
    }

    static void VerPersonas()
    {
        ConsolaUI.LimpiarPantalla();
        ConsolaUI.MostrarTitulo("PERSONAS EN LA RED");

        _servicio.ListarPersonas();
        ConsolaUI.Pausar();
    }

    #endregion

    #region AMISTADES

    static void MenuAmistades()
    {
        bool enMenu = true;

        while (enMenu)
        {
            ConsolaUI.LimpiarPantalla();
            ConsolaUI.MostrarTitulo("GESTIONAR AMISTADES");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  [1] Agregar amistad");
            Console.WriteLine("  [2] Eliminar amistad");
            Console.WriteLine("  [3] Ver amigos de una persona");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  [0] Volver al menú principal");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n  ➤ Selecciona una opción: ");
            Console.ResetColor();

            var opcion = Console.ReadLine()?.Trim() ?? "";

            switch (opcion)
            {
                case "1":
                    AgregarAmistad();
                    break;
                case "2":
                    EliminarAmistad();
                    break;
                case "3":
                    VerAmigos();
                    break;
                case "0":
                    enMenu = false;
                    break;
                default:
                    ConsolaUI.MostrarError("Opción no válida.");
                    ConsolaUI.Pausar();
                    break;
            }
        }
    }

    static void MostrarListaPersonas()
    {
        ConsolaUI.MostrarSubtitulo("Personas disponibles");
        _servicio.ListarPersonas();
        Console.WriteLine();
    }

    static void AgregarAmistad()
    {
        ConsolaUI.LimpiarPantalla();
        ConsolaUI.MostrarTitulo("AGREGAR AMISTAD");
        ConsolaUI.MostrarInfo("(Presiona ESC para cancelar)\n");

        MostrarListaPersonas();

        var nombre1 = ConsolaUI.LeerCampo("Nombre de la primera persona");
        if (nombre1 == null) { ConsolaUI.MostrarCancelado(); return; }

        var p1 = _servicio.Red.BuscarPersona(nombre1);
        if (p1 == null)
        {
            ConsolaUI.MostrarError($"Persona '{nombre1}' no encontrada.");
            ConsolaUI.Pausar();
            return;
        }

        var nombre2 = ConsolaUI.LeerCampo("Nombre de la segunda persona");
        if (nombre2 == null) { ConsolaUI.MostrarCancelado(); return; }

        var p2 = _servicio.Red.BuscarPersona(nombre2);
        if (p2 == null)
        {
            ConsolaUI.MostrarError($"Persona '{nombre2}' no encontrada.");
            ConsolaUI.Pausar();
            return;
        }

        if (_servicio.Red.SonAmigos(p1.Id, p2.Id))
        {
            ConsolaUI.MostrarError($"{p1.Nombre} y {p2.Nombre} ya son amigos.");
            ConsolaUI.Pausar();
            return;
        }

        if (_servicio.Red.AgregarAmistad(p1.Id, p2.Id))
        {
            Console.WriteLine();
            ConsolaUI.MostrarExito($"Amistad creada entre {p1.Nombre} y {p2.Nombre}.");
        }
        else
        {
            ConsolaUI.MostrarError("No se pudo crear la amistad.");
        }

        ConsolaUI.Pausar();
    }

    static void EliminarAmistad()
    {
        ConsolaUI.LimpiarPantalla();
        ConsolaUI.MostrarTitulo("ELIMINAR AMISTAD");
        ConsolaUI.MostrarInfo("(Presiona ESC para cancelar)\n");

        MostrarListaPersonas();

        var nombre1 = ConsolaUI.LeerCampo("Nombre de la primera persona");
        if (nombre1 == null) { ConsolaUI.MostrarCancelado(); return; }

        var p1 = _servicio.Red.BuscarPersona(nombre1);
        if (p1 == null)
        {
            ConsolaUI.MostrarError($"Persona '{nombre1}' no encontrada.");
            ConsolaUI.Pausar();
            return;
        }

        var nombre2 = ConsolaUI.LeerCampo("Nombre de la segunda persona");
        if (nombre2 == null) { ConsolaUI.MostrarCancelado(); return; }

        var p2 = _servicio.Red.BuscarPersona(nombre2);
        if (p2 == null)
        {
            ConsolaUI.MostrarError($"Persona '{nombre2}' no encontrada.");
            ConsolaUI.Pausar();
            return;
        }

        if (!_servicio.Red.SonAmigos(p1.Id, p2.Id))
        {
            ConsolaUI.MostrarError($"{p1.Nombre} y {p2.Nombre} no son amigos.");
            ConsolaUI.Pausar();
            return;
        }

        if (_servicio.Red.EliminarAmistad(p1.Id, p2.Id))
        {
            Console.WriteLine();
            ConsolaUI.MostrarExito($"Amistad eliminada entre {p1.Nombre} y {p2.Nombre}.");
        }
        else
        {
            ConsolaUI.MostrarError("No se pudo eliminar la amistad.");
        }

        ConsolaUI.Pausar();
    }

    static void VerAmigos()
    {
        ConsolaUI.LimpiarPantalla();
        ConsolaUI.MostrarTitulo("VER AMIGOS");
        ConsolaUI.MostrarInfo("(Presiona ESC para cancelar)\n");

        MostrarListaPersonas();

        var nombre = ConsolaUI.LeerCampo("Nombre de la persona");
        if (nombre == null) { ConsolaUI.MostrarCancelado(); return; }

        var persona = _servicio.Red.BuscarPersona(nombre);
        if (persona == null)
        {
            ConsolaUI.MostrarError($"Persona '{nombre}' no encontrada.");
            ConsolaUI.Pausar();
            return;
        }

        Console.WriteLine();
        _servicio.ImprimirAmigos(persona.Id);
        ConsolaUI.Pausar();
    }

    #endregion

    #region CONSULTAS

    static void MenuConsultas()
    {
        bool enMenu = true;

        while (enMenu)
        {
            ConsolaUI.LimpiarPantalla();
            ConsolaUI.MostrarTitulo("CONSULTAS");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  [1] Amigos en común");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  [2] Sugerir amigos (TODO)");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  [0] Volver al menú principal");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n  ➤ Selecciona una opción: ");
            Console.ResetColor();

            var opcion = Console.ReadLine()?.Trim() ?? "";

            switch (opcion)
            {
                case "1":
                    BuscarAmigosEnComun();
                    break;
                case "2":
                    SugerirAmigos();
                    break;
                case "0":
                    enMenu = false;
                    break;
                default:
                    ConsolaUI.MostrarError("Opción no válida.");
                    ConsolaUI.Pausar();
                    break;
            }
        }
    }

    static void BuscarAmigosEnComun()
    {
        ConsolaUI.LimpiarPantalla();
        ConsolaUI.MostrarTitulo("AMIGOS EN COMÚN");
        ConsolaUI.MostrarInfo("(Presiona ESC para cancelar)\n");

        MostrarListaPersonas();

        var nombre1 = ConsolaUI.LeerCampo("Nombre de la primera persona");
        if (nombre1 == null) { ConsolaUI.MostrarCancelado(); return; }

        var p1 = _servicio.Red.BuscarPersona(nombre1);
        if (p1 == null)
        {
            ConsolaUI.MostrarError($"Persona '{nombre1}' no encontrada.");
            ConsolaUI.Pausar();
            return;
        }

        var nombre2 = ConsolaUI.LeerCampo("Nombre de la segunda persona");
        if (nombre2 == null) { ConsolaUI.MostrarCancelado(); return; }

        var p2 = _servicio.Red.BuscarPersona(nombre2);
        if (p2 == null)
        {
            ConsolaUI.MostrarError($"Persona '{nombre2}' no encontrada.");
            ConsolaUI.Pausar();
            return;
        }

        Console.WriteLine();
        _servicio.ImprimirAmigosEnComun(p1.Id, p2.Id);
        ConsolaUI.Pausar();
    }

    // TODO [Compañera]: Completar este método para mostrar sugerencias de amigos
    static void SugerirAmigos()
    {
        ConsolaUI.LimpiarPantalla();
        ConsolaUI.MostrarTitulo("SUGERIR AMIGOS");

        // TODO [Compañera]: Implementar la interfaz para sugerir amigos
        // 1. Mostrar lista de personas
        // 2. Pedir el nombre de la persona
        // 3. Llamar a _servicio.ImprimirSugerencias(persona.Id)
        // 4. Mostrar los resultados

        ConsolaUI.MostrarInfo("Funcionalidad pendiente de implementar.");
        ConsolaUI.Pausar();
    }

    #endregion

    #region RECORRIDOS

    static void MenuRecorridos()
    {
        bool enMenu = true;

        while (enMenu)
        {
            ConsolaUI.LimpiarPantalla();
            ConsolaUI.MostrarTitulo("RECORRIDOS DEL GRAFO");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  [1] Recorrido DFS (Búsqueda en Profundidad)");
            Console.WriteLine("  [2] Recorrido BFS (Búsqueda en Anchura)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  [3] Encontrar camino entre dos personas (TODO)");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  [0] Volver al menú principal");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n  ➤ Selecciona una opción: ");
            Console.ResetColor();

            var opcion = Console.ReadLine()?.Trim() ?? "";

            switch (opcion)
            {
                case "1":
                    EjecutarDFS();
                    break;
                case "2":
                    EjecutarBFS();
                    break;
                case "3":
                    EncontrarCamino();
                    break;
                case "0":
                    enMenu = false;
                    break;
                default:
                    ConsolaUI.MostrarError("Opción no válida.");
                    ConsolaUI.Pausar();
                    break;
            }
        }
    }

    static void EjecutarDFS()
    {
        ConsolaUI.LimpiarPantalla();
        ConsolaUI.MostrarTitulo("RECORRIDO DFS (BÚSQUEDA EN PROFUNDIDAD)");
        ConsolaUI.MostrarInfo("(Presiona ESC para cancelar)\n");

        MostrarListaPersonas();

        var nombre = ConsolaUI.LeerCampo("Persona inicial");
        if (nombre == null) { ConsolaUI.MostrarCancelado(); return; }

        var persona = _servicio.Red.BuscarPersona(nombre);
        if (persona == null)
        {
            ConsolaUI.MostrarError($"Persona '{nombre}' no encontrada.");
            ConsolaUI.Pausar();
            return;
        }

        Console.WriteLine();
        _servicio.ImprimirRecorridoDFS(persona.Id);
        ConsolaUI.Pausar();
    }

    static void EjecutarBFS()
    {
        ConsolaUI.LimpiarPantalla();
        ConsolaUI.MostrarTitulo("RECORRIDO BFS (BÚSQUEDA EN ANCHURA)");
        ConsolaUI.MostrarInfo("(Presiona ESC para cancelar)\n");

        MostrarListaPersonas();

        var nombre = ConsolaUI.LeerCampo("Persona inicial");
        if (nombre == null) { ConsolaUI.MostrarCancelado(); return; }

        var persona = _servicio.Red.BuscarPersona(nombre);
        if (persona == null)
        {
            ConsolaUI.MostrarError($"Persona '{nombre}' no encontrada.");
            ConsolaUI.Pausar();
            return;
        }

        Console.WriteLine();
        _servicio.ImprimirRecorridoBFS(persona.Id);
        ConsolaUI.Pausar();
    }

    // TODO [Compañera]: Implementar EncontrarCamino
    // 1. Mostrar lista de personas con MostrarListaPersonas()
    // 2. Pedir nombre de persona origen con ConsolaUI.LeerCampo("Persona origen")
    // 3. Pedir nombre de persona destino con ConsolaUI.LeerCampo("Persona destino")
    // 4. Buscar ambas personas con _servicio.Red.BuscarPersona
    // 5. Llamar a _servicio.ImprimirCamino(origen.Id, destino.Id)
    static void EncontrarCamino()
    {
        ConsolaUI.LimpiarPantalla();
        ConsolaUI.MostrarTitulo("ENCONTRAR CAMINO");

        ConsolaUI.MostrarInfo("Funcionalidad pendiente de implementar.");
        ConsolaUI.Pausar();
    }

    #endregion

    #region MATRIZ

    static void VerMatriz()
    {
        ConsolaUI.LimpiarPantalla();
        ConsolaUI.MostrarTitulo("MATRIZ DE ADYACENCIA");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  Personas: {_servicio.Red.CantidadPersonas}\n");
        Console.ResetColor();

        _servicio.ImprimirMatrizAdyacencia();
        ConsolaUI.Pausar();
    }

    #endregion

    static void Despedida()
    {
        ConsolaUI.LimpiarPantalla();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n  ╔═══════════════════════════════════════╗");
        Console.WriteLine("  ║    ¡Gracias por usar la aplicación!   ║");
        Console.WriteLine("  ╚═══════════════════════════════════════╝\n");
        Console.ResetColor();
    }
}
