using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    [SerializeField] private Button[] _letrasMay, _letrasMin;
    [SerializeField] private GameObject[] _MayusCardPanels, _MinCardPanels;
    [SerializeField] private Button _settings, _mayus_min, _sound, _music;
    [SerializeField] private GameObject _settingsPanel, _menu3Panel,  _objetoLetrasMin, _objetoLetrasMay;
    [SerializeField] private AudioSource _backgroundMusic; // La música de fondo
    [SerializeField] private AudioSource _buttonSound, _normalAudio, _slowAudio; // El sonido al tocar el botón
    private Vector3 originalSize; // Tamaño original de los botones de letras
    private bool isMusicOn = true; // Estado de la música
    private bool isSoundOn = true; // Estado del sonido
    private Vector3 enlargedSize = new Vector3(1.2f, 1.2f, 1.2f);
    private bool isPanelActive = false; 

    private void Start()
    {
        //Mayusculas
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
         for (int i = 0; i < _letrasMay.Length; i++)
        {
            int index = i;
            _letrasMin[i].onClick.AddListener(() => OnLetterClick(index));
        }

        // Ocultamos todos los cardPanels al inicio
        HideAllMayusCardPanels();
    }

    private void OnLetterClick(int index)
    {
        // Reproducir sonido del botón
        if (isSoundOn)
        {
            _buttonSound.Play();
        }

        ResetButtonSizes();

        // Agranda el botón presionado
        _letrasMay[index].transform.localScale = enlargedSize;
        _letrasMin[index].transform.localScale = enlargedSize;

        // Muestra el panel correspondiente
        HideAllMayusCardPanels();
        HideAllMinCardPanels();
        _MayusCardPanels[index].SetActive(true);
        _MinCardPanels[index].SetActive(true);
    }

    // Oculta todos los cardPanels
    private void HideAllMayusCardPanels()
    {
        foreach (GameObject panel in _MayusCardPanels)
        {
            panel.SetActive(false);
        }
        
    }
    private void HideAllMinCardPanels()
    {
         foreach (GameObject panel in _MinCardPanels)
        {
            panel.SetActive(false);
        }
    }

    // Restaura el tamaño original de todos los botones de letras
    private void ResetButtonSizes()
    {
        foreach (Button letra in _letrasMay)
        {
            letra.transform.localScale = originalSize;
        }
         foreach (Button letra in _letrasMin)
        {
            letra.transform.localScale = originalSize;
        }
    }

    // Método para manejar el clic en el botón de música
    private void OnMusicButtonClick()
    {
        if (isMusicOn)
        {
            // Apagar la música y restaurar tamaño del botón
            _backgroundMusic.Pause();
            _music.transform.localScale = originalSize;
        }
        else
        {
            // Encender la música y agrandar el botón
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
            // Apagar sonido de botones y restaurar tamaño del botón
            _sound.transform.localScale = originalSize;
        }
        else
        {
            // Encender sonido de botones y agrandar el botón
            _sound.transform.localScale = enlargedSize;
        }
        isSoundOn = !isSoundOn; // Cambiar el estado del sonido
    }
    // public void PlayNormalAudio()
    // {
    //     _normalAudio.Play();//reproducir audio normal
    // }
    // public void PlaySlowAudio()
    // {
    //     _slowAudio.Play();//reproducir audio lento
    // }
    public void Info()
    {
        //mostrar informacion por 10s
    }
    public void ToggleSettings()
    {
        isPanelActive = !isPanelActive; // Cambia el estado (true -> false, false -> true)
        _settingsPanel.SetActive(isPanelActive); // Activa o desactiva el panel según el estado
    }
    public void ToggleLetters()
    {
        // Alternar el estado de las letras minúsculas y mayúsculas
        bool areMinLettersActive = _objetoLetrasMin.activeSelf;
        bool areMayLettersActive = _objetoLetrasMay.activeSelf;

        // Cambiar el estado a su opuesto
        _objetoLetrasMin.SetActive(!areMinLettersActive);
        _objetoLetrasMay.SetActive(!areMayLettersActive);
    }
}