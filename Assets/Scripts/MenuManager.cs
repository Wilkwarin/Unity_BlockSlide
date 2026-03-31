using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject menuPanel;
    public TextMeshProUGUI continueButtonText;
    public TextMeshProUGUI inGameLevelText;
    public GameObject inGameLevelRoot;

    [Header("References")]
    public LevelManager levelManager;
    public GameController gameController;

    void Start()
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
        }

        if (inGameLevelRoot != null)
            inGameLevelRoot.SetActive(false);
        else if (inGameLevelText != null)
            inGameLevelText.gameObject.SetActive(false);

        UpdateContinueButton();
    }

    public void HideMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }

        if (inGameLevelRoot != null)
        {
            inGameLevelRoot.SetActive(true);
            RefreshLevelDisplay();
        }
        else if (inGameLevelText != null)
        {
            inGameLevelText.gameObject.SetActive(true);
            RefreshLevelDisplay();
        }

        GameController gc = Object.FindFirstObjectByType<GameController>();
        if (gc != null && gc.restartButton != null)
        {
            gc.restartButton.SetActive(true);
        }
    }

    public void RefreshLevelDisplay()
    {
        int currentLevel = (levelManager != null)
            ? levelManager.currentLevelIndex
            : ProgressManager.Instance.GetCurrentLevel();

        if (inGameLevelText != null)
        {
            inGameLevelText.text = $"Level {currentLevel + 1}";
        }
    }

    void UpdateContinueButton()
    {
        int currentLevel = ProgressManager.Instance.GetCurrentLevel();
        if (continueButtonText != null)
        {
            continueButtonText.text = $"{currentLevel + 1}";
        }
    }

    public void OnContinueButtonPressed()
    {
        int currentLevel = ProgressManager.Instance.GetCurrentLevel();

        if (levelManager != null)
        {
            levelManager.LoadLevel(currentLevel);
        }

        HideMenu();

        GameController gc = FindFirstObjectByType<GameController>();
        if (gc != null)
        {
            gc.OnLevelStarted();
        }
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}