using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDiploma : MonoBehaviour
{
    [SerializeField] private GameObject _diploma;
    private void Start()
    {
        _diploma.SetActive(false); // Asegúrate de que el diploma esté desactivado al inicio
    }
    public void ShowDiploma()
    {
        _diploma.SetActive(true);
    }

}