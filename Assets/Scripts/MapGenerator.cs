
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

    [Header("난이도 설정")]
    [SerializeField] private float initialSafeTime = 1.5f;
    [SerializeField] private float difficultyInterval = 500f;

    [Header("코인 설정")]
    [Range(0f, 1f)]
    [SerializeField] private float coinSpawnChance = 0.55f;

    private float startTimer;
    private float spawnZ;
    private bool isSafeTime;

    // 왼쪽, 가운데, 오른쪽
    private readonly float[] lanePositions = { -2f, 0f, 2f };

    private readonly List<GameObject> activeTiles =
        new List<GameObject>();

    // 직전 타일의 빈 레인
    private int previousSafeLane = -1;

    // 같은 빈 레인이 반복되는 것을 방지
    private int repeatedSafeLaneCount = 0;

    void Start()
    {
        spawnZ = 0f;
        startTimer = 0f;
        isSafeTime = false;

        previousSafeLane = -1;
        repeatedSafeLaneCount = 0;

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

        // 플레이어 앞쪽에 타일을 미리 생성
        float targetZ =
            player.position.z +
            tileLength * amountOfTilesOnScreen;

        while (spawnZ < targetZ)
        {
            if (isSafeTime)
                SpawnTile();
            else
                SpawnFloorTile();
        }

        // 지나간 타일 삭제
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

    void SpawnFloorTile()
    {
        GameObject tile = Instantiate(
            tilePrefab,
            new Vector3(0f, 0f, spawnZ),
            Quaternion.identity
        );

        activeTiles.Add(tile);
        spawnZ += tileLength;
    }

    void SpawnTile()
    {
        float currentTileZ = spawnZ;

        GameObject tile = Instantiate(
            tilePrefab,
            new Vector3(0f, 0f, currentTileZ),
            Quaternion.identity
        );

        activeTiles.Add(tile);
        spawnZ += tileLength;

        // 현재 진행 거리에 따라 난이도 결정
        float distance = Mathf.Max(0f, player.position.z);

        int difficultyLevel =
            Mathf.FloorToInt(distance / difficultyInterval);

        // 장애물 2개가 나올 확률
        float twoObstacleChance;

        if (difficultyLevel == 0)
            twoObstacleChance = 0.30f;
        else if (difficultyLevel == 1)
            twoObstacleChance = 0.50f;
        else if (difficultyLevel == 2)
            twoObstacleChance = 0.65f;
        else if (difficultyLevel == 3)
            twoObstacleChance = 0.80f;
        else
            twoObstacleChance = 0.90f;

        // 빈 레인 선택
        int safeLane = Random.Range(0, lanePositions.Length);

        // 직전 타일과 같은 레인이 반복되면 다른 레인 선택
        if (safeLane == previousSafeLane)
        {
            repeatedSafeLaneCount++;

            if (repeatedSafeLaneCount >= 1)
            {
                int direction = Random.value < 0.5f ? -1 : 1;
                safeLane =
                    (previousSafeLane + direction + 3) % 3;
            }
        }
        else
        {
            repeatedSafeLaneCount = 0;
        }

        previousSafeLane = safeLane;

        // 장애물 배치 여부 결정
        bool[] hasObstacle = new bool[3];

        for (int i = 0; i < lanePositions.Length; i++)
        {
            // 안전 레인은 비워 둠
            if (i == safeLane)
            {
                hasObstacle[i] = false;
                continue;
            }

            // 나머지 레인은 난이도에 따라 장애물 생성
            hasObstacle[i] =
                Random.value < twoObstacleChance;
        }

        // 장애물이 하나도 없으면 안전 레인이 아닌 곳에 하나 생성
        bool hasAnyObstacle = false;

        for (int i = 0; i < hasObstacle.Length; i++)
        {
            if (hasObstacle[i])
            {
                hasAnyObstacle = true;
                break;
            }
        }

        if (!hasAnyObstacle)
        {
            int laneToBlock = (safeLane + 1) % 3;
            hasObstacle[laneToBlock] = true;
        }

        // 장애물 및 코인 생성
        for (int i = 0; i < lanePositions.Length; i++)
        {
            Vector3 spawnPosition = new Vector3(
                lanePositions[i],
                1f,
                currentTileZ
            );

            if (hasObstacle[i])
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
            else
            {
                // 안전 레인에 확률적으로 코인 생성
                if (coinPrefab != null &&
                    Random.value < coinSpawnChance)
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
        if (activeTiles.Count == 0)
            return;

        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
