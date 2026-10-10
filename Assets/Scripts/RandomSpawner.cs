using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    void Start()
    {
        // 바닥 타일 Scale이 5이므로, 바닥 위 안전한 영역 내에서만 랜덤 배치
        // X축(좌우 폭): -2.0 ~ 2.0
        // Z축(앞뒤 길이): -2.0 ~ 2.0
        float randomX = Random.Range(-2.0f, 2.0f);
        float randomZ = Random.Range(-2.0f, 2.0f);

        // Y축(높이)은 바닥 표면(Y=0) 바로 위 살짝 걸치도록 고정 (오브젝트가 붕 뜨지 않게 설정)
        float fixedY = 0.5f;

        // 부모(RoadTile) 기준 로컬 좌표에 안전하게 적용
        transform.localPosition = new Vector3(randomX, fixedY, randomZ);
    }
}




