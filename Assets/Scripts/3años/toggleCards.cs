using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class toggleCards : MonoBehaviour
{
    [System.Serializable]
    public class PanelGroup
    {
        public Button button1;        // El primer botón que muestra el primer panel
        public Button button2;        // El segundo botón que muestra el segundo panel
        public GameObject panel1;     // El primer panel (activo/inactivo)
        public GameObject panel2;     // El segundo panel (activo/inactivo)
    }

    public PanelGroup[] panelGroups;  // Array de pares de botones/paneles

    void Start()
    {
        // Asignar el listener de cada botón a su respectiva función de alternancia
        foreach (PanelGroup group in panelGroups)
        {
            if (group.button1 != null)
            {
                group.button2.onClick.AddListener(() => ShowPanel1(group));
            }

            if (group.button2 != null)
            {
                group.button1.onClick.AddListener(() => ShowPanel2(group));
            }
        }
    }

    // Función para mostrar el primer panel y ocultar el segundo
    void ShowPanel1(PanelGroup group)
    {
        group.panel1.SetActive(true);  // Activa el primer panel
        group.panel2.SetActive(false); // Desactiva el segundo panel
    }

    // Función para mostrar el segundo panel y ocultar el primero
    void ShowPanel2(PanelGroup group)
    {
        group.panel1.SetActive(false); // Desactiva el primer panel
        group.panel2.SetActive(true);  // Activa el segundo panel
    }
}
