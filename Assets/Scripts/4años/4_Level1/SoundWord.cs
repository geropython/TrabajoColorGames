using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundWord : MonoBehaviour
{
    [SerializeField] private Button musicButton; // Botón para activar/desactivar la música
    [SerializeField] private Button backMenuButton; // Botón para volver al menú
    [SerializeField] private Button voiceButton; // Botón para activar el audio de voz
    [SerializeField] private AudioSource musicSource; // Componente AudioSource para la música
    [SerializeField] private AudioSource voiceSource; // Componente AudioSource para el audio de voz

    private bool isMusicPlaying = true; // Estado de la música

    void Start()
    {
        // Asignar eventos a los botones
        musicButton.onClick.AddListener(ToggleMusic);
        backMenuButton.onClick.AddListener(BackToMenu);
        voiceButton.onClick.AddListener(PlayVoice);
        
        // Asegúrate de que la música esté reproduciéndose al inicio
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void ToggleMusic()
    {
        if (isMusicPlaying)
        {
            musicSource.Pause(); // Pausar música
            Debug.Log("Música pausada");
        }
        else
        {
            musicSource.Play(); // Reproducir música
            Debug.Log("Música reproduciéndose");
        }
        isMusicPlaying = !isMusicPlaying; // Cambiar el estado de la música
    }

    public void BackToMenu() 
    {
        SceneManager.LoadScene("4Menu"); 
    }

    void PlayVoice()
    {
        if (!voiceSource.isPlaying) // Verifica que el audio de voz no esté reproduciéndose
        {
            voiceSource.PlayOneShot(voiceSource.clip); // Reproducir el clip de voz
        }
    }
}