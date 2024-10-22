using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuTresScript : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private CanvasGroup fadePanelCanvasGroup; // CanvasGroup del fadePanel
    [SerializeField] private GameObject mainPanel; // Panel principal que se mostrará después del fade
    [SerializeField] private float fadeDuration = 1f;  // Para controlar la opacidad del menú
    [SerializeField] private AudioSource _musicAudioSource;  // Audio de la música

    void Start()
    {
        // Al iniciar, activamos la intro y el menú pero el menú es invisible
        _menuPanel.SetActive(true);
        IniciarJuegoConFade();
        // Nos aseguramos de que los audios no se reproduzcan al inicio
        _musicAudioSource.Stop();
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
    
    // public void OpenWebPage() { Application.OpenURL("https://sites.google.com/view/colorsgames/inicio?authuser=0"); }
    public void Abecedario() { SceneManager.LoadScene("Abecedario"); }
    public void Album() { SceneManager.LoadScene("Album"); }
    public void Nivel1() { SceneManager.LoadScene("Nivel 1"); }
    public void Nivel2() { SceneManager.LoadScene("Nivel 2"); }
    public void BackMenu(){ SceneManager.LoadScene("3Menu"); }
    public void Quit(){Application.Quit();}
}