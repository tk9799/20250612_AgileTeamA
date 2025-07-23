using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 単一の敵種に対するオブジェクトプール
/// </summary>
public class EnemyPool : MonoBehaviour
{
    [Header("プールする敵のプレハブ(EnemyBase付き)")]
    public GameObject enemyPrefab;

    [Header("初期生成数")]
    public int initialPoolSize = 10;

    private Queue<EnemyBase> pool = new Queue<EnemyBase>();
    private List<EnemyBase> activeEnemies = new List<EnemyBase>();

    void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddEnemyToPool();
        }
    }

    private void AddEnemyToPool()
    {
        var obj = Instantiate(enemyPrefab, transform);
        var enemy = obj.GetComponent<EnemyBase>();
        enemy.SetOwnerPool(this);
        obj.SetActive(false);
        pool.Enqueue(enemy);
    }

    public EnemyBase GetEnemy()
    {
        if (pool.Count == 0)
        {
            AddEnemyToPool();
        }

        var enemy = pool.Dequeue();
        enemy.SetOwnerPool(this);
        enemy.gameObject.SetActive(true);
        activeEnemies.Add(enemy);
        return enemy;
    }

    public void ReturnEnemy(EnemyBase enemy)
    {
        enemy.gameObject.SetActive(false);
        activeEnemies.Remove(enemy);
        pool.Enqueue(enemy);
    }

    public List<EnemyBase> GetAllActiveEnemies()
    {
        return new List<EnemyBase>(activeEnemies);
    }
}