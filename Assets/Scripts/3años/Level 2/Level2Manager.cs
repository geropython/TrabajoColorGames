using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level2Manager : MonoBehaviour
{
    #region Variables
    public TextMeshProUGUI letraText; // Texto donde se muestra la letra correcta
    public Button[] botones; // Array de botones para letras
    [SerializeField] private GameObject _objetoPoints;
    [SerializeField] private GameObject _panelPuntaje;

    private int puntajeTotal = 0; // Puntaje total del jugador
    private int aciertos = 0; // Contador de aciertos
    private int errores = 0;  // Contador de errores
    private char letraCorrecta; // Letra correcta seleccionada

    [SerializeField] private GameObject soundObject; 
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip incorrectSound;
    private AudioSource audioSource;
    public AudioSource musicSource; 
    private bool isMusicOn = true;

    // Diccionario que asocia letras a palabras y dibujos
    private Dictionary<char, string> letrasPalabras = new Dictionary<char, string>()
    {
        { 'A', "Avión" },
        { 'B', "Barco" },
        { 'C', "Camión" },
        { 'D', "Delfín" }
    };

    // Lista de los sprites asociados a cada letra
    public Sprite[] dibujos; // Arrastra tus imágenes aquí en el inspector

    #endregion

    void Start()
    {
        _panelPuntaje.SetActive(false);
        audioSource = soundObject.GetComponent<AudioSource>();
        AsignarLetrasABotones(); // Inicializa el juego
    }

    void AsignarLetrasABotones()
    {
        // Seleccionar una letra correcta aleatoriamente de las disponibles
        List<char> letrasDisponibles = new List<char>(letrasPalabras.Keys);
        letraCorrecta = letrasDisponibles[Random.Range(0, letrasDisponibles.Count)];
        letraText.text = letraCorrecta.ToString(); // Mostrar la letra en pantalla

        // Crear una lista temporal de letras para los botones
        List<char> letrasBotones = new List<char>(letrasDisponibles);
        letrasBotones.Remove(letraCorrecta); // Quitar la letra correcta

        // Mezclar las letras y elegir un botón para la letra correcta
        letrasBotones = MezclarLista(letrasBotones);
        int botonCorrecto = Random.Range(0, botones.Length); // Elegir un botón aleatoriamente para la letra correcta

        for (int i = 0; i < botones.Length; i++)
        {
            if (i == botonCorrecto)
            {
                // Asignar la palabra correcta al botón (por ejemplo, "Avión")
                botones[i].GetComponentInChildren<TextMeshProUGUI>().text = letrasPalabras[letraCorrecta]; // Mostrar el nombre de la palabra
                botones[i].onClick.RemoveAllListeners();
                botones[i].onClick.AddListener(() => BotonCorrecto(letraCorrecta)); // Pasar la letra correcta

                // Cambiar el sprite correspondiente a la letra correcta
                botones[i].GetComponent<Image>().sprite = GetSpriteForLetter(letraCorrecta);
            }
            else
            {
                // Asignar una letra incorrecta aleatoria
                char letraIncorrecta = letrasBotones[i % letrasBotones.Count]; // Asegurarse de no exceder el tamaño de la lista
                botones[i].GetComponentInChildren<TextMeshProUGUI>().text = letrasPalabras[letraIncorrecta]; // Mostrar el nombre de la palabra
                botones[i].onClick.RemoveAllListeners();
                botones[i].onClick.AddListener(() => BotonIncorrecto());

                // Cambiar el sprite correspondiente a la letra incorrecta
                botones[i].GetComponent<Image>().sprite = GetSpriteForLetter(letraIncorrecta);
            }
        }
    }

    private Sprite GetSpriteForLetter(char letra)
    {
        switch (letra)
        {
            case 'A': return dibujos[0]; // Avión
            case 'B': return dibujos[1]; // Barco
            case 'C': return dibujos[2]; // Camión
            case 'D': return dibujos[3]; // Delfín
            default: return null; // Retornar null si no se encuentra la letra
        }
    }

    private void MostrarPanelPuntaje()
    {
        _panelPuntaje.SetActive(true);
        TextMeshProUGUI[] textosPanel = _panelPuntaje.GetComponentsInChildren<TextMeshProUGUI>();

        if (textosPanel.Length >= 2)
        {
            textosPanel[0].text = "Puntaje Total: " + puntajeTotal.ToString();
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
        if (letra == letraCorrecta) // Verificar que la letra corresponde
        {
            puntajeTotal += 10; // Sumar puntaje y aciertos
            aciertos++;

            if (aciertos >= 5) // Cambiado a 5 para finalizar el juego
            {
                MostrarPanelPuntaje();
                DesactivarBotones(); // Desactiva los botones
            }
            else
            {
                StartCoroutine(MostrarObjetoTemporalmente(_objetoPoints, 0.5f));
                audioSource.PlayOneShot(correctSound);
                AsignarLetrasABotones(); // Reiniciar letras y botones
            }
        }
    }

    void BotonIncorrecto()
    {
        errores++; // Incrementar errores
        Debug.Log("Incorrecto, intenta de nuevo.");
        audioSource.PlayOneShot(incorrectSound);
    }

    private void DesactivarBotones()
    {
        foreach (Button boton in botones)
        {
            boton.interactable = false; // Desactivar todos los botones
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

    public void BackMenu() { SceneManager.LoadScene("MainMenu"); }
    public void Replay() { SceneManager.LoadScene("Level 1"); }

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