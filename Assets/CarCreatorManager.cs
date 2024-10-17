using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con imágenes de UI

public class CarCreatorManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject car; // Referencia al auto base
    public List<GameObject> wheelOptions; // Lista de opciones de ruedas
    public List<GameObject> windowOptions; // Lista de opciones de ventanas
    public List<GameObject> bodyColorDisplays; // Lista de colores de carrocería

    [SerializeField] private GameObject[] wheelDisplays; // Array de GameObjects que muestra la rueda seleccionada
    [SerializeField] private GameObject[] windowDisplays; // Array de GameObjects que muestra la ventana seleccionada

    private int currentWheelIndex = 0;
    private int currentWindowIndex = 0;
    private int currentBodyColorIndex = 0;

    private GameObject currentWheels;
    private GameObject currentWindows;

    // Para rastrear los componentes tocados recientemente
    private bool wheelTouched = false;
    private bool windowTouched = false;
    private bool bodyColorTouched = false;

    #endregion

    void Start()
    {
        InitializeCar();
        UpdateWheels();
        UpdateWindows();
    }

    void InitializeCar() 
    {
        car = Instantiate(car); 
    }

    #region Select
    public void SelectWheels(int index)
    {
        if (index >= 0 && index < wheelOptions.Count)
        {
            currentWheelIndex = index;
            UpdateWheels();
            wheelTouched = true;
            CheckRecentlyTouched();
        }
    }

    public void SelectWindows(int index)
    {
        if (index >= 0 && index < windowOptions.Count)
        {
            currentWindowIndex = index;
            UpdateWindows();
            windowTouched = true;
            CheckRecentlyTouched();
        }
    }
    #endregion

    #region Change
    void UpdateWheels()
    {
        if (currentWheels != null)
        {
            Destroy(currentWheels);
        }
        currentWheels = Instantiate(wheelOptions[currentWheelIndex], car.transform);
        PositionWheels();
    }

    void UpdateWindows()
    {
        if (currentWindows != null)
        {
            Destroy(currentWindows);
        }
        currentWindows = Instantiate(windowOptions[currentWindowIndex], car.transform);
        PositionWindows();
    }

    public void ChangeBodyColor(int index)
    {
        // Desactivar todos los GameObjects de color
        foreach (var display in bodyColorDisplays)
        {
            display.SetActive(false);
        }

        // Activar solo el GameObject del color seleccionado
        bodyColorDisplays[index].SetActive(true);
        bodyColorTouched = true; // Marcar que se tocó un color
        CheckRecentlyTouched(); // Comprobar qué se ha tocado
    }
    #endregion

    #region Position
    void PositionWheels() 
    { 
        currentWheels.transform.localPosition = new Vector3(0, -0.5f, 0); 
    }

    void PositionWindows() 
    { 
        currentWindows.transform.localPosition = new Vector3(0, 0.5f, 0); 
    }
    #endregion

    // Método para activar/desactivar componentes según los tocados
    void CheckRecentlyTouched()
    {
        // Activar ruedas si fueron tocadas
        foreach (var display in wheelDisplays)
        {
            display.SetActive(false); // Desactivar todos los displays de ruedas
        }
        if (wheelTouched)
        {
            wheelDisplays[currentWheelIndex].SetActive(true); // Activar el display de la rueda seleccionada
        }

        // Activar ventanas si fueron tocadas
        foreach (var display in windowDisplays)
        {
            display.SetActive(false); // Desactivar todos los displays de ventanas
        }
        if (windowTouched)
        {
            windowDisplays[currentWindowIndex].SetActive(true); // Activar el display de la ventana seleccionada
        }
    }
}