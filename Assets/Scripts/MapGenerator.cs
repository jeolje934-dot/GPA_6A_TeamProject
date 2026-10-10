using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;     // 바닥 타일 프리팹
    public GameObject obstaclePrefab; // 장애물 프리팹
    public GameObject coinPrefab;     // 코인 프리팹
    public Transform player;

    private float spawnZ = 0.0f;
    private float tileLength = 5.0f;
    private int amnTilesOnScreen = 6;
    private List<GameObject> activeTiles = new List<GameObject>();

    void Start()
    {
        spawnZ = 0.0f;
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        if (player == null) return;

        if (player.position.z - 10.0f > (spawnZ - amnTilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
    }

    void SpawnTile()
    {
        // 1. 바닥 타일 생성
        Vector3 tilePosition = new Vector3(0, 0, spawnZ);
        GameObject go = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
        activeTiles.Add(go);

        // 2. 바닥 타일 안쪽에서 안전하게 랜덤 X 좌표 설정
        float randomX = Random.Range(-1.8f, 1.8f);

        // 3. 장애물 생성 [수정 부분]
        if (spawnZ > 5.0f && obstaclePrefab != null && Random.value > 0.3f)
        {
            // Y 높이를 1.0f로 올려서 바닥 위에 완전히 올라오도록 수정
            Vector3 obstaclePos = new Vector3(randomX, 1.0f, spawnZ + Random.Range(-1.5f, 1.5f));
            Instantiate(obstaclePrefab, obstaclePos, Quaternion.identity);
        }

        // 4. 코인 생성 [수정된 부분]
        if (spawnZ > 5.0f && coinPrefab != null && Random.value > 0.2f)
        {
            // 코인 프리팹과 코인 전용 위치 변수 사용
            Vector3 coinPos = new Vector3(randomX, 1.0f, spawnZ + Random.Range(-1.5f, 1.5f));
            Instantiate(coinPrefab, coinPos, Quaternion.identity);
        }


        spawnZ += tileLength;
    }

    void DeleteTile()
    {
        if (activeTiles.Count > 0)
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
        }
    }
}



