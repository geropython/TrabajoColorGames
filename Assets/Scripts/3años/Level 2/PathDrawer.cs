using System.Collections;
using System.Collections.Generic; // Usando Visual Scripting para posibles integraciones de lógica visual.
using UnityEngine;
using Unity.VisualScripting;

public class PathDrawer : MonoBehaviour
{
    public Path path;
    public LineRenderer myLineRenderer;

    // Método para crear una nueva ruta (path) y configurar el LineRenderer.
    public void CreatePath()
    {
        if (myLineRenderer == null)
        {
            myLineRenderer = gameObject.AddComponent<LineRenderer>();
            myLineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Asegúrate de que tenga un material.
            myLineRenderer.startColor = Color.red; // Define el color de inicio.
            myLineRenderer.endColor = Color.red; // Define el color de fin.
        }

        myLineRenderer.widthMultiplier = 0.2f;
        path = new Path(transform.position);
    }

    // Método para dibujar el camino usando el LineRenderer.
    public void DrawPath(List<Vector2> points)
    {
        // Asegúrate de que myLineRenderer está inicializado.
        if (myLineRenderer == null)
        {
            Debug.LogError("LineRenderer no está inicializado. Asegúrate de llamar a CreatePath() primero.");
            return; // Sale del método si myLineRenderer no está inicializado.
        }

        // Establece el número de posiciones en el LineRenderer basado en la cantidad de puntos.
        myLineRenderer.positionCount = points.Count;

        // Itera sobre cada punto y lo asigna al LineRenderer.
        for (int i = 0; i < points.Count; i++)
        {
            // Asegúrate de convertir a 3D ya que LineRenderer usa Vector3.
            myLineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0f));
        }
    }
}