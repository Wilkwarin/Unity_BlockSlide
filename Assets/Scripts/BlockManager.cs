using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour
{
    public GameObject blockCellPrefab;
    public Transform blocksParent;
    public BoardManager boardManager;
    public GameController gameController;

    private List<Block> blocks = new List<Block>();
    private Block selectedBlock = null;
    private Vector3 dragStartPos;
    private Vector2Int blockStartGridPos;
    private Vector2Int lastValidGridPos;
    private bool isDragging = false;

    public void CreateBlocks(LevelData.BlockConfiguration[] configs)
    {
        foreach (var config in configs)
        {
            Block block = new Block();
            block.color = config.color;
            block.shape = BlockShapeLibrary.GetShapeByEnum(config.shape);
            block.position = config.startPosition;
            block.CreateVisuals(blockCellPrefab, blocksParent);

            blocks.Add(block);
        }
    }

    public void HandleInput()
    {
        Vector2 inputPosition = Vector2.zero;
        bool touchStarted = false;
        bool touchEnded = false;
        bool isDragging_Active = false;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            isDragging_Active = true;

            if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
                touchStarted = true;

            if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
                touchEnded = true;
        }
        else if (Mouse.current != null)
        {
            inputPosition = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame)
                touchStarted = true;

            if (Mouse.current.leftButton.isPressed)
                isDragging_Active = true;

            if (Mouse.current.leftButton.wasReleasedThisFrame)
                touchEnded = true;
        }

        if (touchStarted && !isDragging)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 0));
            Block clickedBlock = GetBlockAtPosition(worldPos);

            if (clickedBlock != null)
            {
                selectedBlock = clickedBlock;
                dragStartPos = worldPos;
                blockStartGridPos = selectedBlock.position;
                lastValidGridPos = selectedBlock.position;
                isDragging = true;
                selectedBlock.SetHighlight(true);
            }
        }

        if (isDragging && isDragging_Active && selectedBlock != null)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 0));
            Vector3 dragOffset = worldPos - dragStartPos;

            Vector2Int targetGridPos = new Vector2Int(
                blockStartGridPos.x + Mathf.RoundToInt(dragOffset.x),
                blockStartGridPos.y + Mathf.RoundToInt(dragOffset.y)
            );

            if (targetGridPos != selectedBlock.position)
            {
                Vector2Int nextPos = GetNextStepTowards(selectedBlock.position, targetGridPos);

                if (CanMoveBlock(selectedBlock, nextPos))
                {
                    selectedBlock.MoveTo(nextPos);
                    lastValidGridPos = nextPos;
                }
            }
        }

        Vector2Int GetNextStepTowards(Vector2Int current, Vector2Int target)
        {
            Vector2Int direction = target - current;

            int stepX = 0;
            int stepY = 0;

            if (direction.x > 0) stepX = 1;
            else if (direction.x < 0) stepX = -1;

            if (direction.y > 0) stepY = 1;
            else if (direction.y < 0) stepY = -1;

            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                return new Vector2Int(current.x + stepX, current.y);
            }
            else
            {
                return new Vector2Int(current.x, current.y + stepY);
            }
        }

        if (touchEnded && isDragging)
        {
            if (selectedBlock != null)
            {
                if (IsTouchingExit(selectedBlock, out Vector2Int exitPos))
                {
                    Vector2 direction = GetExitDirection(exitPos);
                    selectedBlock.AnimateExit(direction, this);
                    RemoveBlock(selectedBlock);
                    gameController.CheckWinCondition();
                }

                selectedBlock.SetHighlight(false);
                selectedBlock = null;
            }

            isDragging = false;
        }
    }

    Block GetBlockAtPosition(Vector3 worldPos)
    {
        Vector2Int gridPos = new Vector2Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.y)
        );

        foreach (var block in blocks)
        {
            foreach (var offset in block.shape)
            {
                Vector2Int cellPos = block.position + offset;
                if (cellPos == gridPos)
                {
                    return block;
                }
            }
        }

        return null;
    }

    bool CanMoveBlock(Block block, Vector2Int newPosition)
    {
        foreach (var offset in block.shape)
        {
            Vector2Int cellPos = newPosition + offset;

            if (!boardManager.IsCellValid(cellPos))
                return false;

            if (IsOccupiedByOtherBlock(cellPos, block))
                return false;
        }

        return true;
    }

    bool IsOccupiedByOtherBlock(Vector2Int cellPos, Block excludeBlock)
    {
        foreach (var block in blocks)
        {
            if (block == excludeBlock)
                continue;

            foreach (var offset in block.shape)
            {
                Vector2Int occupiedCell = block.position + offset;
                if (occupiedCell == cellPos)
                    return true;
            }
        }

        return false;
    }

    bool IsTouchingExit(Block block, out Vector2Int foundExitPos)
    {
        foundExitPos = Vector2Int.zero;
        Vector2Int blockSize = block.GetBoundingBoxSize();

        foreach (var offset in block.shape)
        {
            Vector2Int cellPos = block.position + offset;

            Vector2Int[] neighbors = new Vector2Int[]
            {
            cellPos + Vector2Int.up,
            cellPos + Vector2Int.down,
            cellPos + Vector2Int.left,
            cellPos + Vector2Int.right
            };

            foreach (var neighborPos in neighbors)
            {
                if (boardManager.IsExitCell(neighborPos, block.color))
                {
                    ExitOrientation exitOrientation = boardManager.GetExitOrientation(neighborPos);
                    int exitSize = boardManager.GetExitSize(neighborPos);
                    Vector2Int exitStartPosition = boardManager.GetExitPosition(neighborPos);

                    bool fitsInExit = CheckIfBlockFitsInExit(block, exitOrientation, exitSize, exitStartPosition);

                    if (fitsInExit)
                    {
                        if (IsPathToExitClear(block, neighborPos, exitOrientation))
                        {
                            foundExitPos = neighborPos;
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    bool IsPathToExitClear(Block block, Vector2Int exitPos, ExitOrientation exitOrientation)
    {
        Vector2Int toExit;

        if (exitOrientation == ExitOrientation.Horizontal)
        {
            toExit = exitPos.y > block.position.y ? Vector2Int.up : Vector2Int.down;
        }
        else
        {
            toExit = exitPos.x > block.position.x ? Vector2Int.right : Vector2Int.left;
        }

        foreach (var offset in block.shape)
        {
            Vector2Int cellPos = block.position + offset;
            Vector2Int check = cellPos + toExit;

            while (boardManager.IsCellValid(check))
            {
                if (IsOccupiedByOtherBlock(check, block))
                    return false;

                check += toExit;
            }
        }

        return true;
    }

    Vector2 GetExitDirection(Vector2Int exitPos)
    {
        int width = boardManager.GetBoardWidth();
        int height = boardManager.GetBoardHeight();

        if (exitPos.y >= height) return Vector2.up;
        if (exitPos.y < 0) return Vector2.down;
        if (exitPos.x >= width) return Vector2.right;
        if (exitPos.x < 0) return Vector2.left;

        return Vector2.up; // fallback
    }

    bool CheckIfBlockFitsInExit(Block block, ExitOrientation exitOrientation, int exitSize, Vector2Int exitStartPosition)
    {
        if (exitOrientation == ExitOrientation.Horizontal)
        {
            int exitMinX = exitStartPosition.x;
            int exitMaxX = exitStartPosition.x + exitSize - 1;

            foreach (var offset in block.shape)
            {
                int cellX = block.position.x + offset.x;
                if (cellX < exitMinX || cellX > exitMaxX)
                    return false;
            }

            return true;
        }
        else
        {
            int exitMinY = exitStartPosition.y;
            int exitMaxY = exitStartPosition.y + exitSize - 1;

            foreach (var offset in block.shape)
            {
                int cellY = block.position.y + offset.y;
                if (cellY < exitMinY || cellY > exitMaxY)
                    return false;
            }

            return true;
        }
    }

    public void RemoveBlock(Block block)
    {
        blocks.Remove(block);
    }

    public int GetRemainingBlocksCount()
    {
        return blocks.Count;
    }

    public void ClearBlocks()
    {
        foreach (var block in blocks)
        {
            block.Destroy();
        }
        blocks.Clear();
    }
}