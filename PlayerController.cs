using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;     // hvor rask spilleren beveger seg
    private Rigidbody2D rb;          // referanse til fysikken
    private Vector2 moveInput;       // input fra tastene

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // henter Rigidbody2D-komponenten
    }

    void Update()
    {
        // Input fra tastatur (WASD/piltaster)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Flytt spilleren basert på input
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
