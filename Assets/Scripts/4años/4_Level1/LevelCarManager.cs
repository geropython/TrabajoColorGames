using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCarManager : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private Button redButton, blueButton, greenButton;
    [SerializeField] private Transform finishLine;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float moveDistance = 2f;  // Distancia que el auto avanza por acierto
    [SerializeField] private int correctAnswers = 0;
    [SerializeField] private int _limite = 12;
    [SerializeField] private string currentColor;

    private List<string> colors = new List<string> { "rojo", "azul", "verde" };
    private bool shouldMove = false;
    private bool hasReachedFinish = false;
    private Vector3 targetPosition;  // Nueva posición objetivo tras cada acierto

    public void Start()
    {
        _endPanel.SetActive(false);
        SetNewColor();  // Configura el primer color al iniciar
        redButton.onClick.AddListener(() => CheckColor("rojo"));
        blueButton.onClick.AddListener(() => CheckColor("azul"));
        greenButton.onClick.AddListener(() => CheckColor("verde"));
    }

    public void Update()
    {
        if (shouldMove && !hasReachedFinish)
        {
            MoveCar();
            
            // Compara la posición Y del auto con la posición Y deseada
            if (Mathf.Abs(car.transform.position.y - 572f) < 0.1f) // Tolerancia de 0.1f para evitar problemas de precisión
            {
                Celebrate();
            }
        }
    }

    void SetNewColor()
    {
        int randomIndex = Random.Range(0, colors.Count);
        currentColor = colors[randomIndex];
        instructionText.text = "Selecciona el color: " + currentColor;
    }

    void CheckColor(string selectedColor)
    {
        if (selectedColor == currentColor)
        {
            correctAnswers++;
            SetTargetPosition();  // Establece una nueva posición objetivo para mover el auto
            shouldMove = true;    // Activa el movimiento del auto

            // Verifica si ha alcanzado el número correcto de respuestas
            if (correctAnswers >= _limite)
            {
                Celebrate(); // Llama a la celebración si correctAnswers es 8 o más
                return; // Sale de la función para evitar que se llame a SetNewColor()
            }
        }

        if (!hasReachedFinish)
        {
            SetNewColor();
        }
    }


    void SetTargetPosition()
    {
        // Establece la nueva posición objetivo a la que el auto debe moverse (una distancia fija hacia adelante)
        targetPosition = car.transform.position + (finishLine.position - car.transform.position).normalized * moveDistance;

        // Asegurarse de que el auto no sobrepase la meta
        if (Vector3.Distance(targetPosition, finishLine.position) < moveDistance)
        {
            targetPosition = finishLine.position;  // Si está cerca de la meta, fija la posición en la meta
        }
    }

    void MoveCar()
    {
        car.transform.position = Vector3.MoveTowards(car.transform.position, targetPosition, speed * Time.deltaTime);

        // Si el auto ha alcanzado la nueva posición objetivo, se detiene el movimiento
        if (Vector3.Distance(car.transform.position, targetPosition) < 0.01f)
        {
            shouldMove = false;  // Detiene el movimiento del auto
        }
    }

    void Celebrate()
    {
        _endPanel.SetActive(true);
        shouldMove = false;  // Asegura que el auto se detenga
        hasReachedFinish = true;  // Marca que ha llegado a la meta
    }
}
