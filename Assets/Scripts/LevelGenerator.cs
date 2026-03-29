using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Generation Parameters")]
    public int boardWidth = 6;
    public int boardHeight = 6;
    public int numberOfBlocks = 5;

    [Header("References")]
    public LevelManager levelManager;

    struct ExitCandidate
    {
        public Vector2Int position;
        public ExitOrientation orientation;
        public int size;
    }

    enum Wall { Top, Bottom, Left, Right }

    public LevelData GenerateLevel()
    {
        LevelData level = ScriptableObject.CreateInstance<LevelData>();
        level.levelNumber = levelManager.currentLevelIndex + 1;
        level.levelName = $"Generated Level {level.levelNumber}";

        var (board, blocks) = GenerateBoardAndBlocks();
        level.board = board;
        level.blocks = blocks;

        return level;
    }

    (LevelData.BoardConfiguration, LevelData.BlockConfiguration[]) GenerateBoardAndBlocks()
    {
        LevelData.BoardConfiguration board = new LevelData.BoardConfiguration();
        int actualWidth = Random.Range(7, 9);
        int actualHeight = Random.Range(7, 10);
        boardWidth = actualWidth;
        boardHeight = actualHeight;
        board.width = actualWidth;
        board.height = actualHeight;
        board.cells = new CellType[actualWidth * actualHeight];

        for (int i = 0; i < board.cells.Length; i++)
            board.cells[i] = CellType.Empty;

        List<LevelData.BlockConfiguration> blocks = new List<LevelData.BlockConfiguration>();
        List<LevelData.ExitCellData> exits = new List<LevelData.ExitCellData>();

        HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();
        HashSet<Vector2Int> occupiedExitCells = new HashSet<Vector2Int>();

        List<BlockColor> availableColors = new List<BlockColor>
        {
            BlockColor.Red, BlockColor.Blue, BlockColor.Green, BlockColor.Yellow,
            BlockColor.Orange, BlockColor.Purple, BlockColor.Cyan, BlockColor.Pink,
            BlockColor.DarkTeal, BlockColor.Black, BlockColor.Scarlet, BlockColor.Brown
        };

        List<BlockShape> unusedShapes = new List<BlockShape>(
            (BlockShape[])System.Enum.GetValues(typeof(BlockShape))
        );

        bool hasFiveBlockShape = false;

        int totalCells = boardWidth * boardHeight;
        int minFreeCells = totalCells / 4;

        int failedAttempts = 0;
        int maxFailedAttempts = 10;

        Dictionary<BlockColor, LevelData.ExitCellData> existingExitByColor = 
    new Dictionary<BlockColor, LevelData.ExitCellData>();

        while (true)
        {
            int usedCells = occupiedCells.Count;
            int freeCells = totalCells - usedCells;

            if (freeCells < minFreeCells)
                break;
            {
                if (availableColors.Count == 0)
                {
                    availableColors = new List<BlockColor>
                {
                    BlockColor.Red, BlockColor.Blue, BlockColor.Green, BlockColor.Yellow,
                    BlockColor.Orange, BlockColor.Purple, BlockColor.Cyan, BlockColor.Pink,
                    BlockColor.DarkTeal, BlockColor.Black, BlockColor.Scarlet, BlockColor.Brown
                };
                }

                int colorIndex = Random.Range(0, availableColors.Count);
                BlockColor color = availableColors[colorIndex];
                availableColors.RemoveAt(colorIndex);

                List<Wall> walls = new List<Wall> { Wall.Top, Wall.Bottom, Wall.Left, Wall.Right };
                ShuffleList(walls);

                bool placed = false;

                foreach (Wall wall in walls)
                {
                    BlockShape chosenShape;
                    Vector2Int[] shapeCoords;

                    if (!TryChooseShape(wall, hasFiveBlockShape, unusedShapes,
                        out chosenShape, out shapeCoords))
                        continue;

                    int exitSize = GetExitSize(wall, shapeCoords);

                    int maxAttempts = 50;
                    bool wallPlaced = false;

                    for (int attempt = 0; attempt < maxAttempts; attempt++)
                    {
                        ExitCandidate exit;
                        if (!TryGetExitCandidate(wall, exitSize, occupiedExitCells, out exit))
                            break;

                        Vector2Int startPos = GetPositionAtExit(exit, shapeCoords);

                        if (!IsWithinBoard(startPos, shapeCoords))
                            continue;

                        if (HasCollision(startPos, shapeCoords, occupiedCells))
                            continue;

                        Vector2Int finalPos = MoveIntoBoard(startPos, shapeCoords,
                            wall, occupiedCells);

                        LevelData.ExitCellData exitData = new LevelData.ExitCellData
                        {
                            position = exit.position,
                            color = color,
                            orientation = exit.orientation,
                            size = exit.size
                        };

                        LevelData.BlockConfiguration block = new LevelData.BlockConfiguration
                        {
                            color = color,
                            shape = chosenShape,
                            startPosition = finalPos
                        };

                        foreach (var exitCell in GetExitCells(exit))
                            occupiedExitCells.Add(exitCell);

                        foreach (var offset in shapeCoords)
                            occupiedCells.Add(finalPos + offset);

                        exits.Add(exitData);
                        blocks.Add(block);

                        if (IsFiveBlockShape(chosenShape))
                            hasFiveBlockShape = true;

                        unusedShapes.Remove(chosenShape);

                        ShuffleExistingBlocks(blocks, exits, occupiedCells);

                        wallPlaced = true;
                        placed = true;
                        break;
                    }

                    if (wallPlaced)
                        break;
                }

                if (!placed)
                {
                    availableColors.Add(color);
                    failedAttempts++;

                    CompactBlocks(blocks, exits, occupiedCells);

                    if (failedAttempts >= maxFailedAttempts)
                        break;
                }
                else
                {
                    failedAttempts = 0;
                }
            }
        }

        int targetOccupied = (totalCells * 3) / 4;
        int maxFinalPasses = 10;

        for (int pass = 0; pass < maxFinalPasses; pass++)
        {
            if (occupiedCells.Count >= targetOccupied)
                break;

            CompactBlocks(blocks, exits, occupiedCells);

            bool addedBlock = false;

            if (availableColors.Count == 0)
            {
                availableColors = new List<BlockColor>
        {
            BlockColor.Red, BlockColor.Blue, BlockColor.Green, BlockColor.Yellow,
            BlockColor.Orange, BlockColor.Purple, BlockColor.Cyan, BlockColor.Pink,
            BlockColor.DarkTeal, BlockColor.Black, BlockColor.Scarlet, BlockColor.Brown
        };
            }

            int colorIndex = Random.Range(0, availableColors.Count);
            BlockColor color = availableColors[colorIndex];
            availableColors.RemoveAt(colorIndex);

            List<Wall> walls = new List<Wall> { Wall.Top, Wall.Bottom, Wall.Left, Wall.Right };
            ShuffleList(walls);

            foreach (Wall wall in walls)
            {
                BlockShape chosenShape;
                Vector2Int[] shapeCoords;

                if (!TryChooseShape(wall, hasFiveBlockShape, unusedShapes,
                    out chosenShape, out shapeCoords))
                    continue;

                int exitSize = GetExitSize(wall, shapeCoords);
                bool wallPlaced = false;

                for (int attempt = 0; attempt < 50; attempt++)
                {
                    ExitCandidate exit;
                    if (!TryGetExitCandidate(wall, exitSize, occupiedExitCells, out exit))
                        break;

                    Vector2Int startPos = GetPositionAtExit(exit, shapeCoords);

                    if (!IsWithinBoard(startPos, shapeCoords))
                        continue;

                    if (HasCollision(startPos, shapeCoords, occupiedCells))
                        continue;

                    Vector2Int finalPos = MoveIntoBoard(startPos, shapeCoords, wall, occupiedCells);

                    LevelData.ExitCellData exitData = new LevelData.ExitCellData
                    {
                        position = exit.position,
                        color = color,
                        orientation = exit.orientation,
                        size = exit.size
                    };

                    LevelData.BlockConfiguration block = new LevelData.BlockConfiguration
                    {
                        color = color,
                        shape = chosenShape,
                        startPosition = finalPos
                    };

                    foreach (var exitCell in GetExitCells(exit))
                        occupiedExitCells.Add(exitCell);

                    foreach (var offset in shapeCoords)
                        occupiedCells.Add(finalPos + offset);

                    exits.Add(exitData);
                    blocks.Add(block);

                    if (IsFiveBlockShape(chosenShape))
                        hasFiveBlockShape = true;

                    unusedShapes.Remove(chosenShape);

                    wallPlaced = true;
                    addedBlock = true;
                    break;
                }

                if (wallPlaced)
                    break;
            }

            if (!addedBlock)
                availableColors.Add(color);
        }

        board.exits = exits.ToArray();
        PushBlocksAwayFromExits(blocks, exits, occupiedCells);
        return (board, blocks.ToArray());

        bool TryChooseShape(Wall wall, bool hasFiveBlockShape,
            List<BlockShape> unusedShapes,
            out BlockShape chosenShape, out Vector2Int[] shapeCoords)
        {
            chosenShape = default;
            shapeCoords = null;

            List<BlockShape> allShapes = new List<BlockShape>(
                (BlockShape[])System.Enum.GetValues(typeof(BlockShape))
            );

            if (hasFiveBlockShape)
                allShapes.RemoveAll(s => IsFiveBlockShape(s));

            allShapes.RemoveAll(s => !ShapeFitsOnWall(s, wall));

            if (allShapes.Count == 0)
                return false;

            List<BlockShape> candidates = new List<BlockShape>();
            foreach (var s in unusedShapes)
            {
                if (allShapes.Contains(s))
                    candidates.Add(s);
            }

            if (candidates.Count == 0)
                candidates = allShapes;

            int index = Random.Range(0, candidates.Count);
            chosenShape = candidates[index];
            shapeCoords = BlockShapeLibrary.GetShapeByEnum(chosenShape);
            return true;
        }

        bool ShapeFitsOnWall(BlockShape shape, Wall wall)
        {
            Vector2Int[] coords = BlockShapeLibrary.GetShapeByEnum(shape);
            Vector2Int size = GetShapeSize(coords);

            if (wall == Wall.Top || wall == Wall.Bottom)
                return size.x <= boardWidth - 1;
            else
                return size.y <= boardHeight - 1;
        }

        int GetExitSize(Wall wall, Vector2Int[] shapeCoords)
        {
            Vector2Int size = GetShapeSize(shapeCoords);
            if (wall == Wall.Top || wall == Wall.Bottom)
                return size.x;
            else
                return size.y;
        }

        bool TryGetExitCandidate(Wall wall, int exitSize,
            HashSet<Vector2Int> occupiedExitCells, out ExitCandidate exit)
        {
            exit = default;

            List<Vector2Int> candidates = new List<Vector2Int>();

            if (wall == Wall.Top)
            {
                for (int x = 0; x <= boardWidth - exitSize; x++)
                {
                    Vector2Int pos = new Vector2Int(x, boardHeight);
                    if (!ExitOverlaps(pos, exitSize, true, occupiedExitCells))
                        candidates.Add(pos);
                }
            }
            else if (wall == Wall.Bottom)
            {
                for (int x = 0; x <= boardWidth - exitSize; x++)
                {
                    Vector2Int pos = new Vector2Int(x, -1);
                    if (!ExitOverlaps(pos, exitSize, true, occupiedExitCells))
                        candidates.Add(pos);
                }
            }
            else if (wall == Wall.Right)
            {
                for (int y = 0; y <= boardHeight - exitSize; y++)
                {
                    Vector2Int pos = new Vector2Int(boardWidth, y);
                    if (!ExitOverlaps(pos, exitSize, false, occupiedExitCells))
                        candidates.Add(pos);
                }
            }
            else
            {
                for (int y = 0; y <= boardHeight - exitSize; y++)
                {
                    Vector2Int pos = new Vector2Int(-1, y);
                    if (!ExitOverlaps(pos, exitSize, false, occupiedExitCells))
                        candidates.Add(pos);
                }
            }

            if (candidates.Count == 0)
                return false;

            Vector2Int chosen = candidates[Random.Range(0, candidates.Count)];

            exit = new ExitCandidate
            {
                position = chosen,
                orientation = (wall == Wall.Top || wall == Wall.Bottom)
                    ? ExitOrientation.Horizontal
                    : ExitOrientation.Vertical,
                size = exitSize
            };

            return true;
        }

        bool ExitOverlaps(Vector2Int pos, int size, bool horizontal,
            HashSet<Vector2Int> occupiedExitCells)
        {
            for (int i = 0; i < size; i++)
            {
                Vector2Int cell = horizontal
                    ? new Vector2Int(pos.x + i, pos.y)
                    : new Vector2Int(pos.x, pos.y + i);

                if (occupiedExitCells.Contains(cell))
                    return true;
            }
            return false;
        }

        List<Vector2Int> GetExitCells(ExitCandidate exit)
        {
            List<Vector2Int> cells = new List<Vector2Int>();
            bool horizontal = exit.orientation == ExitOrientation.Horizontal;

            for (int i = 0; i < exit.size; i++)
            {
                cells.Add(horizontal
                    ? new Vector2Int(exit.position.x + i, exit.position.y)
                    : new Vector2Int(exit.position.x, exit.position.y + i));
            }
            return cells;
        }

        Vector2Int GetPositionAtExit(ExitCandidate exit, Vector2Int[] shapeCoords)
        {
            Vector2Int shapeSize = GetShapeSize(shapeCoords);
            int minX = GetShapeMinX(shapeCoords);
            int minY = GetShapeMinY(shapeCoords);

            switch (GetWallFromExit(exit))
            {
                case Wall.Top:
                    return new Vector2Int(
                        exit.position.x - minX,
                        boardHeight - 1 - (shapeSize.y - 1)
                    );

                case Wall.Bottom:
                    return new Vector2Int(
                        exit.position.x - minX,
                        0 - minY
                    );

                case Wall.Right:
                    return new Vector2Int(
                        boardWidth - 1 - (shapeSize.x - 1),
                        exit.position.y - minY
                    );

                default:
                    return new Vector2Int(
                        0 - minX,
                        exit.position.y - minY
                    );
            }
        }

        Wall GetWallFromExit(ExitCandidate exit)
        {
            if (exit.position.y == boardHeight) return Wall.Top;
            if (exit.position.y == -1) return Wall.Bottom;
            if (exit.position.x == boardWidth) return Wall.Right;
            return Wall.Left;
        }

        Wall GetWallFromExitData(LevelData.ExitCellData exit)
        {
            if (exit.position.y == boardHeight) return Wall.Top;
            if (exit.position.y == -1) return Wall.Bottom;
            if (exit.position.x == boardWidth) return Wall.Right;
            return Wall.Left;
        }

        Vector2Int MoveIntoBoard(Vector2Int startPos, Vector2Int[] shapeCoords,
            Wall wall, HashSet<Vector2Int> occupiedCells)
        {
            Vector2Int pos = startPos;
            int totalSteps = 0;
            int maxSteps = boardWidth * boardHeight;

            Vector2Int inwardDir = GetInwardDirection(wall);

            int mandatorySteps = Random.Range(2, 5);
            for (int s = 0; s < mandatorySteps && totalSteps < maxSteps; s++)
            {
                Vector2Int next = pos + inwardDir;
                if (IsWithinBoard(next, shapeCoords) &&
                    !HasCollision(next, shapeCoords, occupiedCells))
                {
                    pos = next;
                    totalSteps++;
                }
                else
                {
                    break;
                }
            }

            int turns = Random.Range(3, 5);
            Vector2Int currentDir = GetRandomPerpendicularDirection(inwardDir);

            for (int turn = 0; turn < turns && totalSteps < maxSteps; turn++)
            {
                int maxStepsInDir = Random.Range(1, Mathf.Max(boardWidth, boardHeight));
                int stepsInDir = 0;

                while (stepsInDir < maxStepsInDir && totalSteps < maxSteps)
                {
                    Vector2Int next = pos + currentDir;
                    if (IsWithinBoard(next, shapeCoords) &&
                        !HasCollision(next, shapeCoords, occupiedCells))
                    {
                        pos = next;
                        stepsInDir++;
                        totalSteps++;
                    }
                    else
                    {
                        break;
                    }
                }

                currentDir = GetRandomDirection(currentDir, -inwardDir);
            }

            return pos;
        }

        void ShuffleExistingBlocks(List<LevelData.BlockConfiguration> blocks,
            List<LevelData.ExitCellData> exits,
            HashSet<Vector2Int> occupiedCells)
        {
            // Перемешиваем порядок чтобы каждый раз разные фигуры двигались первыми
            List<int> indices = new List<int>();
            for (int i = 0; i < blocks.Count; i++)
                indices.Add(i);
            ShuffleList(indices);

            foreach (int i in indices)
            {
                var block = blocks[i];
                var exit = exits[i];
                Vector2Int[] coords = BlockShapeLibrary.GetShapeByEnum(block.shape);

                Wall wall = GetWallFromExitData(exit);
                Vector2Int inwardDir = GetInwardDirection(wall);

                // Временно убираем клетки этого блока из занятых
                foreach (var offset in coords)
                    occupiedCells.Remove(block.startPosition + offset);

                // Пробуем переместить фигуру в случайном направлении
                // но не в сторону выхода
                Vector2Int pos = block.startPosition;
                int maxSteps = boardWidth * boardHeight;
                int totalSteps = 0;

                // Выбираем случайное направление не к выходу
                Vector2Int moveDir = GetRandomDirection(inwardDir, -inwardDir);

                int stepsInDir = Random.Range(1, Mathf.Max(boardWidth, boardHeight));
                int stepsTaken = 0;

                while (stepsTaken < stepsInDir && totalSteps < maxSteps)
                {
                    Vector2Int next = pos + moveDir;
                    if (IsWithinBoard(next, coords) &&
                        !HasCollision(next, coords, occupiedCells))
                    {
                        pos = next;
                        stepsTaken++;
                        totalSteps++;
                    }
                    else
                    {
                        break;
                    }
                }

                block.startPosition = pos;
                blocks[i] = block;

                // Возвращаем клетки на новой позиции
                foreach (var offset in coords)
                    occupiedCells.Add(pos + offset);
            }
        }

        void CompactBlocks(List<LevelData.BlockConfiguration> blocks,
            List<LevelData.ExitCellData> exits,
            HashSet<Vector2Int> occupiedCells)
        {
            int passes = 3;

            for (int pass = 0; pass < passes; pass++)
            {
                List<int> indices = new List<int>();
                for (int i = 0; i < blocks.Count; i++)
                    indices.Add(i);
                ShuffleList(indices);

                foreach (int i in indices)
                {
                    var block = blocks[i];
                    var exit = exits[i];
                    Vector2Int[] coords = BlockShapeLibrary.GetShapeByEnum(block.shape);

                    Wall wall = GetWallFromExitData(exit);
                    Vector2Int inwardDir = GetInwardDirection(wall);

                    foreach (var offset in coords)
                        occupiedCells.Remove(block.startPosition + offset);

                    Vector2Int pos = block.startPosition;

                    List<Vector2Int> dirs = new List<Vector2Int>
            {
                Vector2Int.up, Vector2Int.down,
                Vector2Int.left, Vector2Int.right
            };
                    ShuffleList(dirs);

                    Vector2Int towardExit = -inwardDir;
                    dirs.Remove(towardExit);

                    foreach (var dir in dirs)
                    {
                        Vector2Int bestPos = pos;
                        Vector2Int tryPos = pos + dir;

                        while (IsWithinBoard(tryPos, coords) &&
                               !HasCollision(tryPos, coords, occupiedCells))
                        {
                            bestPos = tryPos;
                            tryPos = tryPos + dir;
                        }

                        if (bestPos != pos)
                        {
                            pos = bestPos;
                            break;
                        }
                    }

                    block.startPosition = pos;
                    blocks[i] = block;

                    foreach (var offset in coords)
                        occupiedCells.Add(pos + offset);
                }
            }
        }

        void PushBlocksAwayFromExits(List<LevelData.BlockConfiguration> blocks,
    List<LevelData.ExitCellData> exits,
    HashSet<Vector2Int> occupiedCells)
        {
            int passes = 5;

            for (int pass = 0; pass < passes; pass++)
            {
                List<int> indices = new List<int>();
                for (int i = 0; i < blocks.Count; i++)
                    indices.Add(i);
                ShuffleList(indices);

                foreach (int i in indices)
                {
                    var block = blocks[i];
                    var exit = exits[i];
                    Vector2Int[] coords = BlockShapeLibrary.GetShapeByEnum(block.shape);

                    Wall wall = GetWallFromExitData(exit);
                    Vector2Int inwardDir = GetInwardDirection(wall);
                    Vector2Int perpDir = GetRandomPerpendicularDirection(inwardDir);

                    // Временно убираем клетки этого блока
                    foreach (var offset in coords)
                        occupiedCells.Remove(block.startPosition + offset);

                    Vector2Int pos = block.startPosition;
                    int totalSteps = 0;
                    int maxSteps = boardWidth * boardHeight;

                    // Несколько попыток обхода препятствий
                    int maneuvers = 4;

                    for (int maneuver = 0; maneuver < maneuvers && totalSteps < maxSteps; maneuver++)
                    {
                        // Шаг 1 — идём вглубь пока можем
                        bool movedInward = false;
                        for (int step = 0; step < maxSteps && totalSteps < maxSteps; step++)
                        {
                            Vector2Int next = pos + inwardDir;
                            if (IsWithinBoard(next, coords) &&
                                !HasCollision(next, coords, occupiedCells))
                            {
                                pos = next;
                                totalSteps++;
                                movedInward = true;
                            }
                            else
                            {
                                break;
                            }
                        }

                        // Шаг 2 — упёрлись, пробуем сдвинуться в сторону
                        // Пробуем обе перпендикулярных стороны в случайном порядке
                        List<Vector2Int> perpDirs = new List<Vector2Int>
                {
                    GetRandomPerpendicularDirection(inwardDir),
                };
                        // Добавляем противоположное перпендикулярное направление
                        Vector2Int firstPerp = perpDirs[0];
                        perpDirs.Add(new Vector2Int(-firstPerp.x, -firstPerp.y));

                        bool movedSideways = false;
                        foreach (var pd in perpDirs)
                        {
                            int sidewaysSteps = Random.Range(1, Mathf.Max(boardWidth, boardHeight));
                            for (int step = 0; step < sidewaysSteps && totalSteps < maxSteps; step++)
                            {
                                Vector2Int next = pos + pd;
                                if (IsWithinBoard(next, coords) &&
                                    !HasCollision(next, coords, occupiedCells))
                                {
                                    pos = next;
                                    totalSteps++;
                                    movedSideways = true;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (movedSideways)
                                break;
                        }

                        // Если не смогли двигаться ни вглубь ни в сторону — фигура заперта
                        if (!movedInward && !movedSideways)
                            break;
                    }

                    block.startPosition = pos;
                    blocks[i] = block;

                    foreach (var offset in coords)
                        occupiedCells.Add(pos + offset);
                }
            }
        }

        Vector2Int GetInwardDirection(Wall wall)
        {
            switch (wall)
            {
                case Wall.Top: return Vector2Int.down;
                case Wall.Bottom: return Vector2Int.up;
                case Wall.Right: return Vector2Int.left;
                default: return Vector2Int.right;
            }
        }

        Vector2Int GetRandomPerpendicularDirection(Vector2Int dir)
        {
            if (dir.x != 0)
                return Random.value > 0.5f ? Vector2Int.up : Vector2Int.down;
            else
                return Random.value > 0.5f ? Vector2Int.left : Vector2Int.right;
        }

        Vector2Int GetRandomDirection(Vector2Int excludeDir, Vector2Int towardExit)
        {
            List<Vector2Int> dirs = new List<Vector2Int>
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };
            dirs.Remove(excludeDir);
            dirs.Remove(towardExit);

            if (dirs.Count == 0)
            {
                dirs = new List<Vector2Int>
            {
                Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
            };
                dirs.Remove(excludeDir);
            }

            return dirs[Random.Range(0, dirs.Count)];
        }

        Vector2Int GetShapeSize(Vector2Int[] shape)
        {
            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;
            foreach (var v in shape)
            {
                if (v.x < minX) minX = v.x;
                if (v.x > maxX) maxX = v.x;
                if (v.y < minY) minY = v.y;
                if (v.y > maxY) maxY = v.y;
            }
            return new Vector2Int(maxX - minX + 1, maxY - minY + 1);
        }

        int GetShapeMinX(Vector2Int[] shape)
        {
            int min = int.MaxValue;
            foreach (var v in shape) if (v.x < min) min = v.x;
            return min;
        }

        int GetShapeMinY(Vector2Int[] shape)
        {
            int min = int.MaxValue;
            foreach (var v in shape) if (v.y < min) min = v.y;
            return min;
        }

        bool IsFiveBlockShape(BlockShape shape)
        {
            return BlockShapeLibrary.GetShapeByEnum(shape).Length == 5;
        }

        bool IsWithinBoard(Vector2Int position, Vector2Int[] shape)
        {
            foreach (var offset in shape)
            {
                Vector2Int cell = position + offset;
                if (cell.x < 0 || cell.x >= boardWidth ||
                    cell.y < 0 || cell.y >= boardHeight)
                    return false;
            }
            return true;
        }

        bool HasCollision(Vector2Int position, Vector2Int[] shape,
            HashSet<Vector2Int> occupiedCells)
        {
            foreach (var offset in shape)
            {
                if (occupiedCells.Contains(position + offset))
                    return true;
            }
            return false;
        }

        void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}