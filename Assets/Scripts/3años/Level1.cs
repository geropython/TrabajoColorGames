using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    [SerializeField] private Button[] _letrasMay, _letrasMin;
    [SerializeField] private Button _settings, _mayus_min, _sound, _music;
    [SerializeField] private AudioSource _backgroundMusic; // La música de fondo
    [SerializeField] private AudioSource _buttonSound; // El sonido al tocar el botón
    private Vector3 originalSize; // Tamaño original de los botones de letras
    private bool isMusicOn = true; // Estado de la música
    private bool isSoundOn = true; // Estado del sonido
    private Vector3 enlargedSize = new Vector3(1.2f, 1.2f, 1.2f);

    private void Start()
    {
        // Inicializar letras
        StarLetters();

        // Asignar listeners a los botones de música y sonido
        _music.onClick.AddListener(OnMusicButtonClick);
        _sound.onClick.AddListener(OnSoundButtonClick);
    }

    void StarLetters()
    {
        // Guardamos el tamaño original de los botones
        if (_letrasMay.Length > 0)
        {
            originalSize = _letrasMay[0].transform.localScale;
        }
        if (_letrasMin.Length > 0)
        {
            originalSize = _letrasMin[0].transform.localScale;
        }

        // Asignamos el evento onClick a cada botón de letra
        for (int i = 0; i < _letrasMay.Length; i++)
        {
            int index = i;
            _letrasMay[i].onClick.AddListener(() => OnLetterClick(index));
        }
        for (int i = 0; i < _letrasMin.Length; i++)
        {
            int index = i;
            _letrasMin[i].onClick.AddListener(() => OnLetterClick(index));
        }
    }

    private void OnLetterClick(int index)
    {
        // Reproducir sonido del botón
        if (isSoundOn)
        {
            _buttonSound.Play();
        }

        // Agranda el botón presionado
        _letrasMay[index].transform.localScale = enlargedSize;
        _letrasMin[index].transform.localScale = enlargedSize;
        
        // Restaura el tamaño original de los botones de letras (excepto el que se presionó)
        ResetButtonSizes(index);
    }

    // Restaura el tamaño original de todos los botones de letras excepto el que fue presionado
    private void ResetButtonSizes(int activeIndex)
    {
        for (int i = 0; i < _letrasMay.Length; i++)
        {
            if (i != activeIndex)
            {
                _letrasMay[i].transform.localScale = originalSize;
            }
        }
        for (int i = 0; i < _letrasMin.Length; i++)
        {
            if (i != activeIndex)
            {
                _letrasMin[i].transform.localScale = originalSize;
            }
        }
    }

    // Método para manejar el clic en el botón de música
    private void OnMusicButtonClick()
    {
        if (isMusicOn)
        {
            _backgroundMusic.Pause();
            _music.transform.localScale = originalSize;
        }
        else
        {
            _backgroundMusic.Play();
            _music.transform.localScale = enlargedSize;
        }
        isMusicOn = !isMusicOn; // Cambiar el estado de la música
    }

    // Método para manejar el clic en el botón de sonido
    private void OnSoundButtonClick()
    {
        if (isSoundOn)
        {
            _sound.transform.localScale = originalSize;
        }
        else
        {
            _sound.transform.localScale = enlargedSize;
        }
        isSoundOn = !isSoundOn; // Cambiar el estado del sonido
    }
}