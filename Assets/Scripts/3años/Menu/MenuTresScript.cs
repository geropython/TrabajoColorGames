using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuTresScript : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private CanvasGroup fadePanelCanvasGroup; // CanvasGroup del fadePanel
    [SerializeField] private GameObject mainPanel; // Panel principal que se mostrará después del fade
    [SerializeField] private float fadeDuration = 1f; // Para controlar la opacidad del menú
    [SerializeField] private AudioSource musicSource;
    private bool isMusicOn = true; 

    void Awake()
    {
        // Asegúrate de que solo haya una instancia del AudioSource
       if (FindObjectsByType<MenuTresScript>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
     public void ResetProgress()
    {
        SceneProgress progress = FindAnyObjectByType<SceneProgress>();
        if (progress != null)
        {
            progress.ResetProgress(); // Llama al método para reiniciar el progreso
        }
        else
        {
            Debug.LogWarning("No se encontró SceneProgress en la escena.");
        }
    }

    void Start()
    {
        _menuPanel.SetActive(true);
        IniciarJuegoConFade();

        if (isMusicOn)
        {
            musicSource.Play();
        }
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

    #region Transition
    public void LoadScene(string sceneName)
    {
        musicSource.Stop();
        SceneManager.LoadScene(sceneName);
    }

    public void Abecedario() { LoadScene("Abecedario"); }
    public void Album() { LoadScene("Album"); }
    public void Nivel1() { LoadScene("Nivel 1"); }
    public void Nivel2() { LoadScene("Nivel 2"); }
    public void BackMenu() { ResetProgress(); LoadScene("MainMenu"); }
    #endregion

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
}