using UnityEngine;

public class BallVerticalLimit : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;

        // 공이 위로 올라갈 때만 Y축 속도 제한
        if (velocity.y > 0f)
        {
            velocity.y = 0f;
            rb.linearVelocity = velocity;
        }
    }
}