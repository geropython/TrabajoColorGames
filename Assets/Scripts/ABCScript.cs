using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
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

            if (touch.phase == TouchPhase.Moved && !colocadoCorrectamente)
            {
                // Seguir el dedo mientras se arrastra
                letraActual.transform.position = touch.position;
                Debug.Log("Arrastrando letra: " + letraActual.name);
            }

            if (touch.phase == TouchPhase.Ended && !colocadoCorrectamente) // Cuando se suelta el dedo
            {
                arrastrando = false;
                RevisarPosicion();
            }
        }
    }

    private void IniciarArrastre(Button letra)
    {
        // Comenzar a arrastrar la letra si no está colocada correctamente
        if (!letra.interactable) return; // No arrastrar si ya está colocada

        letraActual = letra;
        letraPosicionInicial = letra.transform.position;
        arrastrando = true;
        colocadoCorrectamente = false; // Resetear estado de colocación
        Debug.Log("Iniciando arrastre de letra: " + letraActual.name);
    }

    private void RevisarPosicion()
    {
        GameObject huecoCorrecto = null;
        colocadoCorrectamente = false;

        foreach (GameObject hueco in _huecos)
        {
            TextMeshProUGUI huecoTexto = hueco.GetComponentInChildren<TextMeshProUGUI>();
            string letraTexto = letraActual.GetComponentInChildren<TextMeshProUGUI>().text;

            if (letraTexto == huecoTexto.text)
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
        }

        if (colocadoCorrectamente)
        {
            letraActual.transform.position = huecoCorrecto.transform.position; // Colocar la letra en el hueco
            huecoCorrecto.GetComponent<Image>().color = Color.green; // Cambiar color del hueco
            audioSource.PlayOneShot(correctoClip);
            letraActual.interactable = false; // Desactivar el botón para evitar arrastre posterior

            EventTrigger trigger = letraActual.GetComponent<EventTrigger>();
            if (trigger != null)
            {
                trigger.enabled = false; // Desactivar el EventTrigger
            }
            Debug.Log("Letra colocada correctamente: " + letraActual.name);
        }
        else
        {
            audioSource.PlayOneShot(incorrectoClip);
            letraActual.transform.position = letraPosicionInicial; // Volver a posición inicial
            Debug.Log("Letra no colocada correctamente: " + letraActual.name);
        }
    }
}