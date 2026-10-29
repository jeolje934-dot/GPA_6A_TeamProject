
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement Settings")]
    public float baseSpeed = 10.0f;
    public float maxSpeed = 18.0f;
    public float acceleration = 0.5f;
    public float moveSpeed = 8.0f;

    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        // 전진 속도 서서히 증가
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }
    }

    void FixedUpdate()
    {
        // 좌우 이동 및 자동 전진
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(
            moveHorizontal * moveSpeed,
            0.0f,
            currentSpeed
        );

        rb.linearVelocity = new Vector3(
            movement.x,
            rb.linearVelocity.y,
            movement.z
        );
    }
}
