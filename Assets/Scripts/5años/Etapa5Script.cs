using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Etapa5Script : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadePanelCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private AudioSource musicSource; 
    [SerializeField] private Animator level1ButtonAnimator;
    [SerializeField] private Animator level2ButtonAnimator;
    [SerializeField] private Animator cuentosButtonAnimator;
    private bool isMusicOn = true;
    // Start is called before the first frame update
    void Start()
    {
       StartFade();
    }
    void StartFade()
    {
        StartCoroutine(FadePanel());
    }
     private IEnumerator FadePanel()
    {
        fadePanelCanvasGroup.alpha = 1; // Comienza con el panel de fade completamente negro

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanelCanvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration)); // Disminuir el alpha
            yield return null;
        }

        fadePanelCanvasGroup.alpha = 0; // Asegurarse de que el alpha esté en 0
        fadePanelCanvasGroup.gameObject.SetActive(false); // Desactivar el fadePanel
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
        StartCoroutine(PlayButtonAnimationAndLoadScene(level1ButtonAnimator, "5_Level1"));
    }

    public void Level2()
    {
        // Iniciar la corrutina para el botón de nivel 2 solo cuando el usuario haga clic
        StartCoroutine(PlayButtonAnimationAndLoadScene(level2ButtonAnimator, "5_Level2"));
    }
    public void Cuentos()
    {
        // Iniciar la corrutina para el botón de nivel 2 solo cuando el usuario haga clic
        StartCoroutine(PlayButtonAnimationAndLoadScene(cuentosButtonAnimator, "Cuentos"));
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
    public void BackMenu() { SceneManager.LoadScene("MainMenu"); }
    
}
