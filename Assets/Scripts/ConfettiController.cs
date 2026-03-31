using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfettiController : MonoBehaviour
{
    [Header("Confetti Settings")]
    public int particleCount = 120;
    public float duration = 1.5f;
    public float spawnRadius = 1.5f;

    private static readonly Color[] colors = new Color[]
    {
        new Color(1f, 0.2f, 0.2f),
        new Color(0.2f, 0.6f, 1f),
        new Color(0.2f, 1f, 0.3f),
        new Color(1f, 0.9f, 0.1f),
        new Color(1f, 0.5f, 0f),
        new Color(0.8f, 0.2f, 1f),
        new Color(0f, 1f, 1f),
        new Color(1f, 0.2f, 0.8f),
        new Color(1f, 1f, 1f),
    };

    struct Particle
    {
        public GameObject obj;
        public Vector3 velocity;
        public float angularVelocity;
        public Vector2 scale;
    }

    public void Play()
    {
        StartCoroutine(RunConfetti());
    }

    IEnumerator RunConfetti()
    {
        Vector3 origin = GetBurstOrigin();

        List<Particle> particles = new List<Particle>();

        for (int i = 0; i < particleCount; i++)
        {
            GameObject p = new GameObject("Confetti");
            p.transform.SetParent(transform, false);

            SpriteRenderer sr = p.AddComponent<SpriteRenderer>();
            sr.sprite = CreateSquareSprite();
            sr.color = colors[Random.Range(0, colors.Length)];
            sr.sortingOrder = 100;

            float w = Random.Range(0.12f, 0.28f);
            float h = Random.Range(0.12f, 0.28f);
            p.transform.localScale = new Vector3(w, h, 1f);

            float angle = Random.Range(0f, 360f);
            float speed = Random.Range(4f, 11f);
            // float spreadAngle = Random.Range(-70f, 70f);
            float spreadAngle = Random.Range(0f, 360f);
            Vector3 dir = Quaternion.Euler(0, 0, spreadAngle) * Vector3.up;

            float offsetX = Random.Range(-spawnRadius, spawnRadius);
            p.transform.position = origin;
            // p.transform.position = origin + new Vector3(offsetX, 0f, 0f);
            p.transform.rotation = Quaternion.Euler(0, 0, angle);

            particles.Add(new Particle
            {
                obj = p,
                velocity = dir * speed,
                angularVelocity = Random.Range(-300f, 300f),
                scale = new Vector2(w, h)
            });
        }

        float elapsed = 0f;
        float gravity = -18f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float alpha = t < 0.6f ? 1f : 1f - (t - 0.6f) / 0.4f;

            for (int i = 0; i < particles.Count; i++)
            {
                Particle p = particles[i];
                if (p.obj == null) continue;

                p.velocity += new Vector3(0f, gravity * Time.deltaTime, 0f);
                p.obj.transform.position += p.velocity * Time.deltaTime;
                p.obj.transform.Rotate(0f, 0f, p.angularVelocity * Time.deltaTime);

                float wobble = Mathf.Abs(Mathf.Sin(elapsed * 8f + i));
                p.obj.transform.localScale = new Vector3(
                    p.scale.x * wobble,
                    p.scale.y,
                    1f
                );

                SpriteRenderer sr = p.obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Color c = sr.color;
                    c.a = alpha;
                    sr.color = c;
                }

                particles[i] = p;
            }

            yield return null;
        }

        foreach (var p in particles)
            if (p.obj != null)
                Destroy(p.obj);
    }

    Vector3 GetBurstOrigin()
    {
        Camera cam = Camera.main;
        if (cam == null) return Vector3.zero;

        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.6f, 0f);
        return cam.ScreenToWorldPoint(new Vector3(screenCenter.x, screenCenter.y, Mathf.Abs(cam.transform.position.z)));
    }

    Sprite CreateSquareSprite()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
    }
}