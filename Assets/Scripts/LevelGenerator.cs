using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    [Header("Generation Parameters")]
    public int boardWidth = 6;
    public int boardHeight = 6;
    public int numberOfBlocks = 5;

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
            color = colors[0]
        });

        // Правая стенка
        exits.Add(new LevelData.ExitCellData
        {
            position = new Vector2Int(boardWidth, boardHeight / 2),
            color = colors[1]
        });

        // Нижняя стенка
        exits.Add(new LevelData.ExitCellData
        {
            position = new Vector2Int(boardWidth / 2, -1),
            color = colors[2]
        });

        // Левая стенка
        exits.Add(new LevelData.ExitCellData
        {
            position = new Vector2Int(-1, boardHeight / 2),
            color = colors[3]
        });

        board.exits = exits.ToArray();

        return board;
    }

    LevelData.BlockConfiguration[] GenerateBlocks()
    {
        List<LevelData.BlockConfiguration> blocks = new List<LevelData.BlockConfiguration>();

        BlockColor[] colors = new BlockColor[]
        {
            BlockColor.Red, BlockColor.Blue, BlockColor.Green, BlockColor.Yellow
        };

        // Простые формы блоков
        Vector2Int[][] shapes = new Vector2Int[][]
        {
            // Квадрат 2x2
            new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) },
            
            // Горизонтальная линия 3x1
            new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0) },
            
            // Вертикальная линия 1x3
            new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2) },
            
            // L-форма
            new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0) },
        };

        for (int i = 0; i < numberOfBlocks; i++)
        {
            LevelData.BlockConfiguration block = new LevelData.BlockConfiguration();
            block.blockID = i;
            block.color = colors[i % colors.Length];
            block.shape = shapes[Random.Range(0, shapes.Length)];
            
            block.startPosition = new Vector2Int(
                Random.Range(1, boardWidth - 2),
                Random.Range(1, boardHeight - 2)
            );

            blocks.Add(block);
        }

        return blocks.ToArray();
    }
}