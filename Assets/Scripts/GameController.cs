using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Game References")]
    public BlockManager blockManager;

    [Header("UI References")]
    public GameObject winPanel;
    public GameObject winText;
    public GameObject nextLevelButtonWinPanel;
    public GameObject menuButton;
    public GameObject restartButton;
    public GameObject nextLevelButtonCanvas;
    public MenuManager menuManager;

    private bool levelCompleted = false;
    public bool isBusy = false;

    void Start()
    {
        HideAllButtons();
    }

    void Update()
    {
        if (levelCompleted || isBusy)
            return;

        if (blockManager != null)
        {
            blockManager.HandleInput();
        }
    }

    public void OnLevelStarted()
    {
        if (menuButton != null)
            menuButton.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(true);

        if (nextLevelButtonCanvas != null)
            nextLevelButtonCanvas.SetActive(true);

        if (nextLevelButtonWinPanel != null)
            nextLevelButtonWinPanel.SetActive(false);

        if (winText != null)
            winText.SetActive(false);
    }

    public void CheckWinCondition()
    {
        if (levelCompleted)
            return;

        if (blockManager != null && blockManager.GetRemainingBlocksCount() == 0)
        {
            OnLevelComplete();
        }
    }

    void OnLevelComplete()
    {
        levelCompleted = true;

        LevelManager levelManager = FindFirstObjectByType<LevelManager>();
        if (levelManager != null)
        {
            ProgressManager.Instance.SaveCurrentLevel(levelManager.currentLevelIndex + 1);
        }

        if (nextLevelButtonCanvas != null)
            nextLevelButtonCanvas.SetActive(false);

        if (nextLevelButtonWinPanel != null)
            nextLevelButtonWinPanel.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(true);

        if (winText != null)
            winText.SetActive(true);
    }

    public void RestartLevel()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        levelCompleted = false;
        isBusy = false;

        OnLevelStarted();

        LevelManager levelManager = FindFirstObjectByType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.RestartCurrentLevel();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void OnNextLevelButtonPressed()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        levelCompleted = false;
        isBusy = false;

        OnLevelStarted();

        LevelManager levelManager = FindFirstObjectByType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.NextLevel();
        }
    }

    public void OnMenuButtonPressed()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        levelCompleted = false;
        isBusy = false;

        HideAllButtons();

        LevelManager lm = FindFirstObjectByType<LevelManager>();
        if (lm != null)
        {
            lm.ClearLevel();
        }

        menuManager.ShowMenu();
    }

    void HideAllButtons()
    {
        if (menuButton != null)
            menuButton.SetActive(false);

        if (restartButton != null)
            restartButton.SetActive(false);

        if (nextLevelButtonCanvas != null)
            nextLevelButtonCanvas.SetActive(false);

        if (nextLevelButtonWinPanel != null)
            nextLevelButtonWinPanel.SetActive(false);

        if (winText != null)
            winText.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);
    }
}