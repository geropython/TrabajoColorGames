using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Variables
    [SerializeField] private CanvasGroup _fadeOutCanvasGroup; // Para controlar la opacidad de la intro
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private GameObject _fadeOutPanel;
    [SerializeField] private Animator Etapa3ButtonAnimator;
    [SerializeField] private Animator Etapa4ButtonAnimator;
    [SerializeField] private Animator Etapa5ButtonAnimator;
    #endregion

    private void Start()
    {
        _fadeOutCanvasGroup.alpha = 1;
        // Nos aseguramos de que los audios no se reproduzcan al inicio
        _musicAudioSource.Stop();

        // Iniciamos la corutina para esperar 3 segundos y luego hacer el parpadeo
        StartCoroutine(ShowMenuAfterIntro());
    }

    public void OnResetProgress()
    {
        SceneProgress progress = FindObjectOfType<SceneProgress>();
        if (progress != null)
        {
            progress.ResetProgress();
            Debug.Log("Progreso reiniciado.");
        }
        else
        {
            Debug.LogWarning("No se encontró SceneProgress en la escena.");
        }
    }

    #region Fade
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
    #endregion
    
    #region Animations
    IEnumerator PlayButtonAnimationAndLoadScene(Animator buttonAnimator, string sceneName)
    {
        buttonAnimator.ResetTrigger("Click"); // Asegurarse de que no hay triggers previos
        buttonAnimator.SetTrigger("Click"); // Reproduce la animación del botón
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(sceneName); // Cambia a la escena deseada
    }

    public void Etapa3()
    {
        // Iniciar la corrutina para el botón de nivel 1 solo cuando el usuario haga clic
        StartCoroutine(PlayButtonAnimationAndLoadScene(Etapa3ButtonAnimator, "3Menu"));
    }

    public void Etapa4()
    {
        // Iniciar la corrutina para el botón de nivel 2 solo cuando el usuario haga clic
        StartCoroutine(PlayButtonAnimationAndLoadScene(Etapa4ButtonAnimator, "4Menu"));
    }
    public void Etapa5()
    {
        // Iniciar la corrutina para el botón de nivel 2 solo cuando el usuario haga clic
        StartCoroutine(PlayButtonAnimationAndLoadScene(Etapa5ButtonAnimator, "5Menu"));
    }
    #endregion

    public void Quit()
    {
        Application.Quit(); // Salir de la aplicación
    }
}
