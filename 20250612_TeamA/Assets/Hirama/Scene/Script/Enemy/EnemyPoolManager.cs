using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 複数種類の敵プールを管理するマネージャー
/// </summary>
public class EnemyPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolEntry
    {
        [Header("この敵のデータ")]
        public EnemyData enemyData;

        [Header("対応するEnemyPool")]
        public EnemyPool enemyPool;
    }

    [Header("敵種ごとのプール")]
    public List<PoolEntry> poolEntries = new List<PoolEntry>();

    private Dictionary<EnemyData, EnemyPool> lookup = new Dictionary<EnemyData, EnemyPool>();

    public static EnemyPoolManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (var entry in poolEntries)
        {
            if (entry.enemyData != null && entry.enemyPool != null)
            {
                if (!lookup.ContainsKey(entry.enemyData))
                {
                    lookup.Add(entry.enemyData, entry.enemyPool);
                }
                else
                {
                    //Debug.LogWarning($"EnemyData {entry.enemyData.name} はすでに登録済みです");
                }
            }
        }
    }

    /// <summary>
    /// 敵をスポーンする(EnemyDataからPrefabとPoolを設定)
    /// </summary>
    public EnemyBase SpawnEnemy(EnemyData data, Vector3 position)
    {
        if (data == null)
        {
            Debug.LogError("EnemyDataがnullです");
            return null;
        }

        if (lookup.TryGetValue(data, out var pool))
        {
            var enemy = pool.GetEnemy();
            enemy.Init(data, position);

            var controller = enemy.GetComponent<EnemyController>();
            if (controller != null)
            {
                controller.Init(data);
            }
            else
            {
                Debug.LogWarning("EnemyControllerがアタッチされていません");
            }

            return enemy;
        }
        else
        {
            Debug.LogWarning($"EnemyData {data.name} に対応するプールが見つかりません");
            return null;
        }
    }

    /// <summary>
    /// 全敵の一時停止
    /// </summary>
    public void PauseAllEnemies()
    {
        foreach (var pool in lookup.Values)
        {
            foreach (var enemy in pool.GetAllActiveEnemies())
            {
                enemy.enabled = false;
            }
        }
    }

    /// <summary>
    /// 全敵の再開
    /// </summary>
    public void ResumeAllEnemies()
    {
        foreach (var pool in lookup.Values)
        {
            foreach (var enemy in pool.GetAllActiveEnemies())
            {
                enemy.enabled = true;
            }
        }
    }

    public List<EnemyBase> GetAllActiveEnemies()
    {
        List<EnemyBase> result = new List<EnemyBase>();
        foreach (var pool in lookup.Values)
        {
            result.AddRange(pool.GetAllActiveEnemies());
        }
        return result;
    }
}