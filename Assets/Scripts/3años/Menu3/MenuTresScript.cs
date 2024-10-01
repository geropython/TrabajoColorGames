using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuTresScript : MonoBehaviour
{
    public void Abecedario()
    {
        SceneManager.LoadScene("Abecedario");
    }
    public void Nivel1()
    {
        SceneManager.LoadScene("Nivel 1");
    }
    public void Nivel2()
    {
        SceneManager.LoadScene("Nivel 2");
    }
    public void BackMenu()
    {
        SceneManager.LoadScene("MainMenu");
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