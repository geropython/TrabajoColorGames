using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Words1Script : MonoBehaviour
{
    // Variables
    [SerializeField] private List<string> words; // Lista de palabras
    [SerializeField] private List<Sprite> images; // Lista de imágenes correspondientes
    [SerializeField] private Image imageDisplay; // Componente Image para mostrar la imagen
    [SerializeField] private TMP_InputField inputField; // InputField para ingresar la palabra completa
    [SerializeField] private TextMeshProUGUI scoreText; // Texto para mostrar la puntuación
    [SerializeField] private TextMeshProUGUI rondaActual; // Texto para mostrar la ronda actual
    [SerializeField] private TextMeshProUGUI rondasTotales; // Texto para mostrar el total de rondas
    [SerializeField] private GameObject endPanel; // Panel final que se activa después de las 5 rondas
    [SerializeField] private GameObject correctFeedback; // GameObject para feedback de respuesta correcta
    private int currentRound = 0; // Ronda actual
    private int score = 0; // Puntuación total

    void Start()
    {
        // Iniciar el juego
        endPanel.SetActive(false);
        rondasTotales.text = "Total Rondas: 5"; // Mostrar total de rondas
        StartRound();
    }

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
        string selectedWord = words[randomIndex];
        string firstLetter = selectedWord.Substring(0, 1); // Obtener la primera letra de la palabra

        // Mostrar la imagen y la primera letra
        imageDisplay.sprite = images[randomIndex];
        inputField.text = ""; // Limpiar el InputField
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = $"Completa la palabra: {firstLetter}____"; // Mostrar la primera letra

        // Actualizar la ronda actual
        rondaActual.text = $"Ronda: {currentRound + 1} / 5"; // Mostrar la ronda actual y el total de rondas
    }

    public void CheckAnswer()
    {
        // Comprobar la respuesta
        string correctWord = words[currentRound]; // Obtener la palabra correcta de la lista
        if (inputField.text.Equals(correctWord, System.StringComparison.OrdinalIgnoreCase))
        {
            score += 10; // Sumar puntos
            correctFeedback.SetActive(true); // Activar el feedback correcto
            StartCoroutine(FeedbackCoroutine()); // Iniciar la coroutine para desactivarlo después de 2 segundos
        }
        else
        {
            score -= 5; // Restar puntos
        }

        currentRound++; // Incrementar la ronda
        scoreText.text = "Puntos: " + score; // Actualizar la puntuación
        StartRound(); // Iniciar la siguiente ronda
    }

    private IEnumerator FeedbackCoroutine()
    {
        yield return new WaitForSeconds(2); // Esperar 2 segundos
        correctFeedback.SetActive(false); // Desactivar el feedback correcto
        StartRound(); // Iniciar la siguiente ronda
    }

    void EndGame()
    {
        // Mostrar el panel final y la puntuación final
        endPanel.SetActive(true);
        endPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Puntuación Final: " + score; // Mostrar puntuación final
    }
}