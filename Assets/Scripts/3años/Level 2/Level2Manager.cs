using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class Level2Manager : MonoBehaviour
{
    [SerializeField] private Button[] letrasBotones;
    [SerializeField] private Button[] dibujosBotones;
    [SerializeField] private GameObject _panelPuntaje;
    [SerializeField] private int cantidadALetrasSeleccionar = 4; // Cantidad de letras a seleccionar
    [SerializeField] private int cantidadDibujosSeleccionar = 4; // Cantidad de letras a seleccionar
    [SerializeField] private AudioClip correctoClip;
    [SerializeField] private AudioClip incorrectoClip; 
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource musicSource;
    private bool isMusicOn = true;

    private Button[] letrasSeleccionadas;
    private Button[] DibujosSeleccionados;
    private Button letraSeleccionada;
    private Button dibujoSeleccionado;

    private int puntajeTotal = 0; // Variable para llevar el puntaje total
    private int aciertos = 0; // Contador de aciertos
    private GameObject panelPuntaje; // Panel de puntaje

    private Dictionary<string, List<string>> letraImagenDict = new Dictionary<string, List<string>>()
    {
        { "A", new List<string> { "Arbol", "Avion" } },
        { "B", new List<string> { "Barco","Ballena" } },
        { "C", new List<string> { "Camion", "Caballo" } },
        { "D", new List<string> { "Dado", "Delfin" } },
        { "E", new List<string> { "Estrella","Elefante" } },
        // Agrega más letras e imágenes aquí
    };

    void Start()
    {
        _panelPuntaje.SetActive(false);
        letrasSeleccionadas = SeleccionarBotonesAleatorios(letrasBotones, cantidadALetrasSeleccionar);
        ColocarBotonesEnCuadrado(letrasSeleccionadas, new Vector2(0, 0), 100f);
        
        // Activar y asignar listeners a los botones de letras
        foreach (Button letra in letrasSeleccionadas)
        {
            letra.gameObject.SetActive(true); // Activar los botones seleccionados
            letra.onClick.AddListener(() => SeleccionarBoton(letra, true));
        }

        // Colocar botones de dibujo en el cuadrado, seleccionando solo los correspondientes a las letras elegidas
        ColocarBotonesDibujos();
    }

    void ColocarBotonesDibujos()
    {
        List<Button> dibujosSeleccionados = new List<Button>();

        foreach (Button letra in letrasSeleccionadas)
        {
            string letraTexto = letra.GetComponentInChildren<TextMeshProUGUI>().text;

            if (letraImagenDict.ContainsKey(letraTexto))
            {
                List<string> imagenesCoincidentes = letraImagenDict[letraTexto];
                
                // Escoger un dibujo aleatorio de la lista de imágenes
                string dibujoTexto = imagenesCoincidentes[Random.Range(0, imagenesCoincidentes.Count)];

                // Buscar el botón de dibujo correspondiente
                Button dibujoSeleccionado = dibujosBotones.FirstOrDefault(d => d.GetComponentInChildren<TextMeshProUGUI>().text == dibujoTexto);
                if (dibujoSeleccionado != null)
                {
                    dibujosSeleccionados.Add(dibujoSeleccionado);
                }
            }
        }

        // Comprobar si todos los botones de dibujos seleccionados son válidos
        if (dibujosSeleccionados.Count > 0)
        {
            ColocarBotonesEnCuadrado(dibujosSeleccionados.ToArray(), new Vector2(0, -300f), 100f);
            // Activar y asignar listeners a los botones de dibujos
            foreach (Button dibujo in dibujosSeleccionados)
            {
                dibujo.gameObject.SetActive(true); // Activar los botones seleccionados
                dibujo.onClick.AddListener(() => SeleccionarBoton(dibujo, false));
            }
        }
        else
        {
            Debug.LogError("No se encontraron botones de dibujo correspondientes a las letras seleccionadas.");
        }
    }

    void ColocarBotonesEnCuadrado(Button[] botones, Vector2 centro, float distancia)
    {
        // Coordenadas relativas para formar un cuadrado
        Vector2[] posiciones = new Vector2[] {
            new Vector2(-distancia, distancia), 
            new Vector2(distancia, distancia),   
            new Vector2(-distancia, -distancia), 
            new Vector2(distancia, -distancia)  
        };

        for (int i = 0; i < botones.Length; i++)
        {
            // Asignar la posición
            RectTransform botonTransform = botones[i].GetComponent<RectTransform>();
            botonTransform.anchoredPosition = centro + posiciones[i];
        }
    }

    // Función para seleccionar botones aleatorios de un array
    private Button[] SeleccionarBotonesAleatorios(Button[] botones, int cantidad)
    {
        List<Button> botonesDisponibles = new List<Button>(botones);
        List<Button> seleccionados = new List<Button>();

        for (int i = 0; i < cantidad; i++)
        {
            int indiceAleatorio = Random.Range(0, botonesDisponibles.Count);
            seleccionados.Add(botonesDisponibles[indiceAleatorio]);
            botonesDisponibles.RemoveAt(indiceAleatorio);
        }

        return seleccionados.ToArray();
    }

    private void SeleccionarBoton(Button boton, bool esLetra)
    {
        AudioSource botonAudioSource = boton.GetComponent<AudioSource>();

        if (botonAudioSource != null)
        {
            botonAudioSource.Play();
        }

        if (esLetra)
        {
            letraSeleccionada = boton;
            Debug.Log("Letra seleccionada: " + letraSeleccionada.name);
        }
        else
        {
            dibujoSeleccionado = boton;
            Debug.Log("Dibujo seleccionado: " + dibujoSeleccionado.name);
        }

        if (letraSeleccionada != null && dibujoSeleccionado != null)
        {
            VerificarCoincidencia();
        }
    }

    private void VerificarCoincidencia()
    {
        string letraTexto = letraSeleccionada.GetComponentInChildren<TextMeshProUGUI>().text;
        string dibujoTexto = dibujoSeleccionado.GetComponentInChildren<TextMeshProUGUI>().text;

        if (letraImagenDict.ContainsKey(letraTexto) && letraImagenDict[letraTexto].Contains(dibujoTexto))
        {
            // Coincidencia correcta
            audioSource.PlayOneShot(correctoClip);
            Debug.Log("Coincidencia correcta entre: " + letraTexto + " y " + dibujoTexto);
            
            letraSeleccionada.interactable = false;
            dibujoSeleccionado.interactable = false;

            // Sumar puntaje
            puntajeTotal += 10;
            aciertos++;

            // Verificar si se han acertado todas las letras
            if (aciertos >= cantidadALetrasSeleccionar)
            {
                MostrarPanelPuntaje();
            }
        }
        else
        {
            // Coincidencia incorrecta
            audioSource.PlayOneShot(incorrectoClip);
            Debug.Log("Coincidencia incorrecta entre: " + letraTexto + " y " + dibujoTexto);
        }

        // Resetear las selecciones
        letraSeleccionada = null;
        dibujoSeleccionado = null;
    }

    private void MostrarPanelPuntaje()
    {
        // Activa el panel de puntaje
        _panelPuntaje.SetActive(true);

        // Busca el componente TextMeshProUGUI dentro del panel para actualizar el puntaje
        TextMeshProUGUI puntajeTexto = _panelPuntaje.GetComponentInChildren<TextMeshProUGUI>();

        if (puntajeTexto != null)
        {
            // Actualiza el puntaje total en el panel de puntaje
            puntajeTexto.text = "Puntaje Total: " + puntajeTotal.ToString();
        }
        else
        {
            Debug.LogError("No se encontró el TextMeshProUGUI en el panel de puntaje.");
        }
    }

    public void BackMenu() { SceneManager.LoadScene("MainMenu"); }
    public void Replay() { SceneManager.LoadScene("Nivel 2"); }
    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        if (isMusicOn){ musicSource.Play(); }
        else {musicSource.Pause(); }
    }
}