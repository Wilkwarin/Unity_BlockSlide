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
        public ExitOrientation orientation;
        public int size = 2;
    }

    [System.Serializable]
    public class BlockConfiguration
    {
        public BlockColor color;
        public BlockShape shape;
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
    Pink,
    White,
    Black,
    Scarlet,
    Brown,
    Gray
}

public enum BlockShape
{
    Single,
    Stick_2x1,
    Stick_1x2,
    Stick_3x1,
    Stick_1x3,
    L_BottomLeft,
    L_BottomRight,
    L_TopLeft,
    L_TopRight,
    Square_2x2,
    L4_0deg,
    L4_90deg,
    L4_180deg,
    L4_270deg,
    L4_Mirror_0deg,
    L4_Mirror_90deg,
    L4_Mirror_180deg,
    L4_Mirror_270deg,
    Z_Horizontal,
    Z_Vertical,
    Z_Mirror_Horizontal,
    Z_Mirror_Vertical,
    T_0deg,
    T_90deg,
    T_180deg,
    T_270deg,
    Cross,
    C_Right,
    C_Left,
    C_Up,
    C_Down
}