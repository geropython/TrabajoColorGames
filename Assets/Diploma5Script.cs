using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Diploma5Script : MonoBehaviour
{
    [SerializeField] private GameObject _diploma;

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
        // Cargar la imagen del diploma desde Resources
        Texture2D diplomaTexture = Resources.Load<Texture2D>("Diploma5"); // Nombre sin extensión

        if (diplomaTexture != null)
        {
            // Convertir la textura a bytes PNG y guardarla temporalmente
            string filePath = Path.Combine(Application.temporaryCachePath, "Diploma5.png");
            File.WriteAllBytes(filePath, diplomaTexture.EncodeToPNG());

            // Compartir la imagen usando NativeShare
            new NativeShare()
                .AddFile(filePath)
                .SetText("¡He conseguido un diploma!")
                .SetSubject("Mira mi logro")
                .Share();
        }
        else
        {
            Debug.LogError("No se pudo cargar la imagen del diploma desde Resources.");
        }
    }
}