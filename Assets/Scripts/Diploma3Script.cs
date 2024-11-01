using System.Collections;
using UnityEngine;
using System.IO;

public class Diploma3Script : MonoBehaviour
{
    #region Variables
    [SerializeField] private RectTransform diplomaRectTransform;
    [SerializeField] private Animator OpenChestButtonAnimator;
    [SerializeField] private AudioSource sound;
    private string _filePath;
    #endregion

    public void OnButtonClicked()
    {
        StartCoroutine(HandleButtonClick());   
    }
    private IEnumerator HandleButtonClick()
    {
        OpenChestButtonAnimator.SetTrigger("Click");
        sound.Play();

        float animationDuration = 1.0f;
        yield return new WaitForSeconds(animationDuration);
    }

    #region Diploma

    public void ShareDiploma()
    {
        StartCoroutine(CaptureDiploma());
    }

    private IEnumerator CaptureDiploma()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenTexture = ScreenCapture.CaptureScreenshotAsTexture();
        _filePath = Path.Combine(Application.temporaryCachePath, "Diploma3_Captured.png");

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