using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LetraDibujo
{
    public string nombre;  // El nombre de la letra o palabra
    public Sprite imagen;  // La imagen asociada a esa letra o palabra
}

public class Level2Manager : MonoBehaviour
{
    #region Variables
    public TextMeshProUGUI letraText; // Texto donde se muestra la letra correcta
    public Button[] botones; // Array de botones para letras
    [SerializeField] private GameObject _objetoPoints;
    [SerializeField] private GameObject _panelPuntaje;

    [SerializeField] private CanvasGroup fadePanelCanvasGroup; // CanvasGroup del fadePanel
    [SerializeField] private GameObject mainPanel; // Panel principal que se mostrará después del fade
    [SerializeField] private float fadeDuration = 1f;

    private int puntajeTotal = 0;
    private int aciertos = 0;
    private int errores = 0;
    private char letraCorrecta;

    public int totalOleadas = 10;
    private int oleadaActual = 1;
    public TextMeshProUGUI contadorOleadasText;

    [SerializeField] private GameObject soundObject; 
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip incorrectSound;
    private AudioSource audioSource;
    public AudioSource musicSource; 
    private bool isMusicOn = true;

    private Dictionary<char, string> letrasPalabras = new Dictionary<char, string>()
    {
        { 'A', "Avión" }, { 'B', "Barco" }, { 'C', "Caballo" }, { 'D', "Delfín" },
        { 'E', "Estrella" }, { 'F', "Flamenco" }, { 'G', "Gato" }, { 'H', "Hielo" },
        { 'I', "Iguana" },{ 'J', "Jaguar" },{ 'K', "Koala" },{ 'L', "Leon" },
        { 'M', "Medusa" },{ 'N', "Naranja" },{ 'Ñ', "Ñandu" },{ 'O', "Oso" },
        { 'P', "Pato" },{ 'Q', "Queso" },{ 'R', "Rana" },{ 'S', "Serpiente" },
        { 'U', "Uva" },{ 'V', "Vaca" },{ 'W', "Waffle" },
        { 'X', "Xilófono" }, { 'Y', "Yate" }, { 'Z', "Zorro" }
    };

    // Lista de los sprites asociados a cada letra (27 imágenes en total)
    public Sprite[] dibujos; // Arrastra tus imágenes aquí en el inspector
    #endregion

    void Start()
    {
        ActualizarContadorOleadas();
        mainPanel.SetActive(false);
        IniciarJuegoConFade();

        _panelPuntaje.SetActive(false);
        audioSource = soundObject.GetComponent<AudioSource>();

        AsignarLetrasABotones(); // Inicializa el juego
    }

    void ActualizarContadorOleadas()
    {
        contadorOleadasText.text = "Oleada: " + oleadaActual.ToString() + " / " + totalOleadas.ToString();
    }

    #region Fade Coroutines
    void IniciarJuegoConFade()
    {
        StartCoroutine(FadeInGamePanel());
    }

    private IEnumerator FadeInGamePanel()
    {
        mainPanel.SetActive(true);
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

    #region Letras
    void AsignarLetrasABotones()
    {
        // Suponiendo que tienes una lista de letras y una lista de dibujos
        List<char> letrasDisponibles = new List<char>(letrasPalabras.Keys);
        letraCorrecta = letrasDisponibles[Random.Range(0, letrasDisponibles.Count)];
        letraText.text = letraCorrecta.ToString();

        List<char> letrasBotones = new List<char>(letrasDisponibles);
        letrasBotones.Remove(letraCorrecta);
        letrasBotones = MezclarLista(letrasBotones);
        letrasBotones = letrasBotones.GetRange(0, 3); // Seleccionar 3 letras incorrectas
        letrasBotones.Add(letraCorrecta); // Añadir la correcta
        letrasBotones = MezclarLista(letrasBotones); // Mezclar todas las letras

        for (int i = 0; i < botones.Length; i++)
        {
            if (i < letrasBotones.Count) // Evitar que se salga del rango
            {
                char letra = letrasBotones[i];

                // Asignar el texto correcto del nombre
                botones[i].GetComponentInChildren<TextMeshProUGUI>().text = letrasPalabras[letra];
                
                // Asignar el sprite correcto usando el método GetSpriteForLetter
                botones[i].GetComponent<Image>().sprite = GetSpriteForLetter(letra);

                botones[i].onClick.RemoveAllListeners();
                if (letra == letraCorrecta)
                {
                    botones[i].onClick.AddListener(() => BotonCorrecto(letra));
                }
                else
                {
                    botones[i].onClick.AddListener(BotonIncorrecto);
                }
            }
        }
    }

    private Sprite GetSpriteForLetter(char letra)
    {
        // Aquí tenemos una lista de todas las letras en el mismo orden en el que están los sprites
        string letrasOrdenadas = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
        
        // Encontramos el índice de la letra en la cadena
        int index = letrasOrdenadas.IndexOf(letra);

        // Verificamos que el índice no esté fuera de los límites del array de dibujos
        if (index >= 0 && index < dibujos.Length)
        {
            return dibujos[index]; // Devolver el sprite correspondiente
        }
        else
        {
            Debug.LogError("Índice fuera de rango para la letra: " + letra);
            return null; // Retorna null si hay un error
        }
    }
    #endregion


    private void MostrarPanelPuntaje()
    {
        _panelPuntaje.SetActive(true);
        TextMeshProUGUI[] textosPanel = _panelPuntaje.GetComponentsInChildren<TextMeshProUGUI>();

        if (textosPanel.Length >= 2)
        {
            textosPanel[0].text = "Puntaje: " + puntajeTotal.ToString();
            textosPanel[1].text = "Errores: " + errores.ToString();
        }
        else
        {
            Debug.LogError("No se encontraron suficientes TextMeshProUGUI en el panel de puntaje.");
        }
    }

    #region Botones
    void BotonCorrecto(char letra)
    {
        if (letra == letraCorrecta)
        {
            puntajeTotal += 10;
            aciertos++;
            if (oleadaActual < totalOleadas)
            {
                oleadaActual++;
                ActualizarContadorOleadas();
                AsignarLetrasABotones();
            }
            else
            {
                MostrarPanelPuntaje();
                DesactivarBotones();
            }

            StartCoroutine(MostrarObjetoTemporalmente(_objetoPoints, 0.5f));
            audioSource.PlayOneShot(correctSound);
        }
    }

    void BotonIncorrecto()
    {
        errores++;
        audioSource.PlayOneShot(incorrectSound);
    }

    private void DesactivarBotones()
    {
        foreach (Button boton in botones)
        {
            boton.interactable = false;
        }
    }
    #endregion

    private IEnumerator MostrarObjetoTemporalmente(GameObject objeto, float duracion)
    {
        objeto.SetActive(true);
        yield return new WaitForSeconds(duracion);
        objeto.SetActive(false);
    }

    List<char> MezclarLista(List<char> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            int rand = Random.Range(i, lista.Count);
            char temp = lista[i];
            lista[i] = lista[rand];
            lista[rand] = temp;
        }
        return lista;
    }

    #region Extra
    public void BackMenu()
    {
        musicSource.Stop();
        SceneManager.LoadScene("3Menu");
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
    #endregion
}
