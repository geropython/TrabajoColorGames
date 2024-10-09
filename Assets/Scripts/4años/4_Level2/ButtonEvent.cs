using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    public void Level1()
    {
        SceneManager.LoadScene("4_Level1");
    }
    public void Level2()
    {
        SceneManager.LoadScene("4_Level2");
    }
}
