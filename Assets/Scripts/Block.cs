using UnityEngine;

public class Block
{
    public int id;
    public BlockColor color;
    public Vector2Int[] shape;
    public Vector2Int position;

    private GameObject[] cellObjects;
    private SpriteRenderer[] spriteRenderers;
    private Color originalColor;
    private Color highlightColor;

    public void CreateVisuals(GameObject cellPrefab, Transform parent)
    {
        cellObjects = new GameObject[shape.Length];
        spriteRenderers = new SpriteRenderer[shape.Length];

        originalColor = GetColorFromEnum(color);
        highlightColor = originalColor * 1.3f;

        for (int i = 0; i < shape.Length; i++)
        {
            Vector3 pos = new Vector3(
                position.x + shape[i].x,
                position.y + shape[i].y,
                0
            );

            cellObjects[i] = GameObject.Instantiate(cellPrefab, pos, Quaternion.identity, parent);
            spriteRenderers[i] = cellObjects[i].GetComponent<SpriteRenderer>();

            if (spriteRenderers[i] != null)
            {
                spriteRenderers[i].color = originalColor;
            }
        }
    }

    public void MoveTo(Vector2Int newPosition)
    {
        position = newPosition;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        for (int i = 0; i < shape.Length; i++)
        {
            cellObjects[i].transform.position = new Vector3(
                position.x + shape[i].x,
                position.y + shape[i].y,
                0
            );
        }
    }

    public Vector2Int GetBoundingBoxSize()
    {
        if (shape == null || shape.Length == 0)
            return Vector2Int.one;

        Debug.Log($"=== GetBoundingBoxSize для блока {id} ===");
        Debug.Log($"Shape содержит {shape.Length} элементов:");

        for (int i = 0; i < shape.Length; i++)
        {
            Debug.Log($"  Element {i}: ({shape[i].x}, {shape[i].y})");
        }

        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;

        foreach (var offset in shape)
        {
            minX = Mathf.Min(minX, offset.x);
            maxX = Mathf.Max(maxX, offset.x);
            minY = Mathf.Min(minY, offset.y);
            maxY = Mathf.Max(maxY, offset.y);
        }

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;

        Debug.Log($"minX={minX}, maxX={maxX} → width={width}");
        Debug.Log($"minY={minY}, maxY={maxY} → height={height}");
        Debug.Log($"Итого: {width}×{height}");

        return new Vector2Int(width, height);
    }

    public void SetHighlight(bool highlighted)
    {
        Color targetColor = highlighted ? highlightColor : originalColor;

        foreach (var sr in spriteRenderers)
        {
            if (sr != null)
            {
                sr.color = targetColor;
            }
        }
    }

    public void Destroy()
    {
        foreach (var obj in cellObjects)
        {
            GameObject.Destroy(obj);
        }
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