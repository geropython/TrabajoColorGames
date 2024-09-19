using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ABCScript : MonoBehaviour
{
    [SerializeField] private Button[] _letras; // Letras que serán arrastradas
    [SerializeField] private GameObject[] _huecos; // Huecos donde deben colocarse las letras
    [SerializeField] private AudioClip correctoClip; // Sonido cuando la letra es colocada correctamente
    [SerializeField] private AudioClip incorrectoClip; // Sonido cuando la letra es colocada incorrectamente
    [SerializeField] private AudioSource audioSource; // AudioSource para reproducir los clips

    private Button letraActual;
    private Vector2 letraPosicionInicial;
    private bool arrastrando = false;
    private bool colocadoCorrectamente = false; // Bandera para saber si la letra ya está colocada

    void Start()
    {
        // Asignar a cada botón su función de arrastre
        foreach (Button letra in _letras)
        {
            EventTrigger trigger = letra.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { IniciarArrastre(letra); });
            trigger.triggers.Add(entry);
        }
    }

    void Update()
    {
        if (arrastrando && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // Seguir el dedo mientras se arrastra
                letraActual.transform.position = touch.position;
                Debug.Log("Arrastrando letra: " + letraActual.name);
            }

            if (touch.phase == TouchPhase.Ended) // Cuando se suelta el dedo
            {
                arrastrando = false;
                RevisarPosicion();
            }
        }
    }

    private void IniciarArrastre(Button letra)
    {
        // Comenzar a arrastrar la letra
        letraActual = letra;
        letraPosicionInicial = letra.transform.position;
        arrastrando = true;
        colocadoCorrectamente = false; // Resetear estado de colocación
        Debug.Log("Iniciando arrastre de letra: " + letraActual.name);
    }

    private void RevisarPosicion()
    {
        GameObject huecoCorrecto = null;
        GameObject huecoIncorrecto = null;
        colocadoCorrectamente = false;

        foreach (GameObject hueco in _huecos)
        {
            // Obtener el índice del hueco
            int indiceHueco = System.Array.IndexOf(_huecos, hueco);

            // Obtener la letra del botón
            string letraTexto = letraActual.GetComponentInChildren<TextMeshProUGUI>().text;

            // Comprobar si la letra coincide con el índice del hueco (A=0, B=1, C=2, ...)
            if (letraTexto == ((char)('A' + indiceHueco)).ToString())
            {
                float distancia = Vector2.Distance(letraActual.transform.position, hueco.transform.position);
                Debug.Log("Distancia a hueco: " + distancia);

                if (distancia < 50f) // Rango de tolerancia
                {
                    huecoCorrecto = hueco;
                    colocadoCorrectamente = true;
                    break;
                }
            }
            else
            {
                float distancia = Vector2.Distance(letraActual.transform.position, hueco.transform.position);
                if (distancia < 50f)
                {
                    huecoIncorrecto = hueco;
                }
            }
        }

        if (colocadoCorrectamente)
        {
            letraActual.transform.position = huecoCorrecto.transform.position; // Colocar la letra en el hueco
            huecoCorrecto.GetComponent<Image>().color = Color.green; // Cambiar color del hueco a verde
            audioSource.PlayOneShot(correctoClip); // Reproducir sonido correcto
            letraActual.interactable = false; // Desactivar el botón para evitar arrastre posterior

            // Desactivar el EventTrigger
            EventTrigger trigger = letraActual.GetComponent<EventTrigger>();
            if (trigger != null)
            {
                trigger.enabled = false;
            }
            Debug.Log("Letra colocada correctamente: " + letraActual.name);
        }
        else if (huecoIncorrecto != null)
        {
            audioSource.PlayOneShot(incorrectoClip); // Reproducir sonido incorrecto
            letraActual.transform.position = letraPosicionInicial; // Volver a posición inicial
            // Cambiar el color del hueco incorrecto a rojo
            huecoIncorrecto.GetComponent<Image>().color = Color.red;
            Debug.Log("Letra no colocada correctamente: " + letraActual.name);
        }
    }
}