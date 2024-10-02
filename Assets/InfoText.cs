using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    [SerializeField] private Button toggleBoton;
    [SerializeField] private GameObject panel;
    [SerializeField] private float duracion = 5f;
    private Coroutine panelCoroutine;                 

    void Start()
    {
        // Asigna el evento del botón
        toggleBoton.onClick.AddListener(MostrarPanel);
        
        // Asegúrate de que el panel esté inicialmente desactivado
        panel.SetActive(false);
    }

    public void MostrarPanel()
    {
        // Si ya hay una coroutine en curso, detenerla
        if (panelCoroutine != null)
        {
            StopCoroutine(panelCoroutine);
        }

        // Iniciar la coroutine para mostrar el panel
        panelCoroutine = StartCoroutine(MostrarPanelTemporal());
    }

    private IEnumerator MostrarPanelTemporal()
    {
        // Activa el panel
        panel.SetActive(true);

        // Espera durante la duración especificada
        yield return new WaitForSeconds(duracion);

        // Desactiva el panel después del tiempo
        panel.SetActive(false);
    }

    public void CerrarMensaje()
    {
        // Detiene la coroutine si está activa y cierra el panel inmediatamente
        if (panelCoroutine != null)
        {
            StopCoroutine(panelCoroutine);
        }
        panel.SetActive(false);
    }
}