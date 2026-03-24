using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 3f;
    public float damage = 3f;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.right * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ignore collision with the player itself
        if (other.CompareTag("Player")) return;

        Destroy(gameObject);
    }
}