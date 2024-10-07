using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CuatroAños : MonoBehaviour
{
    [SerializeField] private AudioSource _musicAudioSource; 
    [SerializeField] private GameObject _fadeOut;

    void Start()
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

        _musicAudioSource.Play();
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

    public void Level1()
    {
        SceneManager.LoadScene("4_Level1");
    }
    public void Level2()
    {
        SceneManager.LoadScene("4_Level2");
    }
    public void Level3()
    {
        SceneManager.LoadScene("4_Level3");
    }
    public void BackMenu(){ SceneManager.LoadScene("MainMenu"); }

}
