using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 3f;
    public float tileWidth = 14f;

    private BackgroundScroller[] allTiles;

    void Start()
    {
        allTiles = FindObjectsByType<BackgroundScroller>(FindObjectsSortMode.None);
    }

    void Update()
    {
        transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

        if (transform.position.x <= -24.5f)
        {
            float rightmostX = GetRightmostX();
            transform.position = new Vector3(rightmostX + tileWidth, transform.position.y, transform.position.z);
        }
    }

    float GetRightmostX()
    {
        float max = float.MinValue;
        foreach (BackgroundScroller tile in allTiles)
        {
            if (tile != this && tile.transform.position.x > max)
                max = tile.transform.position.x;
        }
        return max;
    }
}