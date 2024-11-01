using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHelper4 : MonoBehaviour
{
    [SerializeField] private Diploma4Scripts diplomaScript;
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
            Debug.LogWarning("Diploma4Scripts no asignado en AnimationEventHelper.");
        }
    }
    private void OpenPanel()
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
