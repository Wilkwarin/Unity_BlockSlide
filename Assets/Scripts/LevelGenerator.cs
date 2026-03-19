using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    [Header("Generation Parameters")]
    public int boardWidth = 6;
    public int boardHeight = 6;
    public int numberOfBlocks = 5;
    public int minBlockSize = 1;
    public int maxBlockSize = 3;

    [Header("References")]
    public LevelManager levelManager;

    public LevelData GenerateLevel()
    {
        LevelData level = ScriptableObject.CreateInstance<LevelData>();
        level.levelNumber = levelManager.currentLevelIndex + 1;
        level.levelName = $"Generated Level {level.levelNumber}";
        level.board = GenerateBoard();
        level.blocks = GenerateBlocks();

        return level;
    }

    List<Vector2Int> GetOccupiedCells(Vector2Int position, Vector2Int[] shape)
    {
        List<Vector2Int> cells = new List<Vector2Int>();
        foreach (var offset in shape)
        {
            cells.Add(position + offset);
        }
        return cells;
    }

    bool IsWithinBoard(Vector2Int position, Vector2Int[] shape)
    {
        foreach (var offset in shape)
        {
            Vector2Int cell = position + offset;
            if (cell.x < 0 || cell.x >= boardWidth || cell.y < 0 || cell.y >= boardHeight)
                return false;
        }
        return true;
    }

    bool HasCollision(Vector2Int position, Vector2Int[] shape, HashSet<Vector2Int> occupiedCells)
    {
        foreach (var offset in shape)
        {
            Vector2Int cell = position + offset;
            if (occupiedCells.Contains(cell))
                return true;
        }
        return false;
    }

    LevelData.BoardConfiguration GenerateBoard()
    {
        LevelData.BoardConfiguration board = new LevelData.BoardConfiguration();
        board.width = boardWidth;
        board.height = boardHeight;
        board.cells = new CellType[boardWidth * boardHeight];

        for (int i = 0; i < board.cells.Length; i++)
        {
            board.cells[i] = CellType.Empty;
        }

        List<LevelData.ExitCellData> exits = new List<LevelData.ExitCellData>();

        BlockColor[] colors = new BlockColor[]
        {
            BlockColor.Red, BlockColor.Blue, BlockColor.Green, BlockColor.Yellow
        };

        // Верхняя стенка
        exits.Add(new LevelData.ExitCellData
        {
            position = new Vector2Int(boardWidth / 2, boardHeight),
            color = colors[0],
            orientation = ExitOrientation.Horizontal,
            size = 2
        });

        // Правая стенка
        exits.Add(new LevelData.ExitCellData
        {
            position = new Vector2Int(boardWidth, boardHeight / 2),
            color = colors[1],
            orientation = ExitOrientation.Vertical,
            size = 2
        });

        // Нижняя стенка
        exits.Add(new LevelData.ExitCellData
        {
            position = new Vector2Int(boardWidth / 2, -1),
            color = colors[2],
            orientation = ExitOrientation.Horizontal,
            size = 2
        });

        // Левая стенка
        exits.Add(new LevelData.ExitCellData
        {
            position = new Vector2Int(-1, boardHeight / 2),
            color = colors[3],
            orientation = ExitOrientation.Vertical,
            size = 2
        });

        board.exits = exits.ToArray();

        return board;
    }

    LevelData.BlockConfiguration[] GenerateBlocks()
    {
        List<LevelData.BlockConfiguration> blocks = new List<LevelData.BlockConfiguration>();
        HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();

        BlockColor[] colors = new BlockColor[]
        {
        BlockColor.Red, BlockColor.Blue, BlockColor.Green, BlockColor.Yellow
        };

        int maxAttempts = 100;

        for (int i = 0; i < numberOfBlocks; i++)
        {
            int blockSize = Random.Range(minBlockSize, maxBlockSize + 1);
            List<BlockShape> shapesOfSize = BlockShapeLibrary.GetShapeEnumsBySize(blockSize);
            BlockShape chosenShapeEnum = shapesOfSize[Random.Range(0, shapesOfSize.Count)];
            Vector2Int[] shapeCoords = BlockShapeLibrary.GetShapeByEnum(chosenShapeEnum);

            bool placed = false;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Vector2Int candidate = new Vector2Int(
                    Random.Range(0, boardWidth),
                    Random.Range(0, boardHeight)
                );

                if (!IsWithinBoard(candidate, shapeCoords))
                    continue;

                if (HasCollision(candidate, shapeCoords, occupiedCells))
                    continue;

                LevelData.BlockConfiguration block = new LevelData.BlockConfiguration();
                block.blockID = i;
                block.color = colors[i % colors.Length];
                block.shape = chosenShapeEnum;
                block.startPosition = candidate;

                blocks.Add(block);

                foreach (var offset in shapeCoords)
                {
                    occupiedCells.Add(candidate + offset);
                }

                placed = true;
                break;
            }

            if (!placed)
            {
                Debug.LogWarning($"Блок {i} не удалось разместить за {maxAttempts} попыток — пропускаем.");
            }
        }

        return blocks.ToArray();
    }
}