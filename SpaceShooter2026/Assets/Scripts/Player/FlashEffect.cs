using UnityEngine;
using System.Collections;

public class FlashEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Flash(float duration, float intervalSpeed)
    {
        Color originalColor = spriteRenderer.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSecondsRealtime(intervalSpeed); // RealTime ignores timeScale
            spriteRenderer.color = originalColor;
            yield return new WaitForSecondsRealtime(intervalSpeed);
            elapsed += intervalSpeed * 2;
        }

        spriteRenderer.color = originalColor;
    }
}