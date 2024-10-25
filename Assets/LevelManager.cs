using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action LevelCompleted; // Evento que se llama al completar un nivel
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Tecla C presionada"); // Mensaje de depuración
            OnLevelCompleted(); // Llama al método de finalización
        }
    }
    public void OnLevelCompleted()
    {
        LevelCompleted?.Invoke(); // Llama al evento
        SceneProgress sceneProgress = FindObjectOfType<SceneProgress>();
        if (sceneProgress != null)
        {
            sceneProgress.CompleteLevel(); // Registra que se completó un nivel
        }
        else
        {
            Debug.LogWarning("No se encontró SceneProgress en la escena.");
        }
    }
}
