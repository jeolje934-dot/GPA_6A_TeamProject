using UnityEngine;

public class CameraRigFollow : MonoBehaviour
{
    public Transform player;

    public float height = 10f;       // 플레이어와 카메라의 높이 차이
    public float distance = 5f;      // 플레이어 기준 Z축 뒤쪽 거리
    public float rotation = 60f;     // 아래를 내려다보는 각도

    void LateUpdate()
    {
        if (player != null)
        {
            // 플레이어 기준 Z축 뒤쪽에서 따라가기
            Vector3 offset = new Vector3(0, height, -distance);
            transform.position = player.position + offset;

            // 아래를 내려다보는 회전
            transform.rotation = Quaternion.Euler(rotation, 0f, 0f);
        }
    }
}
