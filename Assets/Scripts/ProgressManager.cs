using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    private const string CURRENT_LEVEL_KEY = "CurrentLevel";
    private const string GENERATED_LEVEL_KEY = "GeneratedLevel";
    private const string GENERATED_LEVEL_INDEX_KEY = "GeneratedLevelIndex";

    public static ProgressManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 0);
    }

    public void SaveCurrentLevel(int levelIndex)
    {
        PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, levelIndex);
        PlayerPrefs.Save();
        Debug.Log($"Сохранён уровень {levelIndex}");
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(CURRENT_LEVEL_KEY);
        PlayerPrefs.DeleteKey(GENERATED_LEVEL_KEY);
        PlayerPrefs.DeleteKey(GENERATED_LEVEL_INDEX_KEY);
        PlayerPrefs.Save();
        Debug.Log("Прогресс сброшен");
    }

    public void SaveGeneratedLevel(int levelIndex, LevelData levelData)
    {
        LevelDataJson dto = LevelDataJson.FromLevelData(levelData);
        string json = JsonUtility.ToJson(dto);
        PlayerPrefs.SetString(GENERATED_LEVEL_KEY, json);
        PlayerPrefs.SetInt(GENERATED_LEVEL_INDEX_KEY, levelIndex);
        PlayerPrefs.Save();
    }

    public LevelData LoadGeneratedLevel(int levelIndex)
    {
        if (!PlayerPrefs.HasKey(GENERATED_LEVEL_KEY))
            return null;

        int savedIndex = PlayerPrefs.GetInt(GENERATED_LEVEL_INDEX_KEY, -1);
        if (savedIndex != levelIndex)
            return null;

        string json = PlayerPrefs.GetString(GENERATED_LEVEL_KEY, "");
        if (string.IsNullOrEmpty(json))
            return null;

        try
        {
            LevelDataJson dto = JsonUtility.FromJson<LevelDataJson>(json);
            return dto.ToLevelData();
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    public void ClearGeneratedLevel()
    {
        PlayerPrefs.DeleteKey(GENERATED_LEVEL_KEY);
        PlayerPrefs.DeleteKey(GENERATED_LEVEL_INDEX_KEY);
        PlayerPrefs.Save();
    }

    [System.Serializable]
    private class LevelDataJson
    {
        public int levelNumber;
        public string levelName;
        public BoardJson board;
        public BlockJson[] blocks;

        public static LevelDataJson FromLevelData(LevelData src)
        {
            var dst = new LevelDataJson();
            dst.levelNumber = src.levelNumber;
            dst.levelName   = src.levelName;

            dst.board = new BoardJson();
            dst.board.width  = src.board.width;
            dst.board.height = src.board.height;
            dst.board.cells  = System.Array.ConvertAll(src.board.cells, c => (int)c);

            dst.board.exits = new ExitJson[src.board.exits.Length];
            for (int i = 0; i < src.board.exits.Length; i++)
            {
                var e = src.board.exits[i];
                dst.board.exits[i] = new ExitJson
                {
                    px          = e.position.x,
                    py          = e.position.y,
                    color       = (int)e.color,
                    orientation = (int)e.orientation,
                    size        = e.size
                };
            }

            dst.blocks = new BlockJson[src.blocks.Length];
            for (int i = 0; i < src.blocks.Length; i++)
            {
                var b = src.blocks[i];
                dst.blocks[i] = new BlockJson
                {
                    color = (int)b.color,
                    shape = (int)b.shape,
                    px    = b.startPosition.x,
                    py    = b.startPosition.y
                };
            }

            return dst;
        }

        public LevelData ToLevelData()
        {
            LevelData dst = ScriptableObject.CreateInstance<LevelData>();
            dst.levelNumber = levelNumber;
            dst.levelName   = levelName;

            dst.board = new LevelData.BoardConfiguration();
            dst.board.width  = board.width;
            dst.board.height = board.height;
            dst.board.cells  = System.Array.ConvertAll(board.cells, c => (CellType)c);

            dst.board.exits = new LevelData.ExitCellData[board.exits.Length];
            for (int i = 0; i < board.exits.Length; i++)
            {
                var e = board.exits[i];
                dst.board.exits[i] = new LevelData.ExitCellData
                {
                    position    = new Vector2Int(e.px, e.py),
                    color       = (BlockColor)e.color,
                    orientation = (ExitOrientation)e.orientation,
                    size        = e.size
                };
            }

            dst.blocks = new LevelData.BlockConfiguration[blocks.Length];
            for (int i = 0; i < blocks.Length; i++)
            {
                var b = blocks[i];
                dst.blocks[i] = new LevelData.BlockConfiguration
                {
                    color         = (BlockColor)b.color,
                    shape         = (BlockShape)b.shape,
                    startPosition = new Vector2Int(b.px, b.py)
                };
            }

            return dst;
        }
    }

    [System.Serializable]
    private class BoardJson
    {
        public int width;
        public int height;
        public int[] cells;
        public ExitJson[] exits;
    }

    [System.Serializable]
    private class ExitJson
    {
        public int px, py;
        public int color;
        public int orientation;
        public int size;
    }

    [System.Serializable]
    private class BlockJson
    {
        public int color;
        public int shape;
        public int px, py;
    }

#if UNITY_EDITOR
    [ContextMenu("Reset Progress")]
    private void DebugResetProgress()
    {
        ResetProgress();
    }
#endif
}