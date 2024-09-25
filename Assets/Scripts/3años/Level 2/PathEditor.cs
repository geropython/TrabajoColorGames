using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PathDrawer))]
public class PathEditor : Editor
{
     PathDrawer creator;
     Path path => creator.path;

    private void OnEnable()
    {
        creator = (PathDrawer)target;

        // Asegúrate de que el Path y el LineRenderer estén configurados.
        if (creator.path == null)
        {
            creator.CreatePath();
        }
    }

    private void OnSceneGUI()
    {
        HandleInput();
        DrawPoints();
    }

    private void HandleInput()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
          {
            Undo.RecordObject(creator, "Add Segment");
            path.AddSegment(mousePos);
          }
    }

    private void DrawPoints()
    {
        // Solo llama a DrawPath si el path tiene puntos
          if (path.points.Count > 0)
          {
               creator.DrawPath(path.points);
          }

        Handles.color = Color.red;
        for (int i = 0; i < path.NumPoints; i++)
        {
          Vector2 newPos = Handles.FreeMoveHandle(path[i], 0.05f, Vector2.zero, Handles.CylinderHandleCap);
          if (path[i] != newPos)
          {
               Undo.RecordObject(creator, "Move Point");
               path.MovePoint(i, newPos);
          }
        }
    }
}
