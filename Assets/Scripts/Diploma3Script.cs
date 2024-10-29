using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Diploma3Script : MonoBehaviour
{
    [SerializeField] private GameObject _diploma;
    private string _filePath;

    private void Start()
    {   // Cargar la imagen del diploma desde Resources y guardarla temporalmente
        Texture2D diplomaTexture = Resources.Load<Texture2D>("Diploma3");
        if (diplomaTexture != null)
        {
            Debug.Log("Diploma3 cargado correctamente desde Resources.");
            // Convertir la textura a bytes PNG y guardarla temporalmente
            _filePath = Path.Combine(Application.temporaryCachePath, "Diploma3.png");
            File.WriteAllBytes(_filePath, diplomaTexture.EncodeToPNG());
            Debug.Log("Diploma guardado temporalmente en: " + _filePath);
        }
        else
        {
            Debug.LogError("No se pudo cargar la imagen del diploma desde Resources.");
        }
    }

    public void DiplomaClose()
    {
        _diploma.SetActive(false);
    }

    public void DiplomaOpen()
    {
        _diploma.SetActive(true);
    }

    public void ShareDiploma()
    {
        if (!string.IsNullOrEmpty(_filePath) && File.Exists(_filePath))
        {
            // Compartir la imagen usando NativeShare
            new NativeShare()
                .AddFile(_filePath)
                .SetText("¡He conseguido un diploma!")
                .SetSubject("Mira mi logro")
                .Share();
        }
        else
        {
            Debug.LogError("El archivo de la imagen del diploma no se encontró.");
        }
    }

    public void DownloadDiploma()
    {
        string downloadPath;
        #if UNITY_ANDROID
            // Ruta pública de descargas en Android
            downloadPath = Path.Combine("/storage/emulated/0/Download", "Diploma3.png");
        #else
            //Ruta de descarga en otras plataformas (ej., iOS)
            downloadPath = Path.Combine(Application.persistentDataPath, "Diploma3.png");
        #endif

        if (!string.IsNullOrEmpty(_filePath) && File.Exists(_filePath))
        {
            File.Copy(_filePath, downloadPath, true);
            Debug.Log("Diploma descargado en: " + downloadPath);
        }
        else
        {
            Debug.LogError("No se pudo descargar la imagen del diploma.");
        }
    }
}