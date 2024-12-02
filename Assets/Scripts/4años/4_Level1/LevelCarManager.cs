using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelCarManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private Button redButton, blueButton, greenButton;
    [SerializeField] private Transform finishLine;
    [SerializeField] private AudioSource wordAudioSource;
    [SerializeField] private AudioClip redClip;
    [SerializeField] private AudioClip blueClip;
    [SerializeField] private AudioClip greenClip;
    
    [SerializeField] private GameObject correctFeedback;
    private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI recordScoreText;
    [SerializeField] private TextMeshProUGUI errorsText;
    
    private int score = 0;
    private int errors = 0;
    
    [SerializeField] private float speed = 5f;
    [SerializeField] private float moveDistance = 2f;
    
    [SerializeField] private int correctAnswers = 0;
    [SerializeField] private int _limite = 12;
    [SerializeField] private string currentColor;

    [SerializeField] private CanvasGroup fadePanelCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private List<string> colors = new List<string> { "rojo", "azul", "verde" };
    private bool shouldMove = false;
    private bool hasReachedFinish = false;
    private Vector3 targetPosition;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource correctSound;
    [SerializeField] private AudioSource incorrectSound;
    private bool isMusicOn = true;

    private int recordScore;

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
            fadePanelCanvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration)); // Disminuir el alpha
            yield return null;
        }

        fadePanelCanvasGroup.alpha = 0;
        fadePanelCanvasGroup.gameObject.SetActive(false); 
    }
    #endregion

    public void Start()
    {
        StartFade();
        _endPanel.SetActive(false);
        recordScore = PlayerPrefs.GetInt("RecordScore", 0);
        SetNewColor();
        redButton.onClick.AddListener(() => CheckColor("rojo"));
        blueButton.onClick.AddListener(() => CheckColor("azul"));
        greenButton.onClick.AddListener(() => CheckColor("verde"));
    }

    public void Update()
    {
        if (shouldMove && !hasReachedFinish)
        {
            MoveCar();
            
            if (Mathf.Abs(car.transform.position.y - 572f) < 0.1f)
            {
                Celebrate();
            }
        }
    }

    #region Color
    void SetNewColor()
    {
        int randomIndex = Random.Range(0, colors.Count);
        currentColor = colors[randomIndex];
        instructionText.text = currentColor;
        // Reproduce el audio correspondiente al color actual
        PlayColorAudio(currentColor);
    }
    void PlayColorAudio(string color)
    {
        switch (color)
        {
            case "rojo":
                wordAudioSource.clip = redClip;
                break;
            case "azul":
                wordAudioSource.clip = blueClip;
                break;
            case "verde":
                wordAudioSource.clip = greenClip;
                break;
        }
        wordAudioSource.Play();
    }

    void CheckColor(string selectedColor)
    {
        if (selectedColor == currentColor)
        {
            correctAnswers++;
            score += 10;
            StartCoroutine(ShowCorrectFeedback());
            correctSound.Play();

            SetTargetPosition();
            shouldMove = true;

            if (correctAnswers >= _limite)
            {
                Celebrate();
                return;
            }
        }
        else
        {
            score -= 5;
            incorrectSound.Play();
            errors++;
        }

        if (!hasReachedFinish)
        {
            SetNewColor();
        }
    }
    private IEnumerator ShowCorrectFeedback()
    {
        correctFeedback.SetActive(true);

        yield return new WaitForSeconds(2f);

        correctFeedback.SetActive(false);
    }
    #endregion

    void SetTargetPosition()
    {
        // Establece la nueva posición objetivo a la que el auto debe moverse (una distancia fija hacia adelante)
        targetPosition = car.transform.position + (finishLine.position - car.transform.position).normalized * moveDistance;

        // Asegurarse de que el auto no sobrepase la meta
        if (Vector3.Distance(targetPosition, finishLine.position) < moveDistance)
        {
            targetPosition = finishLine.position;  // Si está cerca de la meta, fija la posición en la meta
        }
    }

    void MoveCar()
    {
        car.transform.position = Vector3.MoveTowards(car.transform.position, targetPosition, speed * Time.deltaTime);

        // Si el auto ha alcanzado la nueva posición objetivo, se detiene el movimiento
        if (Vector3.Distance(car.transform.position, targetPosition) < 0.01f)
        {
            shouldMove = false;  // Detiene el movimiento del auto
        }
    }

    void Celebrate()
    {
        _endPanel.SetActive(true);
        shouldMove = false;  // Asegura que el auto se detenga
        hasReachedFinish = true;  // Marca que ha llegado a la meta

        // Mostrar el puntaje final
        finalScoreText.text = "Puntaje Final: " + score.ToString();
        errorsText.text = "Errores: " + errors.ToString();

        // Verificar si se superó el récord
        if (score > recordScore)
        {
            recordScore = score;
            PlayerPrefs.SetInt("RecordScore", recordScore); // Guardar el nuevo récord
            recordScoreText.text = "¡Nuevo Récord!: " + recordScore.ToString();
        }
        else { recordScoreText.text = "Récord: " + recordScore.ToString(); }
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        if (isMusicOn) { musicSource.Play(); }
        else { musicSource.Pause(); }
    }

    public void BackMenu4() { SceneManager.LoadScene("4Menu"); }
}