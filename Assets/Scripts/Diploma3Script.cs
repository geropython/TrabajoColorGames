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

    private void Start()
    {
        // Asegurarse de que el panel y la animación se reinicien al inicio
        _diplomaPanel.SetActive(false);
        OpenChestButtonAnimator.ResetTrigger("Click"); // Reiniciar el trigger al inicio
    }

    public void OnButtonClicked()
    {
        OpenChestButtonAnimator.SetTrigger("Click"); // Activar el trigger
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
        // Restablecer el estado del trigger al cerrar el diploma para que esté listo para la próxima vez
        OpenChestButtonAnimator.ResetTrigger("Click");
    }
    #endregion
}