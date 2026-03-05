using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject exitCellPrefab;
    public GameObject blockedCellPrefab;
    public Transform boardParent;

    private CellType[,] grid;
    private GameObject[,] cellObjects;
    private Dictionary<Vector2Int, BlockColor> exitCells = new Dictionary<Vector2Int, BlockColor>();

    public enum CornerType
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public void CreateBoard(LevelData.BoardConfiguration config)
    {
        grid = new CellType[config.width, config.height];
        cellObjects = new GameObject[config.width, config.height];

        for (int y = 0; y < config.height; y++)
        {
            for (int x = 0; x < config.width; x++)
            {
                int index = y * config.width + x;
                grid[x, y] = config.cells[index];

                if (grid[x, y] != CellType.Blocked)
                {
                    Vector3 pos = new Vector3(x, y, 0);
                    GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity, boardParent);
                    cellObjects[x, y] = cell;

                    SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.color = new Color(0.9f, 0.9f, 0.9f);
                    }
                }
            }
        }

        Debug.Log($"=== Создано выходов: {config.exits.Length} ===");

        CreateBorder(config.width, config.height);

        foreach (var exit in config.exits)
        {
            CreateExit(exit, config.width, config.height);
        }
    }

    void CreateBorder(int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            CreateBlockedCell(x, height, true);
            CreateBlockedCell(x, -1, true);
        }

        for (int y = 0; y < height; y++)
        {
            CreateBlockedCell(-1, y, false);
            CreateBlockedCell(width, y, false);
        }

        CheckAndCreateCorner(-1, -1, width, height);      // Левый нижний
        CheckAndCreateCorner(width, -1, width, height);   // Правый нижний
        CheckAndCreateCorner(-1, height, width, height);  // Левый верхний
        CheckAndCreateCorner(width, height, width, height); // Правый верхний
    }

    void CheckAndCreateCorner(int x, int y, int width, int height)
    {
        CornerType cornerType;

        if (x < 0 && y < 0)
            cornerType = CornerType.BottomLeft;
        else if (x >= width && y < 0)
            cornerType = CornerType.BottomRight;
        else if (x < 0 && y >= height)
            cornerType = CornerType.TopLeft;
        else if (x >= width && y >= height)
            cornerType = CornerType.TopRight;
        else
            return;

        CreateCornerCell(x, y, cornerType);
    }

    void CreateCornerCell(int x, int y, CornerType cornerType)
    {
        Vector3 pos = new Vector3(x, y, 0);
        GameObject corner = Instantiate(blockedCellPrefab, pos, Quaternion.identity, boardParent);
        corner.name = $"Corner_{cornerType}_{x}_{y}";

        corner.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        switch (cornerType)
        {
            case CornerType.BottomLeft:
                corner.transform.position = new Vector3(-0.75f, -0.75f, 0);
                break;

            case CornerType.BottomRight:
                corner.transform.position = new Vector3(x - 0.25f, -0.75f, 0);
                break;

            case CornerType.TopLeft:
                corner.transform.position = new Vector3(-0.75f, y - 0.25f, 0);
                break;

            case CornerType.TopRight:
                corner.transform.position = new Vector3(x - 0.25f, y - 0.25f, 0);
                break;
        }
    }

    void CreateBlockedCell(int x, int y, bool isHorizontal)
    {
        Vector3 pos = new Vector3(x, y, 0);
        GameObject blocked = Instantiate(blockedCellPrefab, pos, Quaternion.identity, boardParent);
        blocked.name = $"BlockedCell_{x}_{y}";

        if (isHorizontal)
        {
            blocked.transform.localScale = new Vector3(1f, 0.5f, 1f);

            if (y < 0)
            {
                blocked.transform.position = new Vector3(x, -0.75f, 0);
            }
            else
            {
                blocked.transform.position = new Vector3(x, y - 0.25f, 0);
            }
        }
        else
        {
            blocked.transform.localScale = new Vector3(0.5f, 1f, 1f);

            if (x < 0)
            {
                blocked.transform.position = new Vector3(-0.75f, y, 0);
            }
            else
            {
                blocked.transform.position = new Vector3(x - 0.25f, y, 0);
            }
        }
    }

    void CreateExit(LevelData.ExitCellData exit, int width, int height)
    {
        Vector3 pos = new Vector3(exit.position.x, exit.position.y, 0);
        GameObject exitObj = Instantiate(exitCellPrefab, pos, Quaternion.identity, boardParent);
        exitObj.name = $"Exit_{exit.color}_{exit.position.x}_{exit.position.y}";

        bool isHorizontal = (exit.position.y == -1 || exit.position.y == height);

        if (isHorizontal)
        {
            exitObj.transform.localScale = new Vector3(1f, 0.5f, 1f);

            if (exit.position.y < 0)
            {
                exitObj.transform.position = new Vector3(exit.position.x, -0.75f, 0);
            }
            else
            {
                exitObj.transform.position = new Vector3(exit.position.x, height - 0.25f, 0);
            }
        }
        else
        {
            exitObj.transform.localScale = new Vector3(0.5f, 1f, 1f);

            if (exit.position.x < 0)
            {
                exitObj.transform.position = new Vector3(-0.75f, exit.position.y, 0);
            }
            else
            {
                exitObj.transform.position = new Vector3(width - 0.25f, exit.position.y, 0);
            }
        }

        SpriteRenderer sr = exitObj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.black;
        }

        Transform inner = exitObj.transform.Find("Inner");
        if (inner != null)
        {
            SpriteRenderer innerSr = inner.GetComponent<SpriteRenderer>();
            if (innerSr != null)
            {
                innerSr.color = GetColorFromEnum(exit.color);
            }
        }

        exitCells[exit.position] = exit.color;
    }

    public bool IsCellValid(Vector2Int pos)
    {
        bool withinBounds = (pos.x >= 0 && pos.x < grid.GetLength(0) &&
                             pos.y >= 0 && pos.y < grid.GetLength(1));

        if (!withinBounds)
        {
            return false;
        }

        bool isValidType = (grid[pos.x, pos.y] == CellType.Empty || grid[pos.x, pos.y] == CellType.Exit);
        return isValidType;
    }

    public bool IsExitCell(Vector2Int pos, BlockColor blockColor)
    {
        if (exitCells.ContainsKey(pos))
        {
            return exitCells[pos] == blockColor;
        }
        return false;
    }

    public void ClearBoard()
    {
        foreach (var cell in cellObjects)
        {
            if (cell != null)
                Destroy(cell);
        }

        exitCells.Clear();
    }

    Color GetColorFromEnum(BlockColor blockColor)
    {
        switch (blockColor)
        {
            case BlockColor.Red: return new Color(1f, 0f, 0.439f);
            case BlockColor.Blue: return new Color(0f, 0f, 1f);
            case BlockColor.Green: return new Color(0f, 1f, 0f);
            case BlockColor.Yellow: return new Color(1f, 1f, 0f);
            case BlockColor.Orange: return new Color(1f, 0.647f, 0f);
            case BlockColor.Purple: return new Color(0.502f, 0f, 1f);
            case BlockColor.Cyan: return new Color(0f, 1f, 1f);
            case BlockColor.Pink: return new Color(1f, 0f, 1f);
            default: return Color.white;
        }
    }
}