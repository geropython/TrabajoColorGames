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
    public void Trazado()
    {
        SceneManager.LoadScene("Trazado");
    }
    public void Arrastre()
    {
        SceneManager.LoadScene("Arrastre");
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