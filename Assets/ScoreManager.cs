using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int puntajeTotal;

    public ScoreManager()
    {
        puntajeTotal = 0;
    }

    public void AgregarPuntaje(int cantidad)
    {
        puntajeTotal += cantidad;
    }

    public int GetPuntaje()
    {
        return puntajeTotal;
    }
}
