using UnityEngine;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour
{
    [SerializeField] private CarCreatorManager carCreator; // Referencia al script del creador del auto
    [SerializeField] private ScrollRect scrollViewWheels; // Scroll View que contiene los botones de ruedas
    [SerializeField] private ScrollRect scrollViewWindows; // Scroll View que contiene los botones de ventanas
    [SerializeField] private ScrollRect scrollViewBodyColors; // Scroll View que contiene los botones de colores

    void Start()
    {
        // Asignar las funciones OnClick para cada botón de rueda
        AssignButtonListeners(scrollViewWheels, carCreator.SelectWheels);

        // Asignar las funciones OnClick para cada botón de ventana
        AssignButtonListeners(scrollViewWindows, carCreator.SelectWindows);

        // Asignar las funciones OnClick para cada botón de color del cuerpo
        AssignButtonColorListeners(scrollViewBodyColors);
    }

    // Método para asignar listeners a los botones de un Scroll View
    private void AssignButtonListeners(ScrollRect scrollView, System.Action<int> selectFunction)
    {
        foreach (Transform child in scrollView.content)
        {
            int index = child.GetSiblingIndex(); // Usar el índice del botón
            Button button = child.GetComponent<Button>();
            button.onClick.AddListener(() => selectFunction(index));
        }
    }

    // Método para asignar listeners a los botones de color del cuerpo
    private void AssignButtonColorListeners(ScrollRect scrollView)
    {
        foreach (Transform child in scrollView.content)
        {
            int index = child.GetSiblingIndex(); // Usar el índice del botón
            Button button = child.GetComponent<Button>();

            // Asignar el listener para cambiar el color del cuerpo
            button.onClick.AddListener(() => 
            {
                // Cambiar el color del cuerpo llamando al método correspondiente
                carCreator.ChangeBodyColor(index);
                
                // Activar el GameObject correspondiente y desactivar los demás
                ActivateBodyColor(index);
            });
        }
    }

    // Método para activar el GameObject del color seleccionado y desactivar los demás
    private void ActivateBodyColor(int index)
    {
        for (int i = 0; i < carCreator.bodyColorDisplays.Count; i++)
        {
            carCreator.bodyColorDisplays[i].SetActive(i == index); // Activar solo el GameObject del color seleccionado
        }
    }
}