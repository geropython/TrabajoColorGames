using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button _3años,_5años, _4años;
    [SerializeField] private GameObject _nivelesPanel;

    private void Start()
    {
        _nivelesPanel.SetActive(false);
    }

    public void Play(){_nivelesPanel.SetActive(true);}
    public void TresAños_Scene(){SceneManager.LoadScene("3 años");}
    public void CuatroAños_Scene(){SceneManager.LoadScene("4 años");}
    public void CincoAños_Scene(){SceneManager.LoadScene("5 años");}
    public void Quit(){Application.Quit();}
}