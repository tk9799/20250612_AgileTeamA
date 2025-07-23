using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雑魚敵のウェーブ出現管理
/// </summary>
public class EnemyWaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        [Header("出現させる敵の種類(EnemyPoolManagerの識別名)")]
        public string enemyType = "";

        [Header("出現位置")]
        public Vector3 spawnPosition = new Vector3(0f, 0f);

        [Header("連続出現数（1以上）")]
        public int spawnCount = 1;

        [Header("この敵を出すまでの待機時間(秒)")]
        public float spawnDelay = 0f;

        [Header("連続出現時の間隔（秒）")]
        public float spawnInterval = 0.2f;

        [Header("連続出現時のX/Yずらし量")]
        public Vector2 spawnOffsetPerUnit = Vector2.zero;

        [Header("この敵の移動パターン（ScriptableObject）")]
        public EnemyMovementSettings movementSettings;
    }

    [System.Serializable]
    public class EnemyWave
    {
        [Header("このウェーブで出現させる敵リスト")]
        public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();

        [Header("ウェーブ開始前の待機時間(秒)")]
        public float delayBeforeStart = 1f;
    }

    [Header("ウェーブ定義リスト")]
    public List<EnemyWave> waves = new List<EnemyWave>();

    [Header("ボスプレハブ")]
    public GameObject bossPrefab;

    [Header("ボスの出現位置")]
    public Vector3 bossSpawnPosition = new Vector3(7f, 10f, 0f);

    [Header("敵の出現に使うEnemyPoolManager")]
    public EnemyPoolManager enemyPoolManager;

    //現在出現中の敵リスト（倒されたらリストから外す）
    private List<EnemyHP> currentWaveEnemies = new List<EnemyHP>();

    void Start()
    {
        StartCoroutine(HandleWaves());
    }

    /// <summary>
    /// ウェーブの出現を順に処理
    /// </summary>
    public IEnumerator HandleWaves()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            var wave = waves[i];
            currentWaveEnemies.Clear();

            yield return new WaitForSeconds(wave.delayBeforeStart);
            Debug.Log($"[WaveManager] Wave {i + 1} 開始");

            foreach (var spawnInfo in wave.enemies)
            {
                yield return new WaitForSeconds(spawnInfo.spawnDelay);

                for (int j = 0; j < spawnInfo.spawnCount; j++)
                {
                    Vector3 offset = new Vector3(
                        spawnInfo.spawnOffsetPerUnit.x * j,
                        spawnInfo.spawnOffsetPerUnit.y * j,
                        0f);

                    Vector3 actualSpawnPos = spawnInfo.spawnPosition + offset;

                    //目標位置の横ずれ
                    Vector2 centerPosOffset = new Vector2(spawnInfo.spawnOffsetPerUnit.x * j,
                        spawnInfo.spawnOffsetPerUnit.y * j);

                    GameObject enemyObj = enemyPoolManager.GetEnemy(spawnInfo.enemyType, actualSpawnPos);
                    if (enemyObj == null)
                    {
                        Debug.LogError($"[WaveManager] 敵 {spawnInfo.enemyType} の出現に失敗");
                        continue;
                    }

                    EnemyHP enemy = enemyObj.GetComponent<EnemyHP>();
                    if (enemy != null)
                    {
                        currentWaveEnemies.Add(enemy);
                        enemy.OnEnemyDestroyed += HandleEnemyDestroyed;

                        //移動設定の適用
                        var movement = enemy.GetComponent<EnemyMovement>();
                        if (movement != null && spawnInfo.movementSettings != null)
                        {
                            movement.Initialize(spawnInfo.movementSettings, actualSpawnPos, centerPosOffset);
                        }
                    }

                    if (j < spawnInfo.spawnCount - 1)
                    {
                        yield return new WaitForSeconds(spawnInfo.spawnInterval);
                    }
                }
            }

            //このWaveの敵が全滅するまで待つ
            yield return new WaitUntil(() => currentWaveEnemies.Count == 0);
            Debug.Log($"[WaveManager] Wave {i + 1} 終了（全滅検知）");
        }

        Debug.Log("[WaveManager] 全Wave終了 → 次の処理へ");

        if (bossPrefab != null)
        {
            GameObject bossObj = Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);
            Transform player = GameObject.FindWithTag("Player")?.transform;
            BulletPoolManager bulletPool = FindAnyObjectByType<BulletPoolManager>();

            // BossFanBarrage
            var fan = bossObj.GetComponentInChildren<BossFanBarrage>();
            if (fan != null)
            {
                fan.target = player;
                fan.poolManager = bulletPool;
            }

            // BossOmnidirectionalBarrage
            var omni = bossObj.GetComponentInChildren<BossOmnidirectionalBarrage>();
            if (omni != null)
            {
                omni.poolManager = bulletPool;
            }

            Debug.Log("[WaveManager] ボスを出現させました");
        }
        else
        {
            Debug.Log("[WaveManager] ボスプレハブが未設定です");
        }
    }

    void HandleEnemyDestroyed(EnemyHP enemy)
    {
        if (currentWaveEnemies.Contains(enemy))
        {
            currentWaveEnemies.Remove(enemy);
            enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
        }
    }
}