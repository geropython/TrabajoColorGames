using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Diploma5Script : MonoBehaviour
{
    [SerializeField] private GameObject _diploma;
    [SerializeField] private RectTransform diplomaRectTransform; // Agregar el RectTransform del diploma
    private string _filePath;

    private void Start()
    {
        // Cargar la imagen del diploma desde Resources y guardarla temporalmente
        Texture2D diplomaTexture = Resources.Load<Texture2D>("Diploma5");
        if (diplomaTexture != null)
        {
            Debug.Log("Diploma3 cargado correctamente desde Resources.");
            // Convertir la textura a bytes PNG y guardarla temporalmente
            _filePath = Path.Combine(Application.temporaryCachePath, "Diploma5.png");
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
        StartCoroutine(CaptureDiploma());
    }

    private IEnumerator CaptureDiploma()
    {
        yield return new WaitForEndOfFrame();

        // Obtener las dimensiones y posición del diploma en píxeles
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, diplomaRectTransform.position);
        int width = (int)diplomaRectTransform.rect.width;
        int height = (int)diplomaRectTransform.rect.height;
        int x = (int)(screenPoint.x - width / 2);
        int y = (int)(screenPoint.y - height / 2);

        // Capturar solo el área del diploma
        Texture2D screenTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(x, y, width, height), 0, 0);
        screenTexture.Apply();

        // Guardar la imagen temporalmente
        _filePath = Path.Combine(Application.temporaryCachePath, "Diploma5_Captured.png");
        File.WriteAllBytes(_filePath, screenTexture.EncodeToPNG());
        Debug.Log("Captura del diploma guardada temporalmente en: " + _filePath);

        // Compartir la imagen usando NativeShare
        new NativeShare()
            .AddFile(_filePath)
            .SetText("¡He conseguido un diploma!")
            .SetSubject("Mira mi logro")
            .Share();

        // Limpiar la textura
        Destroy(screenTexture);
    }
}