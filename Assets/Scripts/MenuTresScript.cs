using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuTresScript : MonoBehaviour
{
   [SerializeField] private GameObject _abcPanel, _trazadoPanel, _palabraPanel, _menuPanel;

   void Start()
   {
        AllPanelDeactivate();
        _menuPanel.SetActive(true);
   }
   void AllPanelDeactivate()
    {
        _abcPanel.SetActive(false);
        _trazadoPanel.SetActive(false);
        _palabraPanel.SetActive(false);
    }
    public void OpenABC()
    {
        _abcPanel.SetActive(true);
        _trazadoPanel.SetActive(false);
        _palabraPanel.SetActive(false);
        _menuPanel.SetActive(false);
    }
    public void OpenTrazado()
    {
        _abcPanel.SetActive(false);
        _trazadoPanel.SetActive(true);
        _palabraPanel.SetActive(false);
        _menuPanel.SetActive(false);
    }
    public void OpenPalabra()
    {
        _abcPanel.SetActive(false);
        _trazadoPanel.SetActive(false);
        _palabraPanel.SetActive(true);
        _menuPanel.SetActive(false);
    }
    public void BackMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void BackMenu3()
    {
         AllPanelDeactivate();
        _menuPanel.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void MusicOn()
    {
        //apagar y prender musica
    }
}