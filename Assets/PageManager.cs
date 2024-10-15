using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    [SerializeField] private GameObject _pagina1, _pagina2, _pagina3;

    void Start()
    {
        _pagina1.SetActive(true);
        _pagina2.SetActive(false);
        _pagina3.SetActive(false);
    }

    public void Pagina1()
    {
        _pagina1.SetActive(true);
        _pagina2.SetActive(false);
        _pagina3.SetActive(false);
    }
    public void Pagina2()
    {
        _pagina1.SetActive(false);
        _pagina2.SetActive(true);
        _pagina3.SetActive(false);
    }
    public void Pagina3()
    {
        _pagina1.SetActive(false);
        _pagina2.SetActive(false);
        _pagina3.SetActive(true);
    }
}