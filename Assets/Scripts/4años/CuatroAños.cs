using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CuatroAños : MonoBehaviour
{ 
    [SerializeField] private GameObject _fadeOut;
    [SerializeField] private Animator level1ButtonAnimator;
    [SerializeField] private Animator level2ButtonAnimator;
    [SerializeField] private AudioSource musicSource;
    private bool isMusicOn = true;
    void Start()
    {
        // Nos aseguramos de que los audios no se reproduzcan al inicio
        musicSource.Stop();

        // Iniciamos la corutina para esperar 3 segundos y luego hacer el parpadeo
        StartCoroutine(ShowMenuAfterIntro());
    }

    IEnumerator ShowMenuAfterIntro()
    {
        // Esperar 3 segundos
        yield return new WaitForSeconds(4f);

        musicSource.Play();
    }

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

    // Corrutina para reproducir la animación y luego cambiar de escena
    IEnumerator PlayButtonAnimationAndLoadScene(Animator buttonAnimator, string sceneName)
    {
        buttonAnimator.ResetTrigger("Click"); // Asegurarse de que no hay triggers previos
        buttonAnimator.SetTrigger("Click"); // Reproduce la animación del botón
        yield return new WaitForSeconds(buttonAnimator.GetCurrentAnimatorStateInfo(0).length); // Espera a que termine la animación

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
        SceneManager.LoadScene("MainMenu");
    }
}