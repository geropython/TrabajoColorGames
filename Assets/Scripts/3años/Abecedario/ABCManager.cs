using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ABCManager : MonoBehaviour
{
    public bool isPanelActive = false;
    private bool isMusicOn = true;
    [SerializeField]private AudioSource musicSource; 
   [System.Serializable]
    public class PanelGroup
    {
        public Button button;        // El botón que alterna los paneles
        public GameObject panel1;     // El primer panel
        public GameObject panel2;     // El segundo panel
    }

    public PanelGroup[] panelGroups;  // Array de pares de botones/paneles

    void Start()
    {
        // Asignar el listener de cada botón para alternar entre los paneles
        foreach (PanelGroup group in panelGroups)
        {
            if (group.button != null)
            {
                group.button.onClick.AddListener(() => TogglePanels(group));
            }
        }
    }

    // Función para alternar entre los dos paneles
    void TogglePanels(PanelGroup group)
    {
        // Si el primer panel está activo, desactívalo y activa el segundo panel
        if (group.panel1.activeSelf)
        {
            group.panel1.SetActive(false);
            group.panel2.SetActive(true);
        }
        else
        {
            // Si el segundo panel está activo, desactívalo y activa el primer panel
            group.panel1.SetActive(true);
            group.panel2.SetActive(false);
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("3Menu");
    }
    public void ToggleMusic()
    {
        // Si la música está encendida, la apagamos; si está apagada, la encendemos
        isMusicOn = !isMusicOn;

        if (isMusicOn)
        {
            musicSource.Play();
        }
        else
        {
            musicSource.Pause();
        }
    }
}