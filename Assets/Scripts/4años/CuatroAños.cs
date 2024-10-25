using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CuatroAños : MonoBehaviour
{ 
    [SerializeField] private CanvasGroup fadePanelCanvasGroup; // CanvasGroup del fadePanel
    [SerializeField] private GameObject mainPanel; // Panel principal que se mostrará después del fade
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Animator level1ButtonAnimator;
    [SerializeField] private Animator level2ButtonAnimator;
    [SerializeField] private AudioSource musicSource;
    private bool isMusicOn = true;
    void Start()
    {
        // Nos aseguramos de que los audios no se reproduzcan al inicio
        musicSource.Stop();
        IniciarJuegoConFade();
    }

    #region Fade Coroutines
    void IniciarJuegoConFade()
    {
        StartCoroutine(FadeInGamePanel());
    }

    private IEnumerator FadeInGamePanel()
    {
        mainPanel.SetActive(true);
        fadePanelCanvasGroup.alpha = 1;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanelCanvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }

        fadePanelCanvasGroup.alpha = 0;
        fadePanelCanvasGroup.gameObject.SetActive(false);
    }
    #endregion
    public void ResetProgress()
    {
        SceneProgress progress = FindObjectOfType<SceneProgress>();
        if (progress != null)
        {
            progress.ResetProgress(); // Llama al método para reiniciar el progreso
        }
        else
        {
            Debug.LogWarning("No se encontró SceneProgress en la escena.");
        }
    }

    // Corrutina para reproducir la animación y luego cambiar de escena
    IEnumerator PlayButtonAnimationAndLoadScene(Animator buttonAnimator, string sceneName)
    {
        buttonAnimator.ResetTrigger("Click"); // Asegurarse de que no hay triggers previos
        buttonAnimator.SetTrigger("Click"); // Reproduce la animación del botón
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(sceneName); // Cambia a la escena deseada
    }

    public void Level1()
    {
        // Iniciar la corrutina para el botón de nivel 1 solo cuando el usuario haga clic
        StartCoroutine(PlayButtonAnimationAndLoadScene(level1ButtonAnimator, "4_Level1"));
    }

    public void Level2()
    {
        // Iniciar la corrutina para el botón de nivel 2 solo cuando el usuario haga clic
        StartCoroutine(PlayButtonAnimationAndLoadScene(level2ButtonAnimator, "4_Level2"));
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        if (isMusicOn)
        {
            musicSource.Play();
        }
        else
        {
            musicSource.Pause();
        }
    }

    public void BackMenu()
    {
        ResetProgress();
        musicSource.Stop();
        SceneManager.LoadScene("MainMenu");
    }
}