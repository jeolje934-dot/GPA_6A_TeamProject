using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // 초당 회전 속도 (X, Y, Z 축 기준)
    public Vector3 rotationSpeed = new Vector3(0f, 120f, 0f);

    void Update()
    {
        // 매 프레임마다 지정된 속도로 회전시킵니다.
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}


