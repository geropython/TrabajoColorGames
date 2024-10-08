using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private GameObject soundObject; // Asegúrate de asignar este en el Inspector
    [SerializeField] private AudioClip correctSound; // Asegúrate de asignar este en el Inspector
    [SerializeField] private AudioClip incorrectSound; // Asegúrate de asignar este en el Inspector

    private AudioSource audioSource;

    private void Awake()
    {
        // Asegúrate de que solo haya una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: mantener la instancia entre escenas
        }
        else
        {
            Destroy(gameObject); // Destruir si ya hay una instancia
            return; // Salir del Awake para evitar más inicializaciones
        }

        // Inicializa el AudioSource
        if (soundObject != null)
        {
            audioSource = soundObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogError("soundObject no está asignado en el inspector.");
        }
    }

    public void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("audioSource o clip es nulo.");
        }
    }
}