using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement Settings (속도 상향 조정)")]
    public float baseSpeed = 15.0f;     // 시작 기본 속도
    public float acceleration = 0.8f;   // 가속도도 살짝 높여 반응 속도 개선
    public float moveSpeed = 14.0f;      // 좌우 이동 속도

    [Header("구간별 최대 속도 설정 (간격 넓어진 맵 맞춤 밸런스)")]
    [SerializeField] private float zone1MaxSpeed = 19.0f; // 0 ~ 500M 구간 최대 속도
    [SerializeField] private float zone2MaxSpeed = 22.0f; // 500 ~ 1000M 구간 최대 속도
    [SerializeField] private float zone3MaxSpeed = 25.0f; // 1000 ~ 1500M 구간 최대 속도
    [SerializeField] private float zone4MaxSpeed = 28.0f; // 1500 ~ 2000M 구간 최대 속도
    [SerializeField] private float zone5MaxSpeed = 30.0f; // 2000M ~ 무한 구간 최대 속도

    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        // 현재 플레이어의 진행 거리(Z축 위치) 계산
        float distance = Mathf.Max(0f, transform.position.z);
        float targetMaxSpeed = zone1MaxSpeed;

        // 거리에 따른 구간별 목표 최대 속도 지정
        if (distance < 500f)
        {
            targetMaxSpeed = zone1MaxSpeed;
        }
        else if (distance < 1000f)
        {
            targetMaxSpeed = zone2MaxSpeed;
        }
        else if (distance < 1500f)
        {
            targetMaxSpeed = zone3MaxSpeed;
        }
        else if (distance < 2000f)
        {
            targetMaxSpeed = zone4MaxSpeed;
        }
        else
        {
            // 2000M 이후 무한 구간
            targetMaxSpeed = zone5MaxSpeed;
        }

        // 전진 속도를 서서히 증가시켜 현재 구간의 목표 최대 속도까지 도달
        if (currentSpeed < targetMaxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, targetMaxSpeed);
        }
        else if (currentSpeed > targetMaxSpeed)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetMaxSpeed, acceleration * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // 좌우 이동 및 자동 전진 (동적으로 계산된 currentSpeed 적용)
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