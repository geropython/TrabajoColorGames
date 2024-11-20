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
    [SerializeField] private Sprite[] pistasImagenes; // Arreglo de imágenes de pistas
    [SerializeField] private CanvasGroup fadePanelCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private TextMeshProUGUI puntosTexto;
    [SerializeField] private TextMeshProUGUI tiempoTexto;
    [SerializeField] private GameObject panelFinal;
    [SerializeField] private TextMeshProUGUI puntosFinalTexto;
    [SerializeField] private TextMeshProUGUI tiempoFinalTexto;
    [SerializeField] private TextMeshProUGUI recordTexto;
    [SerializeField] private TextMeshProUGUI palabraDisplay;
    [SerializeField] private Image pistaImagen; // Ahora es una imagen
    [SerializeField] private TextMeshProUGUI testTimerText;
    private float testTimer = 0f;
    [SerializeField] private Transform letrasContainer;
    [SerializeField] private TMP_FontAsset fuenteLetras;

    [SerializeField] private GameObject letraButtonPrefab;
    [SerializeField] private Button pistaButton;
    [SerializeField] private GameObject feedbackObject;
    [SerializeField] private TextMeshProUGUI rondaTexto;

    private int rondaActual = 0;
    private int puntos = 0;
    private float tiempoRonda = 0;
    private float tiempoTotal = 0;
    private string palabraActual;
    private bool juegoTerminado = false;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource correctSound;
    [SerializeField] private AudioSource incorrectSound;
    private bool isMusicOn = true;
    private List<string> letrasDisponibles = new List<string>();
    private List<string> letrasSeleccionadas = new List<string>();

    #endregion

    public void Start()
    {
        // Asegúrate de que el panel final y el cofre no estén activos al reiniciar el nivel
        panelFinal.SetActive(false);

        // Resetear la ronda y los puntos al iniciar el nivel
        rondaActual = 0;
        puntos = 0;
        tiempoRonda = 0f;
        tiempoTotal = 0f;
        juegoTerminado = false;
        
        feedbackObject.SetActive(false);
        pistaImagen.gameObject.SetActive(false); // Asegúrate de que la imagen esté oculta al inicio
        pistaButton.onClick.AddListener(MostrarPista);
        StartFade();
        IniciarRonda();
    }

    private void Update()
    {
        testTimer += Time.deltaTime;
        testTimerText.text = testTimer.ToString("F2");
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
        pistaImagen.sprite = pistasImagenes[rondaActual]; // Mostrar la imagen correspondiente a la palabra actual
        pistaImagen.gameObject.SetActive(true); // Activar la imagen de la pista
        StartCoroutine(EsconderPista()); // Esconder la imagen después de 10 segundos
    }

    private IEnumerator EsconderPista()
    {
        yield return new WaitForSeconds(5f); // Esperar 5 segundos
        pistaImagen.gameObject.SetActive(false); // Ocultar la imagen de la pista
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

            // Actualizar el texto de la ronda
            rondaTexto.text = " " + (rondaActual + 1) + "/" + palabras.Length;
        }
        else
        {
            FinalizarJuego();
        }
    }

    #region Letras
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

            // Asignar el texto de la letra
            buttonText.text = letra;

            // Asignar la fuente (Asegúrate de que 'fuenteLetras' esté asignada en el inspector)
            buttonText.font = fuenteLetras;

            // Ajustar el tamaño de la letra
            buttonText.fontSize = 125; // Ajusta el valor según el tamaño que desees

            // Asignar evento al botón
            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(() => SeleccionarLetra(letra, newButton));
        }
    }

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
    private void RestablecerLetras()
    {
        // Volver a agregar las letras seleccionadas a letras disponibles
        letrasDisponibles.AddRange(letrasSeleccionadas); // Agregar letras seleccionadas de vuelta
        letrasSeleccionadas.Clear(); // Limpiar las letras seleccionadas

        // Actualizar la visualización de las letras en los botones
        ActualizarVisualizacionLetras(); 
        ActualizarPalabraDisplay(); // Actualizar el display de la palabra
    }
    #endregion

    #region Palabra
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
            correctSound.Play();
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
            incorrectSound.Play();

            // Restablecer letras seleccionadas y disponibles
            RestablecerLetras();
        }
    }
    #endregion

  
    #region EndGame
    private IEnumerator DesactivarFeedback()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos
        feedbackObject.SetActive(false); // Desactiva el GameObject de feedback
    }

    private void FinalizarJuego()
    {
        juegoTerminado = true; // Marcar el juego como terminado
        panelFinal.SetActive(true); // Mostrar el panel final
        puntosFinalTexto.text = "Puntos: " + puntos;
        tiempoFinalTexto.text = "Tiempo: " + tiempoTotal.ToString("F2") + " s";

        // Guardar y mostrar el récord
        float record = PlayerPrefs.GetFloat("Record", float.MaxValue);
        if (tiempoTotal < record)
        {
            PlayerPrefs.SetFloat("Record", tiempoTotal);
            record = tiempoTotal;
        }
        recordTexto.text = "Record: " + record.ToString("F2") + " s";
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