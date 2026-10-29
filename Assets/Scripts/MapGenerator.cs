
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("프리팹")]
    public GameObject tilePrefab;
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;

    [Header("플레이어")]
    public Transform player;

    [Header("맵 설정")]
    [SerializeField] private float tileLength = 5f;
    [SerializeField] private int amountOfTilesOnScreen = 6;

    [Header("생성 주기")]
    [SerializeField] private float spawnInterval = 2f;

    private float startTimer = 0f;
    private const float initialSafeTime = 1.5f;

    private bool isSafeTime = false;
    private float timer = 0f;

    // 다음 바닥 타일이 생성될 Z 위치
    private float spawnZ = 0f;

    // 3개 라인
    private readonly float[] lanePositions = { -2f, 0f, 2f };

    private readonly List<GameObject> activeTiles =
        new List<GameObject>();

    void Start()
    {
        spawnZ = 0f;
        timer = 0f;
        startTimer = 0f;
        isSafeTime = false;

        // 시작 구간 바닥 생성
        for (int i = 0; i < amountOfTilesOnScreen; i++)
        {
            SpawnFloorTile();
        }
    }

    void Update()
    {
        if (player == null || tilePrefab == null)
            return;

        startTimer += Time.deltaTime;

        if (!isSafeTime && startTimer >= initialSafeTime)
        {
            isSafeTime = true;
        }

        // 플레이어 앞쪽에 바닥을 미리 생성
        float targetZ =
            player.position.z + tileLength * amountOfTilesOnScreen;

        while (spawnZ < targetZ)
        {
            if (isSafeTime)
            {
                SpawnTile();
            }
            else
            {
                SpawnFloorTile();
            }
        }

        // 플레이어가 지나간 타일 삭제
        while (activeTiles.Count > 0)
        {
            GameObject oldestTile = activeTiles[0];

            if (oldestTile == null)
            {
                activeTiles.RemoveAt(0);
                continue;
            }

            float tileEndZ =
                oldestTile.transform.position.z + tileLength;

            if (player.position.z > tileEndZ + 10f)
            {
                DeleteTile();
            }
            else
            {
                break;
            }
        }
    }

    // 바닥 타일만 생성
    void SpawnFloorTile()
    {
        Vector3 tilePosition =
            new Vector3(0f, 0f, spawnZ);

        GameObject tile = Instantiate(
            tilePrefab,
            tilePosition,
            Quaternion.identity
        );

        activeTiles.Add(tile);
        spawnZ += tileLength;
    }

    // 바닥 타일 + 장애물 + 코인 생성
    void SpawnTile()
    {
        float currentTileZ = spawnZ;

        Vector3 tilePosition =
            new Vector3(0f, 0f, currentTileZ);

        GameObject tile = Instantiate(
            tilePrefab,
            tilePosition,
            Quaternion.identity
        );

        activeTiles.Add(tile);
        spawnZ += tileLength;

        // 기존 랜덤 배치 방식 유지
        for (int i = 0; i < lanePositions.Length; i++)
        {
            int result = Random.Range(0, 4);

            Vector3 spawnPosition = new Vector3(
                lanePositions[i],
                1f,
                currentTileZ
            );

            // 0: 비어 있음
            if (result == 0)
            {
                continue;
            }

            // 1, 2: 장애물
            if (result == 1 || result == 2)
            {
                if (obstaclePrefab != null)
                {
                    // 타일의 자식으로 지정하지 않음
                    Instantiate(
                        obstaclePrefab,
                        spawnPosition,
                        Quaternion.identity
                    );
                }
            }
            // 3: 코인
            else if (result == 3)
            {
                if (coinPrefab != null)
                {
                    // 타일의 자식으로 지정하지 않음
                    Instantiate(
                        coinPrefab,
                        spawnPosition,
                        Quaternion.identity
                    );
                }
            }
        }
    }

    void DeleteTile()
    {
        if (activeTiles.Count == 0)
            return;

        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
