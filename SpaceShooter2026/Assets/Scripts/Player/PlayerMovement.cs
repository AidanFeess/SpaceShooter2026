using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float verticalSpeed = 5f;

    [Header("Screen Bounds")]
    public float minY = -4f;
    public float maxY = 4f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float verticalInput = Keyboard.current.wKey.isPressed ? 1f :
                              Keyboard.current.sKey.isPressed ? -1f : 0f;

        // also support arrow keys
        if (Keyboard.current.upArrowKey.isPressed) verticalInput = 1f;
        if (Keyboard.current.downArrowKey.isPressed) verticalInput = -1f;

        rb.linearVelocity = new Vector2(0f, verticalInput * verticalSpeed);
    }

    void Update()
    {
        Vector2 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}