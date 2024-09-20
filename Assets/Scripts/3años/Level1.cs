using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level1 : MonoBehaviour
{
    [SerializeField] private Button[] _letras; // Botones de letras
    [SerializeField] private GameObject[] _cardPanels; // Paneles de cada letra
    [SerializeField] private Button _settings, _mayus; // Botones de settings y mayúsculas
    [SerializeField] private GameObject _settingsPanel; // Panel de configuración
    private Vector3 originalSize; // Tamaño original de los botones de letras

    private void Start()
    {
        // Guardamos el tamaño original de los botones
        if (_letras.Length > 0)
        {
            originalSize = _letras[0].transform.localScale;
        }

        // Asignamos el evento onClick a cada botón de letra
        for (int i = 0; i < _letras.Length; i++)
        {
            int index = i; // Necesario para capturar el índice correcto en el delegado
            _letras[i].onClick.AddListener(() => OnLetterClick(index));
        }

        // Ocultamos todos los cardPanels al inicio
        HideAllCardPanels();
    }

    // Método que se ejecuta cuando se hace clic en una letra
    private void OnLetterClick(int index)
    {
        // Reinicia todos los botones al tamaño original
        ResetButtonSizes();

        // Agranda el botón presionado
        _letras[index].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        // Muestra el panel correspondiente
        HideAllCardPanels();
        _cardPanels[index].SetActive(true);
    }

    // Oculta todos los cardPanels
    private void HideAllCardPanels()
    {
        foreach (GameObject panel in _cardPanels)
        {
            panel.SetActive(false);
        }
    }

    // Restaura el tamaño original de todos los botones de letras
    private void ResetButtonSizes()
    {
        foreach (Button letra in _letras)
        {
            letra.transform.localScale = originalSize;
        }
    }
}
