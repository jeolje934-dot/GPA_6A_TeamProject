
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

    // 처음에는 안전 구간
    private bool isSafeTime = false;

    private float timer = 0f;
    private float spawnZ = 0f;

    // 3개 라인
    private readonly float[] lanePositions = { -2f, -0f, 2f };

    private readonly List<GameObject> activeTiles =
        new List<GameObject>();

    void Start()
    {
        spawnZ = 0f;
        timer = 0f;
        startTimer = 0f;
        isSafeTime = false;

        // 시작할 때 바닥 타일만 생성
        for (int i = 0; i < amountOfTilesOnScreen; i++)
        {
            SpawnFloorTile();
        }
    }

    void Update()
    {
        if (player == null)
            return;

        // 시작 후 경과 시간
        startTimer += Time.deltaTime;

        // 3초가 지나면 안전 구간 종료
        if (!isSafeTime && startTimer >= initialSafeTime)
        {
            isSafeTime = true;
        }

        // 안전 구간에는 생성 주기를 기다리지 않고
        // 바닥 타일만 생성
        if (!isSafeTime)
        {
            SpawnFloorTile();
            return;
        }

        // 안전 구간 종료 후에는 타이머에 맞춰 생성
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnTile();
            timer -= spawnInterval;
        }

        // 플레이어가 충분히 지나간 타일 삭제
        if (activeTiles.Count > 0)
        {
            float oldestTileZ =
                activeTiles[0].transform.position.z;

            if (player.position.z >
                oldestTileZ + tileLength + 15f)
            {
                DeleteTile();
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
        float spawnZ = player.transform.position.z + 15;

        // 바닥 타일 생성
        Vector3 tilePosition =
            new Vector3(0f, 0f, spawnZ);

        GameObject tile = Instantiate(
            tilePrefab,
            tilePosition,
            Quaternion.identity
        );

        activeTiles.Add(tile);

        // 3개 라인에 랜덤 생성
        for (int i = 0; i < lanePositions.Length; i++)
        {
            int result = Random.Range(0, 4);

            Vector3 spawnPosition = new Vector3(
                lanePositions[i],
                1f,
                spawnZ
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
        if (activeTiles.Count > 0)
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
        }
    }
}
