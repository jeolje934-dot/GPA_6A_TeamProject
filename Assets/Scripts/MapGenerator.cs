
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("기본 프리팹")]
    public GameObject tilePrefab;
    public GameObject coinPrefab;

    [Header("구간별 장애물 프리팹")]
    [SerializeField] private GameObject zone1ObstaclePrefab;
    [SerializeField] private GameObject zone2ObstaclePrefab;
    [SerializeField] private GameObject zone3ObstaclePrefab;
    [SerializeField] private GameObject zone4ObstaclePrefab;
    [SerializeField] private GameObject zone5ObstaclePrefab;

    [Header("구간별 장애물 생성 확률")]
    [Range(0f, 1f)]
    [SerializeField] private float zone1ObstacleTileChance = 0.30f;

    [Range(0f, 1f)]
    [SerializeField] private float zone2ObstacleTileChance = 0.38f;

    [Range(0f, 1f)]
    [SerializeField] private float zone3ObstacleTileChance = 0.45f;

    [Range(0f, 1f)]
    [SerializeField] private float zone4ObstacleTileChance = 0.52f;

    [Range(0f, 1f)]
    [SerializeField] private float zone5ObstacleTileChance = 0.58f;

    [Header("구간별 코인 생성 확률")]
    [Range(0f, 1f)]
    [SerializeField] private float zone1CoinChance = 0.45f;

    [Range(0f, 1f)]
    [SerializeField] private float zone2CoinChance = 0.50f;

    [Range(0f, 1f)]
    [SerializeField] private float zone3CoinChance = 0.45f;

    [Range(0f, 1f)]
    [SerializeField] private float zone4CoinChance = 0.40f;

    [Range(0f, 1f)]
    [SerializeField] private float zone5CoinChance = 0.40f;

    [Header("플레이어")]
    public Transform player;

    [Header("맵 설정")]
    [SerializeField] private float tileLength = 5f;
    [SerializeField] private int amountOfTilesOnScreen = 6;

    [Header("초기 안전 시간")]
    [SerializeField] private float initialSafeTime = 1.5f;

    [Header("장애물 간격 설정")]
    [SerializeField] private int safeTilesAfterObstacle = 1;

    private float startTimer;
    private float spawnZ;
    private bool isSafeTime;

    private int safeTilesRemaining = 0;

    private readonly float[] lanePositions = { -2f, 0f, 2f };

    private readonly List<GameObject> activeTiles =
        new List<GameObject>();

    void Start()
    {
        spawnZ = 0f;
        startTimer = 0f;
        isSafeTime = false;

        safeTilesRemaining = 0;

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

        float targetZ =
            player.position.z + tileLength * amountOfTilesOnScreen;

        while (spawnZ < targetZ)
        {
            if (!isSafeTime)
            {
                SpawnFloorTile();
                continue;
            }

            // 장애물 뒤에 빈 타일을 보장
            if (safeTilesRemaining > 0)
            {
                SpawnFloorTile();
                safeTilesRemaining--;
                continue;
            }

            float obstacleChance = GetObstacleTileChance(spawnZ);

            if (Random.value < obstacleChance)
            {
                SpawnTile();

                safeTilesRemaining = safeTilesAfterObstacle;
            }
            else
            {
                SpawnFloorTile();
            }
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

    float GetObstacleTileChance(float distance)
    {
        if (distance < 500f)
            return zone1ObstacleTileChance;

        if (distance < 1000f)
            return zone2ObstacleTileChance;

        if (distance < 1500f)
            return zone3ObstacleTileChance;

        if (distance < 2000f)
            return zone4ObstacleTileChance;

        return zone5ObstacleTileChance;
    }

    float GetCoinChance(float distance)
    {
        if (distance < 500f)
            return zone1CoinChance;

        if (distance < 1000f)
            return zone2CoinChance;

        if (distance < 1500f)
            return zone3CoinChance;

        if (distance < 2000f)
            return zone4CoinChance;

        return zone5CoinChance;
    }

    GameObject GetObstaclePrefab(float distance)
    {
        if (distance < 500f)
            return zone1ObstaclePrefab;

        if (distance < 1000f)
            return zone2ObstaclePrefab != null
                ? zone2ObstaclePrefab : zone1ObstaclePrefab;

        if (distance < 1500f)
            return zone3ObstaclePrefab != null
                ? zone3ObstaclePrefab : zone1ObstaclePrefab;

        if (distance < 2000f)
            return zone4ObstaclePrefab != null
                ? zone4ObstaclePrefab : zone1ObstaclePrefab;

        return zone5ObstaclePrefab != null
            ? zone5ObstaclePrefab
            : (zone4ObstaclePrefab != null
                ? zone4ObstaclePrefab : zone1ObstaclePrefab);
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

        // 타일이 생성될 위치 기준으로 난이도 결정
        float distance = currentTileZ;

        GameObject obstaclePrefab =
            GetObstaclePrefab(distance);

        float coinChance = GetCoinChance(distance);

        if (obstaclePrefab == null)
        {
            Debug.LogWarning("장애물 프리팹이 연결되지 않았습니다.");
            return;
        }

        // 구간별 장애물 개수 결정
        int obstacleCount = 1;

        if (distance < 500f)
        {
            obstacleCount = 1;
        }
        else if (distance < 1000f)
        {
            obstacleCount = Random.value < 0.25f ? 2 : 1;
        }
        else if (distance < 1500f)
        {
            obstacleCount = Random.value < 0.40f ? 2 : 1;
        }
        else if (distance < 2000f)
        {
            obstacleCount = Random.value < 0.55f ? 2 : 1;
        }
        else
        {
            obstacleCount = Random.value < 0.60f ? 2 : 1;
        }

        // 최소 하나의 안전 레인 확보
        List<int> availableLanes =
            new List<int> { 0, 1, 2 };

        // 장애물을 놓을 레인을 무작위로 선택
        for (int i = 0; i < obstacleCount; i++)
        {
            int randomIndex =
                Random.Range(0, availableLanes.Count);

            int laneIndex = availableLanes[randomIndex];

            availableLanes.RemoveAt(randomIndex);

            Vector3 obstaclePosition = new Vector3(
                lanePositions[laneIndex],
                1f,
                currentTileZ
            );

            Instantiate(
                obstaclePrefab,
                obstaclePosition,
                Quaternion.identity
            );
        }

        // 안전 레인에 코인 생성
        foreach (int laneIndex in availableLanes)
        {
            if (coinPrefab != null &&
                Random.value < coinChance)
            {
                Vector3 coinPosition = new Vector3(
                    lanePositions[laneIndex],
                    1f,
                    currentTileZ
                );

                Instantiate(
                    coinPrefab,
                    coinPosition,
                    Quaternion.identity
                );
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
