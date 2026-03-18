namespace GrafosApp.Models;

public class RedSocial
{
    public int[,] MatrizAdyacencia { get; private set; }
    public List<Persona> Personas { get; private set; }
    public int Capacidad { get; private set; }

    public RedSocial(int capacidad)
    {
        Capacidad = capacidad;
        MatrizAdyacencia = new int[capacidad, capacidad];
        Personas = new List<Persona>();
    }

    public int CantidadPersonas => Personas.Count;

    public bool AgregarPersona(string nombre)
    {
        if (Personas.Count >= Capacidad)
            return false;

        if (Personas.Any(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            return false;

        Personas.Add(new Persona(Personas.Count, nombre));
        return true;
    }

    public Persona? BuscarPersona(string nombre)
    {
        return Personas.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
    }

    public Persona? BuscarPersonaPorId(int id)
    {
        return id >= 0 && id < Personas.Count ? Personas[id] : null;
    }

    public bool AgregarAmistad(int id1, int id2)
    {
        if (id1 < 0 || id1 >= CantidadPersonas || id2 < 0 || id2 >= CantidadPersonas)
            return false;

        if (id1 == id2)
            return false;

        MatrizAdyacencia[id1, id2] = 1;
        MatrizAdyacencia[id2, id1] = 1;
        return true;
    }

    public bool EliminarAmistad(int id1, int id2)
    {
        if (id1 < 0 || id1 >= CantidadPersonas || id2 < 0 || id2 >= CantidadPersonas)
            return false;

        MatrizAdyacencia[id1, id2] = 0;
        MatrizAdyacencia[id2, id1] = 0;
        return true;
    }

    public bool SonAmigos(int id1, int id2)
    {
        if (id1 < 0 || id1 >= CantidadPersonas || id2 < 0 || id2 >= CantidadPersonas)
            return false;

        return MatrizAdyacencia[id1, id2] == 1;
    }

    public List<Persona> ObtenerAmigos(int idPersona)
    {
        var amigos = new List<Persona>();

        if (idPersona < 0 || idPersona >= CantidadPersonas)
            return amigos;

        for (int i = 0; i < CantidadPersonas; i++)
        {
            if (MatrizAdyacencia[idPersona, i] == 1)
                amigos.Add(Personas[i]);
        }

        return amigos;
    }

    public List<Persona> EncontrarAmigosEnComun(int id1, int id2)
    {
        var comunes = new List<Persona>();

        if (id1 < 0 || id1 >= CantidadPersonas || id2 < 0 || id2 >= CantidadPersonas)
            return comunes;

        for (int i = 0; i < CantidadPersonas; i++)
        {
            if (MatrizAdyacencia[id1, i] == 1 && MatrizAdyacencia[id2, i] == 1)
                comunes.Add(Personas[i]);
        }

        return comunes;
    }

    // TODO [Compañera]: Implementar recorrido DFS (Búsqueda en Profundidad)
    // Debe recorrer el grafo desde un nodo inicial usando una pila o recursión.
    // Parámetro: int idInicio - el id de la persona desde donde inicia el recorrido.
    // Retorna: List<Persona> con el orden de visita.
    // Algoritmo:
    //   1. Crear un array de booleanos "visitados" del tamaño de CantidadPersonas
    //   2. Crear una pila (Stack<int>) y agregar el nodo inicial
    //   3. Mientras la pila no esté vacía:
    //      a. Sacar un nodo de la pila
    //      b. Si no fue visitado, marcarlo como visitado y agregarlo al resultado
    //      c. Recorrer la fila de la matriz de adyacencia para ese nodo
    //      d. Por cada vecino no visitado, agregarlo a la pila
    //   4. Retornar la lista de personas visitadas
    public List<Persona> RecorridoDFS(int idInicio)
    {
        // TODO [Compañera]: Implementar aquí
        return new List<Persona>();
    }

    // TODO [Compañera]: Implementar recorrido BFS (Búsqueda en Anchura)
    // Debe recorrer el grafo desde un nodo inicial usando una cola.
    // Parámetro: int idInicio - el id de la persona desde donde inicia el recorrido.
    // Retorna: List<Persona> con el orden de visita.
    // Algoritmo:
    //   1. Crear un array de booleanos "visitados" del tamaño de CantidadPersonas
    //   2. Crear una cola (Queue<int>) y agregar el nodo inicial, marcarlo como visitado
    //   3. Mientras la cola no esté vacía:
    //      a. Sacar un nodo de la cola
    //      b. Agregarlo al resultado
    //      c. Recorrer la fila de la matriz de adyacencia para ese nodo
    //      d. Por cada vecino no visitado, marcarlo como visitado y agregarlo a la cola
    //   4. Retornar la lista de personas visitadas
    public List<Persona> RecorridoBFS(int idInicio)
    {
        // TODO [Compañera]: Implementar aquí
        return new List<Persona>();
    }

    // TODO [Compañera]: Implementar detección de camino entre dos personas
    // Usa BFS para encontrar si existe un camino entre dos personas.
    // Parámetros: int idOrigen, int idDestino
    // Retorna: List<Persona> con el camino encontrado (vacía si no hay camino).
    // Algoritmo:
    //   1. Usar BFS desde idOrigen
    //   2. Mantener un diccionario de "padres" para reconstruir el camino
    //   3. Si se llega a idDestino, reconstruir el camino desde destino hasta origen
    //   4. Invertir la lista para que vaya de origen a destino
    public List<Persona> EncontrarCamino(int idOrigen, int idDestino)
    {
        // TODO [Compañera]: Implementar aquí
        return new List<Persona>();
    }

    // TODO [Compañera]: Implementar sugerencia de amigos
    // Sugiere amigos basándose en amigos de amigos que no son amigos directos.
    // Parámetro: int idPersona
    // Retorna: List<(Persona sugerida, int amigosEnComun)> ordenada por más amigos en común.
    // Algoritmo:
    //   1. Obtener los amigos directos de la persona
    //   2. Para cada amigo directo, obtener sus amigos
    //   3. Para cada amigo del amigo, si NO es amigo directo y NO es la misma persona,
    //      contar cuántos amigos en común tiene con la persona original
    //   4. Ordenar por cantidad de amigos en común (descendente)
    public List<(Persona Sugerida, int AmigosEnComun)> SugerirAmigos(int idPersona)
    {
        // TODO [Compañera]: Implementar aquí
        return new List<(Persona, int)>();
    }
}
