using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    [SerializeField] private Diploma3Script diplomaScript;
    [SerializeField] private GameObject _diplomaPanel;
    [SerializeField] private Animator _diplomaAnimator;

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
        _diplomaPanel.SetActive(true); // Activa el panel
        if (_diplomaAnimator != null)
        {
            _diplomaAnimator.SetTrigger("Show"); // Activa la animación con el Trigger "Show"
        }
        else
        {
            Debug.LogWarning("Animator no asignado al panel de diploma.");
        }
    }
}
