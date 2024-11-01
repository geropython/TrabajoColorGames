using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    [SerializeField] private Diploma3Script diplomaScript;
    [SerializeField] private GameObject _diplomaPanel;

    public void OnAnimationEnd()
    {
        if (diplomaScript != null)
        {
            Invoke("OpenPanel", 2f);
        }
        else
        {
            Debug.LogWarning("Diploma3Script no asignado en AnimationEventHelper.");
        }
    }
    public void OpenPanel()
    {
        _diplomaPanel.SetActive(true);
    }
}
