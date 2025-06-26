using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雑魚戦のWave管理。東方式の「時間経過で次Wave」
/// </summary>
public class EnemyWaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        [Header("何秒後にこのWaveを開始するか")]
        public float startTime;
        [Header("このWaveで出す敵の情報リスト")]
        public List<EnemySpawnInfo> enemies;
    }

    [System.Serializable]
    public class EnemySpawnInfo
    {
        public string enemyType;
        public Vector3 spawnPosition;
    }

    [Header("雑魚戦の全Waveデータ")]
    public List<Wave> waves = new List<Wave>();

    [Header("雑魚戦の終了タイム（秒）")]
    public float endTime = 60f;
    private bool allWavesSpawned = false;

    [Header("敵プールマネージャー")]
    public EnemyPoolManager poolManager;

    [Header("ボスのプレハブ")]
    public GameObject bossPrefab;

    [Header("ボスを出す位置（startPositionはBossEntrance内で調整）")]
    public Vector3 bossSpawnPosition = Vector3.zero;

    private float timer = 0f;
    private int currentWaveIndex = 0;

    void Start()
    {
        //横一列に配置
        waves.Add(new Wave
        {
            startTime = 5f,
            enemies = CreateLineEnemies(
                enemyType: "Enemy",
                startPos: new Vector3(-2f, 6f, 0),
                count: 3,
                spacing: 2f
            )
        });

        //3x2のグリッドで配置
        waves.Add(new Wave
        {
            startTime = 10f,
            enemies = CreateGridEnemies(
                enemyType: "Enemy",
                startPos: new Vector3(0, 8f, 0),
                rows: 5,
                cols: 1,
                spacingX: 2f,
                spacingY: 1.5f
            )
        });
    }

    void Update()
    {
        timer += Time.deltaTime;

        //Wave出現チェック
        while (currentWaveIndex < waves.Count && timer >= waves[currentWaveIndex].startTime)
        {
            SpawnWave(waves[currentWaveIndex]);
            currentWaveIndex++;
        }

        //全Wave出現済み & 一定時間経過 & 敵がいない → ボス戦へ
        if (!allWavesSpawned && timer >= endTime)
        {
            allWavesSpawned = true;
        }

        if (allWavesSpawned && AreAllEnemiesGone())
        {
            GoToBossBattle();
        }
    }

    void SpawnWave(Wave wave)
    {
        foreach (var enemyInfo in wave.enemies)
        {
            poolManager.GetEnemy(enemyInfo.enemyType, enemyInfo.spawnPosition);
        }

        Debug.Log($"[EnemyWaveManager] Wave {currentWaveIndex} 出現！");
    }

    bool AreAllEnemiesGone()
    {
        return poolManager.AreAllEnemiesGone();
    }

    void GoToBossBattle()
    {
        Debug.Log("[EnemyWaveManager] 全Wave終了 → ボス戦開始！");

        //ボスを出現
        if (bossPrefab != null)
        {
            Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);
        }

        //もう雑魚処理しない
        this.enabled = false;
    }

    /// <summary>
    /// 横一列に並べて敵を作る
    /// </summary>
    public List<EnemySpawnInfo> CreateLineEnemies(string enemyType, Vector3 startPos, int count, float spacing)
    {
        var list = new List<EnemySpawnInfo>();
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = startPos + new Vector3(i * spacing, 0, 0);
            list.Add(new EnemySpawnInfo() { enemyType = enemyType, spawnPosition = pos });
        }
        return list;
    }

    /// <summary>
    /// 格子状に並べて敵を作る（行×列）
    /// </summary>
    public List<EnemySpawnInfo> CreateGridEnemies(string enemyType, Vector3 startPos, int rows, int cols, float spacingX, float spacingY)
    {
        var list = new List<EnemySpawnInfo>();
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Vector3 pos = startPos + new Vector3(c * spacingX, -r * spacingY, 0);
                list.Add(new EnemySpawnInfo() { enemyType = enemyType, spawnPosition = pos });
            }
        }
        return list;
    }
}