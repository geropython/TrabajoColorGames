using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrazadoManager : MonoBehaviour
{
    [SerializeField] private GameObject _mayusPanel, _minusPanel, _mainPanel, _selectPanel, _settingsPanel;
    private bool isPanelActive = false; 

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
    public void ToggleSettings()
    {
        isPanelActive = !isPanelActive; // Cambia el estado (true -> false, false -> true)
        _settingsPanel.SetActive(isPanelActive); // Activa o desactiva el panel según el estado
    }
    public void BackMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    
    #endregion Panels
}
