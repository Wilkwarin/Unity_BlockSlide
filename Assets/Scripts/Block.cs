using UnityEngine;
using System.Collections.Generic;

public class Block
{
    public BlockColor color;
    public Vector2Int[] shape;
    public Vector2Int position;

    private GameObject[] cellObjects;
    private SpriteRenderer[] spriteRenderers;
    private Color originalColor;
    private Color highlightColor;

    class ParticleData
    {
        public GameObject obj;
        public Vector3 startPos;
        public Vector3 velocity;
        public float size;
    }

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

    public void AnimateExit(Vector2 direction, MonoBehaviour runner)
    {
        runner.StartCoroutine(ExitAnimation(direction));
    }

    System.Collections.IEnumerator ExitAnimation(Vector2 direction)
    {
        foreach (var obj in cellObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        int particlesPerCell = 6;
        float duration = 0.5f;

        List<ParticleData> particles = new List<ParticleData>();

        foreach (var cellObj in cellObjects)
        {
            if (cellObj == null) continue;

            Vector3 cellCenter = cellObj.transform.position + new Vector3(direction.x, direction.y, 0) * 1.25f;

            for (int p = 0; p < particlesPerCell; p++)
            {
                GameObject particle = new GameObject($"Particle_{p}");
                particle.transform.SetParent(cellObj.transform.parent);

                SpriteRenderer psr = particle.AddComponent<SpriteRenderer>();

                SpriteRenderer originalSr = cellObj.GetComponent<SpriteRenderer>();
                if (originalSr != null)
                {
                    psr.sprite = originalSr.sprite;
                    psr.color = originalSr.color;
                    psr.sortingOrder = originalSr.sortingOrder + 1;
                }

                float size = UnityEngine.Random.Range(0.15f, 0.35f);
                particle.transform.localScale = Vector3.one * size;
                particle.transform.position = cellCenter;

                Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;

                Vector2 velocity2d = randomDir * UnityEngine.Random.Range(1.5f, 3f)
                                   - direction * UnityEngine.Random.Range(0.5f, 1.2f);

                Vector3 velocity = new Vector3(velocity2d.x, velocity2d.y, 0);

                particles.Add(new ParticleData
                {
                    obj = particle,
                    startPos = cellCenter,
                    velocity = velocity,
                    size = size
                });
            }
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            foreach (var p in particles)
            {
                if (p.obj == null) continue;

                p.obj.transform.position = p.startPos + p.velocity * (t - t * t * 0.5f);

                float fadeT = Mathf.Max(0f, (t - 0.4f) / 0.6f);
                float currentSize = p.size * (1f - fadeT);
                p.obj.transform.localScale = Vector3.one * currentSize;

                SpriteRenderer psr = p.obj.GetComponent<SpriteRenderer>();
                if (psr != null)
                {
                    Color c = psr.color;
                    c.a = 1f - fadeT;
                    psr.color = c;
                }
            }

            yield return null;
        }

        foreach (var particle in particles)
        {
            if (particle.obj != null)
                GameObject.Destroy(particle.obj);
        }

        foreach (var obj in cellObjects)
        {
            if (obj != null)
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
            case BlockColor.DarkTeal: return new Color(0f, 0.5f, 0.5f);
            case BlockColor.Black: return new Color(0f, 0f, 0f);
            case BlockColor.Scarlet: return new Color(1f, 0.141f, 0f);
            case BlockColor.Brown: return new Color(0.545f, 0.271f, 0.075f);
            default: return new Color(0.5f, 0.5f, 0.5f);
        }
    }
}