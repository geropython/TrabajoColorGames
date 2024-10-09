using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Word2Script : MonoBehaviour
{
    [SerializeField] private string[] palabras;
    [SerializeField] private AudioClip[] audios;
    [SerializeField] private CanvasGroup fadePanelCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private TextMeshProUGUI puntosTexto;
    [SerializeField] private TextMeshProUGUI tiempoTexto;
    [SerializeField] private GameObject panelFinal;
    [SerializeField] private TextMeshProUGUI puntosFinalTexto;
    [SerializeField] private TextMeshProUGUI tiempoFinalTexto;
    [SerializeField] private TextMeshProUGUI recordTexto;
    [SerializeField] private TextMeshProUGUI palabraDisplay;
    [SerializeField] private Transform letrasContainer;
    [SerializeField] private GameObject letraButtonPrefab;

    // Nuevo GameObject para activar
    [SerializeField] private GameObject feedbackObject; // GameObject que se activa al acierto

    private int rondaActual = 0;
    private int puntos = 0;
    private float tiempoRonda = 0;
    private float tiempoTotal = 0;
    private string palabraActual;
    private bool juegoTerminado = false;
    private bool audioReproduciendose = false; // Controla si el audio se está reproduciendo
    private AudioSource audioSource; 
    private List<string> letrasDisponibles = new List<string>();
    private List<string> letrasSeleccionadas = new List<string>();

    private void Start()
    {
        StartFade();
        audioSource = GetComponent<AudioSource>();
        panelFinal.SetActive(false);
        feedbackObject.SetActive(false); // Asegúrate de que el feedbackObject esté desactivado al inicio
        IniciarRonda();
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

    private void Update()
    {
        if (!juegoTerminado)
        {
            tiempoRonda += Time.deltaTime;
            tiempoTotal += Time.deltaTime;
            tiempoTexto.text = "Tiempo: " + tiempoTotal.ToString("F2") + " s";
        }
    }

    private void IniciarRonda()
    {
        if (rondaActual < 5)
        {
            palabraActual = palabras[rondaActual];
            letrasDisponibles.Clear();
            letrasSeleccionadas.Clear();
            tiempoRonda = 0f;
            audioSource.clip = audios[rondaActual];
            audioReproduciendose = false;

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
        if (letrasDisponibles.Contains(letra) && !audioReproduciendose) // Solo permite seleccionar letras válidas
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
        if (palabraFormada.Equals(palabraActual) && !audioReproduciendose)
        {
            puntos += 10;
            puntosTexto.text = "Puntos: " + puntos;

            // Reproducir el audio solo cuando se acierta
            ReproducirAudio();
            StartCoroutine(EsperarFinDeAudio());

            // Activar el feedbackObject
            feedbackObject.SetActive(true); // Activa el GameObject de feedback
            StartCoroutine(DesactivarFeedback()); // Inicia la coroutine para desactivarlo después de 2 segundos
        }
        else if (!palabraFormada.Equals(palabraActual))
        {
            Debug.Log("Palabra incorrecta o audio aún reproduciéndose.");
        }
    }

    // Nueva función para ser asignada al botón en la escena
    public void ReproducirAudio()
    {
        if (audioSource != null && audioSource.clip != null && !audioReproduciendose)
        {
            audioReproduciendose = true; // Marca que se está reproduciendo audio
            audioSource.Play();
        }
    }

    private IEnumerator EsperarFinDeAudio()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audioReproduciendose = false; // Permitir nuevamente la interacción una vez que el audio ha terminado
        rondaActual++;
        IniciarRonda();
    }

    private IEnumerator DesactivarFeedback()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos
        feedbackObject.SetActive(false); // Desactiva el GameObject de feedback
    }

    private void FinalizarJuego()
    {
        juegoTerminado = true;
        panelFinal.SetActive(true);
        puntosFinalTexto.text = "Puntos Finales: " + puntos;
        tiempoFinalTexto.text = "Tiempo Total: " + tiempoTotal.ToString("F2") + " s";

        // Guardar y mostrar el record
        float record = PlayerPrefs.GetFloat("Record", float.MaxValue);
        if (tiempoTotal < record)
        {
            PlayerPrefs.SetFloat("Record", tiempoTotal);
            record = tiempoTotal;
        }
        recordTexto.text = "Record: " + record.ToString("F2") + " s";
    }
}