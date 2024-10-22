using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra_PathFinding : MonoBehaviour
{
    public GameObject[] wayPointsObjList; // Lista con todos los nodos del mapa
    private List<WayPoint> wayPointsScrList; // Lista con todos los nodos del mapa
    [HideInInspector] public WayPoint wayPointFirst, wayPointFinal;

    void Start()
    {
        wayPointsScrList = new List<WayPoint>();
        foreach (GameObject wayPointObj in wayPointsObjList)
        {
            wayPointsScrList.Add(wayPointObj.GetComponent<WayPoint>());
        }
    }

    public List<Transform> FindShortestPath()
    {
        // Diccionario para mantener la distancia más corta conocida a cada nodo desde el inicio.
        var distances = new Dictionary<WayPoint, float>();
        // Diccionario para trazar el camino más corto a cada nodo.
        var previous = new Dictionary<WayPoint, WayPoint>();
        // Lista que actúa como una cola de prioridad para determinar el siguiente nodo a visitar.
        var queue = new List<WayPoint>();

        // Inicializa las distancias y el recorrido previo para cada nodo.
        foreach (var wayPoint in wayPointsScrList) // Asegúrate de que 'nodes' está definido y contiene todos tus nodos.
        {
            distances[wayPoint] = float.MaxValue; // Comienza con distancias infinitas
            previous[wayPoint] = null; // Ningún nodo tiene un 'anterior' al comienzo
            queue.Add(wayPoint); // Agrega el nodo a la cola
        }

        // La distancia al nodo de inicio es obviamente cero
        distances[wayPointFirst] = 0;

        // Mientras aún haya nodos para visitar en la cola...
        while (queue.Count > 0)
        {
            // Ordena la cola por la distancia conocida más corta
            queue.Sort((x, y) => distances[x].CompareTo(distances[y]));
            // 'smallest' es el nodo con la distancia más corta en la cola
            WayPoint smallest = queue[0];
            queue.Remove(smallest); // Lo sacamos de la cola

            // Si 'smallest' es el nodo de destino, construimos el camino más corto y salimos del bucle
            if (smallest == wayPointFinal)
            {
                var path = new List<Transform>(); // Aquí almacenaremos el camino
                while (previous[smallest] != null) // Retrocedemos desde el nodo de destino hasta el inicio
                {
                    path.Add(smallest.transform); // Añadimos el nodo al camino
                    smallest = previous[smallest]; // Vamos al nodo 'anterior'
                }
                path.Add(wayPointFirst.transform); // Añade el nodo de inicio al final de la lista...
                path.Reverse(); // ...y luego invierte la lista para que el nodo de inicio esté al principio
                return path; // Devuelve el camino desde el inicio hasta el destino
            }

            // Para cada 'vecino' del nodo 'smallest'...
            foreach (var neighbor in smallest.neighbors)
            {
                // Calcula la distancia 'tentativa' al vecino como la distancia actual más la distancia al vecino
                float alt = distances[smallest] + Vector3.Distance(smallest.transform.position, neighbor.transform.position);
                // Si la distancia 'tentativa' es menor que la distancia conocida al vecino...
                if (alt < distances[neighbor])
                {
                    distances[neighbor] = alt; // Actualizamos la distancia al vecino
                    previous[neighbor] = smallest; // Marcamos 'smallest' como el camino para llegar allí
                }
            }
        }

        // Si salimos del bucle sin retornar un camino, significa que no se encontró ninguno.
        return new List<Transform>(); // Devuelve una lista vacía
    }
}