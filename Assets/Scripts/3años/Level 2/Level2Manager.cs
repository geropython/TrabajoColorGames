using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrazadoManager : MonoBehaviour
{
    [SerializeField] private GameObject _mayusPanel, _minusPanel, _mainPanel, _selectPanel;
    private bool isPanelActive = false;
    private bool isMusicOn = true;
    [SerializeField] private AudioSource _audio;

    void Start()
    {
        _mainPanel.SetActive(true);
        _mayusPanel.SetActive(false);
        _minusPanel.SetActive(false);
        _selectPanel.SetActive(false);
    }

    #region Panels
    public void SelectPanel()
    {
        _mayusPanel.SetActive(false);
        _selectPanel.SetActive(true);
        _minusPanel.SetActive(false);
        _mainPanel.SetActive(false);
    }
    public void MenuPanel()
    {
        _mayusPanel.SetActive(false);
        _selectPanel.SetActive(false);
        _minusPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }
    public void MayusPanel()
    {
        _mayusPanel.SetActive(true);
        _selectPanel.SetActive(false);
        _minusPanel.SetActive(false);
        _mainPanel.SetActive(false);
    }
    public void MinusPanel()
    {
        _mayusPanel.SetActive(false);
        _selectPanel.SetActive(false);
        _minusPanel.SetActive(true);
        _mainPanel.SetActive(false);
    }
    public void BackMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void ToggleMusic()
    {
        // Si la música está encendida, la apagamos; si está apagada, la encendemos
        isMusicOn = !isMusicOn;

        if (isMusicOn)
        {
            _audio.Play();
        }
        else
        {
            _audio.Pause();
        }
    }
    
    #endregion Panels
}
