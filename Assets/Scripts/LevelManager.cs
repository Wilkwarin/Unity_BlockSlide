using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    public LevelData[] levels;
    public int currentLevelIndex = 0;

    [Header("References")]
    public GameController gameController;
    public BoardManager boardManager;
    public BlockManager blockManager;

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogError($"Уровень {levelIndex} не существует!");
            return;
        }

        currentLevelIndex = levelIndex;
        LevelData level = levels[levelIndex];

        boardManager.CreateBoard(level.board);
        blockManager.CreateBlocks(level.blocks);

        Debug.Log($"Загружен уровень {levelIndex + 1}");
    }

    public void ClearLevel()
    {
        boardManager.ClearBoard();
        blockManager.ClearBlocks();
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        
        if (currentLevelIndex < levels.Length)
        {
            ClearLevel();
            LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("Все уровни пройдены!");
        }
    }

    public void RestartCurrentLevel()
    {
        ClearLevel();
        LoadLevel(currentLevelIndex);
    }
}