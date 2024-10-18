using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para trabajar con botones

public class SceneProgress : MonoBehaviour
{
    [SerializeField] private int _age; // Edad a la que corresponde la escena (3, 4 o 5 años)
    [SerializeField] private int _totalLevels = 3; // Número total de niveles en la escena
    [SerializeField] private GameObject _medal; // Referencia a la medalla en la escena
    [SerializeField] private Button _diplomaButton; // Referencia al botón para activar el diploma
    [SerializeField] private GameObject _diploma; // Referencia al diploma en la escena

    private int _completedLevels = 0; // Niveles completados en la escena

    void Start()
    {
        // Cargar los niveles completados para esta escena
        _completedLevels = PlayerPrefs.GetInt($"LevelsCompleted_Age{_age}", 0);

        // Ocultar la medalla inicialmente
        _medal.SetActive(false);

        // Ocultar el diploma inicialmente
        _diploma.SetActive(false);

        // Desactivar el botón del diploma inicialmente
        _diplomaButton.gameObject.SetActive(false);

        // Mostrar la medalla si ya se ha desbloqueado
        if (PlayerPrefs.GetInt($"Medal_Age{_age}", 0) == 1)
        {
            _medal.SetActive(true); // Mostrar medalla si ya se ha desbloqueado
            _diplomaButton.gameObject.SetActive(true); // Activar el botón del diploma cuando la medalla se muestre
        }

        // Agregar un listener al botón del diploma para que lo active/desactive
        _diplomaButton.onClick.AddListener(ToggleDiploma);
    }

    // Este método se llama cuando el jugador completa un nivel
    public void CompleteLevel()
    {
        _completedLevels++;

        // Guardar progreso de niveles completados
        PlayerPrefs.SetInt($"LevelsCompleted_Age{_age}", _completedLevels);

        // Si ha completado todos los niveles, desbloquear la medalla
        if (_completedLevels >= _totalLevels)
        {
            PlayerPrefs.SetInt($"Medal_Age{_age}", 1); // Desbloquear la medalla
            _medal.SetActive(true); // Mostrar la medalla en la escena
            _diplomaButton.gameObject.SetActive(true); // Activar el botón del diploma
        }

        // Guardar los cambios
        PlayerPrefs.Save();
    }

    // Método para activar o desactivar el diploma
    private void ToggleDiploma()
    {
        _diploma.SetActive(!_diploma.activeSelf); // Cambiar el estado activo del diploma
    }
}