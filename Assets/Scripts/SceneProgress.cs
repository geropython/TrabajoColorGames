using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneProgress : MonoBehaviour
{
    #region Variables
    [SerializeField] private int _stage; // Etapa actual
    [SerializeField] private GameObject _medal; // Medalla a mostrar
    [SerializeField] private Button _diplomaButton; // Botón para activar el diploma
    [SerializeField] private int levelsToComplete = 2; // Cantidad de niveles para completar la etapa
    private int completedLevels = 0; // Contador de niveles completados
    [SerializeField] private TextMeshProUGUI progressText;

    private static SceneProgress instance; // Instancia estática para el singleton
    #endregion

    private void Awake()
    {
        // Implementar el patrón Singleton para que solo haya una instancia de SceneProgress
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Destruir duplicados
        }
    }

    private void Start()
    {
        LoadProgress(); // Cargar progreso al iniciar
        UpdateUI(); // Actualiza la interfaz según el progreso cargado
    }

    #region UpdateLevels

    public void CompleteLevel()
    {
        completedLevels++; // Incrementa el contador de niveles completados
        SaveProgress(); // Guarda el progreso

        // Verifica si se alcanzó el número requerido de niveles completados
        if (completedLevels >= levelsToComplete)
        {
            ActivateMedalAndDiploma();
        }

        UpdateUI(); // Actualiza la interfaz después de completar un nivel
    }

    private void UpdateUI()
    {
        // Muestra la medalla y el botón si el progreso lo requiere
        if (completedLevels >= levelsToComplete)
        {
            ActivateMedalAndDiploma();
        }
        else
        {
            _medal.SetActive(false);
            _diplomaButton.gameObject.SetActive(false);
        }
        // Actualiza el texto de progreso
        progressText.text = $"{completedLevels} / {levelsToComplete}"; // Actualiza el texto
    }

    private void ActivateMedalAndDiploma()
    {
        PlayerPrefs.SetInt($"Medal_Stage{_stage}", 1); // Guarda el estado de la medalla
        _medal.SetActive(true); // Activa la medalla
        _diplomaButton.gameObject.SetActive(true); // Activa el botón de diploma
        PlayerPrefs.Save(); // Guarda los cambios
    }
    #endregion

    #region Progress 
    private void SaveProgress()
    {
        PlayerPrefs.SetInt($"CompletedLevels_Stage{_stage}", completedLevels); // Guarda los niveles completados
        PlayerPrefs.Save(); // Asegúrate de que los datos se guarden
    }

    private void LoadProgress()
    {
        // Carga los niveles completados al iniciar
        completedLevels = PlayerPrefs.GetInt($"CompletedLevels_Stage{_stage}", 0); // 0 es el valor predeterminado si no hay datos
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey($"CompletedLevels_Stage{_stage}"); // Elimina el progreso del nivel actual
        PlayerPrefs.DeleteKey($"Medal_Stage{_stage}"); // Elimina la medalla del nivel actual

        completedLevels = 0; // Reinicia el contador de niveles completados
        _medal.SetActive(false); // Desactiva la medalla
        _diplomaButton.gameObject.SetActive(false); // Desactiva el botón de diploma

        // Actualiza la interfaz de usuario después de reiniciar el progreso
        UpdateUI();
    }
    #endregion
}