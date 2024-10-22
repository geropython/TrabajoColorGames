using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para trabajar con imágenes de UI
using TMPro; // Para manejar textos si usas TextMeshPro

public class CarCreatorManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private CanvasGroup fadePanelCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource correctSound;
    [SerializeField] private AudioSource incorrectSound;
    private bool isMusicOn = true;

    [SerializeField] private TextMeshProUGUI objectivesText; // Texto para mostrar los aciertos
    [SerializeField] private GameObject car;
    [Header("Main Car")]
    public List<GameObject> wheelOptions;
    public List<GameObject> windowOptions;
    public List<GameObject> bodyColorDisplays;

    [SerializeField] private GameObject correctFeedback;

    [SerializeField] private GameObject[] wheelDisplays;
    [SerializeField] private GameObject[] windowDisplays;

    // Objetivos
    [Header("Objetivo")]
    [SerializeField] private GameObject[] objectiveBodyDisplays;
    [SerializeField] private GameObject[] objectiveWindowDisplays;
    [SerializeField] private GameObject[] objectiveWheelDisplays;

    // UI para el puntaje
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI scoreText, errorsText, recordText;

    private int currentWheelIndex = 0;
    private int currentWindowIndex = 0;
    private int currentBodyColorIndex = 0;

    private GameObject currentWheels;
    private GameObject currentWindows;

    private int targetWheelIndex;
    private int targetWindowIndex;
    private int targetBodyColorIndex;

    private int score = 0;
    private int errors = 0;
    private int correctObjectives = 0;
    private int maxCorrectObjectives = 3; // Máximo de objetivos correctos para terminar el nivel
    private int previousRecord = 0;

    private bool wheelTouched = false;
    private bool windowTouched = false;

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

    void Start()
    {
        StartFade();
        InitializeCar();
        UpdateWheels();
        UpdateWindows();
        GenerateRandomObjective();  // Generar un objetivo al iniciar el nivel
    }

    void InitializeCar() 
    {
        car = Instantiate(car); 
    }
    private IEnumerator ShowCorrectFeedback()
    {
        correctFeedback.SetActive(true);

        yield return new WaitForSeconds(2f);

        correctFeedback.SetActive(false);
    }

    #region Select
    public void SelectWheels(int index)
    {
        if (index >= 0 && index < wheelOptions.Count)
        {
            currentWheelIndex = index;
            UpdateWheels();
            wheelTouched = true;
            CheckRecentlyTouched();
        }
    }

    public void SelectWindows(int index)
    {
        if (index >= 0 && index < windowOptions.Count)
        {
            currentWindowIndex = index;
            UpdateWindows();
            windowTouched = true;
            CheckRecentlyTouched();
        }
    }
    #endregion

    #region Change
    void UpdateWheels()
    {
        if (currentWheels != null)
        {
            Destroy(currentWheels);
        }
        currentWheels = Instantiate(wheelOptions[currentWheelIndex], car.transform);
        PositionWheels();
    }

    void UpdateWindows()
    {
        if (currentWindows != null)
        {
            Destroy(currentWindows);
        }
        currentWindows = Instantiate(windowOptions[currentWindowIndex], car.transform);
        PositionWindows();
    }

    public void ChangeBodyColor(int index)
    {
        foreach (var display in bodyColorDisplays)
        {
            display.SetActive(false);
        }

        bodyColorDisplays[index].SetActive(true);
        currentBodyColorIndex = index; // Guardar la selección de color
        CheckRecentlyTouched();
    }
    #endregion

    #region Position
    void PositionWheels() 
    { 
        currentWheels.transform.localPosition = new Vector3(0, -0.5f, 0); 
    }

    void PositionWindows() 
    { 
        currentWindows.transform.localPosition = new Vector3(0, 0.5f, 0); 
    }
    #endregion

    // Método para activar/desactivar componentes según los tocados
    void CheckRecentlyTouched()
    {
        foreach (var display in wheelDisplays)
        {
            display.SetActive(false); 
        }
        if (wheelTouched)
        {
            wheelDisplays[currentWheelIndex].SetActive(true); 
        }

        foreach (var display in windowDisplays)
        {
            display.SetActive(false); 
        }
        if (windowTouched)
        {
            windowDisplays[currentWindowIndex].SetActive(true); 
        }
    }

    #region Objective and Scoring

    void GenerateRandomObjective()
    {
        targetWheelIndex = Random.Range(0, wheelOptions.Count);
        targetWindowIndex = Random.Range(0, windowOptions.Count);
        targetBodyColorIndex = Random.Range(0, bodyColorDisplays.Count);

        ActivateObjectiveDisplays(targetWheelIndex, targetWindowIndex, targetBodyColorIndex);
    }

    void ActivateObjectiveDisplays(int wheelIndex, int windowIndex, int bodyColorIndex)
    {
        foreach (var display in objectiveBodyDisplays)
        {
            display.SetActive(false);
        }
        objectiveBodyDisplays[bodyColorIndex].SetActive(true);

        foreach (var display in objectiveWindowDisplays)
        {
            display.SetActive(false);
        }
        objectiveWindowDisplays[windowIndex].SetActive(true);

        foreach (var display in objectiveWheelDisplays)
        {
            display.SetActive(false);
        }
        objectiveWheelDisplays[wheelIndex].SetActive(true);
    }

    public void CheckPlayerSelections()
    {
        Debug.Log("Color cuerpo jugador: " + currentBodyColorIndex);
        Debug.Log("Color cuerpo objetivo: " + targetBodyColorIndex);
        bool isCorrect = (currentWheelIndex == targetWheelIndex) && (currentWindowIndex == targetWindowIndex) && (currentBodyColorIndex == targetBodyColorIndex);

        if (isCorrect)
        {
            Debug.Log("Correct selection!");
            StartCoroutine(ShowCorrectFeedback());
            score += 50;
            correctSound.Play();
            correctObjectives++;
        }
        else
        {
            Debug.Log("Incorrect selection.");
            score -= 10;
            incorrectSound.Play();
            errors++;
        }

        scoreText.text = "Puntaje: " + score.ToString();
        objectivesText.text = correctObjectives + "/" + maxCorrectObjectives;

        if (correctObjectives >= maxCorrectObjectives)
        {
            EndLevel();
        }
        else
        {
            GenerateRandomObjective();
        }
    }

    void EndLevel()
    {
        endPanel.SetActive(true);
        scoreText.text = "Puntaje final: " + score.ToString();
        errorsText.text = "Errores: " + errors.ToString();

        if (score > previousRecord)
        {
            previousRecord = score;
            recordText.text = "¡Nuevo récord!";
        }
        else
        {
            recordText.text = "Récord: " + previousRecord.ToString();
        }
    }
    #endregion
    public void BackMenu()
    {
        SceneManager.LoadScene("4Menu");
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
}