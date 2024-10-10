using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Word2Script : MonoBehaviour
{
    #region Variables
    [SerializeField] private string[] palabras;
    [SerializeField] private string[] pistas; 
    [SerializeField] private CanvasGroup fadePanelCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private TextMeshProUGUI puntosTexto;
    [SerializeField] private TextMeshProUGUI tiempoTexto;
    [SerializeField] private GameObject panelFinal;
    [SerializeField] private TextMeshProUGUI puntosFinalTexto;
    [SerializeField] private TextMeshProUGUI tiempoFinalTexto;
    [SerializeField] private TextMeshProUGUI recordTexto;
    [SerializeField] private TextMeshProUGUI palabraDisplay;
    [SerializeField] private TextMeshProUGUI pistaTexto;
    [SerializeField] private TextMeshProUGUI testTimerText;
    private float testTimer = 0f;
    [SerializeField] private Transform letrasContainer;
    [SerializeField] private GameObject letraButtonPrefab;
    [SerializeField] private Button pistaButton;
    [SerializeField] private GameObject feedbackObject;

    private int rondaActual = 0;
    private int puntos = 0;
    private float tiempoRonda = 0;
    private float tiempoTotal = 0;
    private string palabraActual;
    private bool juegoTerminado = false;
    [SerializeField] private AudioSource musicSource;
    private bool isMusicOn = true;
    private List<string> letrasDisponibles = new List<string>();
    private List<string> letrasSeleccionadas = new List<string>();

    #endregion

    private void Start()
    {
        pistaTexto.gameObject.SetActive(false);
        pistaButton.onClick.AddListener(MostrarPista);

        StartFade();

        panelFinal.SetActive(false);
        feedbackObject.SetActive(false); // Asegúrate de que el feedbackObject esté desactivado al inicio

        IniciarRonda();
    }

    private void Update()
    {
        testTimer += Time.deltaTime;
        testTimerText.text = "Tiempo: " + testTimer.ToString("F2") + " s";
        if (!juegoTerminado)
        {
            tiempoRonda += Time.deltaTime;
            tiempoTotal += Time.deltaTime;
        }
    }

    #region Fade
    void StartFade() { StartCoroutine(FadePanel()); }
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

    #region Pista
    private void MostrarPista()
    {
        pistaTexto.text = pistas[rondaActual]; // Mostrar la pista correspondiente a la palabra actual
        pistaTexto.gameObject.SetActive(true); // Activar el texto de la pista
        StartCoroutine(EsconderPista()); // Esconder la pista después de 10 segundos
    }

    private IEnumerator EsconderPista()
    {
        yield return new WaitForSeconds(10f); // Esperar 10 segundos
        pistaTexto.gameObject.SetActive(false); // Ocultar el texto de la pista
    }
    #endregion

    private void IniciarRonda()
    {
        if (rondaActual < palabras.Length) // Si quedan rondas por jugar
        {
            palabraActual = palabras[rondaActual];
            letrasDisponibles.Clear();
            letrasSeleccionadas.Clear();
            tiempoRonda = 0f; // Reiniciar el tiempo de la ronda actual

            // Desordenar y agregar las letras de la palabra actual
            letrasDisponibles = DesordenarLetras(palabraActual);

            // Actualizar visualización de las letras
            ActualizarVisualizacionLetras();
            ActualizarPalabraDisplay();
        }
        else
        {
            FinalizarJuego();
        }
    }

    // Función para desordenar las letras
    private List<string> DesordenarLetras(string palabra)
    {
        List<string> letras = palabra.ToCharArray().Select(c => c.ToString()).ToList();
        for (int i = letras.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            string temp = letras[i];
            letras[i] = letras[randomIndex];
            letras[randomIndex] = temp;
        }
        return letras;
    }

    // Actualizar la visualización de las letras en botones
    private void ActualizarVisualizacionLetras()
    {
        foreach (Transform child in letrasContainer)
        {
            Destroy(child.gameObject); // Limpiar letras anteriores
        }

        // Crear un botón por cada letra disponible
        foreach (string letra in letrasDisponibles)
        {
            GameObject newButton = Instantiate(letraButtonPrefab, letrasContainer);
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = letra;

            // Asignar evento al botón
            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(() => SeleccionarLetra(letra, newButton));
        }
    }

    // Cuando una letra es seleccionada, se agrega a la palabra en construcción
    private void SeleccionarLetra(string letra, GameObject button)
    {
        if (letrasDisponibles.Contains(letra)) // Solo permite seleccionar letras válidas
        {
            letrasSeleccionadas.Add(letra);
            letrasDisponibles.Remove(letra);
            button.SetActive(false); // Desactivar el botón cuando se selecciona la letra
            ActualizarPalabraDisplay();
        }
    }

    private void ActualizarPalabraDisplay()
    {
        palabraDisplay.text = string.Join("", letrasSeleccionadas);
    }

    public void ComprobarPalabra()
    {
       string palabraFormada = string.Join("", letrasSeleccionadas);
        if (palabraFormada.Equals(palabraActual))
        {
            puntos += 10;
            puntosTexto.text = "Puntos: " + puntos;

            rondaActual++; // Avanzar a la siguiente ronda
            IniciarRonda(); // Iniciar la siguiente ronda

            // Activar el feedbackObject
            feedbackObject.SetActive(true); // Activa el GameObject de feedback
            StartCoroutine(DesactivarFeedback()); // Inicia la coroutine para desactivarlo después de 2 segundos
        }
        else
        {
            Debug.Log("Palabra incorrecta.");

            // Restablecer letras seleccionadas y disponibles
            RestablecerLetras();
        }
    }
    private void RestablecerLetras()
    {
        // Volver a agregar las letras seleccionadas a letras disponibles
        letrasDisponibles.AddRange(letrasSeleccionadas); // Agregar letras seleccionadas de vuelta
        letrasSeleccionadas.Clear(); // Limpiar las letras seleccionadas

        // Actualizar la visualización de las letras en los botones
        ActualizarVisualizacionLetras(); 
        ActualizarPalabraDisplay(); // Actualizar el display de la palabra
    }

    private IEnumerator DesactivarFeedback()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos
        feedbackObject.SetActive(false); // Desactiva el GameObject de feedback
    }

    private void FinalizarJuego()
    {
        juegoTerminado = true; // Marcar el juego como terminado
        panelFinal.SetActive(true); // Mostrar el panel final
        puntosFinalTexto.text = "Puntos Finales: " + puntos;
        tiempoFinalTexto.text = "Tiempo Total: " + tiempoTotal.ToString("F2") + " s";

        // Guardar y mostrar el récord
        float record = PlayerPrefs.GetFloat("Record", float.MaxValue);
        if (tiempoTotal < record)
        {
            PlayerPrefs.SetFloat("Record", tiempoTotal);
            record = tiempoTotal;
        }
        recordTexto.text = "Record: " + record.ToString("F2") + " s";
    }

    public void BackMenu() { SceneManager.LoadScene("4Menu"); }
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