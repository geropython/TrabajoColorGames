using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fadeOutCanvasGroup; // Para controlar la opacidad de la intro
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private GameObject _fadeOutPanel;

    private void Start()
    {

        // Nos aseguramos de que los audios no se reproduzcan al inicio
        _musicAudioSource.Stop();

        // Iniciamos la corutina para esperar 3 segundos y luego hacer el parpadeo
        StartCoroutine(ShowMenuAfterIntro());

    }
    IEnumerator ShowMenuAfterIntro()
    {
        // Esperar 3 segundos
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeOut(_fadeOutCanvasGroup, 2f));  // Desvanecer la intro

        // Una vez que la intro se desvanezca completamente, desactivar el panel de intro
        _fadeOutPanel.SetActive(false);

        // Reproducir los dos audios cuando el menú se activa
        _musicAudioSource.Play();
    }

    // Corutina para desvanecer (fade out) un panel
    IEnumerator FadeOut(CanvasGroup canvasGroup, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0;

    }


    public void Etapa3()
    {
        SceneManager.LoadScene("3Menu"); // Cargar la escena de "3 años"
    }

    public void Etapa4()
    {
        SceneManager.LoadScene("4Menu"); // Cargar la escena de "4 años"
    }

    public void Etapa5()
    {
        SceneManager.LoadScene("5Menu"); // Cargar la escena de "5 años"
    }

    public void Quit()
    {
        Application.Quit(); // Salir de la aplicación
    }
}
