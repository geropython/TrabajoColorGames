using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Diploma5Script : MonoBehaviour
{
     #region Variables
    [SerializeField] private RectTransform diplomaRectTransform;
    [SerializeField] private Animator OpenChestButtonAnimator;
    [SerializeField] private AudioSource sound;
    private string _filePath;
    #endregion

    public void OnButtonClicked()
    {
        OpenChestButtonAnimator.SetTrigger("Click");
        sound.Play();
    }
    
    #region Diploma
    public void ShareDiploma()
    {
        Texture2D screenTexture = ScreenCapture.CaptureScreenshotAsTexture();
        _filePath = Path.Combine(Application.temporaryCachePath, "Diploma5_Captured.png");

        try
        {
            File.WriteAllBytes(_filePath, screenTexture.EncodeToPNG());
            Debug.Log("Captura del diploma guardada temporalmente en: " + _filePath);
        }
        catch (IOException e)
        {
            Debug.LogError("Error al guardar el diploma: " + e.Message);
        }

        new NativeShare()
            .AddFile(_filePath)
            .SetText("¡He conseguido un diploma!")
            .SetSubject("Mira mi logro")
            .Share();

        Destroy(screenTexture);
    }
    #endregion
}