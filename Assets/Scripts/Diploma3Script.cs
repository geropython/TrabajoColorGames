using System.IO;
using UnityEngine;

public class Diploma3Script : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _diplomaPanel;
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
    public void CloseDiploma()
    {
        _diplomaPanel.SetActive(false);
    }
    #endregion
}
