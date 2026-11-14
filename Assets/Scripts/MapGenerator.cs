using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("기본 프리팹")]
    public GameObject tilePrefab;
    public GameObject coinPrefab;

    [Header("구간별 장애물 프리팹 (기획안 연동)")]
    [SerializeField] private GameObject zone1ObstaclePrefab; // 0 ~ 500M[cite: 1]
    [SerializeField] private GameObject zone2ObstaclePrefab; // 500 ~ 1000M[cite: 1]
    [SerializeField] private GameObject zone3ObstaclePrefab; // 1000 ~ 1500M[cite: 1]
    [SerializeField] private GameObject zone4ObstaclePrefab; // 1500 ~ 2000M[cite: 1]
    [SerializeField] private GameObject zone5ObstaclePrefab; // 2000M ~ 무한[cite: 1]

    [Header("구간별 장애물 타일 생성 확률 (간격 밀도 조절)")]
    [Range(0f, 1f)][SerializeField] private float zone1ObstacleTileChance = 0.55f; // 1단계: 단일 장애물, 적당한 간격 (55%)
    [Range(0f, 1f)][SerializeField] private float zone2ObstacleTileChance = 0.35f; // 2단계: 붙은 장애물, 거리를 더 널찍하게 띄움 (35%)
    [Range(0f, 1f)][SerializeField] private float zone3ObstacleTileChance = 0.65f; // 3단계: 1+2단계 혼합, 밸런스 간격 (65%)
    [Range(0f, 1f)][SerializeField] private float zone4ObstacleTileChance = 0.85f; // 4단계: 랜덤, 거리 좁고 촘촘함 (85%)
    [Range(0f, 1f)][SerializeField] private float zone5ObstacleTileChance = 0.80f; // 무한 구간: 촘촘함 유지 (80%)

    [Header("구간별 코인 생성 확률 (0.0 ~ 1.0)")]
    [Range(0f, 1f)][SerializeField] private float zone1CoinChance = 0.30f;
    [Range(0f, 1f)][SerializeField] private float zone2CoinChance = 0.25f;
    [Range(0f, 1f)][SerializeField] private float zone3CoinChance = 0.20f;
    [Range(0f, 1f)][SerializeField] private float zone4CoinChance = 0.15f;
    [Range(0f, 1f)][SerializeField] private float zone5CoinChance = 0.20f;

    [Header("플레이어")]
    public Transform player;

    [Header("맵 설정")]
    [SerializeField] private float tileLength = 5f;
    [SerializeField] private int amountOfTilesOnScreen = 6;

    [Header("난이도 설정 (초기 안전 시간)")]
    [SerializeField] private float initialSafeTime = 1.5f;

    private float startTimer;
    private float spawnZ;
    private bool isSafeTime;

    // 왼쪽, 가운데, 오른쪽 레인 X 좌표
    private readonly float[] lanePositions = { -2f, 0f, 2f };

    private readonly List<GameObject> activeTiles = new List<GameObject>();

    private int previousSafeLane = -1;
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

        float targetZ = player.position.z + tileLength * amountOfTilesOnScreen;

        while (spawnZ < targetZ)
        {
            if (isSafeTime)
            {
                float tileDistance = spawnZ;
                float obstacleTileChance = GetObstacleTileChance(tileDistance);

                if (Random.value < obstacleTileChance)
                {
                    SpawnTile();
                }
                else
                {
                    SpawnFloorTile(); // 빈 바닥 타일 (장애물 간격 확보용)
                }
            }
            else
            {
                SpawnFloorTile();
            }
        }

        while (activeTiles.Count > 0)
        {
            GameObject oldestTile = activeTiles[0];

            if (oldestTile == null)
            {
                activeTiles.RemoveAt(0);
                continue;
            }

            float tileEndZ = oldestTile.transform.position.z + tileLength;

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

    float GetObstacleTileChance(float distance)
    {
        if (distance < 500f) return zone1ObstacleTileChance;
        else if (distance < 1000f) return zone2ObstacleTileChance;
        else if (distance < 1500f) return zone3ObstacleTileChance;
        else if (distance < 2000f) return zone4ObstacleTileChance;
        else return zone5ObstacleTileChance;
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

        float distance = Mathf.Max(0f, player.position.z);

        GameObject selectedObstaclePrefab = zone1ObstaclePrefab;
        float coinSpawnChance = 0.30f;

        bool[] hasObstacle = new bool[3];

        // 구간별 장애물 생성 패턴 및 프리팹 분기
        if (distance < 500f)
        {
            // [1단계: 0 ~ 500M] 단일 장애물 생성 (1개만 생성, 촘촘하지 않음)
            selectedObstaclePrefab = zone1ObstaclePrefab;
            coinSpawnChance = zone1CoinChance;

            int safeLane = Random.Range(0, 3);
            List<int> nonSafeLanes = new List<int>();
            for (int i = 0; i < 3; i++) if (i != safeLane) nonSafeLanes.Add(i);
            int laneToBlock = nonSafeLanes[Random.Range(0, nonSafeLanes.Count)];
            hasObstacle[laneToBlock] = true;
        }
        else if (distance < 1000f)
        {
            // [2단계: 500 ~ 1000M] 두 개가 붙어 있는 장애물 생성, 거리를 널찍하게 둠
            selectedObstaclePrefab = zone2ObstaclePrefab != null ? zone2ObstaclePrefab : zone1ObstaclePrefab;
            coinSpawnChance = zone2CoinChance;

            int safeLane = (Random.value < 0.5f) ? 0 : 2;
            for (int i = 0; i < 3; i++)
            {
                if (i != safeLane) hasObstacle[i] = true;
            }
        }
        else if (distance < 1500f)
        {
            // [3단계: 1000 ~ 1500M] 1단계(단일)와 2단계(붙어있음) 구조가 합쳐져서 나옴
            selectedObstaclePrefab = zone3ObstaclePrefab != null ? zone3ObstaclePrefab : zone1ObstaclePrefab;
            coinSpawnChance = zone3CoinChance;

            if (Random.value < 0.5f)
            {
                int safeLane = Random.Range(0, 3);
                List<int> nonSafeLanes = new List<int>();
                for (int i = 0; i < 3; i++) if (i != safeLane) nonSafeLanes.Add(i);
                hasObstacle[nonSafeLanes[Random.Range(0, nonSafeLanes.Count)]] = true;
            }
            else
            {
                int safeLane = (Random.value < 0.5f) ? 0 : 2;
                for (int i = 0; i < 3; i++) if (i != safeLane) hasObstacle[i] = true;
            }
        }
        else if (distance < 2000f)
        {
            // [4단계: 1500 ~ 2000M] 랜덤 조합, 거리 좁고 촘촘함
            selectedObstaclePrefab = zone4ObstaclePrefab != null ? zone4ObstaclePrefab : zone1ObstaclePrefab;
            coinSpawnChance = zone4CoinChance;

            int safeLane = Random.Range(0, 3);
            for (int i = 0; i < 3; i++)
            {
                if (i == safeLane) hasObstacle[i] = false;
                else hasObstacle[i] = Random.value < 0.75f;
            }

            bool any = false;
            for (int i = 0; i < 3; i++) if (hasObstacle[i]) any = true;
            if (!any) hasObstacle[(safeLane + 1) % 3] = true;
        }
        else
        {
            // [무한 구간: 2000M ~] 랜덤 조합 및 촘촘한 패턴 유지
            selectedObstaclePrefab = zone5ObstaclePrefab != null ? zone5ObstaclePrefab : zone4ObstaclePrefab;
            coinSpawnChance = zone5CoinChance;

            int safeLane = Random.Range(0, 3);
            for (int i = 0; i < 3; i++)
            {
                if (i == safeLane) hasObstacle[i] = false;
                else hasObstacle[i] = Random.value < 0.70f;
            }

            bool any = false;
            for (int i = 0; i < 3; i++) if (hasObstacle[i]) any = true;
            if (!any) hasObstacle[(safeLane + 1) % 3] = true;
        }

        // 오브젝트(장애물 및 코인) 생성 스폰
        for (int i = 0; i < lanePositions.Length; i++)
        {
            Vector3 spawnPosition = new Vector3(
                lanePositions[i],
                1f,
                currentTileZ
            );

            if (hasObstacle[i])
            {
                if (selectedObstaclePrefab != null)
                {
                    Instantiate(selectedObstaclePrefab, spawnPosition, Quaternion.identity);
                }
            }
            else
            {
                // 안전 레인에 코인 생성
                if (coinPrefab != null && Random.value < coinSpawnChance)
                {
                    Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
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