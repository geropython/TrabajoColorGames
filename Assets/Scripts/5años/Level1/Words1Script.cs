using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Words1Script : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<string> words;
    [SerializeField] private List<Sprite> images;

    [SerializeField] private Image imageDisplay;
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI errorsText;
    [SerializeField] private TextMeshProUGUI recordText;

    [SerializeField] private TextMeshProUGUI rondaActual;
    [SerializeField] private TextMeshProUGUI palabraDisplay;

    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject correctFeedback;

    [SerializeField] private CanvasGroup fadePanelCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private GameObject _feedBack;

    private int currentRound = 0;
    private int score = 0;
    private string currentWord;
    private int revealedLetters = 1;
    private int mistakes = 0;
    private int record = 0;
    private Coroutine revealCoroutine;

    [SerializeField] private AudioSource musicSource; 
    private bool isMusicOn = true;

    private List<string> availableWords;
    private List<Sprite> availableImages;
    #endregion
    
    #region Start/Update
    void Start()
    {
        record = PlayerPrefs.GetInt("Record", 0);
        StartFade();

        // Copiar las listas originales de palabras e imágenes para no modificarlas
        availableWords = new List<string>(words);
        availableImages = new List<Sprite>(images);

        endPanel.SetActive(false);
        StartRound();
    }
    #endregion
    
    #region Fade
    void StartFade()
    {
        StartCoroutine(FadePanel());
    }

    private IEnumerator FadePanel()
    {
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

    #region Letter
    void StartRound()
    {
        if (currentRound >= 5)
        {
            EndGame();
            return;
        }

        int randomIndex = Random.Range(0, availableWords.Count);
        currentWord = availableWords[randomIndex];
        revealedLetters = 1;

        imageDisplay.sprite = availableImages[randomIndex];
        inputField.text = "";

        palabraDisplay.text = GetRevealedWord();

        if (revealCoroutine != null)
        {
            StopCoroutine(revealCoroutine);
        }
        revealCoroutine = StartCoroutine(RevealLetterCoroutine());

        rondaActual.text = $"Ronda: {currentRound + 1} / 5";

        // Eliminar la palabra y la imagen ya usadas
        availableWords.RemoveAt(randomIndex);
        availableImages.RemoveAt(randomIndex);

        currentRound++;
    }

    private IEnumerator RevealLetterCoroutine()
    {
        while (revealedLetters < currentWord.Length)
        {
            yield return new WaitForSeconds(15);
            revealedLetters++;
            palabraDisplay.text = GetRevealedWord();
        }
    }

    private string GetRevealedWord()
    {
        string revealedWord = currentWord.Substring(0, revealedLetters);
        int remainingLetters = currentWord.Length - revealedLetters;
        revealedWord += new string('_', remainingLetters);
        return revealedWord;
    }
    #endregion

    #region Checking
    public void CheckAnswer()
    {
        string playerInput = inputField.text.Trim();
        Debug.Log("Respuesta ingresada: " + playerInput);
        Debug.Log("Palabra correcta: " + currentWord);

        if (string.IsNullOrEmpty(playerInput))
        {
            Debug.Log("El campo de entrada está vacío. No se puede avanzar.");
            return;
        }

        if (playerInput.Equals(currentWord, System.StringComparison.OrdinalIgnoreCase))
        {
            score += 10;
            correctFeedback.SetActive(true);
            StartCoroutine(FeedbackCoroutine());

            scoreText.text = "Puntos: " + score;
        }
        else
        {
            score -= 5;
            mistakes++;
            scoreText.text = "Puntos: " + score;
        }
    }
    #endregion

    private IEnumerator FeedbackCoroutine()
    {
        yield return new WaitForSeconds(1);
        correctFeedback.SetActive(false);
        StartRound();
    }
    #region EndGame
    void EndGame()
    {
        if (revealCoroutine != null)
        {
            StopCoroutine(revealCoroutine);
        }

        if (score > record)
        {
            record = score;
            PlayerPrefs.SetInt("Record", record);
        }

        endPanel.SetActive(true);
        endPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Puntuación Final: " + score;
        errorsText.text = "Errores: " + mistakes;
        recordText.text = "Récord: " + record;
    }

    public void BackMenu() { SceneManager.LoadScene("5Menu"); }
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
    #endregion
}
