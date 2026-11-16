using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private string simulationSceneName = "MainScene";
    [SerializeField] private TextAsset creditsFile; 
    [SerializeField] private TMP_Text creditsText;
    [SerializeField] private ScrollRect creditsScroll;

    public void StartSimulation()
    {
        SceneManager.LoadScene(simulationSceneName);
    }

    public void OpenCredits()
    {
        if (creditsPanel == null) return;

        LoadCredits();
        creditsPanel.SetActive(true);
        Canvas.ForceUpdateCanvases();
        if (creditsScroll != null) creditsScroll.verticalNormalizedPosition = 1f;
    }

    public void CloseCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void LoadCredits()
    {
        if (creditsText == null) return;

        if (creditsFile != null)
            creditsText.text = creditsFile.text;
        else
            creditsText.text = "Credits file not found.";
    }
}