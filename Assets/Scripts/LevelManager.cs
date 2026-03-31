using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    public LevelData[] levels;
    public int currentLevelIndex = 0;

    [Header("Generation")]
    public LevelGenerator levelGenerator;
    public bool useGeneratorAfterHandmadeLevels = true;

    [Header("References")]
    public GameController gameController;
    public BoardManager boardManager;
    public BlockManager blockManager;
    public CameraController cameraController;

    private LevelData cachedGeneratedLevel = null;
    private int cachedGeneratedLevelIndex = -1;

    LevelData GetOrGenerateLevel(int levelIndex)
    {
        if (levelIndex < levels.Length)
            return levels[levelIndex];

        if (!useGeneratorAfterHandmadeLevels || levelGenerator == null)
            return null;

        if (cachedGeneratedLevel != null && cachedGeneratedLevelIndex == levelIndex)
            return cachedGeneratedLevel;

        LevelData saved = ProgressManager.Instance.LoadGeneratedLevel(levelIndex);
        if (saved != null)
        {
            cachedGeneratedLevel = saved;
            cachedGeneratedLevelIndex = levelIndex;
            Debug.Log($"Уровень {levelIndex} восстановлен из сохранения");
            return cachedGeneratedLevel;
        }

        cachedGeneratedLevel = levelGenerator.GenerateLevel();
        cachedGeneratedLevelIndex = levelIndex;
        ProgressManager.Instance.SaveGeneratedLevel(levelIndex, cachedGeneratedLevel);

        return cachedGeneratedLevel;
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0)
            return;

        currentLevelIndex = levelIndex;

        LevelData level = GetOrGenerateLevel(levelIndex);
        if (level == null)
            return;

        boardManager.CreateBoard(level.board);
        blockManager.CreateBlocks(level.blocks);

        if (cameraController != null)
            cameraController.FitToBoard(level.board.width, level.board.height);

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
        cachedGeneratedLevel = null;
        cachedGeneratedLevelIndex = -1;
        ClearLevel();
        LoadLevel(currentLevelIndex);
    }

    public void RestartCurrentLevel()
    {
        ClearLevel();
        LoadLevel(currentLevelIndex);
    }
}