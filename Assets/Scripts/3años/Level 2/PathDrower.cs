using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;  // Usando Visual Scripting para posibles integraciones de lógica visual.
using UnityEngine;

public class PathDrower : MonoBehaviour
{
    public Path path;
    private LineRenderer myLineRenderer;

    // Método para crear una nueva ruta (path) y configurar el LineRenderer.
    public void CreatePath()
    {
        // Inicializa el Path usando la posición actual del GameObject al que está atado este script.
        path = new Path(transform.position);
        
        // Añade dinámicamente un componente LineRenderer al GameObject.
        myLineRenderer = this.AddComponent<LineRenderer>();
        
        // Establece el grosor de la línea que se va a dibujar.
        myLineRenderer.widthMultiplier = 0.2f;
    }
    
    // Método para dibujar el camino usando el LineRenderer. Recibe una lista de puntos (en 2D) y los dibuja.
    public void DrawPath(List<Vector2> points)
    {
        // Establece el número de posiciones en el LineRenderer basado en la cantidad de puntos.
        this.GetComponent<LineRenderer>().positionCount = points.Count;

        // Itera sobre cada punto y lo asigna al LineRenderer para que dibuje la línea.
        for (int i = 0; i < points.Count; i++)  // Error corregido: 'i i' debe ser 'i < points.Count'.
        {
            // Establece la posición de cada punto en el LineRenderer.
            this.GetComponent<LineRenderer>().SetPosition(i, points[i]);
        }
    }
}