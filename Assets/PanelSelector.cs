using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelSelector : MonoBehaviour
{
    [System.Serializable]
    public class PanelGroup
    {
        public Button button;     // Un solo botón
        public GameObject panel;  // Panel correspondiente
    }

    public PanelGroup[] panelGroups;  // Array de pares de botones/paneles

    void Start()
    {
        // Asignar el listener de cada botón a su respectivo panel
        foreach (PanelGroup group in panelGroups)
        {
            if (group.button != null)
            {
                group.button.onClick.AddListener(() => ShowPanel(group));
            }
        }
    }

    // Función para mostrar el panel correspondiente y ocultar los demás
    void ShowPanel(PanelGroup selectedGroup)
    {
        // Desactivar todos los paneles
        foreach (PanelGroup group in panelGroups)
        {
            group.panel.SetActive(false);
        }

        // Activar solo el panel del grupo seleccionado
        selectedGroup.panel.SetActive(true);
    }
}
