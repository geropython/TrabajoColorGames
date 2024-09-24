using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  // Permite que la clase 'Path' sea visible y editable desde el Inspector en Unity.
public class Path
{
    // Lista de puntos que forman el camino. Cada punto es un Vector2 que representa una posición en el espacio 2D.
    public List<Vector2> points;

    // Constructor de la clase 'Path'. Toma un punto central (center) y genera un segmento inicial de cuatro puntos:
    public Path(Vector2 center)
    {
        points = new List<Vector2>
        {
            center + Vector2.left,
            center + Vector2.left * 0.35f,
            center + Vector2.right * 0.35f,
            center + Vector2.right
        };
    }

    // Indexador: permite acceder a los puntos de la lista usando un índice como si fuera un array.
    public Vector2 this[int i] => points[i];

    // Propiedad que devuelve el número total de puntos en el camino.
    public int NumPoints => points.Count;

    // Propiedad que calcula y devuelve el número de segmentos en el camino. Un segmento inicial tiene 4 puntos, y cada nuevo segmento añade 3 puntos más (1 ancla y 2 de control).
    public int NumSegments => (points.Count - 4) / 3 + 1;

    // Método que añade un nuevo punto ancla (anchorPos) al final de la lista de puntos, creando un nuevo segmento.
    public void AddSegment(Vector2 anchorPos)
    {
        points.Add(anchorPos);
    }

    // Método que devuelve un array de 3 puntos que pertenecen a un segmento específico 'i'.
    public Vector2[] GetpointsInSegment(int i) => new Vector2[] 
    { 
        points[i * 3],
        points[i * 3 + 1], 
        points[i * 3 + 2], 
        points[i * 3]
    };

    // Método para mover un punto específico de la lista. Toma el índice 'i' del punto y su nueva posición 'pos'.
    public void MovePoint(int i, Vector2 pos)
    {
        points[i] = pos;
    }
}