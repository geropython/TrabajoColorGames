using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuTresScript : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _introPanel;
    [SerializeField] private CanvasGroup _introCanvasGroup; // Para controlar la opacidad de la intro
    [SerializeField] private CanvasGroup _menuCanvasGroup;  // Para controlar la opacidad del menú
    [SerializeField] private AudioSource _musicAudioSource;  // Audio de la música
    [SerializeField] private AudioSource _introAudioSource;  // Audio de la charla de introducción

    void Start()
    {
        // Al iniciar, activamos la intro y el menú pero el menú es invisible
        _menuPanel.SetActive(true);
        _introPanel.SetActive(true);
        _menuCanvasGroup.alpha = 0; // El menú empieza invisible
        _introCanvasGroup.alpha = 1; // La intro empieza visible

        // Nos aseguramos de que los audios no se reproduzcan al inicio
        _musicAudioSource.Stop();
        _introAudioSource.Stop();

        // Iniciamos la corutina para esperar 3 segundos y luego hacer el parpadeo
        StartCoroutine(ShowMenuAfterIntro());
    }

    IEnumerator ShowMenuAfterIntro()
    {
        // Esperar 3 segundos
        yield return new WaitForSeconds(3f);

        // Activar el menú y empezar el fade in del menú mientras hacemos el fade out de la intro
        StartCoroutine(FadeIn(_menuCanvasGroup, 1f));  // Desvanecer el menú al mismo tiempo
        yield return StartCoroutine(FadeOut(_introCanvasGroup, 1f));  // Desvanecer la intro

        // Una vez que la intro se desvanezca completamente, desactivar el panel de intro
        _introPanel.SetActive(false);

        // Reproducir los dos audios cuando el menú se activa
        _musicAudioSource.Play();
        _introAudioSource.Play();
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

        canvasGroup.alpha = 0; // Asegurarse de que esté completamente invisible
    }

    // Corutina para aparecer (fade in) un panel
    IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 1; // Asegurarse de que esté completamente visible
    }

    public void Abecedario() { SceneManager.LoadScene("Abecedario"); }

    public void Nivel1() { SceneManager.LoadScene("Nivel 1"); }

    public void Nivel2() { SceneManager.LoadScene("Nivel 2"); }

    public void BackMenu(){ SceneManager.LoadScene("MainMenu"); }

    public void Quit(){Application.Quit();}
}