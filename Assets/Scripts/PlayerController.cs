using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement Settings")]
    public float baseSpeed = 10.0f;
    public float maxSpeed = 18.0f;
    public float acceleration = 0.5f;
    public float moveSpeed = 8.0f;

    [Header("Jump Settings")]
    public float jumpForce = 9.0f;
    private bool isGrounded;

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
        }

        // Space 키 입력 시에만 점프 (지면에 있을 때만 가능)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // 점프하는 순간 공중으로 판정
        }
    }

    void FixedUpdate()
    {
        // WASD 입력에 따른 좌우 이동 및 자동 전진(Z축) 처리
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 1.0f) * moveSpeed;
        movement.z = currentSpeed;

        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
    }

    // 바닥에 닿아 있는 동안 확실하게 착지 상태 유지
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("RoadTile"))
        {
            // 아래쪽으로 향하는 속도가 거의 없을 때만 착지로 인정 (혼자 튀어오르는 현상 방지)
            if (rb.linearVelocity.y <= 0.1f)
            {
                isGrounded = true;
            }
        }
    }

    // 바닥에서 떨어졌을 때
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("RoadTile"))
        {
            isGrounded = false;
        }
    }
}

