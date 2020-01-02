using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public Rigidbody rb;
    
    private Vector2 moveDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // Move and rotate the player based on input. If the player is aiming, only
        // allow rotation

        Vector2 d = moveDirection;
        Vector3 m = new Vector3(d.x, 0, d.y);

        if (m != Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 moveVector = m * moveSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + moveVector);

            Quaternion rotateDirection = Quaternion.LookRotation(m);
            transform.rotation = rotateDirection;
        }
    }
    
    public void OnMovement(InputAction.CallbackContext context)
    {
        moveDirection = context.phase == InputActionPhase.Performed ? context.ReadValue<Vector2>() : Vector2.zero;
    }
}
