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

    void BotonCorrecto()
    {
        // Acción para el botón correcto
        Debug.Log("¡Correcto!");
        audioSource.PlayOneShot(correctSound);
        AsignarLetrasABotones(); // Cambiar las letras y volver a empezar
    }

    void BotonIncorrecto()
    {
        // Acción para el botón incorrecto
        Debug.Log("Incorrecto, intenta de nuevo.");
        audioSource.PlayOneShot(incorrectSound);
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

    public void BackMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

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