

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

    [Header("10단계 장애물 타일 생성 확률")]
    [Range(0f, 1f)][SerializeField] private float stage1ObstacleChance = 0.25f;
    [Range(0f, 1f)][SerializeField] private float stage2ObstacleChance = 0.30f;
    [Range(0f, 1f)][SerializeField] private float stage3ObstacleChance = 0.35f;
    [Range(0f, 1f)][SerializeField] private float stage4ObstacleChance = 0.40f;
    [Range(0f, 1f)][SerializeField] private float stage5ObstacleChance = 0.45f;
    [Range(0f, 1f)][SerializeField] private float stage6ObstacleChance = 0.48f;
    [Range(0f, 1f)][SerializeField] private float stage7ObstacleChance = 0.52f;
    [Range(0f, 1f)][SerializeField] private float stage8ObstacleChance = 0.55f;
    [Range(0f, 1f)][SerializeField] private float stage9ObstacleChance = 0.58f;
    [Range(0f, 1f)][SerializeField] private float stage10ObstacleChance = 0.60f;

    [Header("10단계 코인 생성 확률")]
    [Range(0f, 1f)][SerializeField] private float stage1CoinChance = 0.50f;
    [Range(0f, 1f)][SerializeField] private float stage2CoinChance = 0.50f;
    [Range(0f, 1f)][SerializeField] private float stage3CoinChance = 0.48f;
    [Range(0f, 1f)][SerializeField] private float stage4CoinChance = 0.46f;
    [Range(0f, 1f)][SerializeField] private float stage5CoinChance = 0.44f;
    [Range(0f, 1f)][SerializeField] private float stage6CoinChance = 0.42f;
    [Range(0f, 1f)][SerializeField] private float stage7CoinChance = 0.40f;
    [Range(0f, 1f)][SerializeField] private float stage8CoinChance = 0.38f;
    [Range(0f, 1f)][SerializeField] private float stage9CoinChance = 0.36f;
    [Range(0f, 1f)][SerializeField] private float stage10CoinChance = 0.35f;

    [Header("플레이어")]
    public Transform player;

    [Header("맵 설정")]
    [SerializeField] private float tileLength = 5f;
    [SerializeField] private int amountOfTilesOnScreen = 6;

    [Header("초기 안전 시간")]
    [SerializeField] private float initialSafeTime = 1.5f;

    [Header("장애물 간격 설정")]
    [SerializeField] private int safeTilesAfterObstacle = 1;

    [Header("2개 장애물 패턴 확률")]
    [Range(0f, 1f)][SerializeField] private float stage1DoubleChance = 0f;
    [Range(0f, 1f)][SerializeField] private float stage2DoubleChance = 0f;
    [Range(0f, 1f)][SerializeField] private float stage3DoubleChance = 0.08f;
    [Range(0f, 1f)][SerializeField] private float stage4DoubleChance = 0.12f;
    [Range(0f, 1f)][SerializeField] private float stage5DoubleChance = 0.18f;
    [Range(0f, 1f)][SerializeField] private float stage6DoubleChance = 0.22f;
    [Range(0f, 1f)][SerializeField] private float stage7DoubleChance = 0.28f;
    [Range(0f, 1f)][SerializeField] private float stage8DoubleChance = 0.32f;
    [Range(0f, 1f)][SerializeField] private float stage9DoubleChance = 0.38f;
    [Range(0f, 1f)][SerializeField] private float stage10DoubleChance = 0.42f;

    private float startTimer;
    private float spawnZ;
    private bool isSafeTime;

    private int safeTilesRemaining;

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
            player.position.z +
            tileLength * amountOfTilesOnScreen;

        while (spawnZ < targetZ)
        {
            if (!isSafeTime)
            {
                SpawnFloorTile();
                continue;
            }

            // 장애물 뒤에 빈 타일 배치
            if (safeTilesRemaining > 0)
            {
                SpawnFloorTile();
                safeTilesRemaining--;
                continue;
            }

            int stage = GetStage(spawnZ);

            float obstacleChance =
                GetObstacleChance(stage);

            if (Random.value < obstacleChance)
            {
                SpawnTile(stage);

                safeTilesRemaining =
                    safeTilesAfterObstacle;
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

    // 1,000m마다 단계 상승
    int GetStage(float distance)
    {
        int stage = Mathf.FloorToInt(distance / 1000f) + 1;

        return Mathf.Clamp(stage, 1, 10);
    }

    float GetObstacleChance(int stage)
    {
        switch (stage)
        {
            case 1: return stage1ObstacleChance;
            case 2: return stage2ObstacleChance;
            case 3: return stage3ObstacleChance;
            case 4: return stage4ObstacleChance;
            case 5: return stage5ObstacleChance;
            case 6: return stage6ObstacleChance;
            case 7: return stage7ObstacleChance;
            case 8: return stage8ObstacleChance;
            case 9: return stage9ObstacleChance;
            default: return stage10ObstacleChance;
        }
    }

    float GetCoinChance(int stage)
    {
        switch (stage)
        {
            case 1: return stage1CoinChance;
            case 2: return stage2CoinChance;
            case 3: return stage3CoinChance;
            case 4: return stage4CoinChance;
            case 5: return stage5CoinChance;
            case 6: return stage6CoinChance;
            case 7: return stage7CoinChance;
            case 8: return stage8CoinChance;
            case 9: return stage9CoinChance;
            default: return stage10CoinChance;
        }
    }

    float GetDoubleChance(int stage)
    {
        switch (stage)
        {
            case 1: return stage1DoubleChance;
            case 2: return stage2DoubleChance;
            case 3: return stage3DoubleChance;
            case 4: return stage4DoubleChance;
            case 5: return stage5DoubleChance;
            case 6: return stage6DoubleChance;
            case 7: return stage7DoubleChance;
            case 8: return stage8DoubleChance;
            case 9: return stage9DoubleChance;
            default: return stage10DoubleChance;
        }
    }

    GameObject GetObstaclePrefab(int stage)
    {
        // 기존 5개 프리팹을 2단계씩 사용
        GameObject selectedPrefab = null;

        if (stage <= 2)
            selectedPrefab = zone1ObstaclePrefab;
        else if (stage <= 4)
            selectedPrefab = zone2ObstaclePrefab;
        else if (stage <= 6)
            selectedPrefab = zone3ObstaclePrefab;
        else if (stage <= 8)
            selectedPrefab = zone4ObstaclePrefab;
        else
            selectedPrefab = zone5ObstaclePrefab;

        // 연결되지 않은 경우 기본 장애물 사용
        if (selectedPrefab == null)
            selectedPrefab = zone1ObstaclePrefab;

        return selectedPrefab;
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

    void SpawnTile(int stage)
    {
        float currentTileZ = spawnZ;

        GameObject tile = Instantiate(
            tilePrefab,
            new Vector3(0f, 0f, currentTileZ),
            Quaternion.identity
        );

        activeTiles.Add(tile);
        spawnZ += tileLength;

        GameObject obstaclePrefab =
            GetObstaclePrefab(stage);

        if (obstaclePrefab == null)
        {
            Debug.LogWarning(
                "장애물 프리팹이 연결되지 않았습니다."
            );
            return;
        }

        float coinChance = GetCoinChance(stage);
        float doubleChance = GetDoubleChance(stage);

        // 장애물 1개 또는 2개 결정
        int obstacleCount =
            Random.value < doubleChance ? 2 : 1;

        // 안전 레인은 항상 최소 1개 확보
        List<int> availableLanes =
            new List<int> { 0, 1, 2 };

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
