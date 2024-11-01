using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHelper4 : MonoBehaviour
{
    [SerializeField] private Diploma4Scripts diplomaScript;
    [SerializeField] private GameObject _diplomaPanel;

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
        _diplomaPanel.SetActive(true);
    }
}
