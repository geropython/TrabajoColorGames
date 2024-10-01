using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level3Manager : MonoBehaviour
{
    // Referencias
    public TextMeshProUGUI letraText;
    public Button[] botones;
    [SerializeField] private GameObject _objetoPoints;
    [SerializeField] private GameObject _panelPuntaje;

    private int puntajeTotal = 0; // Variable para llevar el puntaje total
    private int aciertos = 0; // Contador de aciertos
    private int errores = 0;  // Contador de errores
    private char letraCorrecta;

    [SerializeField]private GameObject soundObject; 
    [SerializeField]private AudioClip correctSound;
    [SerializeField]private AudioClip incorrectSound;
    private AudioSource audioSource;
    public AudioSource musicSource; 
    private bool isMusicOn = true;
    private List<char> letrasDisponibles = new List<char>();

    void Start()
    {
        _panelPuntaje.SetActive(false);

        // Inicializa la lista de letras de la A a la Z
        for (char c = 'A'; c <= 'Z'; c++)
        {
            letrasDisponibles.Add(c);
        }
        // Obtener el componente AudioSource del objeto de sonido
        audioSource = soundObject.GetComponent<AudioSource>();

        AsignarLetrasABotones();
    }

    void AsignarLetrasABotones()
    {
        // Seleccionar una letra correcta aleatoriamente de la lista
        letraCorrecta = letrasDisponibles[Random.Range(0, letrasDisponibles.Count)];
        letraText.text = letraCorrecta.ToString(); // Mostrar la letra en pantalla

        // Crear una lista temporal de letras para los botones
        List<char> letrasBotones = new List<char>(letrasDisponibles);

        // Mezclar la lista para asignar letras aleatorias
        letrasBotones.Remove(letraCorrecta); // Quitar la letra correcta de la lista temporal
        letrasBotones = MezclarLista(letrasBotones); // Mezclar las letras restantes

        // Asignar letras a los botones
        int botonCorrecto = Random.Range(0, botones.Length); // Elegir un botón aleatoriamente para la letra correcta
        for (int i = 0; i < botones.Length; i++)
        {
            if (i == botonCorrecto)
            {
                // Asignar la letra correcta a este botón
                botones[i].GetComponentInChildren<TextMeshProUGUI>().text = letraCorrecta.ToString();
                botones[i].onClick.RemoveAllListeners();
                botones[i].onClick.AddListener(() => BotonCorrecto());
            }
            else
            {
                // Asignar una letra incorrecta aleatoria
                botones[i].GetComponentInChildren<TextMeshProUGUI>().text = letrasBotones[i].ToString();
                botones[i].onClick.RemoveAllListeners();
                botones[i].onClick.AddListener(() => BotonIncorrecto());
            }
        }
    }

    private void MostrarPanelPuntaje()
    {
        // Activa el panel de puntaje
        _panelPuntaje.SetActive(true);

        // Busca los componentes TextMeshProUGUI dentro del panel para actualizar el puntaje y errores
        TextMeshProUGUI[] textosPanel = _panelPuntaje.GetComponentsInChildren<TextMeshProUGUI>();

        if (textosPanel.Length >= 2)
        {
            // Actualiza el puntaje total y los errores en el panel de puntaje
            textosPanel[0].text = "Puntaje Total: " + puntajeTotal.ToString();
            textosPanel[1].text = "Errores: " + errores.ToString();
        }
        else
        {
            Debug.LogError("No se encontraron suficientes TextMeshProUGUI en el panel de puntaje.");
        }
    }

    #region Botones
    void BotonCorrecto()
    {
        // Sumar puntaje y aciertos
        puntajeTotal += 10;
        aciertos++;

        // Comprobar si se ha alcanzado el número de aciertos para finalizar el juego
        if (aciertos >= 10)
        {
            // Mostrar el panel de puntaje y detener el juego
            MostrarPanelPuntaje();
            DesactivarBotones(); // Desactiva los botones para que no se puedan hacer más clics
        }
        else
        {
            // Activar el objeto temporalmente por 2 segundos
            StartCoroutine(MostrarObjetoTemporalmente(_objetoPoints, 0.5f));
            audioSource.PlayOneShot(correctSound);

            // Cambiar las letras y volver a empezar
            AsignarLetrasABotones(); 
        }
    }

    void BotonIncorrecto()
    {
        // Incrementar los errores cuando se selecciona una opción incorrecta
        errores++;
        
        // Acción para el botón incorrecto
        Debug.Log("Incorrecto, intenta de nuevo.");
        audioSource.PlayOneShot(incorrectSound);
    }

    private void DesactivarBotones()
    {
        // Desactivar todos los botones para que no puedan ser presionados
        foreach (Button boton in botones)
        {
            boton.interactable = false;
        }
    }
    #endregion

    private IEnumerator MostrarObjetoTemporalmente(GameObject objeto, float duracion)
    {
        // Activar el objeto
        objeto.SetActive(true);

        // Esperar por el tiempo definido
        yield return new WaitForSeconds(duracion);

        // Desactivar el objeto
        objeto.SetActive(false);
    }

    List<char> MezclarLista(List<char> lista)
    {
        // Mezcla simple de la lista de letras usando el algoritmo Fisher-Yates
        for (int i = 0; i < lista.Count; i++)
        {
            int rand = Random.Range(i, lista.Count);
            char temp = lista[i];
            lista[i] = lista[rand];
            lista[rand] = temp;
        }
        return lista;
    }

    public void BackMenu(){ SceneManager.LoadScene("MainMenu"); }
    public void Replay(){ SceneManager.LoadScene("Level 2"); }

    public void ToggleMusic()
    {
        // Si la música está encendida, la apagamos; si está apagada, la encendemos
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
