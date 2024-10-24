using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Words1Script : MonoBehaviour
{
    #region  Variables
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
    #endregion
    
    #region Start/Update
    void Start()
    {
        record = PlayerPrefs.GetInt("Record", 0);
        StartFade();
        // Iniciar el juego
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
    #endregion

    #region Letter
    void StartRound()
    {
        // Verifica si se han completado las 5 rondas
        if (currentRound >= 5)
        {
            EndGame();
            return;
        }

        // Seleccionar una palabra aleatoria
        int randomIndex = Random.Range(0, words.Count);
        currentWord = words[randomIndex]; // Guardar la palabra actual
        revealedLetters = 1; // Reiniciar las letras reveladas

        // Mostrar la imagen
        imageDisplay.sprite = images[randomIndex];
        inputField.text = ""; // Limpiar el InputField

        // Mostrar solo la primera letra de la palabra en el nuevo texto
        palabraDisplay.text = GetRevealedWord();

        // Iniciar el coroutine para revelar letras
        if (revealCoroutine != null)
        {
            StopCoroutine(revealCoroutine);
        }
        revealCoroutine = StartCoroutine(RevealLetterCoroutine());

        // Actualizar la ronda actual
        rondaActual.text = $"Ronda: {currentRound + 1} / 5"; // Mostrar la ronda actual y el total de rondas
    }

    // Coroutine que revela una letra cada 15 segundos
    private IEnumerator RevealLetterCoroutine()
    {
        while (revealedLetters < currentWord.Length)
        {
            yield return new WaitForSeconds(15); // Esperar 15 segundos
            revealedLetters++; // Incrementar el número de letras reveladas
            palabraDisplay.text = GetRevealedWord(); // Actualizar el texto con las letras reveladas
        }
    }

    // Método que genera la palabra parcialmente revelada
    private string GetRevealedWord()
    {
        string revealedWord = currentWord.Substring(0, revealedLetters);
        int remainingLetters = currentWord.Length - revealedLetters;
        revealedWord += new string('_', remainingLetters); // Rellenar con guiones bajos las letras que faltan
        return revealedWord;
    }
    #endregion

    #region Checking
    public void CheckAnswer()
    {
        string playerInput = inputField.text.Trim(); // Eliminar espacios adicionales
        Debug.Log("Respuesta ingresada: " + playerInput);
        Debug.Log("Palabra correcta: " + currentWord);

        // Verificar si el campo está vacío
        if (string.IsNullOrEmpty(playerInput))
        {
            Debug.Log("El campo de entrada está vacío. No se puede avanzar.");
            return; // No avanzar la ronda si no hay input
        }

        // Verificar si la respuesta es correcta
        if (playerInput.Equals(currentWord, System.StringComparison.OrdinalIgnoreCase))
        {
            score += 10;
            correctFeedback.SetActive(true);
            StartCoroutine(FeedbackCoroutine());
            
            // Avanzar la ronda solo si la respuesta es correcta
            currentRound++;
            scoreText.text = "Puntos: " + score;
        }
        else//Respuesta incorrecta
        {
            score -= 5;
            mistakes++; // Incrementar el conteo de errores
            scoreText.text = "Puntos: " + score;
        }
        
    }
    #endregion

    private IEnumerator FeedbackCoroutine()
    {
        yield return new WaitForSeconds(1); // Esperar 2 segundos
        correctFeedback.SetActive(false); // Desactivar el feedback correcto
        StartRound(); // Iniciar la siguiente ronda
    }
    #region EndGame
    void EndGame()
    {
        // Detener el revealCoroutine cuando termine el juego
        if (revealCoroutine != null)
        {
            StopCoroutine(revealCoroutine);
        }

        // Actualizar récord si es necesario
        if (score > record)
        {
            record = score;
            PlayerPrefs.SetInt("Record", record); // Guardar el récord
        }

        // Mostrar el panel final
        endPanel.SetActive(true);

        // Asignar la puntuación final, errores y récord en los textos correspondientes
        endPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Puntuación Final: " + score;
        errorsText.text = "Errores: " + mistakes; // Mostrar los errores
        recordText.text = "Récord: " + record;   // Mostrar el récord
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