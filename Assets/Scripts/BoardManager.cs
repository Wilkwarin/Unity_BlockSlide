using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject exitCellPrefab;
    public Transform boardParent;

    private CellType[,] grid;
    private GameObject[,] cellObjects;
    private Dictionary<Vector2Int, BlockColor> exitCells = new Dictionary<Vector2Int, BlockColor>();

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

        foreach (var exit in config.exits)
        {
            Vector3 pos = new Vector3(exit.position.x, exit.position.y, 0);
            GameObject exitObj = Instantiate(exitCellPrefab, pos, Quaternion.identity, boardParent);

            SpriteRenderer sr = exitObj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = GetColorFromEnum(exit.color);
            }

            exitCells[exit.position] = exit.color;
        }
    }

    public bool IsCellValid(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= grid.GetLength(0)) return false;
        if (pos.y < 0 || pos.y >= grid.GetLength(1)) return false;
        return grid[pos.x, pos.y] == CellType.Empty || grid[pos.x, pos.y] == CellType.Exit;
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