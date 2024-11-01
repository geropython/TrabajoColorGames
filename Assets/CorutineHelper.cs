using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorutineHelper : MonoBehaviour
{
   private static CorutineHelper _instance;

    public static CorutineHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("CorutineHelper");
                _instance = go.AddComponent<CorutineHelper>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public void StartCoroutineHelper(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
