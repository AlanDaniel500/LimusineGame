using System.Collections.Generic;
using UnityEngine;

public static class DijkstraPathfinder
{
    public static List<Nodo> FindShortestPath(Nodo start, Nodo goal)
    {
        Dictionary<Nodo, float> dist = new Dictionary<Nodo, float>();
        Dictionary<Nodo, Nodo> prev = new Dictionary<Nodo, Nodo>();
        List<Nodo> unvisited = new List<Nodo>();

      
        foreach (Nodo nodo in Object.FindObjectsOfType<Nodo>())
        {
            dist[nodo] = Mathf.Infinity;
            prev[nodo] = null;
            unvisited.Add(nodo);
        }
        dist[start] = 0;

        while (unvisited.Count > 0)
        {
   
            Nodo current = null;
            float minDist = Mathf.Infinity;
            foreach (Nodo n in unvisited)
            {
                if (dist[n] < minDist)
                {
                    minDist = dist[n];
                    current = n;
                }
            }

            if (current == goal)
                break;

            unvisited.Remove(current);


            foreach (Nodo vecino in current.vecinos)
            {
                float alt = dist[current] + Vector2.Distance(current.Posicion, vecino.Posicion);
                if (alt < dist[vecino])
                {
                    dist[vecino] = alt;
                    prev[vecino] = current;
                }
            }
        }

  
        List<Nodo> path = new List<Nodo>();
        Nodo step = goal;

        while (step != null)
        {
            path.Insert(0, step);
            step = prev[step];
        }

        return path;
    }
}
