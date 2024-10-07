using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] _shadows; // Lugares donde aparecerán las medallas
    [SerializeField] private GameObject[] _medals;  // Medallas a mostrar (deben tener la misma cantidad que _shadows)
    [SerializeField] private Button _diplomaSupremoButton; // Botón para el Diploma Supremo
    [SerializeField] private CanvasGroup _fadeOutCanvasGroup; // Para controlar la opacidad de la intro
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private GameObject _fadeOutPanel;

    private void Start()
    {

        // Nos aseguramos de que los audios no se reproduzcan al inicio
        _musicAudioSource.Stop();

        // Iniciamos la corutina para esperar 3 segundos y luego hacer el parpadeo
        StartCoroutine(ShowMenuAfterIntro());

        // Desactivar las medallas al inicio
        for (int i = 0; i < _medals.Length; i++)
        {
            _medals[i].SetActive(false); // Ocultar todas las medallas inicialmente
        }

        // Desactivar el botón de Diploma Supremo
        _diplomaSupremoButton.interactable = false;

        // Cargar progreso y actualizar UI
        LoadProgress();
    }
    IEnumerator ShowMenuAfterIntro()
    {
        // Esperar 3 segundos
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeOut(_fadeOutCanvasGroup, 3f));  // Desvanecer la intro

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

    // Método para cargar el progreso guardado y actualizar las medallas y el botón del Diploma Supremo
    private void LoadProgress()
    {
        int medalsObtained = 0;

        // Revisar si cada medalla ha sido obtenida
        for (int i = 0; i < _medals.Length; i++)
        {
            if (PlayerPrefs.GetInt($"Medal_Age{i + 3}", 0) == 1) // Revisamos si la medalla del "i" es obtenida
            {
                _medals[i].SetActive(true);  // Mostramos la medalla correspondiente
                medalsObtained++;
            }
        }

        // Activar el botón del Diploma Supremo si todas las medallas están obtenidas
        if (medalsObtained == _medals.Length)
        {
            _diplomaSupremoButton.interactable = true; // Activar el botón si las 3 medallas están obtenidas
        }
    }

    public void TresAños_Scene()
    {
        SceneManager.LoadScene("3 años"); // Cargar la escena de "3 años"
    }

    public void CuatroAños_Scene()
    {
        SceneManager.LoadScene("4 años"); // Cargar la escena de "4 años"
    }

    public void CincoAños_Scene()
    {
        SceneManager.LoadScene("5 años"); // Cargar la escena de "5 años"
    }

    public void Quit()
    {
        Application.Quit(); // Salir de la aplicación
    }
}
