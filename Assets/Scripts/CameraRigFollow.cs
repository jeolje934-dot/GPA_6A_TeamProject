using UnityEngine;

public class CameraRigFollow : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        if (player != null)
        {
            // 위치는 플레이어를 따라가되, 회전(Rotation)은 플레이어의 회전을 무시하고 항상 기본값 유지
            transform.position = player.position;
            transform.rotation = Quaternion.identity;
        }
    }
}