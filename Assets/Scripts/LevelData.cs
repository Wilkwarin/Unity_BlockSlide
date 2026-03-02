using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "BlockSlide/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level Info")]
    public int levelNumber;
    public string levelName = "Level 1";

    [Header("Board Configuration")]
    public BoardConfiguration board;

    [Header("Blocks Configuration")]
    public BlockConfiguration[] blocks;

    [System.Serializable]
    public class BoardConfiguration
    {
        public int width;
        public int height;
        public CellType[] cells;
        public ExitCellData[] exits;
    }

    [System.Serializable]
    public class ExitCellData
    {
        public Vector2Int position;
        public BlockColor color;
    }

    [System.Serializable]
    public class BlockConfiguration
    {
        public int blockID;
        public BlockColor color;
        public Vector2Int[] shape;
        public Vector2Int startPosition;
    }
}

public enum CellType
{
    Empty,
    Blocked,
    Exit
}

public enum BlockColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Orange,
    Purple,
    Cyan,
    Pink
}