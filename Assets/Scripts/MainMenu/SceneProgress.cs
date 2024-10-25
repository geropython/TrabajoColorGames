using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneProgress : MonoBehaviour
{
    #region Variables
    [SerializeField] private int _stage; // Etapa actual
    [SerializeField] private GameObject _medal;
    [SerializeField] private Button _diplomaButton;
    [SerializeField] private int levelsToComplete = 2; // Cantidad de niveles para completar la etapa
    private int completedLevels = 0;

    #endregion

    private void Start()
    {
        _medal.SetActive(false);
        _diplomaButton.gameObject.SetActive(false);
    }

    public void CompleteLevel()
    {
        completedLevels++;

        if (completedLevels >= levelsToComplete)
        {
            CompleteStage();
        }
    }

    private void CompleteStage()
    {
        PlayerPrefs.SetInt($"Medal_Stage{_stage}", 1);
        _medal.SetActive(true);
        _diplomaButton.gameObject.SetActive(true);
        PlayerPrefs.Save();
    }
}