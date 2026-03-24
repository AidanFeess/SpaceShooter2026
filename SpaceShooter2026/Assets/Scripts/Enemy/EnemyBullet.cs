using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 4f;
    public float damage = .34f; // three hits to kill player with 1 health

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.left * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}