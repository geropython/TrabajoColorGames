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
        StartCoroutine(CaptureAndShare());
    }

    private IEnumerator CaptureAndShare()
    {
        // Espera a que termine el frame para asegurar que la UI esté actualizada
        yield return new WaitForEndOfFrame();

        // Capturar la pantalla
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();

        // Guardar la imagen en el dispositivo
        string filePath = Path.Combine(Application.temporaryCachePath, "Diploma5.png");
        File.WriteAllBytes(filePath, screenTexture.EncodeToPNG());

        // Liberar la textura de la memoria
        Destroy(screenTexture);

        // Abrir WhatsApp para compartir la imagen
        string whatsappUrl = $"whatsapp://send?text=¡Mira mi diploma!";
        Application.OpenURL(whatsappUrl);

        // Agregar la imagen al mensaje
        new NativeShare().AddFile(filePath).SetText("¡He conseguido un diploma!").Share();
    }
}