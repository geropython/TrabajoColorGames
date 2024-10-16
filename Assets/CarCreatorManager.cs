using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCreatorManager : MonoBehaviour
{
    [SerializeField] private GameObject car; // Referencia al auto base
    [SerializeField] private List<GameObject> wheelOptions; // Lista de opciones de ruedas
    [SerializeField] private List<GameObject> windowOptions; // Lista de opciones de ventanas
    [SerializeField] private List<GameObject> spoilerOptions; // Lista de opciones de alerones
    [SerializeField] private List<Color> bodyColorOptions; // Lista de colores de carrocería

    private GameObject currentWheels; // Referencia a las ruedas seleccionadas
    private GameObject currentWindows; // Referencia a las ventanas seleccionadas

    void Start()
    {
        InitializeCar();
    }

    void Update()
    {
        
    }

    void InitializeCar()
    {
        // Instanciar el auto base en la escena
        car = Instantiate(car);
    }

    public void AddWheels(GameObject wheels)
    {
        if (currentWheels != null)
        {
            Destroy(currentWheels); // Destruir las ruedas anteriores
        }
        currentWheels = Instantiate(wheels, car.transform);
        PositionWheels();
    }

    public void AddWindows(GameObject windows)
    {
        if (currentWindows != null)
        {
            Destroy(currentWindows); // Destruir las ventanas anteriores
        }
        currentWindows = Instantiate(windows, car.transform);
        PositionWindows();
    }

    public void ChangeBodyColor(Color color)
    {
        car.GetComponent<Renderer>().material.color = color;
    }

    void PositionWheels()
    {
        // Ajustar la posición de las ruedas en relación al auto
        currentWheels.transform.localPosition = new Vector3(0, -0.5f, 0); // Ajusta según el modelo
    }

    void PositionWindows()
    {
        // Ajustar la posición de las ventanas en relación al auto
        currentWindows.transform.localPosition = new Vector3(0, 0.5f, 0); // Ajusta según el modelo
    }

    public void ShowCelebrationAnimation()
    {
        // Aquí puedes implementar la animación de celebración del auto
        // Por ejemplo, mover el auto a una pista y hacer una animación
    }
}