using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Para manejar eventos de UI como arrastrar
using UnityEngine.Audio;

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

    void Start()
    {
        // Asignar a cada botón su función de arrastre
        foreach (Button letra in _letras)
        {
            letra.onClick.AddListener(() => IniciarArrastre(letra));
        }
    }

    void Update()
    {
        if (arrastrando)
        {
            // Seguir el puntero del mouse mientras se arrastra
            letraActual.transform.position = Input.mousePosition;

            if (Input.GetMouseButtonUp(0)) // Cuando se suelta el botón izquierdo del mouse
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
    }

    private void RevisarPosicion()
    {
        bool colocadoCorrectamente = false;

        // Revisar si la letra fue colocada en el hueco correcto
        foreach (GameObject hueco in _huecos)
        {
            float distancia = Vector2.Distance(letraActual.transform.position, hueco.transform.position);

            if (distancia < 50f) // Rango de tolerancia para colocar en el hueco
            {
                // Colocar la letra en el hueco
                letraActual.transform.position = hueco.transform.position;
                colocadoCorrectamente = true;
                break;
            }
        }

        // Reproducir el sonido adecuado
        if (colocadoCorrectamente)
        {
            audioSource.PlayOneShot(correctoClip);
        }
        else
        {
            audioSource.PlayOneShot(incorrectoClip);
            // Devolver la letra a su posición inicial
            letraActual.transform.position = letraPosicionInicial;
        }
    }
}